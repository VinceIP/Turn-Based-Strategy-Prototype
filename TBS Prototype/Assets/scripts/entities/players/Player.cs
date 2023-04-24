using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Busted
{
    public class Player : MonoBehaviour
    {
        public PlayerData playerData;

        public Player()
        {
            playerData = new PlayerData();
        }
        /// <summary>
        /// Let's a player take their turn
        /// </summary>
        public virtual void Play()
        {
        }

        /// <summary>
        /// Ends the player's turn.
        /// </summary>
        //public abstract void EndTurn();

    }

}
