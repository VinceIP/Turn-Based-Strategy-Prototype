using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

namespace Busted
{
    public class Tile : MonoBehaviour, IGraphNode
    {
        public GameObject tilePrefab;
        public static GameObject TilePrefab;

        public Map map;

        public enum TileType
        {
            GRASS, WATER, ROAD, RIVER, FOREST, MOUNTAIN
        }

        public TileType tileType;
        public Neighbors neighbors;
        public string N_UP, N_DOWN, N_LEFT, N_RIGHT;

        public Unit containsUnit;
        public Building containsBuilding;

        public float terrainDefense;
        public bool passableByGround;
        public bool isOccupied = false;
        public int currentMoveCost;

        public int xCord, yCord;

        protected Sprite m_defaultSprite;
        public SpriteRenderer marker;
        private Autotile m_autotile;
        private SpriteRenderer m_rend;

        public delegate void UpdateOnTile(Tile t);
        public static UpdateOnTile updateOnTile;

        [SerializeField] private Vector2 _offsetCoord = new Vector2();
        public Vector2 OffsetCoord 
        {
            get
            {
                _offsetCoord = new Vector2(xCord, yCord);
                return _offsetCoord;
            }
            set { _offsetCoord = value; }
        }

        public static Tile CreateTile(Vector2 coords, TileType tileType)
        {

            GameObject t = Instantiate(GRID.Prefabs.Tile);
            //print(t.name + " instantiated");
            //print(GRID.Map.tilesHolder);
            t.transform.SetParent(GRID.Map.tilesHolder.transform);
            Tile tile = null;
            switch (tileType)
            {
                case TileType.GRASS:
                    tile = t.AddComponent<Tile_Grass>();
                    break;
                case TileType.WATER:
                    tile = t.AddComponent<Tile_Water>();
                    break;
                case TileType.ROAD:
                    tile = t.AddComponent<Tile_Road>();
                    break;
                case TileType.RIVER:
                    tile = t.AddComponent<Tile_River>();
                    break;
            }
            tile.xCord = (int)coords.x;
            tile.yCord = (int)coords.y;
            tile.Init();
            return tile;
        }

        public void Init()
        {
            marker = transform.GetChild(0).GetComponent<SpriteRenderer>();
            name = tileType.ToString() + " " + xCord + ", " + yCord;
            m_rend = GetComponent<SpriteRenderer>();
            neighbors = new Neighbors();
            BoxCollider2D c = gameObject.GetComponent<BoxCollider2D>();
            c.size = new Vector2(1, 1);
            m_autotile = gameObject.AddComponent<Autotile>();
            m_autotile.Init();

        }

        void Start()
        {

        }
        void OnEnable()
        {
            //print(name + " onEnable");
            Map.onMapLoaded += OnMapLoaded;
        }
        void OnDisable()
        {
            Map.onMapLoaded -= OnMapLoaded;
        }

        void OnMouseOver()
        {
            if(GRID.Map != null && GRID.Map.isLoaded)
            {
                updateOnTile(this);

            }
        }

        void OnMouseDown()
        {
            if(GRID.GameController.sceneState == GameController.GameControllerSceneState.Game &&
                !EventSystem.current.IsPointerOverGameObject())
            {
                GRID.GameController.GameControllerState.OnTileSelected(this);
            }

        }

        public void OnMapLoaded()
        {
            map = GRID.Map;
            transform.position = new Vector2(xCord, yCord);
            transform.SetParent(map.tilesHolder.transform);
            FindNeighbors();
            //m_autotile.AutotileUpdate();
        }

        public void FindNeighbors()
        {
            if (map == null) map = GRID.Map;
            //print(name + " finding neighbors");
            neighbors.left = null;
            neighbors.up = null;
            neighbors.right = null;
            neighbors.down = null;
            int w = map.tiles.GetLength(0)-1;
            int h = map.tiles.GetLength(1)-1;
            if (xCord - 1 >= 0) neighbors.left = map.tiles[xCord - 1, yCord];
            if (xCord + 1 <= w) neighbors.right = map.tiles[xCord + 1, yCord];
            if (yCord - 1 >= 0) neighbors.down = map.tiles[xCord, yCord - 1];
            if (yCord + 1 <= h) neighbors.up = map.tiles[xCord, yCord + 1];

            if (neighbors.left != null) N_LEFT = neighbors.left.name;
            if (neighbors.up != null) N_UP = neighbors.up.name;
            if (neighbors.right != null) N_RIGHT = neighbors.right.name;
            if (neighbors.down != null) N_DOWN = neighbors.down.name;

        }

        public void UnMark()
        {
            marker.gameObject.SetActive(false);
        }
        public void MarkAsReachable()
        {
            marker.gameObject.SetActive(true);
            marker.color = new Color(0.25f, 0.25f, 1, 1);
            //SpriteRenderer rend = GetComponent<SpriteRenderer>();
            //rend.color = Color.blue;
        }

        #region PathFinding
        protected static readonly Vector2[] _directions =
    {
        new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, -1)
    };
        //internal bool isOccupied;
        //internal int currentMoveCost;
        //internal bool passableByGround;

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




    public class Neighbors
    {
        public Tile up;
        public Tile right;
        public Tile down;
        public Tile left;
    }
}