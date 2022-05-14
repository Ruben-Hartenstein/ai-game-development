using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumMoveCommandComparer : IComparer<MGPumMoveCommand>
    {
        private MGPumDecisionTree decisionTree;
        public MGPumMoveCommandComparer(MGPumDecisionTree decisionTree)
        {
            this.decisionTree = decisionTree;
        }
        public int Compare(MGPumMoveCommand move1, MGPumMoveCommand move2)
        {
            float scoreMove1 = prefereableNumberOfPrey(getNumberOfPrey(move1.mover, move1.chain.last)) - getNumberOfAttackers(move1.mover, move1.chain.last);
            float scoreMove2 = prefereableNumberOfPrey(getNumberOfPrey(move2.mover, move2.chain.last)) - getNumberOfAttackers(move2.mover, move2.chain.last);
            return scoreMove2.CompareTo(scoreMove1);
        }

        private float prefereableNumberOfPrey(float numberOfPrey)
        {
            return (-Math.Abs(numberOfPrey-1)) + numberOfPrey/4 + 1;
        }

        private int getNumberOfAttackers(MGPumUnit unit, MGPumField field)
        {
            return this.decisionTree.getNumberOfAttackers(unit, field);
        }

        private int getNumberOfPrey(MGPumUnit unit, MGPumField field)
        {
            return this.decisionTree.getNumberOfPrey(unit, field);
        }
    }
}