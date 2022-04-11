using System;
using UnityEngine;

namespace mg.pummelz
{
    /// <summary>
    /// Helper class containing command for the game controller, abilities, etc. to use
    /// Commands are applied towards a game state through a GameStateHandler
    /// </summary>
    class MGPumPlayerCommands
    {
        private MGPumGameCommands gc;
        private MGPumGameEventHandler handler;
        private MGPumGameStateOracle oracle;

        public MGPumPlayerCommands(MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumGameStateOracle oracle)
        {
            this.gc = gc;
            this.handler = handler;
            this.oracle = oracle;
        }

        public bool checkCommand(MGPumCommand c, bool checkChain)
        {
            return oracle.checkCommand(c, checkChain);
        }
        
        public bool processCommand(MGPumCommand c)
        {
            if (c is MGPumAttackCommand)
            {
                return processAttackCommand((MGPumAttackCommand)c);
            }
            else if (c is MGPumMoveCommand)
            {
                return processMoveCommand((MGPumMoveCommand)c);
            }
            else if (c is MGPumConcedeGameCommand)
            {
                return processConcedeCommand((MGPumConcedeGameCommand)c);
            }
            else if (c is MGPumEndTurnCommand)
            {
                return processEndTurnCommand((MGPumEndTurnCommand)c);
            }
            else
            {
                throw new NotImplementedException("Unknown command: " + c.GetType().Name);
            }
        }

        public bool processConcedeCommand(MGPumConcedeGameCommand c)
        {

            handler.applyEvent(c);

            MGPumPlayer p = gc.state.getPlayer(c.issuingPlayer);

            gc.concedeGame(c, p);

            return true;

        }

        public bool processEndTurnCommand(MGPumEndTurnCommand c)
        {
            if (!oracle.checkEndTurnCommand(c).result)
            {
                return false;
            }
            else
            {
                handler.applyEvent(c);

                MGPumPlayer p = gc.state.getPlayer(c.issuingPlayer);

                gc.switchTurn(c);

                return true;
            }
           

        }


        public bool processAttackCommand(MGPumAttackCommand c)
        {
            if (!oracle.checkAttackCommand(c))
            {
                return false;
            }
            else
            {
                handler.applyEvent(c);

                gc.attack(c, c.issuingPlayer, c.chain, c.attacker, c.defender);

                return true;
            }
        }


        public bool processMoveCommand(MGPumMoveCommand c)
        {
            if (!oracle.checkMoveCommand(c))
            {
                return false;
            }
            else
            {
                handler.applyEvent(c);

                gc.move(c, c.issuingPlayer, c.chain, c.mover);

                return true;
            }
        }



    }
}
