using UnityEngine;
using Busted.UI;

namespace Busted
{
    public class GameControllerState_UnitAction : GameControllerState
    {
        private Unit _staySelected; //GameControllerState_HumanUnitSelected will deselect the unit when it changes states; override that here
        private Unit _unit;
        bool _selectedSelf;

        public GameControllerState_UnitAction(Unit unit, bool selectedSelf) : base()
        {
            stateName = "GameControllerState_UnitAction";
            _staySelected = unit;
            _unit = unit;
            _selectedSelf = selectedSelf;
        }

        public GameControllerState_UnitAction(string goingTo, Unit unit, bool selectedSelf) : base()
        {
            this.goingTo = goingTo;
            stateName = "GameControllerState_UnitAction";
            _staySelected = unit;
            _unit = unit;
            _selectedSelf = selectedSelf;
        }

        public override void OnStateEnter()
        {
            gameController.playerTurn.playerData.selectedUnit = _staySelected;
            if (_selectedSelf) //If the player selected a unit twice and didn't move it
            {
                if (_unit.attackableUnits.Count > 0)//And has attack targets
                {
                    gameControllerUI.ShowActionPanel(_unit, true, false);
                }
                else gameControllerUI.ShowActionPanel(_unit, false, false);
            }
            else if (_selectedSelf == false) //If the player selected a unit, then moved it
            {
                if (_unit.attackableUnits.Count > 0) //And has targets
                {
                    gameControllerUI.ShowActionPanel(_unit, true, true);
                }
                else gameControllerUI.ShowActionPanel(_unit, false, true);
            }
        }

        public override void OnStateExit()
        {
            //_staySelected.UnitState = new UnitStateIdle(_staySelected);
            //GRID.GameController.playerTurn.playerData.selectedUnit = null;
        }

        public override void OnUnitSelected(Unit unit)
        {
        }

        public override void OnTileSelected(Tile tile)
        {
            gameControllerUI.HideActionPanel();
            _staySelected.UnitState.Revert();
            gameController.GameControllerState = new GameControllerState_HumanSelectedUnit(_staySelected);
        }

        public override void OnClickedAttack()
        {
            if (gameController.playerTurn.playerData.selectedUnit.attackableUnits.Count > 0)
            {
                gameController.GameControllerState = new GameControllerState_UnitSelectingTarget(gameController.playerTurn.playerData.selectedUnit, _selectedSelf);
            }
        }

        public override void OnClickedWait()
        {
            gameController.playerTurn.playerData.selectedUnit.UnitState.Wait();
            gameController.GameControllerState = new GameControllerState_HumanTurn();
            gameController.playerTurn.playerData.selectedUnit = null;
        }

        public override void OnClickedRevert()
        {
            _staySelected.UnitState.Revert();
            gameController.GameControllerState = new GameControllerState_HumanSelectedUnit(_staySelected);
        }
    }
}
