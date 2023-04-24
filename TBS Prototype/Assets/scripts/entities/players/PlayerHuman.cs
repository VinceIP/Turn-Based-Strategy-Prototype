using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Busted
{
    public class PlayerHuman : Player
    {
        public PlayerHuman()
        {
            playerData.playerName = "Human Player";
        }
        void Start()
        {
            //Debug.Log("Human player joined the game");
        }
        public override void Play()
        {
            GRID.GameController.GameControllerState = new GameControllerState_HumanTurn();
        }
    }

}
