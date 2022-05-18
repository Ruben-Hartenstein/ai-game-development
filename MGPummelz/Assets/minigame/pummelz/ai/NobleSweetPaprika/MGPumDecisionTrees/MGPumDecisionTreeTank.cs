using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumDecisionTreeTank : MGPumDecisionTree
    {
        public MGPumDecisionTreeTank(MGPumNobleSweetPaprikaAIPlayerController controller) : base(controller) { }
        public override MGPumCommand getDecision(MGPumUnit unit)
        {
            Debug.Log("getDecision of Tank for: " + unit.name);

            if (this.stateOracle.canAttack(unit) && unit.currentRange >= 1)
            {
                List<MGPumAttackCommand> attackCommands = getAllAttackCommands(unit);
                if (attackCommands.Count > 0)
                {
                    List<MGPumUnit> enemyUnits = new List<MGPumUnit>();
                    List<MGPumUnit> killableUnits = new List<MGPumUnit>();
                    attackCommands.ForEach(attackCommand => enemyUnits.Add(attackCommand.chain.getLast().getUnit(this.state)));
                    Dictionary<MGPumUnit, MGPumAttackCommand> unitAttacks = new Dictionary<MGPumUnit, MGPumAttackCommand>();
                    for (int index = 0; index < enemyUnits.Count; index++)
                    {
                        unitAttacks.Add(enemyUnits[index], attackCommands[index]);
                        if ((enemyUnits[index].currentHealth - unit.currentPower) <= 0)
                            killableUnits.Add(enemyUnits[index]);
                    }
                    MGPumAttackCommand attackCommand;
                    if (killableUnits.Count > 0)
                        enemyUnits = killableUnits;
                    else
                        enemyUnits = getPreferableUnitsToAttack(unit, enemyUnits);

                    enemyUnits.Add(null);
                    enemyUnits.Sort(new MGPumUnitComparer(this, unit));
                    if (enemyUnits[0] != null)
                        return unitAttacks[enemyUnits[0]];
                }
            }

            if (this.stateOracle.canMove(unit) && unit.currentSpeed >= 1)
            {
                if (state.turnNumber >= state.lastChangeTurnNumber + 20)
                    return this.getMoveCommandToEnemy(unit);
 
                List<MGPumMoveCommand> moveCommands = getAllMoveCommands(unit);
                moveCommands.Add(new MGPumMoveCommand(this.controller.playerID, null, unit));
                moveCommands.Sort(new MGPumMoveCommandComparer(this, 8, true));
                if (moveCommands[0].chain != null)
                    return moveCommands[0];
            }
            return null;
        }
    }
}