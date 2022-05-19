using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumNobleSweetPaprikaAIPlayerController : MGPumStudentAIPlayerController
    {

        public const string type = "NobleSweetPaprika";
        public MGPumBreadthFirstSearch pathFinder;
        private MGPumDecisionTreeManager decisionTreeManager;
        protected override int[] getTeamMartikels()
        {
            return new int[] { 2746235, 8366074 };
        }

        public MGPumNobleSweetPaprikaAIPlayerController(int playerID) : base(playerID)
        {
            pathFinder = new MGPumBreadthFirstSearch();
            decisionTreeManager = new MGPumDecisionTreeManager(this);
        }

        public MGPumGameStateOracle GetStateOracle()
        {
            return this.stateOracle;
        }

        public MGPumGameState GetState()
        {
            return this.state;
        }

        internal override MGPumCommand calculateCommand()
        {
            List<MGPumUnit> availableUnits = state.getAllUnitsInZone(MGPumZoneType.Battlegrounds, this.playerID);
            foreach (MGPumUnit unit in availableUnits)
            {
                MGPumCommand command = decisionTreeManager.calculateDecision(unit);
                if (command != null)
                    return command;
            }
            return new MGPumEndTurnCommand(this.playerID);
        }
    }
}
