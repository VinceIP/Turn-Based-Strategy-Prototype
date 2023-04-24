using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Busted
{
    public class Infantry : Unit
    {

        public Infantry() : base()
        {
            UnitName = "Infantry";
            Health = maxHealth;
        }

        public override float SetDamageTarget(UnitType unitType)
        {
            switch (unitType)
            {
                case UnitType.Infantry:
                    currentBaseDamage = VsInfantry;
                    break;
                case UnitType.Tank:
                    currentBaseDamage = VsTank;
                    break;
            }

            return currentBaseDamage;

        }

    }

}
