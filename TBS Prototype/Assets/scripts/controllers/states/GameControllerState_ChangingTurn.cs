using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Busted
{
    public class GameControllerState_ChangingTurn : GameControllerState
    {
        public GameControllerState_ChangingTurn() : base()
        {
            stateName = "GameControllerState_ChangingTurn";
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            int currentIndex = gameController.players.IndexOf(gameController.playerTurn);
            foreach (Unit u in gameController.playerTurn.playerData.ownedUnits) //Reset previous player's units
            {
                u.turnSpent = false;
                u.MarkAsNormal();
            }
            Player nextPlayer;
            if (currentIndex + 1 < gameController.players.Count) //Set the current player to the next one in the list, making sure we dont go out of index range
            {
                nextPlayer = gameController.players[currentIndex + 1];
            }
            else
            {
                nextPlayer = gameController.players[0];
            }
            gameController.playerTurn = nextPlayer;
            Debug.Log("Changed turn to: " + gameController.playerTurn.playerData.playerName);
            gameController.GameControllerState = new GameControllerState_TurnStart();
            //gameController.playerTurn.Play();
        }

        public override void OnStateExit()
        {
            base.OnStateExit();

        }

        public override void OnTileSelected(Tile tile)
        {
        }

        public override void OnUnitSelected(Unit unit)
        {
        }


    }

}

