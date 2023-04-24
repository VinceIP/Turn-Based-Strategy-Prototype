using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;
using System.Collections.Generic;
using Busted.Data;
using Busted.UI;

namespace Busted
{
    public class GameController : MonoBehaviour
    {
        public string loadThis;
        //public static GameController instance;
        public GameControllerUI gameControllerUI;
        public Map map;
        public GameObject mapObj;
        public BattleCalculator battleCalculator = new BattleCalculator();
        public List<Player> players;
        /// <summary>
        /// Whose turn is it?
        /// </summary>
        public Player playerTurn;

        public string currentState; //for inspector debugging

        private GameControllerState _gameControllerState;
        public GameControllerState GameControllerState
        {
            get { return _gameControllerState; }
            set
            {
                if (_gameControllerState != null)
                {
                    if (value.goingTo != null) //Allow special OnStateExit events based on the state we are transitioning to
                    {
                        _gameControllerState.OnStateExit(value.goingTo);
                    }
                    else
                    {
                        _gameControllerState.OnStateExit();
                    }
                }
                _gameControllerState = value;
                _gameControllerState.OnStateEnter();
                currentState = _gameControllerState.stateName;
                print("Gamestate changed to " + _gameControllerState.stateName);

            }
        }

        public enum GameControllerSceneState
        {
            Title,
            Editor,
            Game
        }


        /// <summary>
        /// Changes behaviours based on which scene or game mode we are in
        /// </summary>
        public GameControllerSceneState sceneState;

        public void Awake()
        {
            //DontDestroyOnLoad(gameObject);
            GRID.GameController = this;
            mapObj = null;
            Cursor.lockState = CursorLockMode.Confined;
        }

        void OnEnable()
        {
            Map.onMapLoaded += OnMapLoaded;
        }

        void Start()
        {
            //GameControllerState = new GameControllerState_SettingUp();
        }

        void OnMapLoaded()
        {
            //SetupPlayers();
            //StartGame();
            print("map loaded");
            if(sceneState == GameControllerSceneState.Game)
            {
                GameControllerState = new GameControllerState_SettingUp();
            }
        }

        void OnSceneChange(String thisScene)
        {
            switch(thisScene)
            {
                case "title":
                    break;
                case "editor":
                    break;
                case "game":
                    break;
            }
        }

        public static void SetScene(GameControllerSceneState state)
        {
            String thisScene = "";
            switch(state)
            {
                case GameControllerSceneState.Title:
                    SceneManager.LoadScene("title");
                    thisScene = "title";
                    GRID.GameController.sceneState = GameControllerSceneState.Title;
                    break;
                case GameControllerSceneState.Editor:
                    SceneManager.LoadScene("editor");
                    thisScene = "editor";
                    GRID.GameController.sceneState = GameControllerSceneState.Editor;
                    break;
                case GameControllerSceneState.Game:
                    SceneManager.LoadScene("game");
                    thisScene = "game";
                    GRID.GameController.sceneState = GameControllerSceneState.Game;
                    break;
            }
            GRID.GameController.OnSceneChange(thisScene);
        }

        public void LoadTestMap()
        {
            Busted.Data.MapSaveLoad.LoadMapFromFile(loadThis);
        }

        public void LoadNewMap(SavedMap map)
        {

            GameObject[] cleanup = GameObject.FindGameObjectsWithTag("Player");
            foreach (GameObject player in cleanup)
            {
                Destroy(player);
            }
            Destroy(GameObject.Find("Players"));
            Destroy(GameObject.Find("Units"));
            Map.GenerateMapFromFile(map);
            SetupPlayers();
            StartGame();
        }

        public void SetupPlayers()
        {
            players = new List<Player>();
            GameObject playersHolder = Instantiate(new GameObject());
            playersHolder.name = "Players";
            playersHolder.tag = "Player";
            Player humanPlayer = Instantiate(GRID.Prefabs.HumanPlayer).GetComponent<Player>();
            humanPlayer.tag = "Player";
            humanPlayer.playerData.teamColor = PlayerData.TeamColor.BLUE;
            Player aiPlayer = Instantiate(GRID.Prefabs.AIPlayer).GetComponent<Player>();
            aiPlayer.playerData.teamColor = PlayerData.TeamColor.GREEN;
            aiPlayer.tag = "Player";
            humanPlayer.transform.SetParent(playersHolder.transform);
            aiPlayer.transform.SetParent(playersHolder.transform);
            aiPlayer.playerData.isCPU = true;
            players.Add(humanPlayer);
            players.Add(aiPlayer);
            AssignUnitsTo(players[0]);
            AssignUnitsTo(players[1]);
            playerTurn = humanPlayer;
        }
        public void AssignUnitsTo(Player player)
        {
            player.playerData.ownedUnits = new List<Unit>();
            foreach (Unit unit in GRID.Map.unitList)
            {
                if (unit.teamColor == player.playerData.teamColor)
                {
                    unit.OwnedBy = player;
                    player.playerData.ownedUnits.Add(unit);
                }
            }
        }
        public void StartGame()
        {
            GameControllerState = new GameControllerState_HumanTurn();
        }

        public void PauseGame(bool paused)
        {
            if (paused == true)
            {
                if (playerTurn.playerData.selectedUnit != null)
                {
                    playerTurn.playerData.selectedUnit.UnitState.Revert();
                }
                GameControllerState = new GameControllerState_GamePaused();
            }
            else GameControllerState = new GameControllerState_HumanTurn();
        }

    }
}
