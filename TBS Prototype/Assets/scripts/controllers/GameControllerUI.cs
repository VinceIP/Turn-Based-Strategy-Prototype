using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Busted.UI
{
    public class GameControllerUI : UIController
    {
        //public static GameControllerUI instance;

        public Button btn_EndTurn;

        public GameObject pnl_startMenu;
        public RectTransform pnl_Blackout;
        public RectTransform pnl_UnitAction;
        public Button btn_Wait;
        public Button btn_Revert;
        public Button btn_Attack;
        public GameObject obj_Options;

        public GameObject unitHealthContainer;


        protected override void Awake()
        {
            base.Awake();
            menuPanels.Add(pnl_startMenu);
        }


        public void clicked_NewGame()
        {
            ToggleMenu(pnl_LoadMap);
            savedMapList.UpdateMaps();
        }

        public override void clicked_LoadMap_Cancel()
        {
            base.clicked_LoadMap_Cancel();
            ToggleMenu(pnl_LoadMap);
            ToggleMenu(pnl_startMenu);
        }


        /// <summary>
        /// Show action panel above given unit
        /// </summary>
        /// <param name="unit"></param>
        public void ShowActionPanel(Unit unit, bool canAttack, bool showRevert)
        {
            if (!GRID.GameController.playerTurn.playerData.isCPU)
            {
                pnl_UnitAction.gameObject.SetActive(true);
                //GRID.Map.cursor.canMove = false;
                Camera.main.GetComponent<CamScroll>().ScrollAllowed = false;
                Vector3 pt = Camera.main.WorldToScreenPoint(unit.transform.position); //Panel target position
                pnl_UnitAction.transform.position = new Vector3(pt.x, pt.y + 60f, pt.z);
                if (canAttack == false)
                {
                    btn_Attack.gameObject.SetActive(false);
                }
                else btn_Attack.gameObject.SetActive(true);
                if (showRevert == true)
                {
                    btn_Revert.gameObject.SetActive(true);
                }
                else btn_Revert.gameObject.SetActive(false);
            }

        }

        public void HideActionPanel()
        {
            pnl_UnitAction.gameObject.SetActive(false);
            //GRID.Map.cursor.canMove = true;
            Camera.main.GetComponent<CamScroll>().ScrollAllowed = true;
        }

        public void EndTurnAllowed(bool allow)
        {
            if (allow == true)
            {
                btn_EndTurn.interactable = true;
            }
            else btn_EndTurn.interactable = false;
        }


        public void clicked_Attack()
        {
            GRID.GameController.GameControllerState.OnClickedAttack();

        }

        public void clicked_Wait()
        {
            HideActionPanel();
            GRID.GameController.GameControllerState.OnClickedWait();
        }

        public void clicked_Revert()
        {
            GRID.GameController.GameControllerState.OnClickedRevert();
            HideActionPanel();
        }

        public void clicked_EndTurn()
        {
            gameController.GameControllerState.EndTurn();
        }

        public void clicked_Options()
        {
            gameController.PauseGame(true);
            pnl_Blackout.gameObject.SetActive(true);
            obj_Options.SetActive(true);
        }

        public void clicked_Unpause()
        {
            gameController.PauseGame(false);
            pnl_Blackout.gameObject.SetActive(false);
            obj_Options.SetActive(false);
        }


        public void clicked_LoadMap_cancel()
        {
            pnl_LoadMap.gameObject.SetActive(false);
            obj_Options.SetActive(true);
        }

        public void clicked_LoadMap_Selection()
        {
            ToggleMenu(pnl_LoadMap);
            //pnl_LoadMap.gameObject.SetActive(false);
            //obj_Options.SetActive(false);
            //pnl_Blackout.gameObject.SetActive(false);
        }

    }

}
