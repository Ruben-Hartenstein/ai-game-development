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
            MGPumGameState state = this.controller.GetState();
            MGPumGameStateOracle stateOracle = this.controller.GetStateOracle();
            Debug.Log("getDecision of Default for: " + unit.name);
            // If he can attack, he (in most cases) should

            if (stateOracle.canAttack(unit) && unit.currentRange >= 1)
            {
                List<MGPumAttackCommand> attackCommands = getAllAttackCommands(unit);
                Debug.Log("Number of Attacks: " + attackCommands.Count);
                if (attackCommands.Count > 0)
                {
                    List<MGPumUnit> enemyUnits = new List<MGPumUnit>();
                    List<MGPumUnit> killableUnits = new List<MGPumUnit>();
                    attackCommands.ForEach(attackCommand => enemyUnits.Add(attackCommand.chain.getLast().getUnit(state)));
                    Dictionary<MGPumUnit, MGPumAttackCommand> unitAttacks = new Dictionary<MGPumUnit, MGPumAttackCommand>();
                    for (int index = 0; index < enemyUnits.Count; index++)
                    {
                        Debug.Log("Unit in range: " + enemyUnits[index].name);
                        unitAttacks.Add(enemyUnits[index], attackCommands[index]);
                        if ((enemyUnits[index].currentHealth - unit.currentPower) <= 0)
                        {
                            killableUnits.Add(enemyUnits[index]);
                            Debug.Log("Oneshot unit in range: " + enemyUnits[index].name);
                        }

                    }
                    Debug.Log("Number of oneshot Units: " + killableUnits.Count);
                    MGPumAttackCommand attackCommand;
                    if (killableUnits.Count > 0)
                    {
                        Debug.Log("oneshot Unit to attack: " + killableUnits.Count);
                        attackCommand = unitAttacks[getPreferableUnitToAttack(unit, killableUnits)];
                    }
                    else
                    {
                        Debug.Log("Unit to attack: " + enemyUnits.Count);
                        attackCommand = unitAttacks[getPreferableUnitToAttack(unit, enemyUnits)];
                    }
                    Debug.Log(attackCommand.ToString());
                    return attackCommand;
                }
            }

            // If he can't attack, maybe he should move

            if (this.controller.GetStateOracle().canMove(unit) && unit.currentSpeed >= 1)
            {
                List<MGPumAttackCommand> attackCommands = getAllAttackCommands(unit);
                Debug.Log("Number of Moves: " + attackCommands.Count);
                if (attackCommands.Count > 0)
                {
                }
            }

            Debug.Log("Next player");
            return null;
        }
    }
}