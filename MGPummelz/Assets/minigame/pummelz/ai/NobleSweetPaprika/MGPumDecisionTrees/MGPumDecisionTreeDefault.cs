using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumDecisionTreeDefault : MGPumDecisionTree 
    {
        public MGPumDecisionTreeDefault(MGPumNobleSweetPaprikaAIPlayerController controller) : base(controller) {}
        public override MGPumCommand getDecision() {
            Debug.Log("getDecision of Default");
            return new MGPumEndTurnCommand(1);
        }
    }
}