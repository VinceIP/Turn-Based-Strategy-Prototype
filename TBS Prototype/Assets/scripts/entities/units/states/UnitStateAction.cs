using UnityEngine;
using System.Collections.Generic;

namespace Busted
{
    /// <summary>
    /// State entered after a unit has been moved; awaits player "wait, attack" command
    /// </summary>
    public class UnitStateAction : UnitState
    {
        public Vector2 _previous;
        bool _selectedSelf;
        bool _isAI;
        Unit _aiTarget;
        List<Unit> _attackableNeighbors;
        public UnitStateAction(Unit unit, Vector2 previous, bool selectedSelf) : base(unit)
        {
            _unit = unit;
            _previous = previous;
            _selectedSelf = selectedSelf;
            _isAI = false;
            stateName = "UnitStateAction";
        }
        public UnitStateAction(Unit unit, Unit aiTarget) : base(unit)
        {
            _unit = unit;
            _aiTarget = aiTarget;
            _isAI = true;
            stateName = "UnitStateAction";
        }

        public override void OnStateEnter()
        {

            _unit.onTile.isOccupied = false;
            _unit.onTile.containsUnit = null;
            _unit.UpdateOnTileLate();
            _unit.GetAttackable();
            foreach (Unit u in _unit.attackableUnits)
            {
                u.MarkAsAttackable();
            }

            GRID.GameController.GameControllerState = new GameControllerState_UnitAction(_unit, _selectedSelf);


        }


        public override void OnStateExit()
        {
            foreach (Unit u in _unit.attackableUnits)
            {
                u.MarkAsNormal();
            }

        }

        public override void OnTileSelected(Tile tile)
        {
        }

        public override void OnUnitSelected(Unit unit)
        {
        }

        public override void Wait()
        {
            _unit.turnSpent = true;
            _unit.MarkAsSpent();
            _unit.UnitState = new UnitStateIdle(_unit);
        }

        public override void OnPause()
        {
            //Revert();
        }

        public override void Revert()
        {
            _unit.transform.position = new Vector2(_previous.x, _previous.y);
            _unit.onTile.isOccupied = false;
            _unit.onTile.containsUnit = null;
            _unit.onTile = GRID.Map.tiles[(int)_previous.x, (int)_previous.y].GetComponent<Tile>();
            _unit.onTile.isOccupied = true;
            _unit.onTile.containsUnit = _unit;
            gameController.playerTurn.playerData.selectedUnit = null;
            _unit.UnitState = new UnitStateIdle(_unit);
        }

        public override void Attack()
        {
            base.Attack();
            _unit.UnitState = new UnitStateSelectingTarget(_unit, _previous);
        }
        public override void Attack(Unit target, Unit aiTarget)
        {
            base.Attack();
            _unit.UnitState = new UnitStateSelectingTarget(_unit, aiTarget);
        }


    }
}
