using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Busted
{/// <summary>
 /// Contains non-MonoBehaviour reliant player data
 /// </summary>
    public class PlayerData
    {
        public int playerIndex;
        public string playerName;
        public bool isCPU;
        public int funds;
        public List<Unit> ownedUnits;
        public List<Building> ownedBuildings;
        public Unit selectedUnit;

        /// <summary>
        /// Available teams
        /// </summary>
        public enum TeamColor
        {
            RED,
            GREEN,
            BLUE,
            YELLOW,
            NEUTRAL
        }
        public TeamColor teamColor;

        public PlayerData()
        {
            playerName = "New Player";
            ownedBuildings = new List<Building>();
            ownedUnits = new List<Unit>();
        }
    }
}