using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumMoveCommandComparer : IComparer<MGPumMoveCommand>
    {
        private MGPumDecisionTree decisionTree;
        private int preferableNumberOfPrey;
        private bool aggressive;
        private bool hazardous;
        public MGPumMoveCommandComparer(MGPumDecisionTree decisionTree, int preferableNumberOfPrey = 1, bool aggressive = false, bool hazardous = false)
        {
            this.decisionTree = decisionTree;
            this.preferableNumberOfPrey = preferableNumberOfPrey;
            this.aggressive = aggressive;
            this.hazardous = hazardous;
        }
        public int Compare(MGPumMoveCommand move1, MGPumMoveCommand move2)
        {
            float scoreMove1 = 0.0f;
            float scoreMove2 = 0.0f;
            MGPumUnit unit1 = move1.mover;
            MGPumUnit unit2 = move2.mover;
            MGPumField field1;
            MGPumField field2;

            if (move1.chain == null)
                field1 = unit1.field;
            else
                field1 = move1.chain.last;
            if (move2.chain == null)
                field2 = unit2.field;
            else
                field2 = move2.chain.last;
            if (this.aggressive && this.hazardous)
            {
                scoreMove1 = preyScore(getNumberOfPrey(unit1, field1)) - getNumberOfSurroundingAllies(field1);
                scoreMove2 = preyScore(getNumberOfPrey(unit2, field2)) - getNumberOfSurroundingAllies(field2);
            }
            else if (this.aggressive && !this.hazardous)
            {
                scoreMove1 = preyScore(getNumberOfPrey(unit1, field1)) + getNumberOfAttackers(field1) + 0.5f * getNumberOfSurroundingAllies(field1);
                scoreMove2 = preyScore(getNumberOfPrey(unit2, field2)) + getNumberOfAttackers(field2) + 0.5f * getNumberOfSurroundingAllies(field1);
            }
            else
            {
                scoreMove1 = preyScore(getNumberOfPrey(unit1, field1)) - getNumberOfAttackers(field1);
                scoreMove2 = preyScore(getNumberOfPrey(unit2, field2)) - getNumberOfAttackers(field2);
            }

            scoreMove1 -= 0.1f * getClosestDistance(field1);
            scoreMove2 -= 0.1f * getClosestDistance(field2);

            return scoreMove2.CompareTo(scoreMove1);
        }

        private float preyScore(int numberOfPrey)
        {
            return (-Math.Abs((float)numberOfPrey - (float)this.preferableNumberOfPrey)) + ((float)numberOfPrey - (float)this.preferableNumberOfPrey) / 4 + 1.25f;
        }

        private int getNumberOfAttackers(MGPumField field)
        {
            return this.decisionTree.getNumberOfAttackers(field);
        }

        private int getNumberOfSurroundingAllies(MGPumField field)
        {
            return this.decisionTree.getFieldsInRange(field, 1, this.decisionTree.controller.playerID).Count;
        }

        private int getNumberOfPrey(MGPumUnit unit, MGPumField field)
        {
            return this.decisionTree.getNumberOfPrey(unit, field);
        }

        private int getClosestDistance(MGPumField field)
        {
            int closestDistance = int.MaxValue;
            List<MGPumUnit> enemyUnits = this.decisionTree.state.getAllUnitsInZone(MGPumZoneType.Battlegrounds, 1 - this.decisionTree.controller.playerID);
            foreach (MGPumUnit enemyUnit in enemyUnits)
            {
                int distance = this.decisionTree.getAbsoluteDistance(enemyUnit.field, field);
                if (distance < closestDistance)
                    closestDistance = distance;
            }
            return closestDistance;
        }
    }
}