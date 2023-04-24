using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Busted;

namespace Busted.Data
{

    [Serializable]
    public class SavedMap
    {
        public string mapName;
        public string mapDescription;

        public int width;
        public int height;

        public SavedTile[,] tiles;
    }
    [Serializable]
    public class SavedPlayer
    {
        public PlayerData.TeamColor teamColor;
    }

    [Serializable]
    public class SavedTile
    {
        public Tile.TileType tileType;
        public SavedUnit containsUnit;
        public SavedBuilding containsBuilding;

        public int xCord;
        public int yCord;

        public bool passableByGround;
        public bool passableByAir;
        public bool isOccupied;

        public int currentMoveCost;
        public int defaultMoveCost;
        public int terrainDefense;

        public SavedTile()
        {
            containsUnit = null;
            containsBuilding = null;
        }
    }

    [Serializable]
    public class SavedEntity
    {
        public int xCord;
        public int yCord;
        public PlayerData.TeamColor teamColor;
    }


    [Serializable]
    public class SavedUnit : SavedEntity
    {
        public Unit.UnitType unitType;

        public int health;
        public int ammo;
        public int fuel;
        public Rank rank;
    }

    [Serializable]
    public class SavedBuilding : SavedEntity
    {
        public Building.BuildingType buildingType;
        public string buildingName;
        public int defenseBonus;
        public int attackBonus;
        public int recovery;
        public int fundsPerTurn;
        public SavedPlayer ownedBy;
        public SavedUnit containsUnit;
    }
}