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
                commands.AddRange(getAllMoveCommands(unit));

            if (stateOracle.canAttack(unit) && unit.currentRange >= 1)
                commands.AddRange(getAllAttackCommands(unit));

            return commands;
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
                    if(field.isEmpty() && ownerID == -1)
                        fieldsInRange.Add(field);
                    else if(!field.isEmpty() && field.getUnit(this.state).ownerID == ownerID)
                        fieldsInRange.Add(field);                            
                }
            }
            return fieldsInRange;
        }

        private List<MGPumMoveCommand> getAllMoveCommands(MGPumUnit unit)
        {
            List<MGPumMoveCommand> moveCommands = new List<MGPumMoveCommand>();
            List<MGPumField> fieldsInRange = getFieldsInRange(unit.field, unit.currentSpeed, -1);
            foreach (MGPumField field in fieldsInRange)
            {
                MGPumFieldChain chain = getChainBreadthFirst(unit, field, unit.getMoveMatcher());
                if (chain != null)
                {
                    moveCommands.Add(new MGPumMoveCommand(this.playerID, chain, unit));
                }
            }
            return moveCommands;
        }

        private List<MGPumAttackCommand> getAllAttackCommands(MGPumUnit unit)
        {
            List<MGPumAttackCommand> attackCommands = new List<MGPumAttackCommand>();
            List<MGPumField> fieldsInRange = getFieldsInRange(unit.field, unit.currentRange, 1 - this.playerID);
            foreach (MGPumField field in fieldsInRange)
            {
                MGPumFieldChain chain = getChainBreadthFirst(unit, field, unit.getAttackMatcher());
                if (chain != null)
                    attackCommands.Add(new MGPumAttackCommand(this.playerID, chain, unit));
            }
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

        private MGPumFieldChain getChainBreadthFirst(MGPumUnit unit, MGPumField targetField, MGPumFieldChainMatcher matcher)
        {
            MGPumFieldChain chain = new MGPumFieldChain(this.playerID, matcher);
            List<Vector2Int> path = pathFinder.findPathInternal(unit, targetField, this.state.fields);
            if (path == null)
                return null;
            foreach (Vector2Int coords in path)
            {
                MGPumField field = this.state.fields.getField(coords);
                if (chain.canAdd(field))
                    chain.add(field);
                else
                    return null;
            }
            if (chain.isValidChain())
                return chain;
            return null;
        }
    }
}
