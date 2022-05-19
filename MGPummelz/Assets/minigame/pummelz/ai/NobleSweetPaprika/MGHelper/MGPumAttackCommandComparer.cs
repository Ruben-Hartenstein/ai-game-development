using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public abstract class MGPumAttackCommandComparer : IComparer<MGPumAttackCommand>
    {
        public MGPumDecisionTree decisionTree;
        public MGPumUnit attackerUnit;
        public Dictionary<MGPumUnit, int> examinedUnitsBummz;
        public MGPumAttackCommandComparer(MGPumDecisionTree decisionTree)
        {
            this.decisionTree = decisionTree;
            this.examinedUnitsBummz = new Dictionary<MGPumUnit, int>();
        }
        public abstract int Compare(MGPumAttackCommand attackCommand1, MGPumAttackCommand attackCommand2);

        public abstract int scoreUnit(MGPumUnit unit, int recursionLevel);
        public int linkScoreBonus(MGPumUnit unit)
        {
            return unit.currentPower * 5;
        }

        public int buffScoreBonus(MGPumUnit unit)
        {
            return (this.decisionTree.getFieldsInRange(unit.field, 1, this.decisionTree.controller.playerID).Count - 1) * 5;
        }
        
        public int bummzScore(MGPumUnit unit, int recursionLevel)
        {
            int currentHealth = unit.currentHealth;
            if (this.examinedUnitsBummz.ContainsKey(unit))
                currentHealth = this.examinedUnitsBummz[unit];
            else
                this.examinedUnitsBummz.Add(unit, currentHealth);
            if (currentHealth <= 0)
                return 0;

            if (recursionLevel == 0)                                        // If recursionLevel is 0, the attacker was the unit
                this.examinedUnitsBummz[unit] -= attackerUnit.currentPower;
            else                                                            // Otherwise, damage through bomb is 2
                this.examinedUnitsBummz[unit] -= 2;
            currentHealth = this.examinedUnitsBummz[unit];

            int affiliation = unit.ownerID != this.decisionTree.controller.playerID ? 1 : -1;
            if (currentHealth > 0)
                return 100 * affiliation;

            int enemyScore = 0;
            int allyScore = 0;

            List<MGPumField> surroundingEnemyFields = this.decisionTree.getFieldsInRange(unit.field, 1, 1 - this.decisionTree.controller.playerID);
            List<MGPumField> surroundingAllyFields = this.decisionTree.getFieldsInRange(unit.field, 1, this.decisionTree.controller.playerID);
            foreach (MGPumField enemyField in surroundingEnemyFields)
            {
                MGPumUnit enemyUnit = enemyField.getUnit(this.decisionTree.state);
                int currentEnemyHealth = enemyUnit.currentHealth;

                if (this.examinedUnitsBummz.ContainsKey(enemyUnit))
                    currentEnemyHealth = this.examinedUnitsBummz[enemyUnit];
                else
                    this.examinedUnitsBummz.Add(enemyUnit, currentEnemyHealth);

                if (currentEnemyHealth > 0)
                {
                    enemyScore += scoreUnit(enemyUnit, recursionLevel + 1);
                    this.examinedUnitsBummz[enemyUnit] -= 2;
                }
            }

            foreach (MGPumField allyField in surroundingAllyFields)
            {
                MGPumUnit allyUnit = allyField.getUnit(this.decisionTree.state);
                int currentAllyHealth = allyUnit.currentHealth;

                if (this.examinedUnitsBummz.ContainsKey(allyUnit))
                    currentAllyHealth = this.examinedUnitsBummz[allyUnit];
                else
                    this.examinedUnitsBummz.Add(allyUnit, currentAllyHealth);

                if (currentAllyHealth > 0)
                {
                    allyScore += allyUnit.name == "Bummz" ? -scoreUnit(allyUnit, recursionLevel + 1) : scoreUnit(allyUnit, recursionLevel + 1);
                    this.examinedUnitsBummz[allyUnit] -= 2;
                }
            }
            return (enemyScore - allyScore) + 100 * affiliation;
        }
    }
}