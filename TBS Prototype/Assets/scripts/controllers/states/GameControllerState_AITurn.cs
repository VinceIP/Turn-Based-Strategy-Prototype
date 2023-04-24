using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Busted
{
    public class GameControllerState_AITurn : GameControllerState
    {
        public GameControllerState_AITurn() : base()
        {
            stateName = "GameControllerState_AITurn";
        }

        public override void OnStateEnter()
        {
            gameController.playerTurn.Play();
        }

        public override void OnTileSelected(Tile tile)
        {
        }
        public override void OnUnitSelected(Unit unit)
        {
        }

    }

}
