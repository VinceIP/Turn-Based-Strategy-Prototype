using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Busted.AI
{
    public class PlayerDumbAI : Player
    {
        private bool _attackDone = false;
        private Coroutine _currentCoroutine;


        public List<Unit> enemyUnits;

        public PlayerDumbAI()
        {
            playerData.playerName = "Dumbass AI";
        }
        void Start()
        {
            //Debug.Log("AI: Joining the party");
        }

        void GetAttackable()
        {

        }
        public override void Play()
        {
            _currentCoroutine = StartCoroutine(AIPlay());
        }

        public IEnumerator AIAttack(Unit attacker, Unit target)
        {
            StopCoroutine(_currentCoroutine); //Stops the play coroutine when an attack begins
            attacker.UnitState = new UnitStateAction(attacker, target);
            target.MarkAsAttackable();
            attacker.UnitState.Attack();
            GRID.GameController.GameControllerState = new GameControllerState_UnitSelectingTarget(attacker, false);
            yield return new WaitForSeconds(0.4f);
            GRID.GameController.GameControllerState.OnUnitSelected(target);
            attacker.MarkAsSpent();
            attacker.turnSpent = true;
            attacker.GetAttackable();
            yield return new WaitForSeconds(0.4f);
            yield return null;
            SortUnits();
            Play(); //Restart play. This is necessary to re-evaluate the game after an attack occurs and units are potentially destroyed
        }

        public List<Unit> SortUnits()
        {
            List<Unit> newOwnedUnits = new List<Unit>();
            playerData.ownedUnits = new List<Unit>();
            foreach (Unit u in GRID.Map.unitList)
            {
                if (u.teamColor == playerData.teamColor)
                {
                    playerData.ownedUnits.Add(u);
                }
            }
            newOwnedUnits = playerData.ownedUnits.OrderBy(u => u.unitType.ToString()).ToList();
            playerData.ownedUnits = newOwnedUnits;
            return playerData.ownedUnits;
        }

        void AISelectUnit(Unit unit)
        {
            unit.UnitState = new UnitStateSelected(unit);
        }

        void AIDeselectUnit()
        {
            Debug.Log("De selecting: " + playerData.selectedUnit.name);
            playerData.selectedUnit.UnitState = new UnitStateIdle(playerData.selectedUnit);
            playerData.selectedUnit.turnSpent = true;
            playerData.selectedUnit = null;
        }

        void AIMoveUnit(Unit unit, Tile destination)
        {
            unit.UnitState = new UnitStateMoving(unit, destination);
        }

        public IEnumerator AIPlay()
        {
            Debug.Log("AI: Taking my turn now.");
            //GRID.GameController.GameControllerState = new GameControllerState_AITurn();
            SortUnits();
            enemyUnits = new List<Unit>();
            enemyUnits.AddRange(GRID.Map.unitList.FindAll(u => u.teamColor != playerData.teamColor)); //Find all enemy units
                                                                                               //Debug.Log("AI: I count " + enemyUnits.Count + " enemies.");
            List<Unit> attackableEnemies = new List<Unit>();
            foreach (Unit u in playerData.ownedUnits)
            {
                if (u.turnSpent == false)
                {
                    u.GetAttackable(); //Get attackable units within range
                    if (u.attackableUnits.Count > 0) attackableEnemies.AddRange(u.attackableUnits);
                }
            }
            //Debug.Log("AI: Looks like I have " + attackableEnemies.Count + " possible attacks.");
            //List<Unit> sortedOwnedUnits = new List<Unit>();

            foreach (Unit u in SortUnits())
            {
                u.GetAttackable();
                if (u.turnSpent == false)
                {
                    playerData.selectedUnit = u;
                    AISelectUnit(playerData.selectedUnit);
                    yield return new WaitForSeconds(0.05f);

                    //If my units can attack anything
                    if (u.attackableUnits.Count > 0)
                    {
                        Unit target = u.attackableUnits[Random.Range(0, u.attackableUnits.Count - 1)];
                        _attackDone = false;
                        StartCoroutine(AIAttack(u, target));
                        yield return new WaitUntil(() => u.turnSpent == true);
                    }
                    //If there are no attackable units right now
                    else
                    {
                        List<Tile> potential = new List<Tile>();
                        //Find all tiles I my units can attack from
                        foreach (Unit uu in enemyUnits)
                        {
                            potential.AddRange(GRID.Map.tileList.FindAll(t => u.IsTileMovableTo(t) && u.IsUnitAttackableFrom(uu, t) && uu.teamColor != playerData.teamColor));
                            yield return null;

                        }


                        if (potential.Count > 0) //If we found a tile to attack from
                        {
                            List<Tile> potentialPathValid = new List<Tile>(); //Attackable tiles that selected unit can path to, that are within move range
                                                                              //Loop through each tile in potential, get each one that the selected unit has a path to
                            foreach (Tile t in potential)
                            {
                                List<Tile> testPath = new List<Tile>();
                                testPath = playerData.selectedUnit.FindPath(GRID.Map.tileList, t);
                                //Debug.Log("testpath count: " + testPath.Count);
                                if (testPath.Count <= playerData.selectedUnit.MoveRange)
                                {
                                    potentialPathValid.Add(t);
                                }
                                yield return null;
                            }
                            Tile moveHere = null;
                            if (potentialPathValid.Count > 0)
                            {
                                Debug.Log(potentialPathValid.Count);
                                potentialPathValid = potentialPathValid.OrderBy(t => t.GetDistance(u.onTile)).ToList();
                                moveHere = potentialPathValid[0];
                            }

                            //Tile moveHere = potentialPathValid.FindAll(t => t.GetDistance(u.onTile) <= u.MoveRange); //Pick one of the found destinations within move range
                            //Tile moveHere = potential.Find(t => t.GetDistance(u.onTile) <= u.MoveRange); //Pick one of the found destinations within move range
                            if (moveHere != null) //If we found a tile in move range
                            {

                                Debug.Log(playerData.selectedUnit.name + " found a tile in moverange to attack from");
                                moveHere.MarkAsReachable();
                                yield return new WaitForSeconds(0.4f);
                                moveHere.UnMark();

                                //List<Tile> newPath = u.FindPath(Map.instance.tileList, moveHere);
                                potential.Remove(moveHere);
                                //u.MoveTo(moveHere, newPath);
                                AIMoveUnit(playerData.selectedUnit, moveHere);
                                yield return new WaitUntil(() => u.isMoving == false);
                                u.GetAttackable();
                                yield return new WaitUntil(() => u.attackableUnits.Count > 0);
                                if (u.attackableUnits.Count > 0)
                                {
                                    Unit target = u.attackableUnits[Random.Range(0, u.attackableUnits.Count)];
                                    StartCoroutine(AIAttack(u, target));
                                    yield return new WaitUntil(() => u.turnSpent == true);
                                }
                                yield return null;
                            }
                            else if (moveHere == null) //If we couldnt find a tile to attack from that is in move range
                            { //Move as far as possible towards one
                                List<Tile> tilesInRange = new List<Tile>();
                                List<Tile> newPotential = new List<Tile>();
                                List<Unit> sortedEnemies = new List<Unit>();
                                Unit potentialTarget;
                                tilesInRange = u.GetAvailableDestinations(GRID.Map.tileList);
                                foreach (Tile t in tilesInRange)
                                {
                                    //t.MarkAsReachable();
                                    if (t.GetDistance(u.onTile) == u.MoveRange) //If this tile is at maximum move range
                                    {
                                        newPotential.Add(t); //Add it to potential, because we want to move as far as possible
                                    }
                                }
                                sortedEnemies = enemyUnits.OrderByDescending(uu => uu.onTile.GetDistance(u.onTile)).ToList(); //Get the closest enemy unit
                                potentialTarget = sortedEnemies[sortedEnemies.Count - 1];
                                //Debug.Log("Marking closest enemy unit: " + potentialTarget.xCord + "," + potentialTarget.yCord);
                                //potentialTarget.MarkAsAttackable();
                                //foreach (Tile t in tilesInRange) t.UnMark();
                                //yield return new WaitForSeconds(0.5f);
                                //foreach (Tile t in newPotential) t.MarkAsReachable();
                                //yield return new WaitForSeconds(0.5f);
                                //foreach (Tile t in newPotential) t.UnMark();
                                //int dist = 0;
                                //newPotential = newPotential.OrderByDescending(tt => )
                                newPotential = newPotential.OrderByDescending(tt => tt.GetDistance(potentialTarget.onTile)).ToList();
                                Tile targetTile = newPotential[newPotential.Count - 1];
                                //Debug.Log("Marking target tile");
                                targetTile.MarkAsReachable();
                                yield return new WaitForSeconds(0.5f);
                                targetTile.UnMark();
                                AIMoveUnit(playerData.selectedUnit, targetTile);
                                //u.MoveTo(targetTile, u.FindPath(u.GetAvailableDestinations(Map.instance.tileList), targetTile));
                                //u.MoveTo(targetTile, u.FindPath(Map.instance.tileList, targetTile));
                                yield return new WaitUntil(() => u.isMoving == false);
                                yield return new WaitForSeconds(0.15f);
                                playerData.selectedUnit.GetAttackable();
                                if (playerData.selectedUnit.attackableUnits.Count > 0)
                                {
                                    //Pick target with lowest health
                                    Unit target;
                                    playerData.selectedUnit.attackableUnits = playerData.selectedUnit.attackableUnits.OrderByDescending(uu => u.Health).ToList();
                                    target = playerData.selectedUnit.attackableUnits[playerData.selectedUnit.attackableUnits.Count - 1];
                                    _currentCoroutine = StartCoroutine(AIAttack(playerData.selectedUnit, target));
                                    yield return new WaitUntil(() => u.turnSpent == true);
                                }
                                u.UnitState.Wait();
                                //potentialTarget.MarkAsNormal();


                            }

                        }
                        //Debug
                        foreach (Tile t in potential)
                        {
                            t.UnMark();
                        }

                    }
                }
                yield return null;

            }
            Debug.Log("AI: Uhhh.... That's all for now");
            GRID.GameController.GameControllerState.EndTurn();
            yield return null;
        }
    }

}
