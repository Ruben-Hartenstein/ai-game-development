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
            foreach (MGPumField field in fieldsInRange)
            {
                MGPumFieldChain chain = getChainBreadthFirst(unit, field, unit.getAttackMatcher());
                if (chain != null)
                    attackCommands.Add(new MGPumAttackCommand(this.controller.playerID, chain, unit));
            }
            return attackCommands;
        }

        protected MGPumUnit getPreferableUnitToAttack(MGPumUnit attacker, List<MGPumUnit> units)
        {
            List<MGPumUnit> killableUnits = new List<MGPumUnit>();
            foreach (MGPumUnit prey in units)
            {
                Debug.Log("Prey in range: " + prey.name);
                if (canKill(prey))
                {
                    Debug.Log("Killable");
                    killableUnits.Add(prey);
                }

            }
            if (killableUnits.Count > 0)
            {
                Debug.Log("KillableCount: " + killableUnits.Count);
                killableUnits.Sort(new MGPummelzComparer());
                Debug.Log("Attack order");
                foreach (MGPumUnit prey in killableUnits)
                {
                    Debug.Log(prey.name);
                }
                return killableUnits[0];
            }
            else
            {
                Debug.Log("KillableCount: " + killableUnits.Count);
                units.Sort(new MGPummelzComparer());
                Debug.Log("Attack order");
                foreach (MGPumUnit prey in units)
                {
                    Debug.Log(prey.name);
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
                    if (getChainBreadthFirst(unit, prey.field, unit.getAttackMatcher()) != null)
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

        private MGPumFieldChain getChainBreadthFirst(MGPumUnit unit, MGPumField targetField, MGPumFieldChainMatcher matcher)
        {
            MGPumFieldChain chain = new MGPumFieldChain(this.controller.playerID, matcher);
            List<Vector2Int> path = pathFinder.findPathInternal(unit, targetField, this.controller.GetState().fields);
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