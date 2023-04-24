using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System;

namespace Busted
{
    public class Unit : Entity
    {


        public static int zDepth = -1;
        public static int maxHealth = 100;

        public enum UnitType
        {
            Infantry,
            Tank,
        }
        public UnitType unitType;
        public Animator animator;
        public GameObject unitPrefab;

        public Unit[] neighbors;
        public List<Unit> attackableUnits;
        public GameObject targetSprite;
        public Unit damageTarget;
        [Header("Stats")]
        [SerializeField]
        private string _unitName;
        [SerializeField] private int _health = maxHealth;
        [SerializeField] private int _firepower;
        [SerializeField] private int _defense;
        [SerializeField] private int _moveRange;
        [SerializeField] private int _addsMoveCost;
        [SerializeField] private int _attackRange;
        [SerializeField] private int _ammo;
        [SerializeField] private int _fuel;
        [SerializeField] private Rank _rank;
        [Header("")]
        [Header("Firepower vs.")]
        [SerializeField]
        private int _vsInfantry;
        [SerializeField] private int _vsTank;
        [Space(5)]

        [SerializeField]
        private Player _ownedBy;

        private UnitState _unitState;

        private Sprite _spr;
        private SpriteRenderer _rend;
        [SerializeField]private UnitTargetGraphic unitTargetGraphic;

        private static IPathfinding _pathFinder = new AStarPathfinding();

        #region stats
        public string UnitName { get { return _unitName; } set { _unitName = value; } }
        public int Health { get { return _health; } set { _health = value; } }
        public int Firepower { get { return _firepower; } set { _firepower = value; } }
        public int Defense { get { return _defense; } set { _defense = value; } }
        public int MoveRange { get { return _moveRange; } set { _moveRange = value; } }
        public int AddsMoveCost { get { return _addsMoveCost; } set { _addsMoveCost = value; } }
        public int AttackRange { get { return _attackRange; } set { _attackRange = value; } }
        public int Ammo { get { return _ammo; } set { _ammo = value; } }
        public int Fuel { get { return _fuel; } set { _fuel = value; } }
        public Rank Rank { get { return _rank; } set { _rank = value; } }
        #endregion
        public float currentBaseDamage;
        #region Firepower Vs
        public int VsInfantry { get { return _vsInfantry; } set { _vsInfantry = value; } }
        public int VsTank { get { return _vsTank; } set { _vsTank = value; } }
        #endregion

        public float moveSpeed;

        public string stateName;
        public bool isMoving = false;
        public bool turnSpent = false;
        public bool markedAttackable = false;

        public Player OwnedBy
        {
            get { return _ownedBy; }
            set { _ownedBy = value; }
        }

        public UnitState UnitState
        {
            get { return _unitState; }
            set
            {
                if (_unitState != null)
                {
                    _unitState.OnStateExit();
                    _unitState = value;
                    stateName = value.stateName;
                    _unitState.OnStateEnter();
                }
                else
                {
                    _unitState = value;
                    stateName = value.stateName;
                }
            }
        }
        public List<Tile> movableTiles;


        public Sprite Spr { get; set; }

        public Tile onTile;

        public Sprite[] sprites;

        public Unit()
        {
            Health = maxHealth;

        }

        protected override void Start()
        {
            base.Start();
            if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "editor")
            {
                UnitState = new UnitStateEditor(this);
            }
            else UnitState = new UnitStateIdle(this);
            gameObject.transform.SetParent(GRID.Map.unitsHolder.transform);
            _rend = GetComponent<SpriteRenderer>();
            neighbors = new Unit[4];
            UpdateOnTile();
        }

        public void Update()
        {       //LEAVE IT ALONE FOR GOD SAKES
            xCord = (int)transform.position.x;
            yCord = (int)transform.position.y;
        }

        #region Combat

        public virtual void Attack(Unit target)
        {

        }

        public virtual void Defend(Unit attacker)
        {

        }


        #endregion

        public void MarkAsSpent()
        {
            _rend.color = new Color(0.5f, 0.5f, 0.5f, 1f);
            markedAttackable = false;
        }

        public void MarkAsNormal()
        {
            _rend.color = new Color(1, 1, 1, 1);
            markedAttackable = false;
            if (unitTargetGraphic.gameObject.activeInHierarchy) unitTargetGraphic.gameObject.SetActive(false);
        }

        public void MarkAsAttackable()
        {
            _rend.color = new Color(1f, 0.25f, 0.25f, 0.75f);
            if (targetSprite != null) Destroy(targetSprite);
            markedAttackable = true;
        }

        public void MarkAsTarget()
        {
            Debug.Log("Marking target");
            unitTargetGraphic.gameObject.SetActive(true);
        }
        /// <summary>
        /// Get neighboring units
        /// </summary>
        public void GetNeighbors()
        {
            neighbors[0] = Physics2D.Raycast(transform.position, Vector2.left).transform.GetComponent<Unit>();
            neighbors[1] = Physics2D.Raycast(transform.position, Vector2.up).transform.GetComponent<Unit>();
            neighbors[2] = Physics2D.Raycast(transform.position, Vector2.right).transform.GetComponent<Unit>();
            neighbors[3] = Physics2D.Raycast(transform.position, Vector2.down).transform.GetComponent<Unit>();
        }

        public void GetAttackable()
        {
            attackableUnits = new List<Unit>();
            foreach (Unit u in GRID.Map.unitList)
            {
                if (onTile.GetDistance(u.onTile) <= AttackRange && u != this && u.teamColor != teamColor)
                {
                    attackableUnits.Add(u);
                }
            }

        }
        /// <summary>
        /// Set this unit's base damage based on an enemy unit type
        /// </summary>
        /// <param name="unitType"></param>
        public virtual float SetDamageTarget(UnitType unitType)
        {
            return 0;
        }

        void UpdateOnTile()
        {
            onTile = GRID.Map.tiles[(int)transform.position.x, (int)transform.position.y].GetComponent<Tile>();
            GRID.Map.tiles[(int)transform.position.x, (int)transform.position.y].GetComponent<Tile>().containsUnit = this;
            onTile.isOccupied = true;
            //onTile.UpdateContainingUnit(this);
        }
        public void UpdateOnTileLate()
        {
            onTile = GRID.Map.tiles[xCord, yCord].GetComponent<Tile>();
            onTile.containsUnit = this;
            onTile.isOccupied = true;
        }

        public override void OnSelect() //Nothing is using this
        {
            print("selected unit");
            //UnitState.OnUnitSelected(this);
            GRID.GameController.GameControllerState.OnUnitSelected(this);
        }

        /// <summary>
        /// Move to given tile
        /// </summary>
        public void MoveTo(Tile target, List<Tile> path)
        {
            if (isMoving)
                return;

            Vector2 previous = onTile.transform.position;
            var totalMovementCost = path.Sum(h => h.currentMoveCost);
            var moveCounter = MoveRange;
            if (moveCounter < totalMovementCost)
                return;

            moveCounter -= totalMovementCost;

            if (moveSpeed > 0)
                StartCoroutine(MovementAnimation(path, previous));
            else
                transform.position = onTile.transform.position;

            //if (UnitMoved != null)
            //    UnitMoved.Invoke(this, new MovementEventArgs(Cell, destinationCell, path));
        }

        public IEnumerator MovementAnimation(List<Tile> path, Vector2 previous)
        {
            isMoving = true;
            path.Reverse();
            //StartCoroutine(WaitForMove(previous));
            foreach (var cell in path)
            {
                while (new Vector2(transform.position.x, transform.position.y) != new Vector2(cell.transform.position.x, cell.transform.position.y))
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(cell.transform.position.x, cell.transform.position.y, transform.position.z), Time.deltaTime * moveSpeed);
                    yield return null;
                }
            }
            isMoving = false;
            UnitState = new UnitStateAction(this, previous, false);
        }

        /// <summary>
        /// Wait for the movement to complete, then enter UnitStateAction
        /// </summary>
        /// <returns></returns>
        public IEnumerator WaitForMove(Vector2 previous)
        {
            while (isMoving == true)
            {
                yield return null;
            }
            UnitState = new UnitStateAction(this, previous, false);

        }


        protected virtual void ChangeUnitState(UnitState unitState)
        {
            UnitState = unitState;
        }

        public void BecomeType(UnitType t)
        {
            switch (t)
            {
                case UnitType.Infantry:
                    unitType = UnitType.Infantry;
                    break;
            }
        }

        /// <summary>
        /// Spawn a new unit.
        /// </summary>
        /// <param name="type">Type of unit</param>
        /// <param name="position"></param>
        public static GameObject CreateUnit(UnitType type, Vector2 position)
        {
            GameObject uType = null;
            switch (type)
            {
                case UnitType.Infantry:
                    uType = GRID.Prefabs.Infantry;
                    break;
                case UnitType.Tank:
                    uType = GRID.Prefabs.Tank;
                    break;
            }
            if (uType != null)
            {
                GameObject newUnit = Instantiate(uType, new Vector3(position.x, position.y, -1f), Quaternion.identity);
                newUnit.transform.SetParent(GRID.Map.unitsHolder.transform);
                Unit newUnitComp = newUnit.GetComponent<Unit>();
                //newUnitComp.teamColor = Player.TeamColor.BLUE; //TEMPORARY? no, more like default ;; nevermind that
                newUnitComp.xCord = (int)position.x;
                newUnitComp.yCord = (int)position.y;
                newUnit.name = newUnitComp.UnitName + " " + newUnitComp.xCord + "," + newUnitComp.yCord;
                newUnitComp.unitType = type;
                GRID.Map.unitList.Add(newUnitComp);
                return newUnit;
            }
            else return null;
        }

        public static void DestroyUnit(Unit target)
        {
            if (GRID.Map.cursor.onTile.containsUnit == target) GRID.Map.cursor.onTile.containsUnit = null;
            GRID.Map.unitList.Remove(target);
            Destroy(target.gameObject);
        }

        public bool IsUnitAttackableFrom(Unit other, Tile source)
        {
            if (source.GetDistance(other.onTile) <= AttackRange)
                return true;

            return false;
        }
        #region Pathfinding
        ///<summary>
        /// Method indicates if unit is capable of moving to cell given as parameter.
        /// </summary>
        public virtual bool IsTileMovableTo(Tile tile)
        {
            if (tile.passableByGround == true && tile.isOccupied == false)
            {
                return true;
            }
            else return false;
        }
        /// <summary>
        /// Method indicates if unit is capable of moving through cell given as parameter.
        /// </summary>
        public virtual bool IsCellTraversable(Tile tile)
        {
            if (tile.passableByGround == true && tile.isOccupied == false)
            {
                return true;
            }
            else if (tile.passableByGround == true && tile.isOccupied == true)
            {
                if (tile.containsUnit.teamColor == GRID.GameController.playerTurn.playerData.teamColor)
                {
                    return true;
                }
                else return false;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Method returns all cells that the unit is capable of moving to.
        /// </summary>
        public List<Tile> GetAvailableDestinations(List<Tile> tiles)
        {
            var ret = new List<Tile>();
            //Find all tiles that are not occupied by a unit and less than move range distance from current tile
            var cellsInMovementRange = tiles.FindAll(c => IsTileMovableTo(c) && c.GetDistance(onTile) <= _moveRange).ToList();
            var traversableCells = tiles.FindAll(c => IsCellTraversable(c) && c.GetDistance(onTile) <= _moveRange).ToList();
            //traversableCells.Add(onTile);
            cellsInMovementRange.Add(onTile);
            Debug.Log(cellsInMovementRange.Count);

            foreach (var cellInRange in cellsInMovementRange)
            {
                if (cellInRange.Equals(onTile)) continue;

                var path = FindPath(traversableCells, cellInRange);
                var pathCost = path.Sum(c => c.currentMoveCost);

                if (pathCost > 0 && pathCost <= MoveRange)
                    ret.AddRange(path);
            }
            //Myadditions
            List<Tile> friendlies = new List<Tile>();
            foreach (var t in traversableCells)
            {
                if (t.containsUnit != null)
                {
                    if (t.containsUnit.teamColor == GRID.GameController.playerTurn.playerData.teamColor)
                    {
                        friendlies.Add(t);
                    }
                }
            }
            //ret.AddRange(friendlies);
            return ret.FindAll(IsTileMovableTo).Distinct().ToList();

        }


        /// <summary>
        /// Grab a chunk of map around selected unit, for faster pathfinding
        /// </summary>
        /// <returns></returns>
        public List<Tile> GetMapChunk(Unit unit)
        {
            Map map = GRID.Map;
            int dimension = 10;
            List<Tile> chunk = new List<Tile>();
            Tile t = unit.onTile;
            int startX = (int)t.OffsetCoord.x - (dimension / 2);
            int startY = (int)t.OffsetCoord.y - (dimension / 2);
            int endX = (int)t.OffsetCoord.x + (dimension / 2);
            int endY = (int)t.OffsetCoord.y + (dimension / 2);

            for (int x = startX; x < endX; x++)//Grid loop
            {
                for (int y = startY; y < endY; y++)
                {
                    // map.tiles[x, y].GetComponent<Tile>().MarkAsReachable();
                    int xx = x;
                    int yy = y;
                    xx = Mathf.Clamp(xx, 0, map.width - 1); //Keep loop within map bounds, to avoid array errors
                    yy = Mathf.Clamp(yy, 0, map.height - 1);
                    Tile tt = map.tiles[xx, yy].GetComponent<Tile>();
                    chunk.Add(tt);
                }
            }
            return chunk;
        }

        public List<Tile> GetMapChunk(Unit unit, float multiplier)
        {
            Map map = GRID.Map;
            int dimension = 10;
            List<Tile> chunk = new List<Tile>();
            Tile t = unit.onTile;
            int startX = Mathf.RoundToInt(t.OffsetCoord.x - (dimension / 2) * multiplier);
            int startY = Mathf.RoundToInt(t.OffsetCoord.y - (dimension / 2) * multiplier);
            int endX = Mathf.RoundToInt(t.OffsetCoord.x + (dimension / 2) * multiplier);
            int endY = Mathf.RoundToInt(t.OffsetCoord.y + (dimension / 2) * multiplier);

            for (int x = startX; x < endX; x++)//Grid loop
            {
                for (int y = startY; y < endY; y++)
                {
                    // map.tiles[x, y].GetComponent<Tile>().MarkAsReachable();
                    int xx = x;
                    int yy = y;
                    xx = Mathf.Clamp(xx, 0, map.width - 1); //Keep loop within map bounds, to avoid array errors
                    yy = Mathf.Clamp(yy, 0, map.height - 1);
                    Tile tt = map.tiles[xx, yy].GetComponent<Tile>();
                    chunk.Add(tt);
                    tt.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            return chunk;
        }
        public List<Tile> GetMapChunk(Tile tile, Vector2 dimension)
        {
            Map map = GRID.Map;
            //float count = dimension.x + dimension.y;
            List<Tile> chunk = new List<Tile>();
            Tile t = tile;
            int startX = Mathf.RoundToInt(t.OffsetCoord.x - (dimension.x / 2));
            int startY = Mathf.RoundToInt(t.OffsetCoord.y - (dimension.y / 2));
            int endX = Mathf.RoundToInt(t.OffsetCoord.x + (dimension.x / 2));
            int endY = Mathf.RoundToInt(t.OffsetCoord.y + (dimension.y / 2));

            for (int x = startX; x < endX; x++)//Grid loop
            {
                for (int y = startY; y < endY; y++)
                {
                    // map.tiles[x, y].GetComponent<Tile>().MarkAsReachable();
                    int xx = x;
                    int yy = y;
                    xx = Mathf.Clamp(xx, 0, map.width - 1); //Keep loop within map bounds, to avoid array errors
                    yy = Mathf.Clamp(yy, 0, map.height - 1);
                    Tile tt = map.tiles[xx, yy].GetComponent<Tile>();
                    chunk.Add(tt);
                    //tt.GetComponent<SpriteRenderer>().color = Color.red;
                }
            }
            return chunk;
        }

        public List<Tile> FindPath(List<Tile> tiles, Tile destination)
        {
            return _pathFinder.FindPath(GetGraphEdges(tiles), onTile, destination);
        }
        /// <summary>
        /// Method returns graph representation of cell grid for pathfinding.
        /// </summary>
        protected virtual Dictionary<Tile, Dictionary<Tile, int>> GetGraphEdges(List<Tile> tiles)
        {
            Dictionary<Tile, Dictionary<Tile, int>> ret = new Dictionary<Tile, Dictionary<Tile, int>>();
            foreach (var tile in tiles)
            {
                if (IsCellTraversable(tile) || tile.Equals(onTile))
                {
                    ret[tile] = new Dictionary<Tile, int>();
                    foreach (var neighbour in tile.GetNeighbours(tiles).FindAll(IsCellTraversable))
                    {
                        ret[tile][neighbour] = neighbour.currentMoveCost;
                    }
                }
            }
            return ret;
        }
        #endregion
    }

}

