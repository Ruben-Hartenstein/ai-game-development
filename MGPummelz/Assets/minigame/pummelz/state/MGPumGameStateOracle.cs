using RelegatiaCCG.rccg.i18n;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumGameStateOracle
    {
        public MGPumGameState state {
            get; set;
        }

        public MGPumGameStateOracle(MGPumGameState state)
        {
            this.state = state;
        }

  
        public int determineNextTurnPlayer()
        {

            if(state.extraTurnQueue.Count == 0)
            {
                if (state.activePlayer == 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return state.extraTurnQueue.Peek();
            }
            
        }




        public bool checkCommand(MGPumCommand c, bool checkChain)
        {
            if (c is MGPumAttackCommand)
            {
                return checkAttackCommand((MGPumAttackCommand)c);
            }
            else if (c is MGPumMoveCommand)
            {
                return checkMoveCommand((MGPumMoveCommand)c);
            }
            if (c is MGPumEndTurnCommand)
            {
                return checkSkipTurnCommandDetailed((MGPumEndTurnCommand)c).result;
            }
            else
            {
                throw new NotImplementedException("Unknown command: " + c.GetType().Name);
            }

        }

        public MGPumOracleBoolResult checkEndTurnCommand(MGPumEndTurnCommand stc)
        {
            MGPumOracleBoolResult result = new MGPumOracleBoolResult();

            MGPumPlayer p = state.players[stc.issuingPlayer];

            if (state.activePlayer != p.playerID)
            {
                result.result = false;
                result.reason = "ERROR_NOT_ACTIVE_PLAYER";
                return result;
            }

            return result;
        }

    

        public MGPumUnit getAttackedUnit(MGPumFieldChain chain)
        {
            if(chain.getLength() > 0)
            {
                return chain.getLast().unit;
            }
            else
            {
                return null;
            }
        }


        public bool canAttack(MGPumUnit c)
        {
            return c.attacksThisTurn < 1;
        }

        public bool checkAttackCommand(MGPumAttackCommand c)
        {
            return checkAttackCommandDetailed(c).result;
        }

        public bool canMove(MGPumUnit c)
        {
            return c.movesThisTurn < 1;
        }

        public bool checkMoveCommand(MGPumMoveCommand c)
        {
            return checkMoveCommandDetailed(c).result;
        }

        public MGPumOracleBoolResult checkCommandDetailed(MGPumCommand c, bool checkChain)
        {

            
            if (c is MGPumEndTurnCommand)
            {
                return checkSkipTurnCommandDetailed((MGPumEndTurnCommand)c);
            }
            else if (c is MGPumAttackCommand)
            {
                return checkAttackCommandDetailed((MGPumAttackCommand)c);
            }
            else if (c is MGPumMoveCommand)
            {
                return checkMoveCommandDetailed((MGPumMoveCommand)c);
            }
            else
            {
                throw new NotImplementedException("Unknown command: " + c.GetType().Name);
            }

        }

        public MGPumOracleBoolResult checkSkipTurnCommandDetailed(MGPumEndTurnCommand stc)
        {
            MGPumOracleBoolResult result = new MGPumOracleBoolResult();

            MGPumPlayer p = state.players[stc.issuingPlayer];

            if (state.activePlayer != p.playerID)
            {
                result.result = false;
                result.reason = "ERROR_NOT_ACTIVE_PLAYER";
                return result;
            }

            return result;
        }

        public MGPumOracleBoolResult checkAttackCommandDetailed(MGPumAttackCommand c)
        {
            MGPumOracleBoolResult result = new MGPumOracleBoolResult();
            result.result = true;
            if(c.issuingPlayer != state.activePlayer || c.chain.ownerID != c.issuingPlayer)
            {
                result.result = false;
                result.reason = "ERROR_CANT_ATTACK_NOT_YOUR_TURN";
                return result;
            }

            MGPumAttackChainMatcher matcher = c.attacker.getAttackMatcher();

            if (!matcher.isValid(c.chain))
            {
                result.result = false;
                result.reason = "ERROR_INVALID_CHAIN_FOR_ATTACK";

                return result;
            }

            if (c.attacker.attacksThisTurn >= 1)
            {
                result.result = false;
                result.reason = "ERROR_ALREADY_ATTACKED";

                return result;
            }

            return result;

        }


        public MGPumOracleBoolResult checkMoveCommandDetailed(MGPumMoveCommand c)
        {
            MGPumOracleBoolResult result = new MGPumOracleBoolResult();
            result.result = true;
            if (c.issuingPlayer != state.activePlayer || c.chain.ownerID != c.issuingPlayer)
            {
                result.result = false;
                result.reason = "ERROR_CANT_MOVE_NOT_YOUR_TURN";
                return result;
            }

            MGPumMoveChainMatcher matcher = c.mover.getMoveMatcher();

            if (!matcher.isValid(c.chain))
            {
                result.result = false;
                result.reason = "ERROR_INVALID_CHAIN_FOR_MOVE";

                return result;
            }

            if (c.mover.movesThisTurn >= 1)
            {
                result.result = false;
                result.reason = "ERROR_ALREADY_MOVED";

                return result;
            }

            return result;

        }



    }
}

