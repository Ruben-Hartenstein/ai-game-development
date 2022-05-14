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
            MGPumCommand moveCommand = new MGPumEndTurnCommand(this.playerID);
            List<MGPumCommand> commands = new List<MGPumCommand>();
            List<MGPumUnit> availableUnits = state.getAllUnitsInZone(MGPumZoneType.Battlegrounds, this.playerID);
            foreach (MGPumUnit unit in availableUnits)
            {
                // Debug.Log(unit.coords + ": " + unit.name);
                MGPumCommand command = decisionTreeManager.getDecision(unit);
                if (command != null)
                {
                    return command;
                }
                //commands.AddRange(getAllCommands(unit));
            }
            // Debug.Log("End Turn");
            return new MGPumEndTurnCommand(this.playerID);
            if (commands.Count == 0)
            {
                return moveCommand;
            }
            MGPumOracleBoolResult result = this.stateOracle.checkCommandDetailed(commands[0], false);
            if (result.result)
            {
                return commands[rng.Next(commands.Count)];
            }

            return moveCommand;
        }

        private List<MGPumField> getFieldsInRange(MGPumField home, int range, int ownerID)
        {
            List<MGPumField> fieldsInRange = new List<MGPumField>();
            for (int x = home.x - range; x <= home.x + range; x++)
            {
                for (int y = home.y - range; y <= home.y + range; y++)
                {
                    MGPumField field = this.state.getField(new Vector2Int(x, y));
                    if (field == null)
                        continue;
                    if (field.isEmpty() && ownerID == -1)
                        fieldsInRange.Add(field);
                    else if (!field.isEmpty() && field.getUnit(this.state).ownerID == ownerID)
                        fieldsInRange.Add(field);
                }
            }
            return fieldsInRange;
        }

        private int getAbsoluteDistance(MGPumField here, MGPumField there)
        {
            int x1 = here.x;
            int y1 = here.y;
            int x2 = there.x;
            int y2 = there.y;
            return Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));
        }
    }
}
