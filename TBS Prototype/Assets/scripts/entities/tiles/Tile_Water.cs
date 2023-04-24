using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Busted
{
    public class Tile_Water : Tile
    {
        Tile_Water()
        {
            //m_defaultSprite = GRID.Autotiles.Water[0];
            tileType = Tile.TileType.WATER;
        }
    }
}