using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumNobleSweetPaprikaAIPlayerController : MGPumStudentAIPlayerController
    {

        public const string type = "NobleSweetPaprika";
        public MGPumBreadthFirstSearch pathFinder = new MGPumBreadthFirstSearch();
        protected override int[] getTeamMartikels()
        {
            return new int[] { 2746235, 8366074 };
        }

        public MGPumNobleSweetPaprikaAIPlayerController(int playerID) : base(playerID) { }

        internal override MGPumCommand calculateCommand()
        {
            MGPumCommand moveCommand = new MGPumEndTurnCommand(this.playerID);
            List<MGPumCommand> commands = new List<MGPumCommand>();
            List<MGPumUnit> availableUnits = state.getAllUnitsInZone(MGPumZoneType.Battlegrounds, this.playerID);
            foreach (MGPumUnit unit in availableUnits)
            {  
                commands.AddRange(getAllCommands(unit));
            }
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

        private List<MGPumCommand> getAllCommands(MGPumUnit unit)
        {
            List<MGPumCommand> commands = new List<MGPumCommand>();
            if (stateOracle.canMove(unit) && unit.currentSpeed >= 1)
            {
                commands.AddRange(getAllMoveCommands(unit));
            }

            //if (stateOracle.canAttack(unit) && unit.currentRange >= 1)
            //{
            //    Debug.Log("Can Attack");
            //    commands.AddRange(getAllAttackCommands(unit));
            //}

            return commands;
        }

        private List<MGPumMoveCommand> getAllMoveCommands(MGPumUnit unit)
        {
            List<MGPumMoveCommand> moveCommands = new List<MGPumMoveCommand>();
            MGPumField[,] fields = this.state.fields.fieldArray;
            foreach (MGPumField field in fields)
            {
                if (field == null || !field.isEmpty())
                    continue;
                int dist = getAbsoluteDistance(unit.field, field);
                if (dist <= unit.currentSpeed)
                {   
                    MGPumMoveCommand command = getMoveCommandBreadthFirst(unit, field);
                    if (command != null)
                    {
                        moveCommands.Add(command);
                    }                        
                }
            }
            return moveCommands;
        }

        private List<MGPumAttackCommand> getAllAttackCommands(MGPumUnit unit)
        {
            List<MGPumAttackCommand> attackCommands = new List<MGPumAttackCommand>();
            return attackCommands;
        }

        private int getAbsoluteDistance(MGPumField here, MGPumField there)
        {
            int x1 = here.x;
            int y1 = here.y;
            int x2 = there.x;
            int y2 = there.y;
            return Math.Max(Math.Abs(x1 - x2), Math.Abs(y1 - y2));
        }

        private MGPumMoveCommand getMoveCommandBreadthFirst(MGPumUnit unit, MGPumField targetField)
        {
            MGPumFieldChain chain = new MGPumFieldChain(this.playerID, unit.getMoveMatcher());
            List<Vector2Int> path = pathFinder.findPathInternal(unit, targetField, this.state.fields);
            if (path == null)
            {
                return null;
            }
            foreach (Vector2Int coords in path)
            {
                MGPumField field = this.state.fields.getField(coords);
                if (chain.canAdd(field))
                {
                    chain.add(field);
                }
                else
                {
                    return null;
                }
            }
            if(chain.isValidChain())
            {
                return new MGPumMoveCommand(this.playerID, chain, unit);  
            }
            return null;
        }
    }
}
