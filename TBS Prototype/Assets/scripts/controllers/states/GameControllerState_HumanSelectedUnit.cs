using UnityEngine;

namespace Busted
{
    public class GameControllerState_HumanSelectedUnit : GameControllerState_HumanTurn
    {
        /// <summary>
        /// Reference to the unit currently selected
        /// </summary>
        Unit _unit;
        public GameControllerState_HumanSelectedUnit(Unit unit) : base()
        {
            stateName = "GameControllerState_HumanSelectedUnit";
            _unit = unit;
        }

        /// <summary>
        /// Called when a human player clicks another unit, while a unit is already selected
        /// Unselects the current unit, and selects the new one
        /// Logic handled by unit states, so do nothing here?? -- No
        /// </summary>
        /// <param name="unit"></param>
        public override void OnUnitSelected(Unit unit)
        {
            base.OnUnitSelected(unit);
        }

        public override void OnStateEnter()
        {
            _unit.UnitState.OnUnitSelected(_unit);
        }

        public override void OnTileSelected(Tile tile)
        {
            if (tile.containsUnit)
            { 
                gameController.GameControllerState.OnUnitSelected(tile.containsUnit);
                return;
            }
            if (gameController.playerTurn.playerData.selectedUnit != null)
            {
                if (gameController.playerTurn.playerData.selectedUnit.UnitState.pathsInRange.Contains(tile))
                {
                    gameController.playerTurn.playerData.selectedUnit.UnitState.OnTileSelected(tile);
                }
                else
                {
                    GRID.GameController.playerTurn.playerData.selectedUnit.UnitState = new UnitStateIdle(GRID.GameController.playerTurn.playerData.selectedUnit);
                    GRID.GameController.GameControllerState = new GameControllerState_HumanTurn();
                }
            }
        }

        //Temporary
        //Deselect the unit when any cell is clicked
        /*
        if(GRID.GameController.playerTurn.playerData.selectedUnit != null) //If current player has a unit selected
        {
            if(GRID.GameController.playerTurn.playerData.selectedUnit.UnitState.pathsInRange.Contains(tile)) //If the current player's selected unit contains a path to the clicked tile
            {
                //Debug.Log("clicked valid move point");
            }
            else
            {
                //Deselect the unit
                GRID.GameController.playerTurn.playerData.selectedUnit.UnitState = new UnitStateIdle(GRID.GameController.playerTurn.playerData.selectedUnit);
                GRID.GameController.GameControllerState = new GameControllerState_HumanTurn();
            }
        }
        }
        */


        public override void OnStateExit()
        {
            base.OnStateExit();
            GRID.GameController.playerTurn.playerData.selectedUnit = null;
        }
    }
}
