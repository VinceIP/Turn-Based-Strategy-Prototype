using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Busted
{
    public class HQ : Building
    {
        public HQ()
        {
            buildingType = BuildingType.HQ;
            buildingName = "HQ";
            defenseBonus = 4;
            attackBonus = 0;
            recovery = 3;
            fundsPerTurn = 1000;
        }

    }
}
