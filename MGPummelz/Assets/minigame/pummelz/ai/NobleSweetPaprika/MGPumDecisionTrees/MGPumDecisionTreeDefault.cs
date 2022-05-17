using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumDecisionTreeDefault : MGPumDecisionTree
    {
        public MGPumDecisionTreeDefault(MGPumNobleSweetPaprikaAIPlayerController controller) : base(controller) { }
        public override MGPumCommand getDecision(MGPumUnit unit)
        {
            Debug.Log("getDecision of Default for: " + unit.name);
            // If he can attack, he (in most cases) should

            if (this.stateOracle.canAttack(unit) && unit.currentRange >= 1)
            {
                List<MGPumAttackCommand> attackCommands = getAllAttackCommands(unit);
                // Debug.Log("Number of Attacks: " + attackCommands.Count);
                if (attackCommands.Count > 0)
                {
                    List<MGPumUnit> enemyUnits = new List<MGPumUnit>();
                    List<MGPumUnit> killableUnits = new List<MGPumUnit>();
                    attackCommands.ForEach(attackCommand => enemyUnits.Add(attackCommand.chain.getLast().getUnit(this.state)));
                    Dictionary<MGPumUnit, MGPumAttackCommand> unitAttacks = new Dictionary<MGPumUnit, MGPumAttackCommand>();
                    for (int index = 0; index < enemyUnits.Count; index++)
                    {
                        // Debug.Log("Unit in range: " + enemyUnits[index].name);
                        unitAttacks.Add(enemyUnits[index], attackCommands[index]);
                        if ((enemyUnits[index].currentHealth - unit.currentPower) <= 0)
                        {
                            killableUnits.Add(enemyUnits[index]);
                            // Debug.Log("Oneshot unit in range: " + enemyUnits[index].name);
                        }

                    }
                    // Debug.Log("Number of oneshot Units: " + killableUnits.Count);
                    MGPumAttackCommand attackCommand;
                    if (killableUnits.Count > 0)
                        enemyUnits = killableUnits;
                    else
                        enemyUnits = getPreferableUnitsToAttack(unit, enemyUnits);

                    enemyUnits.Add(null);
                    enemyUnits.Sort(new MGPumUnitComparer(this));
                    if (enemyUnits[0] != null)
                        return unitAttacks[enemyUnits[0]];
                }
            }

            // If he can't attack, maybe he should move MoveCommand more suitable for snipers
            if (this.stateOracle.canMove(unit) && unit.currentSpeed >= 1)
            {
                List<MGPumMoveCommand> moveCommands = getAllMoveCommands(unit);
                if (moveCommands.Count == 0)
                    return null;
                // If nothing happend for 20+ moves, move directly to enemy
                if (state.turnNumber >= state.lastChangeTurnNumber + 20)
                {
                    MGPumUnit closestEnemy = null;
                    int closestDistance = int.MaxValue;
                    List<MGPumUnit> enemyUnits = this.controller.GetState().getAllUnitsInZone(MGPumZoneType.Battlegrounds, 1 - this.controller.playerID);
                    foreach (MGPumUnit enemyUnit in enemyUnits)
                    {
                        int distance = getAbsoluteDistance(enemyUnit.field, unit.field);
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestEnemy = enemyUnit;
                        }
                    }
                    Debug.Log(closestEnemy.name);

                    MGPumMoveCommand closestMoveCommand = null;
                    closestDistance = int.MaxValue;
                    foreach (MGPumMoveCommand moveCommand in moveCommands)
                    {
                        int distance = getAbsoluteDistance(closestEnemy.field, moveCommand.chain.getLast());
                        if (distance < closestDistance)
                        {
                            closestDistance = distance;
                            closestMoveCommand = moveCommand;
                        }
                    }
                    return closestMoveCommand;
                }
                else
                {
                    moveCommands.Add(new MGPumMoveCommand(this.controller.playerID, null, unit));
                    moveCommands.Sort(new MGPumMoveCommandComparer(this));
                    if (moveCommands[0].chain != null)
                        return moveCommands[0];
                }
            }
            return null;
        }
    }
}