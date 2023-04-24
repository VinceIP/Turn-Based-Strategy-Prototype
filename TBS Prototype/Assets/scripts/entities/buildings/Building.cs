using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Busted
{
    public class Building : Entity
    {
        public enum BuildingType
        {
            City,
            HQ,
            Factory,
            Airfield,
            Seaport
        }
        public BuildingType buildingType;
        public string buildingName;
        public bool producesUnits;
        public int defenseBonus;
        public int attackBonus;
        public int recovery;
        public int fundsPerTurn;
        public Player ownedBy = null;
        public Unit containsUnit;


        protected override void Start()
        {
            base.Start();
        }

        public static Building CreateBuilding(BuildingType type, Vector2 position, PlayerData.TeamColor teamColor)
        {
            GameObject bType = null;
            switch (type)
            {
                case BuildingType.City:
                    bType = GRID.Prefabs.City;
                    break;
                case BuildingType.HQ:
                    bType = GRID.Prefabs.HQ;
                    break;
                case BuildingType.Factory:
                    bType = GRID.Prefabs.Factory;
                    break;
            }
            GameObject newBuilding = Instantiate(bType, new Vector3(position.x, position.y, -1f), Quaternion.identity);

            //If an HQ already exists of the same color, destroy it first
            if(type == BuildingType.HQ)
            {
                HQ existingHQ = null;
                existingHQ = GRID.Map.buildingList.Find(h => h.buildingType == BuildingType.HQ && h.teamColor == teamColor) as HQ;
                if(existingHQ != null)
                {
                    GRID.Map.hqList.Remove(existingHQ);
                    DestroyBuilding(existingHQ);
                }
            }
            //

            Building building = newBuilding.GetComponent<Building>();
            //Curs.instance.onTile.containsBuilding = building;
            GRID.Map.tiles[(int)position.x, (int)position.y].GetComponent<Tile>().containsBuilding = building;
            newBuilding.transform.position = position;
            newBuilding.transform.SetParent(GRID.Map.buildingsHolder.transform);
            building.teamColor = teamColor;
            //building.UpdateTeamColorSprite();
            building.xCord = (int)newBuilding.transform.position.x;
            building.yCord = (int)newBuilding.transform.position.y;
            GRID.Map.buildingList.Add(building);
            if (building.buildingType == BuildingType.HQ) GRID.Map.hqList.Add((HQ)building);
            newBuilding.name = building.buildingName + " " + building.xCord + "," + building.yCord;
            return building;
        }

        public static void DestroyBuilding(Building target)
        {
            if (GRID.Map.cursor.onTile.containsBuilding == target) GRID.Map.cursor.onTile.containsBuilding = null;
            GRID.Map.buildingList.Remove(target);
            Destroy(target.gameObject);
        }

        public virtual void OnTurnStart()
        {
        }

        public override void OnSelect()
        {
            print("selected building");
        }

    }
}
