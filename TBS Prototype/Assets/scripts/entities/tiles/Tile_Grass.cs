using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Busted
{
    public class Tile_Grass : Tile
    {
        Tile_Grass()
        {
            //m_defaultSprite = GRID.TileSprites.Grass;
            tileType = Tile.TileType.GRASS;
            passableByGround = true;
            isOccupied = false;
            currentMoveCost = 1;
        }
    }
}