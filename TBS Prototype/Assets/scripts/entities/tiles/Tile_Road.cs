using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Busted
{
    public class Tile_Road : Tile
    {
        Tile_Road()
        {
            //m_defaultSprite = GRID.Autotiles.Road[0];
            tileType = Tile.TileType.ROAD;
            passableByGround = true;
        }
    }
}