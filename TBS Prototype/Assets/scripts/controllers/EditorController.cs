using UnityEngine;
using System.Collections;
using Busted.UI;
namespace Busted
{
    [System.Serializable]
    public class EditorController : MonoBehaviour
    {
        //public static EditorController instance;
        public enum EditorState
        {
            Drawing,
            inMenu
        }
        public EditorState editorState;
        public enum PaintType
        {
            Terrain,
            Unit,
            Building,
            Eraser
        }

        public delegate void OnPainted();
        public static event OnPainted onPainted;

        public Tile.TileType selectedTileType;
        public Unit.UnitType selectedUnit;
        public Building.BuildingType selectedBuilding;
        public PaintType paintType;
        public EditorState editState;

        public Curs curs;
        public EditorControllerUI editorControllerUI;

        //public Map.OnMapLoaded onMapLoaded;

        public void OnEnable()
        {
            Map.onMapLoaded += OnMapLoaded;
        }

        public void OnDisable()
        {
            Map.onMapLoaded -= OnMapLoaded;
        }

        public void Awake()
        {
            //if (instance == null) instance = this;
            //else Destroy(gameObject);
        }

        public void OnMapLoaded()
        {

            curs = GRID.Map.cursor;
            GRID.Map.EditorController = this;
            GRID.AudioController.EditorController = this;
        }

        void Start()
        {
            Map.CreateMap(10, 10);
        }

        /// <summary>
        /// Paint a tile, unit, or structure; based on what is selected in EditorController
        /// </summary>
        public void Paint()
        {
            PlayerData.TeamColor selectedColor = editorControllerUI.currentTeam;
            if (editorControllerUI.pnl_blackout.activeInHierarchy == false)
            {
                if (paintType == PaintType.Terrain && curs.onTile.tileType != selectedTileType)
                {
                    //Debug.Log ("painting");
                    GameObject toDel = curs.onTile.gameObject; //Get reference to the tile the curs is on, which we will delete
                    Tile toDelComp = toDel.GetComponent<Tile>();
                    Unit newContainsUnit;
                    Building newContainsBuilding;

                    if (toDelComp.containsUnit != null)
                    {
                        newContainsUnit = toDelComp.containsUnit;
                    }
                    else { newContainsUnit = null; }

                    if(toDelComp.containsBuilding != null)
                    {
                        newContainsBuilding = toDelComp.containsBuilding;
                    }
                    else { newContainsBuilding = null; }

                    toDel.name = "delete this";
                    Transform toDelTrans = toDel.GetComponent<Transform>(); //Get the transform of toDel
                    Vector2 paintHere = new Vector2(toDel.transform.position.x, toDel.transform.position.y); //Set the location of the new tile
                    Tile toPaint = Tile.CreateTile(paintHere, selectedTileType); // Lay down the new tile
                    //GRID.Map.OnPainted(paintType);
                    Tile c = toPaint.GetComponent<Tile>();
                    c.containsUnit = newContainsUnit;
                    c.containsBuilding = newContainsBuilding;
                    Vector3 t = toPaint.GetComponent<Transform>().position;

                    GRID.Map.tiles[(int)toDelTrans.position.x, (int)toDelTrans.position.y] = toPaint; //Store the new tile into the map array
                    curs.onTile = toPaint.GetComponent<Tile>(); //Tell the curs that the new tile is what it's hovering over; this avoids errors
                    c.OnMapLoaded();
                    Autotile at = c.GetComponent<Autotile>();
                    //at.Init();
                    at.AutotileUpdateNeighbors();
                    //Destroy the old tile
                    Destroy(toDel);
                    onPainted();

                }
                else if (paintType == PaintType.Unit)
                {
                    //if a unit is already there, and is a different type than what is selected, or is a different team color
                    if (curs.onTile.containsUnit != null)
                    {//Destroy it first to replace with new unit
                        if (curs.onTile.containsUnit.unitType != selectedUnit || editorControllerUI.currentTeam != curs.onTile.containsUnit.teamColor)
                        {
                            Unit.DestroyUnit(curs.onTile.containsUnit);
                        }
                    }
                    if (curs.onTile.containsUnit == null)
                    {
                        Transform t = curs.onTile.GetComponent<Transform>();
                        Unit newUnit = Unit.CreateUnit(selectedUnit, new Vector2(t.position.x, t.position.y)).GetComponent<Unit>();
                        newUnit.teamColor = editorControllerUI.currentTeam;
                        curs.onTile.containsUnit = newUnit;
                        onPainted();
                    }



                }
                else if (paintType == PaintType.Building)
                {
                    if (curs.onTile.containsBuilding != null)
                    {//Destroy it first to replace with new unit
                        if (curs.onTile.containsBuilding.buildingType != selectedBuilding || editorControllerUI.currentTeam != curs.onTile.containsBuilding.teamColor)
                        {
                            Building.DestroyBuilding(curs.onTile.containsBuilding);
                        }
                    }
                    if (curs.onTile.containsBuilding == null)
                    {
                        Transform t = curs.onTile.GetComponent<Transform>();
                        Building newBuilding = Building.CreateBuilding(selectedBuilding, new Vector2(t.position.x, t.position.y), selectedColor);
                        //newBuilding.teamColor = GUIEditorController.instance.currentTeam;
                        curs.onTile.containsBuilding = newBuilding;
                        onPainted();
                    }
                }
                /*
                else if(paintType == PaintType.Building)
                {
                    if(selectedBuilding == Building.BuildingType.City)
                    {
                        if(Curs.instance.onTile.containsBuilding == null || Curs.instance.onTile.containsBuilding.buildingType != selectedBuilding || Curs.instance.onTile.containsBuilding.teamColor != GUIEditorController.instance.currentTeam)
                        {
                            Building.CreateBuilding(Building.BuildingType.City, Curs.instance.transform.position, selectedColor);
                        }
                        else
                        {
                            Destroy(Curs.instance.onTile.containsBuilding.gameObject);
                            Curs.instance.onTile.containsBuilding = null;
                        }
                    }
                    else if (selectedBuilding == Building.BuildingType.HQ)
                    {
                        if (Curs.instance.onTile.containsBuilding == null || Curs.instance.onTile.containsBuilding.buildingType != selectedBuilding ||
                            (Curs.instance.onTile.containsBuilding.buildingType == selectedBuilding && Curs.instance.onTile.containsBuilding.teamColor != selectedColor))
                        {
                            if (Curs.instance.onTile.containsBuilding != null) Destroy(Curs.instance.onTile.containsBuilding.gameObject);
                            Building.CreateBuilding(Building.BuildingType.HQ, Curs.instance.transform.position, selectedColor);
                        }
                    }
                }
                */
                else if (paintType == PaintType.Eraser)
                {
                    if (editorControllerUI.pnl_Units.activeInHierarchy)
                    {
                        if (curs.onTile.containsUnit) Unit.DestroyUnit(curs.onTile.containsUnit);
                    }
                    else if (editorControllerUI.pnl_Buildings.activeInHierarchy)
                    {
                        if (curs.onTile.containsBuilding) Building.DestroyBuilding(curs.onTile.containsBuilding);
                    }
                    onPainted();
                }
            }
        }



    }



    /*using UnityEngine;
    using System.Collections;

    public class EditorController : MonoBehaviour
    {

        public static int 			mapWidth;				//Default map width
        public static int 			mapHeight;				//Default map height

        public static GameObject	selectedTile;			//Tile currently selected

        public GameObject 			tile;						
        public Transform 			mapContainer;				//Contains all map tiles for clean hierarchy
        public Camera 				camera;						//Main camera reference
        public GameObject			inputController;

        public static GameObject[,] map;						//Each entry in array contains a tile


        void Awake()
        {

        }

        void Start()
        {
            if(!GameObject.Find("InputController"))
            {
                inputController = Instantiate(inputController);
            }
            CreateMap(20,20);
        }


        public void CreateMap(int width, int height)
        {
            ClearMap();

            mapWidth = width;
            mapHeight = height;
            map = new GameObject[mapWidth,mapHeight];	//Create the 2D map array, set its width and height

            //Create new map

            //Flood fill map with grass
            for(int i = 0; i < mapWidth; i++)
            {
                for(int j = 0; j < mapHeight; j++)
                {

                    Vector3 pos = new Vector3 (i,j,0f);		//Spaces each tile 1 unit apart (1 unit = 32 pixels)
                    map[i,j] = Tile.CreateTile(pos.x,pos.y,Tile.TileType.Grass);
                    //map[i,j] = (GameObject) Instantiate(tile, pos, Quaternion.identity);	//Create a tile, store it in map[i,j]+
                    //Tile thisTile = map[i,j].GetComponent<Tile>();	//Get a reference to our new tile properties
                    map[i,j].transform.SetParent(mapContainer);		//Parent each tile to container
                    //thisTile.tileType = Tile.TileType.Water;		//Set the new tile type to grass

                }
            }

            Vector3 camPos = map[mapWidth / 2, mapHeight / 2].transform.position + Vector3.back;
            camera.transform.position = camPos;		//Place the camera on the center tile;

        }


        public void ClearMap()
        {
                for(int i = 0; i < mapWidth; i ++)
                {
                    for(int j = 0; j < mapHeight; j ++)
                    {
                        GameObject.Destroy(map[i,j]);
                    }
                }
        }

        public void SetSelectedTile(Tile tile)
        {

        }




    }









    /*using UnityEngine;
    using System.Collections;

    public class EditorController : MonoBehaviour
    {

        public static int 			mapWidth;				//Default map width
        public static int 			mapHeight;				//Default map height

        public static GameObject	selectedTile;
        public GameObject			terrain_Grass;
        public GameObject			terrain_Water;

        public GameObject[] 		tiles;						//Tiles that can be selected from
        public Transform 			mapContainer;				//Contains all map tiles for clean hierarchy
        public Camera 				camera;						//Main camera reference
        public GameObject			inputController;

        public static GameObject[,] map;						//Each entry in array contains a tile


        void Awake()
        {

        }

        void Start()
        {
            if(!GameObject.Find("InputController"))
            {
                inputController = Instantiate(inputController);
            }
            CreateMap(20,20);
        }


        public void CreateMap(int width, int height)
        {
            ClearMap();

            mapWidth = width;
            mapHeight = height;
            map = new GameObject[mapWidth,mapHeight];

            //Create new map

            //Flood fill map with grass
            for(int i = 0; i < mapWidth; i++)
            {
                for(int j = 0; j < mapHeight; j++)
                {

                    Vector3 pos = new Vector3 (i,j,0f);		//Spaces each tile 1 unit apart (1 unit = 32 pixels)
                    map[i,j] = (GameObject) Instantiate(tiles[1], pos, Quaternion.identity);	//Create a tile, store it in map[i,j]+

                    map[i,j].transform.SetParent(mapContainer);		//Parent each tile to container

                }
            }

            Vector3 camPos = map[mapWidth / 2, mapHeight / 2].transform.position + Vector3.back;
            camera.transform.position = camPos;		//Place the camera on the center tile;

        }


        public void ClearMap()
        {
                for(int i = 0; i < mapWidth; i ++)
                {
                    for(int j = 0; j < mapHeight; j ++)
                    {
                        GameObject.Destroy(map[i,j]);
                    }
                }
        }

        public void SetSelectedTile(Tile tile)
        {

        }




    }
    */
}
