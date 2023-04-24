using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Busted.Data;

namespace Busted
{
    public class Map : MonoBehaviour
    {
        public Tile[,] tiles;

        public int width;
        public int height;

        public string mapName;
        public string mapDescription;

        public bool loadingSavedMap;
        public bool isLoaded;
        public bool hasUnsavedChanges;
        public bool okForPlay;          //true if map meets requirements for play

        public GameObject entitiesHolder;
        public GameObject tilesHolder;
        public GameObject unitsHolder;
        public GameObject buildingsHolder;

        public List<Unit> unitList;
        public List<Building> buildingList;
        public List<Tile> tileList;
        public List<HQ> hqList;

        public HQ hqRed;
        public HQ hqBlue;
        public HQ hqYellow;
        public HQ hqGreen;

        public Curs cursor;

        public delegate void OnMapLoaded();
        public delegate void OnMapUnloaded();
        public static OnMapLoaded onMapLoaded;
        public static OnMapUnloaded onMapUnloaded;

        private EditorController _editorController;

        void OnEnable()
        {
            EditorController.onPainted += OnPainted;
        }

        public void Start()
        {
            if (isLoaded == false)
            {
                Init();
            }

        }

        public void Init()
        {
            CreateHolders();
            InitializeValues();
            if(loadingSavedMap == false) GenerateNewMap(); //Generates a new, default map, only if we aren't about to generate a map from a saved file
        }

        void CreateHolders()
        {
            tilesHolder = new GameObject();
            unitsHolder = new GameObject();
            buildingsHolder = new GameObject();
            Instantiate(tilesHolder);
            Instantiate(unitsHolder);
            Instantiate(buildingsHolder);
            tilesHolder.name = "Tiles";
            unitsHolder.name = "Units";
            buildingsHolder.name = "Buildings";
            tilesHolder.transform.SetParent(transform);
            unitsHolder.transform.SetParent(transform);
            buildingsHolder.transform.SetParent(transform);
        }

        void InitializeValues()
        {
            if (tiles != null)
            {
                print("clearing tiles array");
                Array.Clear(tiles, 0, tiles.Length);
            }
            tiles = new Tile[width, height];
            //print(width);
            //print(height);
            print("initialized tile array with size of: " + tiles.GetLength(0) + " " + tiles.GetLength(1));
            unitList = new List<Unit>();
            buildingList = new List<Building>();
            tileList = new List<Tile>();
            hqList = new List<HQ>();
        }

        void GenerateNewMap()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Tile tile = Tile.CreateTile(new Vector2(x, y), Tile.TileType.GRASS);
                    tile.map = this;
                    tiles[x, y] = tile;
                    tileList.Add(tile);
                }
            }
            if (cursor == null) cursor = Instantiate(GRID.Prefabs.Cursor).GetComponent<Curs>();
            cursor.map = this;
            cursor.canMove = true;
            GRID.Map = this;
            GRID.Map.isLoaded = true;
            onMapLoaded();

        }

        public static Map CreateMap(int w, int h, SavedMap savedMap) //Use when loading a saved map
        {
            print("creating map");
            Map map;
            GameObject o;
            if (GRID.Map == null) //If no map exists, the game was just now loaded
            {   //Create new map
                print("No map loaded; creating new one");
                o = Instantiate(new GameObject());
                o.name = "Map";
                GRID.Map = o.AddComponent<Map>();
                //GRID.Map = o.GetComponent<Map>();
                GRID.Map.width = w;
                GRID.Map.height = h;
                GRID.Map.isLoaded = true;
                GRID.Map.loadingSavedMap = true;
                GRID.Map.Init();
                //map.InitializeValues();
                //map.CreateHolders();
            }
            else //Map is already loaded
            {
                print("CLearing out a prior map");
                map = GRID.Map;
                ClearMap(); //Clear it out
                map.width = w;
                map.height = h;
                map.isLoaded = false;
                //map.InitializeValues(); //Reinitialize data'
                map.Start();

            }
            return GRID.Map;
        }

        public static Map CreateMap(int w, int h)
        {
            print("creating map");
            Map map;
            GameObject o;
            if (GRID.Map == null) //If no map exists, the game was just now loaded
            {   //Create new map
                print("No map loaded; creating new one");
                o = Instantiate(new GameObject());
                o.name = "Map";
                GRID.Map = o.AddComponent<Map>();
                //GRID.Map = o.GetComponent<Map>();
                GRID.Map.width = w;
                GRID.Map.height = h;
                GRID.Map.isLoaded = true;
                GRID.Map.Init();
                //map.InitializeValues();
                //map.CreateHolders();
            }
            else //Map is already loaded
            {
                print("CLearing out a prior map");
                map = GRID.Map;
                ClearMap(); //Clear it out
                map.width = w;
                map.height = h;
                map.isLoaded = false;
                //map.InitializeValues(); //Reinitialize data'
                map.Start();

            }
            return GRID.Map;
        }

        public void OnPainted()
        {
            hasUnsavedChanges = true;
            if (_editorController.paintType != EditorController.PaintType.Terrain)
            {   //If a unit or building has been placed or erased, recheck if the map is playable
                CheckIfPlayable();
            }
        }

        public void OnSaved()
        {
            hasUnsavedChanges = false;
        }

        /// <summary>
        /// Evaluate the map to determine if it's ready to be played on.
        /// TBD: Still flags playable as true when an HQ is erased
        /// </summary>
        public void CheckIfPlayable()
        {
            print("checking if map is playable yet");
            //Map can only be played if these conditions are met:
            //At least 2 HQs, 1 for each team
            //At least 1 unit per team, or at least 1 unit producing building
            List<PlayerData> players = new List<PlayerData>();
            List<PlayerData> okPlayers = new List<PlayerData>(); //list of players that are capable of playing
            if (hqList.Count >= 2)
            {
                foreach (HQ hq in hqList)//For each potential player
                {
                    //Create a player for each color HQ that is placed on the map, add it to a list
                    PlayerData newPlayer = new PlayerData();
                    newPlayer.teamColor = hq.teamColor;
                    players.Add(new PlayerData());

                    foreach (Building b in buildingList)
                    {   //Assign each building to the appropriate player
                        if (b.teamColor == newPlayer.teamColor)
                        {
                            print(newPlayer);
                            newPlayer.ownedBuildings.Add(b);
                        }
                    }

                    foreach (Unit u in unitList)
                    {   //Same for units
                        if (u.teamColor == newPlayer.teamColor)
                        {
                            newPlayer.ownedUnits.Add(u);
                        }
                    }

                    List<Building> unitBuilders = new List<Building>(); //Get a list of all buildings that can make units
                    unitBuilders.AddRange(newPlayer.ownedBuildings.FindAll(b => b.producesUnits == true));

                    if (unitBuilders.Count > 0 || newPlayer.ownedUnits.Count > 0)
                    {
                        okPlayers.Add(newPlayer);
                    }
                }
                print(okPlayers.Count);
                if (okPlayers.Count == hqList.Count)
                {
                    okForPlay = true;
                }
                else okForPlay = false;

                print("okForPlay: " + okForPlay);
            }
        }

        public static void GenerateMapFromFile(SavedMap savedMap)
        {
            if (savedMap != null)
            {
                if (GRID.Map == null)
                { 
                    CreateMap(savedMap.width, savedMap.height, savedMap); 
                }
                else
                {
                    print("Clearing out an old map");
                    Map.ClearMap();
                    GRID.Map.width = savedMap.width;
                    GRID.Map.height = savedMap.height;
                    GRID.Map.mapName = savedMap.mapName;
                    GRID.Map.CreateHolders();
                    GRID.Map.InitializeValues();
                }

                //Generate tiles
                for (int x = 0; x < GRID.Map.width; x++)
                {
                    for (int y = 0; y < GRID.Map.height; y++)
                    {
                        //Tiles
                        Tile tile = Tile.CreateTile(new Vector2(x, y), savedMap.tiles[x, y].tileType);
                        tile.map = GRID.Map;
                        GRID.Map.tiles[x, y] = tile;
                        GRID.Map.tileList.Add(tile);


                        //Units
                        if (savedMap.tiles[x, y].containsUnit != null)
                        {   //If a unit exists on this tile, create it, then set its data
                            Unit newUnit = Unit.CreateUnit(savedMap.tiles[x, y].containsUnit.unitType, new Vector2(x, y)).GetComponent<Unit>();
                            newUnit.teamColor = savedMap.tiles[x, y].containsUnit.teamColor;
                            newUnit.Health = savedMap.tiles[x, y].containsUnit.health;
                            newUnit.Ammo = savedMap.tiles[x, y].containsUnit.ammo;
                            newUnit.Fuel = savedMap.tiles[x, y].containsUnit.fuel;
                            newUnit.Rank = savedMap.tiles[x, y].containsUnit.rank;
                        }

                        //Buildings
                        if (savedMap.tiles[x, y].containsBuilding != null)
                        {   //If a building exists on this tile, create it, then set its data
                            Building newBuilding = Building.CreateBuilding(savedMap.tiles[x, y].containsBuilding.buildingType, new Vector2(x, y), savedMap.tiles[x, y].containsBuilding.teamColor);
                        }

                    }
                }

                //if (GRID.Map.cursor != null) GRID.Map.cursor.JumpToCenter();
                //else CreateCursor();
                if (GRID.Map.cursor == null) CreateCursor();
                //EditorCamScroll.CenterCameraOnMap();
                GRID.Map.isLoaded = true;
                onMapLoaded();
            }
        }

        public static void CreateCursor()
        {
            GRID.Map.cursor = Instantiate(GRID.Prefabs.Cursor).GetComponent<Curs>();
        }

        public static void DestroyMap(Map map) //Destroy all tiles on the map
        {
            /*
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
            */
            Destroy(GRID.Map);

        }
        public static void DestroyMap()
        {
            Destroy(GRID.Map.gameObject);
        }

        public static void ClearMap()
        {
            onMapUnloaded();
            foreach (Tile t in GRID.Map.tiles)
            {
                Destroy(t.gameObject);
            }
            foreach (Transform u in GRID.Map.unitsHolder.transform)
            {
                Destroy(u.gameObject);
            }
            foreach (Transform b in GRID.Map.buildingsHolder.transform)
            {
                Destroy(b.gameObject);
            }
            Destroy(GRID.Map.tilesHolder);
            Destroy(GRID.Map.unitsHolder);
            Destroy(GRID.Map.buildingsHolder);
        }

        public EditorController EditorController
        {
            get{ return _editorController; }
            set{ _editorController = value; }
        }

    }
}
