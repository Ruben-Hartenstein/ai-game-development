using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumDecisionTreeBummz : MGPumDecisionTree
    {
        public MGPumDecisionTreeBummz(MGPumNobleSweetPaprikaAIPlayerController controller) : base(controller) { }
        public override MGPumCommand getDecision(MGPumUnit unit)
        {
            Debug.Log("getDecision of Bummz for: " + unit.name);

            if (this.stateOracle.canAttack(unit) && unit.currentRange >= 1)
            {
                List<MGPumAttackCommand> attackCommands = getAllAttackCommands(unit);
                if (attackCommands.Count > 0)
                {
                    List<MGPumAttackCommand> killCommands = new List<MGPumAttackCommand>();
                    foreach (MGPumAttackCommand attackCommand in attackCommands)
                    {
                        if (attackCommand.defender.currentHealth - unit.currentPower <= 0)
                            killCommands.Add(attackCommand);
                    }
                    if (killCommands.Count == 0)
                        killCommands = getKillingAttackCommands(attackCommands);

                    if (killCommands.Count == 0)
                    {
                        // no kill commands, different comparer
                        attackCommands.Add(null);
                        attackCommands.Sort(new MGPumAttackCommandComparerAttack(this));
                    }
                    else
                    {
                        killCommands.Add(null);
                        killCommands.Sort(new MGPumAttackCommandComparerKill(this));
                        attackCommands = killCommands;
                    }
                    if (attackCommands[0] != null)
                        return attackCommands[0];
                }
            }

            if (this.stateOracle.canMove(unit) && unit.currentSpeed >= 1)
            {
                if (state.turnNumber >= state.lastChangeTurnNumber + 20)
                    return this.getMoveCommandToEnemy(unit);

                List<MGPumMoveCommand> moveCommands = getAllMoveCommands(unit);
                moveCommands.Add(new MGPumMoveCommand(this.controller.playerID, null, unit));
                moveCommands.Sort(new MGPumMoveCommandComparerBummz(this, 8));
                if (moveCommands[0].chain != null)
                    return moveCommands[0];
            }
            return null;
        }
    }
}