using UnityEngine;
using System.Collections;
using System.Collections.Generic;
/*
namespace Busted
{
    [System.Serializable]
    public class Tile_Old : MonoBehaviour, IGraphNode
    {
        public Sprite spriteAngles;
        public GameObject tilePrefab;

        public enum TileType
        {
            Grass,
            Water,
            Road,
            Forest,
            Mountain,
            Blank
        }
        public TileType tileType;
        public Sprite[] spr;

        //public Tile[] neighbors;
        public Neighbors_Old neighbors;
        public Tile blankTile;
        /// <summary>The unit on this tile - null if empty
        ///</summary>
        public Unit containsUnit;
        public Building containsBuilding;

        public int xCord;
        public int yCord;
        public int overlayCount = 0;
        //public string tileName;

        public bool passableByGround;
        public bool passableByAir;
        /// <summary> True if tile is occupied by a unit </summary>
        public bool isOccupied;

        public int currentMoveCost;
        public int defaultMoveCost;
        public int terrainDefense;

        public Autotile autotile;

        public SpriteRenderer spriteRend;
        public SpriteRenderer marker;
        [SerializeField] private Vector2 _offsetCoord = new Vector2();
        public Vector2 OffsetCoord { get { return _offsetCoord; } set { _offsetCoord = value; } }


        void Awake()
        {
            //Load sprite angles
            //[x,0] = Center
            //[x,1] = TL
            //[x,2] = T
            //[x,3] = TR
            //[x,4] = L
            //[x,5] = R
            //[x,6] = BL
            //[x,7] = B
            //[x,8] = BR
            //[0,x] = water to grass

            //spriteAngles[0, 0] = Resources.Load<Sprite>("sprites/sprWater_C");
            //spriteAngles[0, 1] = Resources.Load<Sprite>("sprites/sprWater_TL");

            //spriteAngles = Resources.Load<Sprite>("bgTiles");
            //Debug.Log(spriteAngles);

            autotile = GetComponent<Autotile>();
         }

        void Start()
        {
            gameObject.transform.SetParent(GRID.Map.tilesHolder.transform);
            gameObject.name = tileType.ToString() + " " + xCord + ", " + yCord;
            currentMoveCost = defaultMoveCost;
        }

        void Update()
        {
            if (tileType == TileType.Water)
            {
                passableByGround = false;
            }
            else
            {
                passableByGround = true;
                //moveCost = 1;
            }
        }

        public void OnMouseDown()
        {
            if (GRID.GameController.sceneState == GameController.SceneState.Game && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                GRID.GameController.GameControllerState.OnTileSelected(this);
                if (GRID.GameController.playerTurn.selectedUnit != null)
                {
                    //GRID.GameController.playerTurn.selectedUnit.UnitState.OnTileSelected(this);
                }
            }

        }

        public void UpdateContainingUnit(Unit unit)
        {
            containsUnit = unit;
        }

        public void SetTileType(TileType type)
        {
            //Debug.Log ("changing tile type to " + type);
            spriteRend = GetComponent<SpriteRenderer>();
            switch (type)
            {
                case TileType.Grass:
                    tileType = TileType.Grass;
                    spriteRend.sprite = spr[0];
                    break;
                case TileType.Water:
                    tileType = TileType.Water;
                    spriteRend.sprite = spr[1];
                    break;
                case TileType.Road:
                    tileType = TileType.Road;
                    spriteRend.sprite = spr[2];
                    break;

            }
        }

        public void KillOverlays()
        {
            if (overlayCount > 0)
            {
                /*
                for (int i = 0; i < overlayCount; i++)
                {
                    //Debug.Log("KILLING A CHILD, ITERATION: " + i);
                    GameObject killMe = gameObject.transform.Find("overlay").gameObject;
                    if (killMe == null) Debug.Log("overlay is null");
                    Destroy(killMe);
                    overlayCount--;
                }
                
                foreach (Transform child in transform)
                {
                    if (child.name == "overlay")
                    {
                        Destroy(child.gameObject);
                        overlayCount--;
                    }
                }
            }

        }

        public void CalculateTileIndex()
        {

        }

        public void OnMapLoaded()
        {
            FindNeighbors();
        }

        /*
        public void AutoTileUpdate()
        {
            FindNeighbors();
            KillOverlays();
            #region //WATER
            //Water
            if (tileType == TileType.Water)
            {
                //If on map edges
                //If on the left edge
                if (neighbors[0] == null)
                {
                    if (neighbors[2].tileType == TileType.Grass)
                    {
                        if (neighbors[1] == null) //Top left edge
                        {
                            if (neighbors[3].tileType == TileType.Grass && neighbors[2].tileType == TileType.Grass) //Single top left
                            {
                                OverlayTile(8, true);
                            }
                            else if (neighbors[3].tileType == TileType.Water)
                            {
                                OverlayTile(5, true);
                                if (neighbors[2].tileType == TileType.Water)
                                {
                                    OverlayTile(16, true);
                                }
                            }
                        }
                        else if (neighbors[1].tileType == TileType.Grass && neighbors[3].tileType == TileType.Grass)
                        {
                            OverlayTile(21, false); //Single left edge
                        }

                        else if (neighbors[1].tileType == TileType.Grass && neighbors[3].tileType == TileType.Water) //Top right, water below
                        {
                            OverlayTile(3, true);
                        }
                        else if (neighbors[3].tileType == TileType.Grass && neighbors[1].tileType == TileType.Water)//Bottom right, water above
                        {
                            OverlayTile(8, true);
                        }
                        else if (neighbors[1].tileType == TileType.Water && neighbors[3].tileType == TileType.Water) //Right, water above and below, on left edge
                        {
                            OverlayTile(5, true);
                        }

                    }
                    else if (neighbors[2].tileType == TileType.Water) //Straight river from left edge
                    {
                        OverlayTile(2, false);
                        OverlayTile(7, false);
                    }

                }
                //If not on the edge of the map

                if (neighbors[0] != null &&
                         neighbors[1] != null &&
                         neighbors[2] != null &&
                         neighbors[3] != null)
                {
                    //Single
                    if (neighbors[0].tileType == TileType.Grass && neighbors[2].tileType == TileType.Grass &&
                            neighbors[1].tileType == TileType.Grass && neighbors[3].tileType == TileType.Grass)
                    {
                        OverlayTile(17, true);
                    }

                    //Center
                    else if (neighbors[0].tileType == TileType.Water &&
                    neighbors[1].tileType == TileType.Water &&
                    neighbors[2].tileType == TileType.Water &&
                    neighbors[3].tileType == TileType.Water)
                    {
                        //Small corner bottom-right
                        if (neighbors[7].tileType == TileType.Grass)
                        {
                            OverlayTile(13, false);
                        }

                        //Small corner bottom-left
                        if (neighbors[6].tileType == TileType.Grass)
                        {
                            OverlayTile(14, false);
                        }
                        //Small corner top-right
                        if (neighbors[5].tileType == TileType.Grass)
                        {
                            OverlayTile(15, false);
                        }
                        //Small Corner top-left
                        if (neighbors[4].tileType == TileType.Grass)
                        {
                            OverlayTile(16, false);
                        }

                        //Water surrounded by water on 8 sides
                        else
                        {
                            if (transform.childCount > 0 && neighbors[7].tileType != TileType.Grass &&
                              neighbors[6].tileType != TileType.Grass && neighbors[5].tileType != TileType.Grass &&
                              neighbors[4].tileType != TileType.Grass)
                            {
                                KillOverlays();
                            }
                        }
                    }

                    //Top-left conditions
                    else if (neighbors[0].tileType == TileType.Grass &&
                       neighbors[1].tileType == TileType.Grass)
                    {
                        OverlayTile(1);
                        //Corner river check
                        if (neighbors[7].tileType == TileType.Grass)
                        {
                            OverlayTile(13, false);
                            //River left check
                            if (neighbors[3].tileType == TileType.Grass)
                            {
                                OverlayTile(20, true);
                            }

                            //River up check
                            else if (neighbors[2].tileType == TileType.Grass)
                            {
                                OverlayTile(18, true);
                            }
                        }

                    }

                    //Top
                    else if (neighbors[0].tileType == TileType.Water &&
                       neighbors[2].tileType == TileType.Water &&
                       neighbors[1].tileType == TileType.Grass)
                    {
                        OverlayTile(2);
                        //River check horizontal
                        if (neighbors[3].tileType == TileType.Grass)
                        {
                            OverlayTile(7, false);
                        }
                        //T-junction check
                        else if (neighbors[6].tileType == TileType.Grass)
                        {
                            OverlayTile(14, false);
                        }
                        if (neighbors[7].tileType == TileType.Grass)
                        {
                            OverlayTile(13, false);
                        }
                    }

                    //Top-right
                    else if (neighbors[2].tileType == TileType.Grass &&
                        neighbors[1].tileType == TileType.Grass)
                    {
                        OverlayTile(3);
                        //
                        if (neighbors[6].tileType == TileType.Grass)
                        {
                            OverlayTile(14, false);

                        }
                        if (neighbors[5].tileType == TileType.Grass)
                        {
                            OverlayTile(15, false);
                        }

                        //River right check
                        if (neighbors[3].tileType == TileType.Grass)
                        {
                            OverlayTile(21, true);
                        }



                    }

                    //Left
                    else if (neighbors[1].tileType == TileType.Water &&
                        neighbors[3].tileType == TileType.Water &&
                        neighbors[0].tileType == TileType.Grass)
                    {
                        OverlayTile(4);
                        //River check vertical
                        if (neighbors[2].tileType == TileType.Grass)
                        {
                            OverlayTile(5, false);
                        }
                    }

                    //Right
                    else if (neighbors[1].tileType == TileType.Water &&
                        neighbors[3].tileType == TileType.Water &&
                        neighbors[2].tileType == TileType.Grass)
                    {
                        OverlayTile(5);
                    }

                    //Bottom-left
                    else if (neighbors[0].tileType == TileType.Grass &&
                        neighbors[3].tileType == TileType.Grass)
                    {
                        OverlayTile(6);

                        //Small corner bottom-left and top-right
                        if (neighbors[6].tileType == TileType.Grass)
                        {
                            OverlayTile(14, false);
                        }
                        if (neighbors[5].tileType == TileType.Grass)
                        {
                            OverlayTile(15, false);
                        }

                        //River down check
                        if (neighbors[2].tileType == TileType.Grass)
                        {
                            OverlayTile(19, true);
                        }
                    }

                    //Bottom
                    else if (neighbors[0].tileType == TileType.Water &&
                        neighbors[2].tileType == TileType.Water &&
                        neighbors[3].tileType == TileType.Grass)
                    {
                        OverlayTile(7);
                    }

                    //Bottom-right
                    else if (neighbors[3].tileType == TileType.Grass &&
                        neighbors[2].tileType == TileType.Grass)
                    {
                        OverlayTile(8, true);
                        //River check bottom-right
                        if (neighbors[4].tileType == TileType.Grass)
                        {
                            OverlayTile(16, false);
                        }

                    }

                }


            }
            #endregion
        }
        

        public void OverlayTile(int angle, bool erase = true)
        {
            //Destroy any existing overlays, to prevent overlapping shit
            //Run this if erase is true, dont if erase is false to allow multiple overlays where neccessary
            if (erase == true)
            {
                KillOverlays();
                /*
                foreach (Transform child in transform)
                {
                    if (child.name == "overlay")
                    {
                        Destroy(child.gameObject);
                    }
                }
                
            }
            if (overlayCount >= 4)
            {
                Debug.Log("Cancelling overlay creation, because 4 overlays already exist.");
                return;
            }
            GameObject overlay = new GameObject();
            overlay.name = "overlay";

            overlay.AddComponent<SpriteRenderer>();

            SpriteRenderer spr = overlay.GetComponent<SpriteRenderer>();
            Transform t = overlay.GetComponent<Transform>();
            t.SetParent(transform);
            t.position = transform.position - (Vector3.forward * 2.1f);

            spr.sprite = TileLoader.instance.grassToWater[angle];

            /*
            switch (angle)
            {
                case 0:
                    spr.sprite = TileLoader.instance.grassToWater[0];
                    break;
                case 1:
                    Debug.Log("changing to TL sprite");
                    spr.sprite = TileLoader.instance.grassToWater[1];
                    break;
            }
            

            spr.sortingLayerName = "Tiles";
            overlayCount++;
        }

        public void FindNeighbors()
        {
            Map m = GRID.Map;
            //[0,1,2,3] LEFT, UP, RIGHT, DOWN
            
            //Left neighbor
            if (xCord - 1 >= 0) //If not on left edge
            {
                neighbors.left = m.tiles[(int)xCord - 1, (int)yCord].GetComponent<Tile>();
            }
            else if (xCord - 1 < 0) //If on the left edge
            {
                //Debug.Log("im on the left");
                neighbors.left = null;
            }

            //Right neighbor
            if (xCord + 1 < m.width) //If not on the right edge
            {
                neighbors.right = m.tiles[xCord + 1, yCord].GetComponent<Tile>();
            }
            else//If on the right edge
            {
                neighbors.right = null;
            }

            //Up neighbor
            if (yCord + 1 <= m.height - 1) //If not on the top edge
            {
                neighbors.up = m.tiles[(int)xCord, (int)yCord + 1].GetComponent<Tile>();
            }
            else if (yCord + 1 > m.height - 1) //If on the top edge
            {
                neighbors.up = null;
            }

            //Down neighbor
            if (yCord - 1 >= 0)
            {
                neighbors.down = m.tiles[(int)xCord, (int)yCord - 1].GetComponent<Tile>();
            }
            else if (yCord - 1 < 0)
            {
                neighbors.down = null;
            }
            /*
            //Top-left
            if (xCord - 1 >= 0 && yCord + 1 <= Map.instance.height - 1)
            {
                neighbors[4] = Map.instance.tiles[(int)xCord - 1, (int)yCord + 1].GetComponent<Tile>();
            }
            else if (xCord - 1 < 0 && yCord + 1 > Map.instance.height + 1)
            {
                neighbors[4] = null;
            }

            //Top-right
            if (xCord + 1 <= Map.instance.width - 1 && yCord + 1 <= Map.instance.height - 1)
            {
                neighbors[5] = Map.instance.tiles[(int)xCord + 1, (int)yCord + 1].GetComponent<Tile>();
            }
            else if (xCord + 1 > Map.instance.width - 1 && yCord + 1 > Map.instance.height - 1)
            {
                neighbors[5] = null;
            }

            //Bottom-left
            if (xCord - 1 >= 0 && yCord - 1 >= 0)
            {
                neighbors[6] = Map.instance.tiles[(int)xCord - 1, (int)yCord - 1].GetComponent<Tile>();
            }
            else if (xCord - 1 < 0 && yCord - 1 < 0)
            {
                neighbors[6] = null;
            }

            //Bottom-right
            if (xCord + 1 <= Map.instance.width - 1 && yCord - 1 >= 0)
            {
                neighbors[7] = Map.instance.tiles[(int)xCord + 1, (int)yCord - 1].GetComponent<Tile>();
            }
            else if (xCord + 1 > Map.instance.width - 1 && yCord - 1 < 0)
            {
                neighbors[7] = null;
            }
            


        }

        public void MarkAsReachable()
        {
            marker.gameObject.SetActive(true);
            marker.color = new Color(0.25f, 0.25f, 1, marker.color.a);
            //SpriteRenderer rend = GetComponent<SpriteRenderer>();
            //rend.color = Color.blue;
        }
        public void UnMark()
        {
            marker.gameObject.SetActive(false);
        }

        public static Tile CreateTile(Vector2 pos, Tile.TileType newType)
        {
            GameObject newTile = Instantiate(PrefabController.instance.tile, new Vector3(pos.x, pos.y, 0f), Quaternion.identity) as GameObject;
            Tile newTileComp = newTile.GetComponent<Tile>();
            newTileComp.SetTileType(newType);
            newTileComp.xCord = (int)pos.x;
            newTileComp.yCord = (int)pos.y;
            newTileComp.OffsetCoord = new Vector2(pos.x, pos.y);
            newTile.AddComponent<Autotile>();
            return newTileComp;
        }

        #region PathFinding
        protected static readonly Vector2[] _directions =
    {
        new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1)
    };

        public int GetDistance(Tile other)
        {
            return (int)(Mathf.Abs(OffsetCoord.x - other.OffsetCoord.x) + Mathf.Abs(OffsetCoord.y - other.OffsetCoord.y));
        }//Distance is given using Manhattan Norm.
        public List<Tile> GetNeighbours(List<Tile> cells)
        {
            List<Tile> ret = new List<Tile>();
            foreach (var direction in _directions)
            {
                var neighbour = cells.Find(c => c.OffsetCoord == OffsetCoord + direction);
                if (neighbour == null) continue;

                ret.Add(neighbour);
            }
            return ret;
        }

        public int GetDistance(IGraphNode other)
        {
            return GetDistance(other as Tile);
        }
        #endregion



    }

    [System.Serializable]
    public class Neighbors_Old
    {
        public Tile up;
        public Tile right;
        public Tile down;
        public Tile left;
    }
}
*/