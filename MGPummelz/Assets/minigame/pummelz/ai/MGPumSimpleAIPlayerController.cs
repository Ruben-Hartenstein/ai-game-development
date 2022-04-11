using System.Collections.Generic;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumSimpleAIPlayerController : MGPumAIPlayerController
    {
        public const string type = "CHIMNEY_CAT";


        private List<Vector2Int> _directions = null;

        private List<Vector2Int> getDirections()
        {
            if(_directions == null)
            {
                _directions = new List<Vector2Int>();
                _directions.Add(Vector2Int.left);
                _directions.Add(Vector2Int.right);
                _directions.Add(Vector2Int.up);
                _directions.Add(Vector2Int.down);
                _directions.Add(Vector2Int.left + Vector2Int.up);
                _directions.Add(Vector2Int.left +Vector2Int.down);
                _directions.Add(Vector2Int.right + Vector2Int.up);
                _directions.Add(Vector2Int.right + Vector2Int.down);
            }
            return _directions;
        }


        public MGPumSimpleAIPlayerController(int playerID, bool threaded) : base(playerID, threaded)
        {
        }

        internal override MGPumCommand calculateCommand()
        {
            //shoot first, check movement later :-)
            
            MGPumAttackCommand ac = findAttackCommand();
            if (ac != null)
            {
                return ac;
            }

            MGPumMoveCommand mc = findMoveCommand();
            if (mc != null)
            {
                return mc;
            }

            return new MGPumEndTurnCommand(this.playerID);
        }

        private MGPumAttackCommand findAttackCommand()
        {
            List<MGPumUnit> possibleAttackers = new List<MGPumUnit>();
            
            foreach(MGPumUnit unit in state.getAllUnitsInZone(MGPumZoneType.Battlegrounds, this.playerID))
            {
                if(stateOracle.canAttack(unit))
                {
                    possibleAttackers.Add(unit);
                }
            }

            foreach (MGPumUnit unit in possibleAttackers)
            {
                MGPumAttackCommand ac = findAttackCommand(unit);
                if(ac != null)
                {
                    return ac;
                }
            }
            return null;
        }

        private MGPumAttackCommand findAttackCommand(MGPumUnit unit)
        {
            MGPumAttackChainMatcher matcher = unit.getAttackMatcher();

            foreach(Vector2Int direction in getDirections())
            {
                MGPumFieldChain chain = new MGPumFieldChain(unit.ownerID, matcher);
                chain.add(unit.field);

                Vector2Int position = unit.field.coords;
                position += direction;

                while (state.fields.inBounds(position))
                {
                    MGPumField fieldAtPosition = state.fields.getField(position);

                    if (chain.canAdd(fieldAtPosition))
                    {
                        chain.add(fieldAtPosition);
                    }

                    if(chain.isValidChain())
                    {
                        break;
                    }
                    
                    position += direction;
                }

                if(chain.isValidChain())
                {
                    MGPumAttackCommand ac = new MGPumAttackCommand(this.playerID, chain, unit);
                    if(stateOracle.checkAttackCommand(ac))
                    {
                        return ac;
                    }
                }
            }
            return null;
        }


        private MGPumMoveCommand findMoveCommand()
        {
            List<MGPumUnit> possibleMovers = new List<MGPumUnit>();

            foreach (MGPumUnit unit in state.getAllUnitsInZone(MGPumZoneType.Battlegrounds, this.playerID))
            {
                if (stateOracle.canMove(unit))
                {
                    possibleMovers.Add(unit);
                }
            }

            foreach (MGPumUnit unit in possibleMovers)
            {
                MGPumMoveCommand mc = findMoveCommand(unit);
                if (mc != null)
                {
                    return mc;
                }
            }
            return null;
        }

        private MGPumMoveCommand findMoveCommand(MGPumUnit unit)
        {
            MGPumMoveChainMatcher matcher = unit.getMoveMatcher();

            //best command leads us closest towards an enemy
            int currentBestDistance = int.MaxValue;
            MGPumMoveCommand currentBestCommand = null;

            //Debug.Log("Looking for " + unit);

            foreach (Vector2Int direction in getDirections())
            {
                //Debug.Log("Looking in " + direction);

                MGPumFieldChain chain = new MGPumFieldChain(unit.ownerID, matcher);
                chain.add(unit.field);

                bool chainFinished = false;
                int fieldsSearchedForEnemy = 0;

                Vector2Int position = unit.field.coords;
                position += direction;

                //we go straight into the direction as fast as possible, then see how far we are from an enemy
                while (state.fields.inBounds(position))
                {
                    MGPumField fieldAtPosition = state.fields.getField(position);

                    if(!chainFinished)
                    {
                        if (chain.canAdd(fieldAtPosition))
                        {
                            chain.add(fieldAtPosition);
                        }
                        else
                        {
                            chainFinished = true;
                        }
                    }
                    
                    if(chainFinished)
                    {
                        fieldsSearchedForEnemy++;
                        if (fieldAtPosition.unit != null && fieldAtPosition.unit.ownerID == state.getOpponent(this.playerID).playerID)
                        {
                            //Debug.Log("Found enemy " + fieldAtPosition + "  " + fieldAtPosition.unit.ownerID);
                            if(fieldsSearchedForEnemy <= currentBestDistance)
                            {
                                if (chain.isValidChain())
                                {
                                    //Debug.Log("chain valid");
                                    MGPumMoveCommand mc = new MGPumMoveCommand(this.playerID, chain, unit);
                                    if (stateOracle.checkMoveCommand(mc))
                                    {
                                        currentBestDistance = fieldsSearchedForEnemy;
                                        currentBestCommand = mc;
                                    }
                                }
                            }
                        }
                    }
                    
                    

                    position += direction;
                }
            }
            return currentBestCommand;
        }

    }
    
}
