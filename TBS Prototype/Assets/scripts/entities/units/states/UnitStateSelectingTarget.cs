using UnityEngine;
using System.Collections.Generic;

namespace Busted
{
    public class UnitStateSelectingTarget : UnitState
    {
        //Vector2 previous;
        bool _isAI;
        public UnitStateSelectingTarget(Unit unit, Vector2 Previous) : base(unit)
        {
            stateName = "UnitStateSelectingTarget";
            _unit = unit;
            previous = Previous;
        }
        public UnitStateSelectingTarget(Unit unit, Unit aiTarget) : base(unit)
        {
            stateName = "UnitStateSelectingTarget";
            _unit = unit;
            _isAI = true;
        }

        public override void OnStateEnter()
        {
            base.OnStateEnter();
            foreach (Unit u in _unit.attackableUnits)
            {
                u.MarkAsTarget();
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
        }

        public override void OnUnitSelected(Unit unit)
        {
            _unit.turnSpent = true;
            _unit.MarkAsSpent();
            _unit.UnitState = new UnitStateIdle(_unit);

        }

        public override void OnTileSelected(Tile tile)
        {
            base.OnTileSelected(tile);
            _unit.UnitState = new UnitStateAction(_unit, previous, false);
        }
        //Lets the AI attack
        public override void Attack()
        {

        }
    }
}
