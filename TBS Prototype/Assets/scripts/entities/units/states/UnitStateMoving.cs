using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace Busted
{
    /// <summary>
    /// State entered when a unit starts moving
    /// </summary>
    public class UnitStateMoving : UnitState
    {
        Tile _destination;
        List<Tile> chunk;
        List<Tile> path;
        public UnitStateMoving(Unit unit, Tile destination) : base(unit)
        {
            _unit = unit;
            _destination = destination;
            stateName = "UnitStateMoving";
        }

        public override void OnStateEnter()
        {
            if (_unit.MoveRange >= 10)
            {
                chunk = _unit.GetMapChunk(_unit, 4);
            }
            else chunk = _unit.GetMapChunk(_unit.onTile, new Vector2(14, 14));

            path = _unit.FindPath(chunk, _destination);
            _unit.MoveTo(_destination, path);
           
        }

        public override void OnStateExit()
        {

        }

    }
}