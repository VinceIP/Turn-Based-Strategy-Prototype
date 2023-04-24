using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Busted
{
    /// <summary>
    /// State entered when a unit has been selected by its owner
    /// </summary>
    public class UnitStateSelected : UnitState
    {
        private Color highlightColor;
        private float brightenFactor = 100;

        public UnitStateSelected(Unit unit) : base(unit)
        {
            _unit = unit;
            stateName = "UnitStateSelected";
            pathsInRange = new List<Tile>();
            _unit.movableTiles = new List<Tile>();
        }

        public override void OnStateEnter()
        {
            if (GRID.GameController.playerTurn.playerData.isCPU == false)
            {
                Unit oldUnit = gameController.playerTurn.playerData.selectedUnit; //Get a reference to any unit that is already selected
                if (oldUnit != null) //There's already a unit selected
                {
                    oldUnit.UnitState = new UnitStateIdle(oldUnit); //Deselect the old unit
                }
                gameController.playerTurn.playerData.selectedUnit = _unit; //Select the new unit
            }
            _unit.StartCoroutine(Glow(_unit));
            Debug.Log("Move range: " + _unit.MoveRange);
            //Debug.Log(GRID.Map.tileList.Count);
            _unit.UnitState.pathsInRange = _unit.GetAvailableDestinations(GRID.Map.tileList); //Find all tiles I can move to
            Debug.Log(pathsInRange.Count);
            //_unit.UnitState.pathsInRange.Add(_unit.onTile); //Make sure onTile is marked too, becuase it looks better kind of
            foreach (var tile in pathsInRange)
            {
                tile.GetComponent<Tile>().MarkAsReachable(); //Mark them
                _unit.movableTiles.Add(tile);
            }
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            if (_unit.UnitState.stateName != "UnitStateMoving") UnMark();
            if (pathsInRange != null) //If there are any tiles flagged as possible destinations
            {
                foreach (var tile in pathsInRange)
                {
                    tile.UnMark(); //Unmark all of them
                }
            }
        }

        public override void OnTileSelected(Tile tile)
        {
            base.OnTileSelected(tile);
            if (_unit.movableTiles != null)
            {
                if (pathsInRange.Contains(tile))
                {
                    _unit.UnitState = new UnitStateMoving(_unit, tile);
                }
            }

        }

        public void DebugLightUpChunk(List<Tile> chunk)
        {
            foreach (Tile tile in chunk)
            {
                tile.GetComponent<Tile>().MarkAsReachable();
            }
        }

        public override void OnUnitSelected(Unit unit)
        {
            if (unit == _unit) //If selected self again
            {
                _unit.UnitState = new UnitStateAction(_unit, _unit.transform.position, true);
            }
            else
            {
                gameController.GameControllerState = new GameControllerState_HumanTurn();
                unit.UnitState = new UnitStateIdle(unit); //Deselect this
            }

        }

        public void FindMovableTiles(Unit unit)
        {
            pathsInRange = unit.GetAvailableDestinations(GRID.Map.tileList);
        }
    }

}
