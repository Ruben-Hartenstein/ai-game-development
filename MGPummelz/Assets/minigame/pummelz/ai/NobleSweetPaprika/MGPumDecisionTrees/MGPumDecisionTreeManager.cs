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

        private MGPumDecisionTree getDecisionTree(String PUMID)
        {
            switch (PUMID)
            {
                case "PUM003":
                    return new MGPumDecisionTreeSneip(controller);
                    break;
                default:
                    return new MGPumDecisionTreeDefault(controller);
                    break;
            }
        }

        public MGPumCommand getDecision(MGPumUnit unit)
        {
            MGPumDecisionTree decisionTree = getDecisionTree(unit.unitID);
            MGPumCommand command = decisionTree.getDecision();
            decisionTree = null;
            return command;
        }
    }
}