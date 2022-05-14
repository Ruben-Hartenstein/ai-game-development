using System.Collections.Generic;
using System;
using UnityEngine;

namespace mg.pummelz
{
    public abstract class MGPumDecisionTree
    {
        protected MGPumNobleSweetPaprikaAIPlayerController controller { get; private set; }
        protected MGPumBreadthFirstSearch pathFinder = new MGPumBreadthFirstSearch();
        protected MGPumDecisionTree(MGPumNobleSweetPaprikaAIPlayerController controller)
        {
            this.controller = controller;
        }
        public abstract MGPumCommand getDecision(MGPumUnit unit);

        protected List<MGPumAttackCommand> getAllAttackCommands(MGPumUnit unit)
        {

            List<MGPumAttackCommand> attackCommands = new List<MGPumAttackCommand>();
            List<MGPumField> fieldsInRange = getFieldsInRange(unit.field, unit.currentRange, 1 - this.controller.playerID);
            foreach (MGPumField targetField in fieldsInRange)
            {
                MGPumFieldChain chain = getChainBreadthFirst(unit.field, targetField, unit.currentRange, unit.getAttackMatcher());
                if (chain != null)
                {
                    attackCommands.Add(new MGPumAttackCommand(this.controller.playerID, chain, unit));
                }
            }
            return attackCommands;
        }

        public int getNumberOfPrey(MGPumUnit unit, MGPumField field)
        {
            int numberOfPrey = 0;
            List<MGPumField> fieldsInRange = getFieldsInRange(field, unit.currentRange, 1 - this.controller.playerID);
            foreach (MGPumField targetField in fieldsInRange)
            {
                List<Vector2Int> path = getPathBreadthFirst(field, targetField, unit.currentRange);
                    if (path != null && checkShotDirection(path))
                    {
                        numberOfPrey++;
                    }
            }
            return numberOfPrey;
        }

        public int getNumberOfAttackers(MGPumUnit unit, MGPumField field)
        {
            int numberOfAttackers = 0;
            List<MGPumUnit> enemyUnits = this.controller.GetState().getAllUnitsInZone(MGPumZoneType.Battlegrounds, 1 - this.controller.playerID);
            foreach (MGPumUnit enemyUnit in enemyUnits)
            {
                if (this.controller.GetStateOracle().canAttack(enemyUnit) && enemyUnit.currentRange >= 1)
                {
                    List<Vector2Int> path = getPathBreadthFirst(enemyUnit.field, field, enemyUnit.currentRange);
                    if (path != null && checkShotDirection(path))
                    {
                        numberOfAttackers++;
                    }
                }
            }
            return numberOfAttackers;
        }

        private bool checkShotDirection(List<Vector2Int> path)
        {
            Vector2Int start = path[0];
            Vector2Int last = path[path.Count - 1];
            int xdir = 0;
            int ydir = 0;

            for (int i = 1; i < path.Count; i++)
            {
                if(xdir == 0)
                {
                    xdir = path[i].x - path[i-1].x;
                }
                else if (path[i].x - path[i-1].x != 0 && xdir != path[i].x - path[i-1].x) {
                    return false;
                }
                if(ydir == 0)
                {
                    ydir = path[i].y - path[i-1].y;
                }
                else if (path[i].y - path[i-1].y != 0 && ydir != path[i].y - path[i-1].y){
                    return false;
                }
            }
            return true;
        }

        protected List<MGPumMoveCommand> getAllMoveCommands(MGPumUnit unit)
        {
            List<MGPumMoveCommand> moveCommands = new List<MGPumMoveCommand>();
            List<MGPumField> fieldsInRange = getFieldsInRange(unit.field, unit.currentSpeed, -1);
            foreach (MGPumField field in fieldsInRange)
            {
                MGPumFieldChain chain = getChainBreadthFirst(unit.field, field, unit.currentRange, unit.getMoveMatcher());
                if (chain != null)
                {
                    moveCommands.Add(new MGPumMoveCommand(this.controller.playerID, chain, unit));
                }
            }
            return moveCommands;
        }

        protected MGPumUnit getPreferableUnitToAttack(MGPumUnit attacker, List<MGPumUnit> units)
        {
            List<MGPumUnit> killableUnits = new List<MGPumUnit>();
            foreach (MGPumUnit prey in units)
            {
                // Debug.Log("Prey in range: " + prey.name);
                if (canKill(prey))
                {
                    // Debug.Log("Killable");
                    killableUnits.Add(prey);
                }

            }
            if (killableUnits.Count > 0)
            {
                // Debug.Log("KillableCount: " + killableUnits.Count);
                killableUnits.Sort(new MGPumUnitComparer());
                // Debug.Log("Attack order");
                foreach (MGPumUnit prey in killableUnits)
                {
                    // Debug.Log(prey.name);
                }
                return killableUnits[0];
            }
            else
            {
                // Debug.Log("KillableCount: " + killableUnits.Count);
                units.Sort(new MGPumUnitComparer());
                // Debug.Log("Attack order");
                foreach (MGPumUnit prey in units)
                {
                    // Debug.Log(prey.name);
                }
                return units[0];
            }
        }

        protected bool canKill(MGPumUnit prey)
        {
            int preyHealth = prey.currentHealth;
            List<MGPumUnit> availableUnits = this.controller.GetState().getAllUnitsInZone(MGPumZoneType.Battlegrounds, this.controller.playerID);
            foreach (MGPumUnit unit in availableUnits)
            {
                if (this.controller.GetStateOracle().canAttack(unit) && unit.currentRange >= 1)
                {
                    if (getChainBreadthFirst(unit.field, prey.field, unit.currentRange, unit.getAttackMatcher()) != null)
                    {
                        preyHealth -= unit.currentPower;
                        if (preyHealth <= 0)
                            return true;
                    }
                }
            }
            return false;
        }

        protected List<MGPumField> getFieldsInRange(MGPumField home, int range, int ownerID)
        {
            List<MGPumField> fieldsInRange = new List<MGPumField>();
            for (int x = home.x - range; x <= home.x + range; x++)
            {
                for (int y = home.y - range; y <= home.y + range; y++)
                {
                    MGPumField field = this.controller.GetState().getField(new Vector2Int(x, y));
                    if (field == null)
                        continue;
                    if (field.isEmpty() && ownerID == -1)
                        fieldsInRange.Add(field);
                    else if (!field.isEmpty() && field.getUnit(this.controller.GetState()).ownerID == ownerID)
                        fieldsInRange.Add(field);
                }
            }
            return fieldsInRange;
        }

        private List<Vector2Int> getPathBreadthFirst(MGPumField startField, MGPumField targetField, int maxRange)
        {
            return pathFinder.findPathInternal(startField, targetField, maxRange, this.controller.GetState().fields);
        }

        private MGPumFieldChain getChainBreadthFirst(MGPumField startField, MGPumField targetField, int maxRange, MGPumFieldChainMatcher matcher)
        {
            MGPumFieldChain chain = new MGPumFieldChain(this.controller.playerID, matcher);
            List<Vector2Int> path = getPathBreadthFirst(startField, targetField, maxRange);
            if (path == null)
                return null;
            foreach (Vector2Int coords in path)
            {
                MGPumField field = this.controller.GetState().fields.getField(coords);
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