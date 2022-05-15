using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumUnitComparer : IComparer<MGPumUnit>
    {
        private MGPumDecisionTree decisionTree;
        private List<MGPumUnit> examinedBummz;
        public MGPumUnitComparer(MGPumDecisionTree decisionTree)
        {
            this.decisionTree = decisionTree;
        }
        public int Compare(MGPumUnit unit1, MGPumUnit unit2)
        {
            examinedBummz = new List<MGPumUnit>();
            return scoreUnit(unit2, 0).CompareTo(scoreUnit(unit1, 0));
        }

        private int scoreUnit(MGPumUnit unit, int recursionLevel)
        {
            String unitName; 
            if(unit == null)
                unitName = "null";
            else
                unitName = unit.name;
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
                    return 0;
                default:
                    return 140;
            }
        }

        private int bummzScore(MGPumUnit unit, int recursionLevel)
        {
            if (examinedBummz.Contains(unit))
                return 0;
            examinedBummz.Add(unit);
            int enemyScore = 0;
            int allyScore = 0;
            int affiliation = 1;
            List<MGPumField> surroundingEnemyFields = this.decisionTree.getFieldsInRange(unit.field, 1, 1 - this.decisionTree.controller.playerID); 
            List<MGPumField> surroundingAllyFields = this.decisionTree.getFieldsInRange(unit.field, 1, this.decisionTree.controller.playerID);
            foreach (MGPumField enemyField in surroundingEnemyFields)
            {
                MGPumUnit enemyUnit = enemyField.getUnit(this.decisionTree.state);
                if ((enemyUnit.currentHealth - recursionLevel * 2) > 0)
                    enemyScore += scoreUnit(enemyUnit, recursionLevel + 1);
            }
            foreach (MGPumField allyField in surroundingAllyFields)
            {
                MGPumUnit allyUnit = allyField.getUnit(this.decisionTree.state);
                if ((allyUnit.currentHealth - recursionLevel * 2) > 0)
                    allyScore += scoreUnit(allyUnit, recursionLevel + 1);
            }
            if (unit.ownerID == this.decisionTree.controller.playerID) {
                affiliation = -1;
            }
            Debug.Log((enemyScore - allyScore) + 100 * affiliation);

            return (enemyScore - allyScore) + 100 * affiliation;
        }
    }
}