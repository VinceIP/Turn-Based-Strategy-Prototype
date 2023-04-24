using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Busted.Data;
/*
namespace Busted
{
    public class Map_Old : MonoBehaviour
    {

        public static Map instance = null;       //Singleton reference
        [SerializeField] public GameObject[,] tiles;
        //[SerializeField]public GameObject[,]     units;
        public GameObject entitiesHolder;
        public GameObject tilesHolder;
        public GameObject unitsHolder;
        public GameObject buildingsHolder;
        //public Structure[] 	structure;
        /// <summary>
        /// List of all tiles, used by pathfinder
        /// </summary>
        public List<Tile> tileList;
        public List<Unit> unitList;
        public List<Building> buildingList;
        public List<Tile>[] chunks; //Break the map into 10x10 chunks


        public int width;
        public int height;
        public int tileCount;
        public int unitCount;
        public string mapName;
        public string mapDescription;
        public string tempMapName;
        public string recoverableMapName;

        public bool hasUnsavedChanges;

        public bool isCampaign;      //Is this a campaign map?

        public Vector3 centerTile;

        public delegate void OnMapLoaded();
        public static OnMapLoaded onMapLoaded;

        public int tilesCount;
        public string test;


        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != null)
            {
                Destroy(gameObject);
            }

            CreateHolders();


        }

        public void OnPainted()
        {
            hasUnsavedChanges = true;
        }

        public void OnSaved()
        {
            hasUnsavedChanges = false;
        }

        public void CreateHolders()
        {
            entitiesHolder = Instantiate(PrefabController.instance.empty);
            tilesHolder = Instantiate(PrefabController.instance.empty);
            unitsHolder = Instantiate(PrefabController.instance.empty);
            buildingsHolder = Instantiate(PrefabController.instance.empty);
            entitiesHolder.name = "Entities";
            tilesHolder.name = "Tiles";
            unitsHolder.name = "Units";
            buildingsHolder.name = "Buildings";
            entitiesHolder.transform.SetParent(transform);
            tilesHolder.transform.SetParent(entitiesHolder.transform);
            unitsHolder.transform.SetParent(entitiesHolder.transform);
            buildingsHolder.transform.SetParent(entitiesHolder.transform);
        }

        public static Map InitializeMap(int w, int h)
        {
            if (GRID.Map != null) DestroyObject(GRID.Map);
            Map map = Instantiate(PrefabController.instance.map).GetComponent<Map>();
            //Clear out any previous tiles
            if (map.tiles != null) KillTiles(map);
            map.width = w;
            map.height = h;
            map.tiles = new GameObject[w, h];
            map.tileList = new List<Tile>();
            map.unitList = new List<Unit>();
            return map;
        }
        /// <summary>
        /// Create a new map
        /// </summary>
        /// <param name="w"></param>
        /// <param name="h"></param>
        /// <param name="fillType"></param>
        public static void CreateMap(int w, int h, Tile.TileType fillType)
        {
            Map map = Map.InitializeMap(w,h);
            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    map.tiles[x, y] = Tile.CreateTile(new Vector2(x, y), fillType).gameObject;
                    map.tileList.Add(map.tiles[x, y].GetComponent<Tile>());
                }
            }

            foreach (Tile tile in map.tileList)
            {
                tile.OnMapLoaded();
                //tile.GetComponent<Tile>().autotile.AutotileUpdate();
            }

            if (GRID.Map.cursor != null) Curs_Old.ResetCursorPosition();
            else CreateCursor();

            GRID.Map.centerTile = new Vector3(GRID.Map.width / 2, GRID.Map.height / 2, -1f); //Calculate the middle tile
            EditorCamScroll.CenterCameraOnMap();


        }

        public static void CreateCursor()
        {
            Instantiate(PrefabController.instance.curs);
        }
        public static void KillTiles(Map map) //Destroy all tiles on the map
        {
            foreach (Transform g in map.entitiesHolder.transform)
            {
                Destroy(g.GetComponent<Autotile>());
                Destroy(g.GetComponent<Tile>());
                Destroy(g.gameObject);
            }
            Destroy(GRID.Map.entitiesHolder);
            map.tiles = null;
            map.tileList.Clear();
            map.CreateHolders();
        }

        public static void PlayTest()
        {
            GRID.Map.tempMapName = UnityEngine.Random.Range(10000, 90000).ToString();
            GRID.Map.recoverableMapName = GRID.Map.mapName;
            GRID.Map.mapName = GRID.Map.tempMapName;
            MapSaveLoad.Save();
            SceneManager.LoadScene("game");
            Debug.Log("hellooo!");
        }


        public static void GenerateMapFromFile(MapSerial savedMap)
        {
            if (GRID.Map == null) { CreateMap(0, 0, Tile.TileType.Grass); }
            Map.KillTiles(GRID.Map);
            //Debug.Log(Map.instance.mapName);
            GRID.Map.width = savedMap.width;
            GRID.Map.height = savedMap.height;
            GRID.Map.mapName = savedMap.mapName;
            GRID.Map.tiles = new GameObject[GRID.Map.width, GRID.Map.height];
            GRID.Map.tileList = new List<Tile>();
            GRID.Map.unitList = new List<Unit>();

            //Generate tiles
            for (int x = 0; x < GRID.Map.width; x++)
            {
                for (int y = 0; y < GRID.Map.height; y++)
                {

                    //Tiles
                    if (savedMap.tiles[x, y] == 0) //Grass?
                    {
                        GRID.Map.tiles[x, y] = Tile.CreateTile(new Vector2(x, y), Tile.TileType.Grass).gameObject;
                    }
                    else if (savedMap.tiles[x, y] == 1) //Water?
                    {
                        GRID.Map.tiles[x, y] = Tile.CreateTile(new Vector2(x, y), Tile.TileType.Water).gameObject;
                    }

                    GRID.Map.tileList.Add(GRID.Map.tiles[x, y].GetComponent<Tile>());

                    //Units
                    if (savedMap.units[x, y] != null)
                    {
                        Unit newUnit = null;
                        string type = savedMap.units[x, y].Substring(0, 3);
                        if (type == "Inf")
                        {
                            newUnit = Unit.CreateUnit(Unit.UnitType.Infantry, new Vector2(x, y)).GetComponent<Unit>();
                        }
                        else if (type == "Tan")
                        {
                            newUnit = Unit.CreateUnit(Unit.UnitType.Tank, new Vector2(x, y)).GetComponent<Unit>();
                        }
                        if (newUnit != null)
                        {
                            string team = savedMap.units[x, y].Substring(3, 1);
                            if (team == "B")
                            {
                                newUnit.teamColor = Player.TeamColor.BLUE;
                            }
                            else if (team == "R")
                            {
                                newUnit.teamColor = Player.TeamColor.RED;
                            }
                            else if (team == "Y")
                            {
                                newUnit.teamColor = Player.TeamColor.YELLOW;
                            }
                            else if (team == "G")
                            {
                                newUnit.teamColor = Player.TeamColor.GREEN;
                            }
                        }
                    }

                    //Buildings
                    if (savedMap.buildings != null && savedMap.buildings[x, y] != null)
                    {
                        Building newBuilding = null;
                        string type = savedMap.buildings[x, y].Substring(0, 3);
                        string team = savedMap.buildings[x, y].Substring(3, 1);
                        if (type == "Cit")
                        {
                            newBuilding = Building.CreateBuilding(Building.BuildingType.City, new Vector2(x, y), Player.TeamColor.NEUTRAL);
                        }
                        if (newBuilding != null)
                        {
                            team = savedMap.buildings[x, y].Substring(3, 1);
                            if (team == "B")
                            {
                                newBuilding.teamColor = Player.TeamColor.BLUE;
                            }
                            else if (team == "R")
                            {
                                newBuilding.teamColor = Player.TeamColor.RED;
                            }
                            else if (team == "Y")
                            {
                                newBuilding.teamColor = Player.TeamColor.YELLOW;
                            }
                            else if (team == "G")
                            {
                                newBuilding.teamColor = Player.TeamColor.GREEN;
                            }
                            else if (team == "N")
                            {
                                newBuilding.teamColor = Player.TeamColor.NEUTRAL;
                            }
                        }
                    }

                }
            }

            foreach (GameObject tile in GRID.Map.tiles)
            {
                //tile.GetComponent<Tile>().autotile.AutotileUpdate();
                //tile.GetComponent<Tile>().autotile.AutotileUpdateNeighbors();
            }

            if (GRID.Map.cursor != null) Curs_Old.ResetCursorPosition();
            else CreateCursor();
            EditorCamScroll.CenterCameraOnMap();
        }
        /// <summary>
        /// Convert the in-game map to a new class that can be serialized and saved to disk
        /// </summary>
        /// 

        public static void GenerateMapFromFile_New(SavedMap savedMap)
        {
            if (GRID.Map == null) { CreateMap(0, 0, Tile.TileType.Grass); }
            Map.KillTiles(GRID.Map);
            //Debug.Log(Map.instance.mapName);
            GRID.Map.width = savedMap.width;
            GRID.Map.height = savedMap.height;
            GRID.Map.mapName = savedMap.mapName;
            GRID.Map.tiles = new GameObject[GRID.Map.width, GRID.Map.height];
            GRID.Map.tileList = new List<Tile>();
            GRID.Map.unitList = new List<Unit>();

            //Generate tiles
            for (int x = 0; x < GRID.Map.width; x++)
            {
                for (int y = 0; y < GRID.Map.height; y++)
                {
                    //Tiles
                    Tile newTile = Tile.CreateTile(new Vector2(x, y), savedMap.tiles[x, y].tileType);
                    GRID.Map.tiles[x, y] = newTile.gameObject;
                    GRID.Map.tileList.Add(newTile);

                    //Units
                    if (savedMap.tiles[x,y].containsUnit != null)
                    {   //If a unit exists on this tile, create it, then set its data
                        Unit newUnit = Unit.CreateUnit(savedMap.tiles[x, y].containsUnit.unitType, new Vector2(x, y)).GetComponent<Unit>();
                        newUnit.teamColor = savedMap.tiles[x, y].containsUnit.teamColor;
                        newUnit.Health = savedMap.tiles[x, y].containsUnit.health;
                        newUnit.Ammo = savedMap.tiles[x, y].containsUnit.ammo;
                        newUnit.Fuel = savedMap.tiles[x, y].containsUnit.fuel;
                        newUnit.Rank = savedMap.tiles[x, y].containsUnit.rank;
                    }

                    //Buildings
                    if(savedMap.tiles[x,y].containsBuilding != null)
                    {   //If a building exists on this tile, create it, then set its data
                        Building newBuilding = Building.CreateBuilding(savedMap.tiles[x, y].containsBuilding.buildingType, new Vector2(x, y), savedMap.tiles[x, y].containsBuilding.teamColor);
                    }

                }
            }

            onMapLoaded();

            if (GRID.Map.cursor != null) Curs_Old.ResetCursorPosition();
            else CreateCursor();
            EditorCamScroll.CenterCameraOnMap();
        }


        public static MapSerial SerializeMap()
        {
            MapSerial serializedMap = new MapSerial(GRID.Map);

            //Serialize tiles
            for (int x = 0; x < GRID.Map.width; x++)
            {
                for (int y = 0; y < GRID.Map.height; y++)
                {
                    Tile t = GRID.Map.tiles[x, y].GetComponent<Tile>();
                    //Tiles
                    if (t.tileType == Tile.TileType.Grass)
                    {
                        serializedMap.tiles[x, y] = 0;
                    }
                    else if (t.tileType == Tile.TileType.Water)
                    {
                        serializedMap.tiles[x, y] = 1;
                    }
                }
            }
            #region Units
            foreach (Transform unit in GRID.Map.unitsHolder.transform)
            {
                Unit u = unit.GetComponent<Unit>();
                string val = "";
                switch (u.unitType)
                {
                    case Unit.UnitType.Infantry:
                        val = val.Insert(val.Length, "Inf");
                        break;
                    case Unit.UnitType.Tank:
                        val = val.Insert(val.Length, "Tan");
                        break;
                }
                switch (u.teamColor)
                {
                    case Player.TeamColor.BLUE:
                        val = val.Insert(val.Length, "B");
                        break;
                    case Player.TeamColor.RED:
                        val = val.Insert(val.Length, "R");
                        break;
                    case Player.TeamColor.YELLOW:
                        val = val.Insert(val.Length, "Y");
                        break;
                    case Player.TeamColor.GREEN:
                        val = val.Insert(val.Length, "G");
                        break;
                }

                serializedMap.units[u.xCord, u.yCord] = val;
            }
            #endregion
            #region Buildings
            foreach (Transform building in GRID.Map.buildingsHolder.transform)
            {
                string val = "";
                Building b = building.GetComponent<Building>();
                switch (b.buildingType)
                {
                    case Building.BuildingType.City:
                        val = val.Insert(val.Length, "Cit");
                        break;
                }

                switch (b.teamColor)
                {
                    case Player.TeamColor.BLUE:
                        val = val.Insert(val.Length, "B");
                        break;
                    case Player.TeamColor.RED:
                        val = val.Insert(val.Length, "R");
                        break;
                    case Player.TeamColor.YELLOW:
                        val = val.Insert(val.Length, "Y");
                        break;
                    case Player.TeamColor.GREEN:
                        val = val.Insert(val.Length, "G");
                        break;
                    case Player.TeamColor.NEUTRAL:
                        val = val.Insert(val.Length, "N");
                        break;
                }
                serializedMap.buildings[b.xCord, b.yCord] = val;
            }
            #endregion
            return serializedMap;
        }

    }
    /// <summary>
    /// Serialized version of Map
    /// </summary>
    [Serializable]
    public class MapSerial
    {
        /*
         * Tiles
        0 = grass
        1 = water
        */

        /*
         * Units
         * 0 = blank
         * 1 = Infantry
         * 2 = Tank
         */

        /*
         * Buildings
         * 
   /*
    * TeamColor
    * 0 - blue
    * 1 - red
    * 2 - yellow
    * 3 - green 
    */

    /*
        public int[,] tiles;
        public string[,] units;
        public string[,] buildings;

        public int width;
        public int height;
        public string mapName;
        public string mapDescription;

        public bool isCampaign;

        public MapSerial() { }
        public MapSerial(Map map)
        {
            width = map.width;
            height = map.height;
            tiles = new int[width, height];
            units = new string[width, height];
            buildings = new string[width, height];
            mapName = map.mapName;
            mapDescription = map.mapDescription;
            isCampaign = map.isCampaign;

        }
    }
}
*/
