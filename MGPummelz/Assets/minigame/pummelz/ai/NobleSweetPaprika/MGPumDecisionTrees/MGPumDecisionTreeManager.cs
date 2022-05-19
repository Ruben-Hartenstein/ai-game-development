using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumDecisionTreeManager
    {
        private MGPumNobleSweetPaprikaAIPlayerController controller;

        public MGPumDecisionTreeManager(MGPumNobleSweetPaprikaAIPlayerController controller)
        {
            this.controller = controller;
        }

        private MGPumDecisionTree getDecisionTree(String pumName)
        {
            switch (pumName)
            {
                case "Czaremir":
                    return new MGPumDecisionTreeCzaremir(controller);
                case "Bummz":
                    return new MGPumDecisionTreeBummz(controller);
                case "Wolli":
                    return new MGPumDecisionTreeTank(controller);
                case "Mampfred":
                    return new MGPumDecisionTreeTank(controller);
                case "Bellie":
                    return new MGPumDecisionTreeTank(controller);
                case "Haley":
                    return new MGPumDecisionTreeBuff(controller);
                case "Buffy":
                    return new MGPumDecisionTreeBuff(controller);
                default:
                    return new MGPumDecisionTreeDefault(controller);
            }
        }

        public MGPumCommand calculateDecision(MGPumUnit unit)
        {
            MGPumDecisionTree decisionTree = getDecisionTree(unit.name);
            MGPumCommand command = decisionTree.getDecision(unit);
            decisionTree = null;
            return command;
        }
    }
}