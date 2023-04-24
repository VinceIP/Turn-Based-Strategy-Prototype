using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Busted.UI;

namespace Busted
{
    public abstract class GameControllerState
    {
        public string stateName;
        public string goingTo;
        protected GameController gameController = GRID.GameController;
        protected GameControllerUI gameControllerUI;

        public GameControllerState()
        {
            gameControllerUI = GameObject.FindGameObjectWithTag("GameControllerUI").GetComponent<GameControllerUI>();
            stateName = "GameControllerState";
        }

        public GameControllerState(string goingTo)
        {
            gameControllerUI = GameObject.FindGameObjectWithTag("GameControllerUI").GetComponent<GameControllerUI>();
            this.goingTo = goingTo;
            stateName = "GameControllerState";
        }

        public virtual void OnUnitSelected(Unit unit)
        {
            unit.UnitState.OnUnitSelected(unit);
        }

        public virtual void OnTileSelected(Tile tile)
        {

        }
        public virtual void OnStateEnter()
        {

        }
        public virtual void OnStateExit()
        {

        }

        public virtual void OnStateExit(string goingTo)
        {

        }

        public virtual void OnClickedWait()
        {

        }

        public virtual void OnClickedAttack()
        {
            gameController.playerTurn.playerData.selectedUnit.UnitState.Attack();
        }

        public virtual void OnClickedRevert()
        {
            gameController.playerTurn.playerData.selectedUnit.UnitState.Revert();
        }

        public virtual void EndTurn()
        {
            gameController.GameControllerState = new GameControllerState_ChangingTurn();
        }
    }

}

