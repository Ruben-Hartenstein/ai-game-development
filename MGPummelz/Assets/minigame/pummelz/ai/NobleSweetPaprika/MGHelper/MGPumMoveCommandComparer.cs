using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumMoveCommandComparer : IComparer<MGPumMoveCommand>
    {
        private MGPumDecisionTree decisionTree;
        private int preferableNumberOfPrey;
        public MGPumMoveCommandComparer(MGPumDecisionTree decisionTree, int preferableNumberOfPrey = 1)
        {
            this.decisionTree = decisionTree;
            this.preferableNumberOfPrey = preferableNumberOfPrey;
        }
        public int Compare(MGPumMoveCommand move1, MGPumMoveCommand move2)
        {
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

            float scoreMove1 = preyScore(getNumberOfPrey(unit1, field1)) - getNumberOfAttackers(field1);
            float scoreMove2 = preyScore(getNumberOfPrey(unit2, field2)) - getNumberOfAttackers(field2);
            return scoreMove2.CompareTo(scoreMove1);
        }

        private float preyScore(int numberOfPrey)
        {
            return (-Math.Abs((float) numberOfPrey - (float) this.preferableNumberOfPrey)) + ((float) numberOfPrey - (float) this.preferableNumberOfPrey) / 4 + 1.25f;
        }

        private int getNumberOfAttackers(MGPumField field)
        {
            return this.decisionTree.getNumberOfAttackers(field);
        }

        private int getNumberOfPrey(MGPumUnit unit, MGPumField field)
        {
            return this.decisionTree.getNumberOfPrey(unit, field);
        }
    }
}