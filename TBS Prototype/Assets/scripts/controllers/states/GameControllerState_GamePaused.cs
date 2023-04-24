using UnityEngine;
using Busted.UI;

namespace Busted
{
    public class GameControllerState_GamePaused : GameControllerState
    {
        private CamScroll camScroll;
        public GameControllerState_GamePaused() : base()
        {
            stateName = "GameControllerState_GamePaused";
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            gameControllerUI.HideActionPanel();
            if (gameController.playerTurn.playerData.selectedUnit != null)
            {
                Debug.Log(":#)");
                gameController.playerTurn.playerData.selectedUnit.UnitState = new UnitStateIdle(gameController.playerTurn.playerData.selectedUnit);
                gameController.playerTurn.playerData.selectedUnit = null;
            }
            //GRID.Map.cursor.canMove = false;
            camScroll = Camera.main.GetComponent<CamScroll>();
            camScroll.ScrollAllowed = false;
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            //GRID.Map.cursor.canMove = true;
            camScroll = Camera.main.GetComponent<CamScroll>();
            camScroll.ScrollAllowed = true;
        }

        
    }
}

