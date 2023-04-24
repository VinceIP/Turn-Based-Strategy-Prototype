using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Busted
{

    public class Autotile : MonoBehaviour
    {
        private int m_tileIndex;
        private Tile m_tile;
        private Neighbors m_neighbors;
        private SpriteRenderer m_rend;
        private AutotileSpriteContainer m_autotiles;
        public int sum = 0;

        public Tile.TileType tileType;

        void Awake()
        {

        }

        public void Init()
        {
            m_autotiles = GRID.Autotiles;
            m_tile = gameObject.GetComponent<Tile>();
            tileType = m_tile.tileType;
            m_neighbors = m_tile.neighbors;
            if (GRID.Map.isLoaded)
            {
                //print("map loaded!");
                //m_tile.FindNeighbors();
                m_neighbors = m_tile.neighbors;
                AutotileUpdate();
                //AutotileUpdateNeighbors();
            }
        }

        int CalculateTileIndex()
        {
            sum = 0;

            if ( (m_neighbors.up != null && m_neighbors.up.tileType == m_tile.tileType) || m_neighbors.up == null) sum += 1;
            if ( (m_neighbors.left != null && m_neighbors.left.tileType == m_tile.tileType) || m_neighbors.left == null) sum += 2;
            if ( (m_neighbors.down != null && m_neighbors.down.tileType == m_tile.tileType) || m_neighbors.down == null) sum += 4;
            if ( (m_neighbors.right != null && m_neighbors.right.tileType == m_tile.tileType) || m_neighbors.right == null) sum += 8;
            /*
            if (m_neighbors.up == null || m_neighbors.up.tileType != m_tile.tileType) sum += 1;
            if (m_neighbors.left == null || m_neighbors.left.tileType != m_tile.tileType) sum += 2;
            if (m_neighbors.down == null || m_neighbors.down.tileType != m_tile.tileType) sum += 4;
            if (m_neighbors.right == null || m_neighbors.right.tileType != m_tile.tileType) sum += 8;
            */
            
            return sum;
        }

        public void AutotileUpdate()
        {
            m_tile.FindNeighbors();
            m_neighbors = m_tile.neighbors;
            m_rend = gameObject.GetComponent<SpriteRenderer>();
            switch (tileType)
            {
                case Tile.TileType.WATER:
                    m_rend.sprite = m_autotiles.Water[CalculateTileIndex()];
                    break;
                case Tile.TileType.ROAD:
                    m_rend.sprite = m_autotiles.Road[CalculateTileIndex()];
                    break;
                case Tile.TileType.RIVER:
                    m_rend.sprite = m_autotiles.River[CalculateTileIndex()];
                    break;
                case Tile.TileType.GRASS:
                    m_rend.sprite = GRID.TileSprites.Grass;
                    break;
            }
        }

        public void AutotileUpdateNeighbors()
        {
            /*
            print(_neighbors.up.GetComponent<Autotile>());
            if(_neighbors.up != null && _neighbors.up.tileType == tileType) _neighbors.up.GetComponent<Autotile>().AutotileUpdate();
            if (_neighbors.down != null && _neighbors.down.tileType == tileType) _neighbors.down.GetComponent<Autotile>().AutotileUpdate();
            if (_neighbors.right != null && _neighbors.right.tileType == tileType) _neighbors.right.GetComponent<Autotile>().AutotileUpdate();
            if (_neighbors.left != null && _neighbors.left.tileType == tileType) _neighbors.left.GetComponent<Autotile>().AutotileUpdate();
            */
            if (m_neighbors.up != null) m_neighbors.up.GetComponent<Autotile>().AutotileUpdate();
            if (m_neighbors.down != null) m_neighbors.down.GetComponent<Autotile>().AutotileUpdate();
            if (m_neighbors.right != null) m_neighbors.right.GetComponent<Autotile>().AutotileUpdate();
            if (m_neighbors.left != null) m_neighbors.left.GetComponent<Autotile>().AutotileUpdate();
        }

        public int GetTileIndex()
        {
            return m_tileIndex;
        }
    }
}