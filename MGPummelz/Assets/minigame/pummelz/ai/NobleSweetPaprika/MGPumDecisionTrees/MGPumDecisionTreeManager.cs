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
                case "Sneip":
                    return new MGPumDecisionTreeDefault(controller);
                    break;
                default:
                    return new MGPumDecisionTreeDefault(controller);
                    break;
            }
        }

        public MGPumCommand getDecision(MGPumUnit unit)
        {
            MGPumDecisionTree decisionTree = getDecisionTree(unit.name);
            MGPumCommand command = decisionTree.getDecision(unit);
            decisionTree = null;
            return command;
        }
    }
}