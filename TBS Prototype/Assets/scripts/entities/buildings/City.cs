using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Busted
{
    public class City : Building
    {
        public City()
        {
            buildingType = BuildingType.City;
            buildingName = "City";
            defenseBonus = 3;
            attackBonus = 0;
            recovery = 2;
            fundsPerTurn = 1000;
        }

    }
}
