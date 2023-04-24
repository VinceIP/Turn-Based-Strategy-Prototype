using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Busted
{
    /// <summary>
    /// Represents what state the unit is in
    /// </summary>
    public abstract class UnitState
    {
        protected Unit _unit;
        protected GameController gameController = GRID.GameController;
        public string stateName;
        public List<Tile> pathsInRange;
        public Vector2 previous;

        public UnitState(Unit unit)
        {
            _unit = unit;
            stateName = "UnitState";
        }
        public virtual void OnStateEnter()
        {
        }

        public virtual void OnStateExit()
        {
        }

        public virtual void OnStateExit(UnitState goingToThis)
        {

        }

        public virtual void OnUnitSelected(Unit unit)
        {
            //GRID.GameController.GameControllerState.OnUnitSelected(unit);
        }

        public virtual void OnTileSelected(Tile tile)
        {

        }

        /// <summary>
        /// Stop coroutines, set unit as unmarked visually
        /// </summary>
        public virtual void UnMark()
        {
            _unit.StopAllCoroutines();
            _unit.GetComponent<SpriteRenderer>().color = Color.white;
        }

        public virtual void Attack()
        {

        }

        public virtual void Attack(Unit target, Unit aiTarget)
        {

        }

        public virtual void BeingAttacked(Unit unit)
        {
            Debug.Log(unit.gameObject.name + ": HELP! IM BEING ATTACKED!");
        }

        public virtual void Wait()
        {

        }

        public virtual void Revert()
        {

        }

        public virtual void OnPause()
        {

        }


        /// <summary>
        /// Unit glows when selected
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        protected IEnumerator Glow(Unit unit)
        {
            var rend = unit.GetComponent<SpriteRenderer>();
            bool done = false;
            bool change = false;
            bool permLoop = true;
            float factor = 0.2f;
            do
            {
                while (done == false && change == false)
                {
                    if (rend.color.a > 0.5f)
                    {
                        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, rend.color.a - factor);
                        yield return new WaitForSeconds(0.1f);
                    }
                    else
                    {
                        done = true;
                        change = true;
                    }
                }

                if (change == true)
                {
                    yield return new WaitForSeconds(1);
                    change = false;
                }

                while (done == true && change == false)
                {
                    if (rend.color.a < 1)
                    {
                        rend.color = new Color(rend.color.r, rend.color.g, rend.color.b, rend.color.a + factor);
                        yield return new WaitForSeconds(0.1f);
                    }
                    else
                    {
                        done = false;
                        change = true;
                    }
                }



                yield return null;
            }
            while (permLoop == true);
            yield return null;

        }

    }
}

