using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public abstract class MGPumMoveCommandComparer : IComparer<MGPumMoveCommand>
    {
        private MGPumDecisionTree decisionTree;
        private int preferableNumberOfPrey;
        public MGPumMoveCommandComparer(MGPumDecisionTree decisionTree, int preferableNumberOfPrey = 1)
        {
            this.decisionTree = decisionTree;
            this.preferableNumberOfPrey = preferableNumberOfPrey;
        }
        public abstract int Compare(MGPumMoveCommand move1, MGPumMoveCommand move2);

        public float preyScore(int numberOfPrey)
        {
            return (-Math.Abs((float)numberOfPrey - (float)this.preferableNumberOfPrey)) + ((float)numberOfPrey - (float)this.preferableNumberOfPrey) / 4 + 1.25f;
        }

        public int getNumberOfAttackers(MGPumField field)
        {
            return this.decisionTree.getNumberOfAttackers(field);
        }

        public int getNumberOfSurroundingAllies(MGPumField field)
        {
            return this.decisionTree.getFieldsInRange(field, 1, this.decisionTree.controller.playerID).Count;
        }

        public int getNumberOfPrey(MGPumUnit unit, MGPumField field)
        {
            return this.decisionTree.getNumberOfPrey(unit, field);
        }

        public int getClosestDistance(MGPumField field)
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