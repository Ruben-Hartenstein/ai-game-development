using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumUnitComparer : IComparer<MGPumUnit>
    {
        private MGPumDecisionTree decisionTree;
        private MGPumUnit attackerUnit;
        private Dictionary<MGPumUnit, int> examinedUnits;
        public MGPumUnitComparer(MGPumDecisionTree decisionTree, MGPumUnit attackerUnit)
        {
            this.decisionTree = decisionTree;
            this.attackerUnit = attackerUnit;
            this.examinedUnits = new Dictionary<MGPumUnit, int>();
        }
        public int Compare(MGPumUnit unit1, MGPumUnit unit2)
        {
            this.examinedUnits.Clear();
            int unitScore1 = scoreUnit(unit1, 0);
            this.examinedUnits.Clear();
            int unitScore2 = scoreUnit(unit2, 0);
            return unitScore2.CompareTo(unitScore1);
        }

        private int scoreUnit(MGPumUnit unit, int recursionLevel)
        {
            String unitName = unit == null ? "null" : unit.name;
            switch (unitName)
            {
                case "Czaremir":
                    return 5000;
                case "Killy":
                    return 200;
                case "Chilly":
                    return 190;
                case "Ängli":
                    return 180;
                case "Sneip":
                    return 170;
                case "Buffy":
                    return 160;
                case "Haley":
                    return 150;
                case "Link":
                    return 140;
                case "Mampfred":
                    return 130;
                case "Frömmli":
                    return 125;
                case "Wolli":
                    return 120;
                case "Hoppel":
                    return 110;
                case "Bummz":
                    return bummzScore(unit, recursionLevel);
                case "Bellie":
                    return 90;
                case "null":
                    return -1;
                default:
                    return 140;
            }
        }

        private int bummzScore(MGPumUnit unit, int recursionLevel)
        {
            int currentHealth = unit.currentHealth;
            int unitBonus = 100;
            if (examinedUnits.ContainsKey(unit))
                currentHealth = examinedUnits[unit];
            else
                examinedUnits.Add(unit, currentHealth);
            if (currentHealth <= 0)
                return 0;

            if (recursionLevel == 0)                                        // If recursionLevel is 0, the attacker was the unit
                examinedUnits[unit] -= attackerUnit.currentPower;
            else                                                            // Otherwise, damage through bomb is 2
                examinedUnits[unit] -= 2;
            currentHealth = examinedUnits[unit];

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

                if (examinedUnits.ContainsKey(enemyUnit))
                    currentEnemyHealth = examinedUnits[enemyUnit];
                else
                    examinedUnits.Add(enemyUnit, currentEnemyHealth);

                if (currentEnemyHealth > 0)
                {
                    enemyScore += scoreUnit(enemyUnit, recursionLevel + 1);
                    examinedUnits[enemyUnit] -= 2;
                }
            }

            foreach (MGPumField allyField in surroundingAllyFields)
            {
                MGPumUnit allyUnit = allyField.getUnit(this.decisionTree.state);
                int currentAllyHealth = allyUnit.currentHealth;

                if (examinedUnits.ContainsKey(allyUnit))
                    currentAllyHealth = examinedUnits[allyUnit];
                else
                    examinedUnits.Add(allyUnit, currentAllyHealth);

                if (currentAllyHealth > 0)
                {
                    allyScore += allyUnit.name == "Bummz" ? -scoreUnit(allyUnit, recursionLevel + 1) : scoreUnit(allyUnit, recursionLevel + 1);
                    examinedUnits[allyUnit] -= 2;
                }
            }
            return (enemyScore - allyScore) + 100 * affiliation;
        }
    }
}