using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Busted
{
    public class Tile_River : Tile
    {
        Tile_River()
        {
            m_defaultSprite = GRID.Autotiles.River[0];
            tileType = Tile.TileType.RIVER;
            passableByGround = false;
        }
    }
}