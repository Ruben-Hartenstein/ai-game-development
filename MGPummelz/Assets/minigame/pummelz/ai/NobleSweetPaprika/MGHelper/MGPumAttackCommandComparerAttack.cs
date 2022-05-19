using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumAttackCommandComparerAttack : MGPumAttackCommandComparer
    {
        public MGPumAttackCommandComparerAttack(MGPumDecisionTree decisionTree) : base(decisionTree) { }
        public override int Compare(MGPumAttackCommand attackCommand1, MGPumAttackCommand attackCommand2)
        {
            int unitScore1 = -1;
            int unitScore2 = -1;
            if (attackCommand1 != null)
            {
                this.examinedUnitsBummz.Clear();
                this.attackerUnit = attackCommand1.attacker;
                unitScore1 = scoreUnit(attackCommand1.defender, 0);
            }
            if (attackCommand2 != null)
            {
                this.examinedUnitsBummz.Clear();
                this.attackerUnit = attackCommand2.attacker;
                unitScore2 = scoreUnit(attackCommand2.defender, 0);
            }
            return unitScore2.CompareTo(unitScore1);
        }

        public override int scoreUnit(MGPumUnit unit, int recursionLevel)
        {
            switch (unit.name)
            {
                case "Czaremir":
                    return 5000;
                case "Killy":
                    return 200;
                case "Ängli":
                    return 170;
                case "Sneip":
                    return 160;
                case "Buffy":
                    return 150 + buffScoreBonus(unit);
                case "Link":
                    return 140 + linkScoreBonus(unit);
                case "Haley":
                    return 130 + buffScoreBonus(unit);
                case "Mampfred":
                    return 130;
                case "Bellie":
                    return 120;
                case "Hoppel":
                    return 110;
                case "Frömmli":
                    return 100;
                case "Bummz":
                    return bummzScore(unit, recursionLevel);
                case "Wolli":
                    return 90;
                case "Chilly":
                    return 80;
                default:
                    return 140;
            }
        }
    }
}