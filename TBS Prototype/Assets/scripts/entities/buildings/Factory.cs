using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Busted
{
    public class Factory : Building
    {
        public Factory()
        {
            producesUnits = true;
            buildingType = BuildingType.Factory;
            buildingName = "Factory";
            defenseBonus = 3;
            attackBonus = 0;
            recovery = 2;
            fundsPerTurn = 1000;
        }

    }
}
