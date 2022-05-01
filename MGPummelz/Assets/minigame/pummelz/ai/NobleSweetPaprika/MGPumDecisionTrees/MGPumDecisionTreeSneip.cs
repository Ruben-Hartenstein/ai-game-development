using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumDecisionTreeSneip : MGPumDecisionTree 
    {

        public MGPumDecisionTreeSneip(MGPumNobleSweetPaprikaAIPlayerController controller) : base(controller) {}
        public override MGPumCommand getDecision() {
            Debug.Log("getDecision of Sneip");
            return new MGPumEndTurnCommand(1);
        }
    }
}