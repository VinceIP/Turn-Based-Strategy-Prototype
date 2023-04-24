using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Busted
{
    /// <summary>
    /// Calculate all damage with this
    /// </summary>
    public class BattleCalculator
    {

        public void AttackUnit(Unit attacker, Unit defender)
        {
            float outgoing;
            float incoming;
            CalculateDamage(attacker, defender, out outgoing, out incoming);
            //Debug.Log("dealt: " + outgoing);
            //Debug.Log("Recieved: " + incoming);

            defender.Health -= (int)outgoing;
            attacker.Health -= (int)incoming;
        }


        public static void CalculateDamage(Unit attacker, Unit defender, out float outgoing, out float incoming)
        {
            float attackerBaseATK = attacker.currentBaseDamage;
            float attackerBaseDamage = attacker.SetDamageTarget(defender.unitType);
            float defenderBaseATK = 80;
            float attackerBaseDEF = 80;
            float defenderBaseDamage = defender.SetDamageTarget(attacker.unitType);
            float attackerTerrainDEF = attacker.onTile.terrainDefense;
            float defenderBaseDEF = 80;
            float luck = 0;
            //defenderBaseDamage = 100 - (attackerBaseDamage - 10);

            luck = Random.Range(-0.10f, 1.10f) * attacker.Health;
            attackerBaseDEF = attackerBaseDEF + (((attackerTerrainDEF / 100) * 5));//Add 5% def bonus for each terrain defense point
            defenderBaseDEF = defenderBaseDEF + (((attackerTerrainDEF / 100) * 5));
            //Debug.Log("Attacker defense: " + attackerBaseDEF);
            //Debug.Log("Defender defense: " + defenderBaseDEF);
            float damage = (((attackerBaseATK * attackerBaseDamage) / 10) * ((attacker.Health) / 11) * (attackerBaseATK / (defenderBaseDEF * defender.Health / 100))) / 100;
            damage += luck / 10;
            damage = Mathf.CeilToInt(damage);
            //Debug.Log(damage);
            float incomingDamage = (((defenderBaseATK * defenderBaseDamage) / 10) * (defender.Health / 11) - damage) * (defenderBaseATK / (attackerBaseDEF * attacker.Health / 100)) / 100;
            //incomingDamage = incomingDamage - (incomingDamage * counterFactor);
            luck = Random.Range(-0.10f, 1.10f) * defender.Health;
            incomingDamage += luck / 10;
            //Debug.Log(incomingDamage);
            outgoing = damage;
            incoming = Mathf.CeilToInt(incomingDamage);
            if (incoming < 0) incoming = 0;
        }

    }
}


