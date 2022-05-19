using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumMoveCommandComparerBuff : MGPumMoveCommandComparer
    {
        public MGPumMoveCommandComparerBuff(MGPumDecisionTree decisionTree) : base(decisionTree) { }
        public override int Compare(MGPumMoveCommand move1, MGPumMoveCommand move2)
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

            // Stay away from enemies, stay with allies
            scoreMove1 = getNumberOfSurroundingAllies(field1) - getNumberOfAttackers(field1);
            scoreMove2 = getNumberOfSurroundingAllies(field2) - getNumberOfAttackers(field2);
            scoreMove1 -= 0.1f * getClosestDistance(field1);
            scoreMove2 -= 0.1f * getClosestDistance(field2);

            return scoreMove2.CompareTo(scoreMove1);
        }
    }
}