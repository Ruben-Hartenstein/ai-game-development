using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumUnitComparer : IComparer<MGPumUnit>
    {
        public int Compare(MGPumUnit unit1, MGPumUnit unit2)
        {
           return scoreUnits(unit2.name).CompareTo(scoreUnits(unit1.name));
        }

        private int scoreUnits(String pumName)
        {
            switch (pumName)
            {
                case "Czaremir":
                    return int.MaxValue;
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
                case "Wolli":
                    return 120;
                case "Hoppel":
                    return 110;
                case "Bummz":
                    return 100;
                case "Bellie":
                    return 90;
                default:
                    return 80;
            }
        }
    }
}