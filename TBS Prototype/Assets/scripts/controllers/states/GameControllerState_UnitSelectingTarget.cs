using UnityEngine;
using System.Collections.Generic;
using Busted.UI;

namespace Busted
{
    public class GameControllerState_UnitSelectingTarget : GameControllerState
    {
        Unit _unit;
        bool _selectedSelf;
        public GameControllerState_UnitSelectingTarget(Unit unit, bool selectedSelf) : base()
        {
            stateName = "GameControllerState_UnitSelectingTarget";
            _unit = unit;
            _selectedSelf = selectedSelf;
        }

        public override void OnStateEnter()
        {   //Display targeting sprite over all attackable units
            gameControllerUI.HideActionPanel();
            _unit.UnitState = new UnitStateSelectingTarget(_unit, _unit.UnitState.previous);
        }

        public override void OnStateExit(string goingTo) //Probably a better way to do this?
        {
            if (goingTo == "GameControllerState_HumanTurn")
            {
                foreach (Unit u in _unit.attackableUnits)
                {
                    u.MarkAsNormal();
                }
            }
            else if (goingTo == "GameControllerState_UnitAction")
            {
                foreach (Unit u in _unit.attackableUnits)
                {
                    u.MarkAsAttackable();
                }
            }

        }

        public override void OnUnitSelected(Unit unit)
        {
            Debug.Log("unit selected to attack");
            unit.MarkAsAttackable();
            if (unit.teamColor != gameController.playerTurn.playerData.teamColor && unit.markedAttackable)
            {
                Debug.Log("attacking unit now");
                Unit attacker = gameController.playerTurn.playerData.selectedUnit;
                Unit defender = unit;

                //Do damage things here
                gameController.battleCalculator.AttackUnit(attacker, defender);

                //Once the battle is over....
                gameController.playerTurn.playerData.selectedUnit.turnSpent = true;
                gameController.playerTurn.playerData.selectedUnit.MarkAsSpent();
                gameController.playerTurn.playerData.selectedUnit.UnitState = new UnitStateIdle(gameController.playerTurn.playerData.selectedUnit);
                foreach (Unit u in gameController.playerTurn.playerData.selectedUnit.attackableUnits)
                {
                    u.MarkAsNormal();
                }
                if (defender.Health <= 4)
                {
                    defender.onTile.isOccupied = false;
                    defender.onTile.containsUnit = null;
                    GRID.Map.unitList.Remove(defender);
                    defender.OwnedBy.playerData.ownedUnits.Remove(defender);
                    GameObject.Destroy(defender.gameObject);
                }
                if (attacker.Health <= 4)
                {
                    attacker.onTile.isOccupied = false;
                    attacker.onTile.containsUnit = null;
                    GRID.Map.unitList.Remove(attacker);
                    attacker.OwnedBy.playerData.ownedUnits.Remove(attacker);
                    GameObject.Destroy(attacker.gameObject);
                }
                gameController.GameControllerState = new GameControllerState_HumanTurn("GameControllerState_HumanTurn");
            }
        }

        public override void OnTileSelected(Tile tile)
        {
            if(tile.containsUnit != null && tile.containsUnit.teamColor != gameController.playerTurn.playerData.teamColor) //If a tile is selected that contains an enemy unit
            {
                OnUnitSelected(tile.containsUnit);
            }
            else
            gameController.GameControllerState = new GameControllerState_UnitAction("GameControllerState_UnitAction", _unit, _selectedSelf);
        }
    }
}
