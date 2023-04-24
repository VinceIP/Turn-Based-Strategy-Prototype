using UnityEngine;

namespace Busted
{
    /// <summary>
    /// Default unit state; when it's inactive
    /// </summary>
    public class UnitStateIdle : UnitState
    {
        public UnitStateIdle(Unit unit) : base(unit)
        {
            stateName = "UnitStateIdle";
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
        }

        /// <summary>
        /// Tell a unit that it's been clicked
        /// </summary>
        /// <param name="unit"></param>
        public override void OnUnitSelected(Unit unit)
        {
            if (unit.OwnedBy == GRID.GameController.playerTurn && unit.turnSpent == false) //If the unit being clicked on it owned by the current player whose turn it is
            {
                unit.UnitState = new UnitStateSelected(unit); //Tell the unit to change states
                                                              //GRID.GameController.GameControllerState.OnUnitSelected(unit); //Let the game controller state know a unit has been selected
            }
        }
    }
}
