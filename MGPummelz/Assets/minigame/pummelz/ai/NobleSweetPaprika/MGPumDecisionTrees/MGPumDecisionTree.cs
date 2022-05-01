using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public abstract class MGPumDecisionTree 
    {
        public MGPumNobleSweetPaprikaAIPlayerController controller { get; private set; }
        protected MGPumDecisionTree(MGPumNobleSweetPaprikaAIPlayerController controller) {
            this.controller = controller;
        }
        public abstract MGPumCommand getDecision();
    }
}