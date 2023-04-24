using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Busted.UI;

namespace Busted
{
    /// <summary>
    /// A human player's turn. Awaits input from player and processes turn logic.
    /// </summary>
    public class GameControllerState_HumanTurn : GameControllerState
    {
        public GameControllerState_HumanTurn() : base()
        {
            stateName = "GameControllerState_HumanTurn";
        }
        public GameControllerState_HumanTurn(string goingTo) : base(goingTo)
        {
            stateName = "GameControllerState_HumanTurn";
            base.goingTo = goingTo;
        }


        public override void OnStateEnter()
        {
            base.OnStateEnter();
            gameController.playerTurn.playerData.selectedUnit = null;
            gameControllerUI.EndTurnAllowed(true);
        }

        public override void OnStateExit()
        {
            base.OnStateExit();
            gameControllerUI.EndTurnAllowed(false);

        }

        /// <summary>
        /// Tell the game controller that a human player selected a unit during their turn
        /// </summary>
        /// <param name="unit"></param>
        public override void OnUnitSelected(Unit unit)
        {
            if (unit.turnSpent == false && unit.teamColor == gameController.playerTurn.playerData.teamColor) //If this is my unit and its turn isnt spent
            {
                if (gameController.GameControllerState.stateName != "GameControllerState_HumanSelectedUnit") //If we're not in the selected state already...
                {
                    GRID.GameController.GameControllerState = new GameControllerState_HumanSelectedUnit(unit);
                } //If we are, don't create a new state, which would cause chaos. This allows selecting units while one is already selected
                else base.OnUnitSelected(unit);
            }

        }

        public override void OnTileSelected(Tile tile)
        {
            base.OnTileSelected(tile);
            if(tile.containsUnit)
            {
                gameController.GameControllerState.OnUnitSelected(tile.containsUnit);
            }
        }

    }

}
