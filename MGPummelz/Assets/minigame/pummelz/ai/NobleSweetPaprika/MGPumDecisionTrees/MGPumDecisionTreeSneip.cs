using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumDecisionTreeSneip : MGPumDecisionTree 
    {
        public MGPumDecisionTreeSneip(MGPumNobleSweetPaprikaAIPlayerController controller) : base(controller) {}
        public override MGPumCommand getDecision(MGPumUnit unit)
        {
            // MGPumGameState state = this.controller.GetState();
            // MGPumGameStateOracle stateOracle = this.controller.GetStateOracle();
            // Debug.Log("getDecision of Sneip (but the same)");
            //If he can attack, he (in most cases) should
            // List<MGPumAttackCommand> attackCommands = new List<MGPumAttackCommand>();
            // if (stateOracle.canAttack(unit) && unit.currentRange >= 1)
            // {
                // attackCommands = getAllAttackCommands(unit);
                // if (attackCommands.Count > 0)
                // {
                    // List<MGPumUnit> enemyUnits = new List<MGPumUnit>();
                    // List<MGPumUnit> killableUnits = new List<MGPumUnit>();
                    // attackCommands.ForEach(attackCommand => enemyUnits.Add(attackCommand.chain.getLast().getUnit(state)));
                    // Dictionary<MGPumUnit, MGPumAttackCommand> unitAttacks = new Dictionary<MGPumUnit, MGPumAttackCommand>();
                    // for (int index = 0; index < enemyUnits.Count; index++)
                    // {
                        // unitAttacks.Add(enemyUnits[index], attackCommands[index]);
                        // if ((enemyUnits[index].currentHealth - unit.currentPower) >= 0)
                            // killableUnits.Add(enemyUnits[index]);     
                    // }
                    // MGPumAttackCommand attackCommand;
                    // if (killableUnits.Count > 0)
                        // attackCommand = unitAttacks[getPreferableUnitToAttack(killableUnits)];
                    // else
                        // attackCommand = unitAttacks[getPreferableUnitToAttack(new List<MGPumUnit>(unitAttacks.Keys))];
                    // Debug.Log(attackCommand.ToString());
                    // return attackCommand;
                // }
            // }
            return new MGPumEndTurnCommand(this.controller.playerID);
        }
    }
}