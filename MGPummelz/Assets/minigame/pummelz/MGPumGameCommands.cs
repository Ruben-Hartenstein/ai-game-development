using RelegatiaCCG.rccg.engine;
using RelegatiaCCG.rccg.engine.exceptions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace mg.pummelz
{
    /// <summary>
    /// Helper class containing command for the game controller, abilities, etc. to use
    /// Commands are applied towards a game state through a GameStateHandler
    /// </summary>
    public class MGPumGameCommands
    {
        private MGPumGameEventHandler commandHandler;
        private MGPumGameEventHandler handler;
        private MGPumGameStateOracle oracle;

        public MGPumGameState state { get; internal set; }

        public MGPumGameCommands(MGPumGameEventHandler commandHandler, MGPumGameEventHandler handler, MGPumGameState state, MGPumGameStateOracle oracle)
        {
            this.commandHandler= commandHandler;
            this.handler = handler;
            this.state = state;
            this.oracle = oracle;
        }





        internal void transform(MGPumGameEvent parent, MGPumUnit unit, string formID)
        {
            MGPumTransformUnitEvent gae = new MGPumTransformUnitEvent(unit, formID);
            gae.parent = parent;
            handler.applyEvent(gae);
        }

        //internal void grantAbility(MGPumGameEvent parent, MGPumUnit permanent, MGPumAbility ability)
        //{
        //    MGPumGrantAbilityEvent gae = new MGPumGrantAbilityEvent(permanent, ability);
        //    gae.parent = parent;
        //    handler.applyEvent(gae);
        //}

        internal void removeAbilities(MGPumGameEvent parent, MGPumUnit unit)
        {
            MGPumRemoveAllAbilitiesEvent raae = new MGPumRemoveAllAbilitiesEvent(unit);
            raae.parent = parent;
            handler.applyEvent(raae);
        }


        public void grantExtraTurn(MGPumGameEvent parent, int playerID)
        {
            MGPumGrantExtraTurnEvent dcse = new MGPumGrantExtraTurnEvent(playerID);
            dcse.parent = parent;
            handler.applyEvent(dcse);
        }


        public void startGame(int startPlayerID)
        {

            if (state.gameRunning)
            {
                throw new GameException("Cannot start game - already running");
            }

            //all the setup first

            //TODO: determineStartingPlayer
            //int startingPlayer = 0;
            MGPumStartGameSEvent sge = new MGPumStartGameSEvent();
            handler.applyEvent(sge);

            MGPumInitialSpawnSEvent isse = new MGPumInitialSpawnSEvent();
            isse.parent = sge;
            handler.applyEvent(isse);

            //TODO: spawnInitial

            //game starts after setup

            startGameRunning(sge);

            //first turn begins

            startTurn(sge, startPlayerID);


        }

    
        public void startGameRunning(MGPumGameEvent parent)
        {
            MGPumStartGameRunningEvent e = new MGPumStartGameRunningEvent();
            e.parent = parent;
            handler.applyEvent(e);

            //state rules cannot be applied before the first turn starts
            recalculateStateWithoutRuleApplication(parent);
        }

        public void switchTurn(MGPumGameEvent parent)
        {
            endTurn(null);

            nextTurn(null, commandHandler);

        }

        public void endTurn(MGPumGameEvent parent)
        {
            MGPumEndTurnSEvent ete = new MGPumEndTurnSEvent(state.activePlayer);
            ete.parent = parent;
            handler.applyEvent(ete);

        }

        public void nextTurn(MGPumGameEvent parent, MGPumGameEventHandler handlerToUse)
        {
            MGPumNextTurnSEvent nte = new MGPumNextTurnSEvent();
            nte.parent = parent;
            handlerToUse.applyEvent(nte);
        }


        public void startTurn(MGPumGameEvent parent, int playerID)
        {
            MGPumStartTurnSEvent ste = new MGPumStartTurnSEvent(playerID);
            ste.parent = parent;
            handler.applyEvent(ste);

            //Increase turn counter, switch active player, etc.
            MGPumNewTurnEvent nte = new MGPumNewTurnEvent(playerID);
            nte.parent = ste;
            handler.applyEvent(nte);

            recalculateState(ste);


        }

        internal void fizzleEffect(MGPumGameEvent parent, MGPumOneTimeEffect effect, MGPumGameEvent trigger, MGPumEntity source, MGPumGameEventHandler handlerToUse)
        {
            MGPumFizzleEffectEvent fze = new MGPumFizzleEffectEvent(effect, trigger, source);
            fze.parent = parent;
            handlerToUse.applyEvent(fze);
        }

        public void damageUnit(MGPumGameEvent parent, MGPumEntity source, MGPumUnit damagedUnit, int damage, bool isAttackDamage)
        {
            damageUnit(parent, source, damagedUnit, damage, isAttackDamage, handler);
        }

        public void damageUnit(MGPumGameEvent parent, MGPumEntity source, MGPumUnit damagedUnit, int damage, bool isAttackDamage, MGPumGameEventHandler handlerToUse)
        {
            if (damagedUnit.zone == MGPumZoneType.Battlegrounds)
            {
                MGPumDamageUnitEvent dce = new MGPumDamageUnitEvent(source, damagedUnit, damage, isAttackDamage);
                dce.parent = parent;
                handlerToUse.applyEvent(dce);
            }
        }

        public void healUnit(MGPumGameEvent parent, MGPumEntity source, MGPumUnit healedUnit, int amount)
        {
            if(healedUnit.zone == MGPumZoneType.Battlegrounds)
            {
                MGPumHealUnitEvent hce = new MGPumHealUnitEvent(source, healedUnit, amount);
                hce.parent = parent;
                handler.applyEvent(hce);
            }
            
        }

        public void removeDamageFromUnit(MGPumGameEvent parent, MGPumEntity source, MGPumUnit unit)
        {
            MGPumRemoveDamageEvent hce = new MGPumRemoveDamageEvent(source, unit);
            hce.parent = parent;
            handler.applyEvent(hce);
        }

        public void attack(MGPumGameEvent parent, int playerID, MGPumFieldChain chain, MGPumUnit attacker, MGPumUnit defender)
        {
            if(chain.ownerID != playerID)
            {
                Debug.LogError("Attacker is " + playerID + " but chain belongs to " + chain.ownerID);
            }

            int damage = attacker.currentPower;

            MGPumAttackSEvent ae = new MGPumAttackSEvent(playerID, chain, damage, attacker, defender);
            ae.parent = parent;
            handler.applyEvent(ae);

            damageUnit(ae, attacker, defender, damage, true);

        }

        public void move(MGPumGameEvent parent, int playerID, MGPumFieldChain chain, MGPumUnit mover)
        {
            if (chain.ownerID != playerID)
            {
                Debug.LogError("Attacker is " + playerID + " but chain belongs to " + chain.ownerID);
            }

            MGPumMoveSEvent ae = new MGPumMoveSEvent(playerID, chain, mover);
            ae.parent = parent;
            handler.applyEvent(ae);

            bool firstSkipped = false;
            MGPumField previousField = null;
            foreach (MGPumField field in chain)
            {
                if(!firstSkipped)
                {
                    previousField = field;
                    firstSkipped = true;
                    continue;
                }

                moveUnit(ae, mover, previousField, field, handler);
                previousField = field;
            }
        }

        public void moveUnit(MGPumGameEvent parent, MGPumUnit mover, MGPumField originField, MGPumField targetField, MGPumGameEventHandler handlerToUse)
        {
            MGPumMoveUnitEvent mue = new MGPumMoveUnitEvent(mover, originField, targetField);
            mue.parent = parent;
            handlerToUse.applyEvent(mue);
        }



        public void buffUnit(MGPumGameEvent parent, MGPumUnit unit, int healthBuff)
        {
            MGPumBuffUnitEvent bce = new MGPumBuffUnitEvent(unit, healthBuff);
            bce.parent = parent;
            handler.applyEvent(bce);

        }

        public void setHealth(MGPumGameEvent parent, MGPumUnit unit, int health)
        {
            MGPumSetHealthEvent spe = new MGPumSetHealthEvent(unit, health);
            spe.parent = parent;
            handler.applyEvent(spe);
        }

        public MGPumCreateUnitCEvent createUnit(MGPumGameEvent parent, string unitID, int ownerID, MGPumField f, MGPumEntity source)
        {
            MGPumInstantiateUnitEvent ice = new MGPumInstantiateUnitEvent(unitID, ownerID);
            ice.parent = parent;
            handler.applyEvent(ice);

            MGPumCreateUnitCEvent cte = new MGPumCreateUnitCEvent(ice, f, source);
            cte.parent = parent;
            handler.applyEvent(cte);
            return cte;
        }

        public void spawnCreatedUnit(MGPumCreateUnitCEvent parent, MGPumGameEventHandler handler)
        {
            MGPumCreateUnitEvent cte = new MGPumCreateUnitEvent(parent.ice, parent.destination);
            cte.parent = parent;
            handler.applyEvent(cte);

        }


        public void destroyUnit(MGPumGameEvent parent, MGPumUnit unit)
        {
            MGPumField f = state.getFieldForUnit(unit);

            //if the unit is not on the battleground we don't need to do anything
            if (f != null)
            {
                MGPumDestroyUnitSEvent dte = new MGPumDestroyUnitSEvent(unit);
                dte.parent = parent;
                handler.applyEvent(dte);
                
                changeZone(dte, unit, f, new MGPumSimpleZoneURI(unit.ownerID, MGPumZoneType.Destroyed));
            }



        }

        public void changeZone(MGPumGameEvent parent, MGPumUnit unit, MGPumZoneURI origin, MGPumZoneURI destination)
        {
            changeZone(parent, unit, origin, destination, handler, false);
        }

        public void changeZone(MGPumGameEvent parent, MGPumUnit unit, MGPumZoneURI origin, MGPumZoneURI destination, bool revealInTransit)
        {
            changeZone(parent, unit, origin, destination, handler, revealInTransit);
        }

        public void changeZone(MGPumGameEvent parent, MGPumUnit unit, MGPumZoneURI origin, MGPumZoneURI destination, MGPumGameEventHandler handlerToUse, bool revealInTransit)
        {
            MGPumZoneChangeEvent ze = new MGPumZoneChangeEvent(unit, origin, destination);
            ze.parent = parent;
            handlerToUse.applyEvent(ze);
        }


        internal void determineResult(MGPumGameEvent parent)
        {
            MGPumPlayer p0 = state.players[MGPumGameController.PLAYER0];
            MGPumPlayer p1 = state.players[MGPumGameController.PLAYER1];

            MGPumGameResultType result = MGPumGameResultType.NotYet;

            //if both players are losing the game ends in a draw
            if (p0.losing && p1.losing)
            {
                result = MGPumGameResultType.Draw;
            }
            //if one player is losing and the other is not, that player is winning
            else if (!p0.losing && p1.losing)
            {
                result = MGPumGameResultType.WinPlayer0;
            }
            else if (p0.losing && !p1.losing)
            {
                result = MGPumGameResultType.WinPlayer1;
            }
            //if no player is losing, a player may be winning (e.g. by some alternate victory unit)
            else if (p0.winning && p1.winning)
            {
                result = MGPumGameResultType.Draw;
            }
            //if one player is losing and the other is not, that player is winning
            else if (p0.winning && !p1.winning)
            {
                result = MGPumGameResultType.WinPlayer0;
            }
            else if (!p0.winning && p1.winning)
            {
                result = MGPumGameResultType.WinPlayer1;
            }


            if (result != MGPumGameResultType.NotYet)
            {
                //the game is decided
                MGPumEndGameEvent e = new MGPumEndGameEvent(result);
                e.parent = parent;
                handler.applyEvent(e);
            }

        }

        public void recalculateState(MGPumGameEvent parent)
        {
            recalculateState(parent, handler);
        }

        public void recalculateStateWithoutRuleApplication(MGPumGameEvent parent)
        {
            MGPumRecalculateStateEvent rse = new MGPumRecalculateStateEvent();
            rse.parent = parent;
            handler.applyEvent(rse);
        }

        public void recalculateState(MGPumGameEvent parent, MGPumGameEventHandler handlerToUse)
        {
            MGPumRecalculateStateEvent rse = new MGPumRecalculateStateEvent();
            rse.parent = parent;
            handlerToUse.applyEvent(rse);
            applyGameStateRules(parent, handlerToUse);
        }

        public void applyGameStateRules(MGPumGameEvent parent, MGPumGameEventHandler handlerToUse)
        {
            MGPumApplyStateRulesSEvent sre = new MGPumApplyStateRulesSEvent();
            sre.parent = parent;
            handlerToUse.applyEvent(sre);
        }

        public void concedeGame(MGPumGameEvent parent, MGPumPlayer player)
        {
            loseGame(parent, player);
        }

        public void winGame(MGPumGameEvent parent, MGPumPlayer player)
        {
            MGPumWinGameEvent wge = new MGPumWinGameEvent(player.playerID);
            wge.parent = parent;
            handler.applyEvent(wge);
        }

        public void loseGame(MGPumGameEvent parent, MGPumPlayer player)
        {
            MGPumLoseGameEvent wge = new MGPumLoseGameEvent(player.playerID);
            wge.parent = parent;
            handler.applyEvent(wge);
        }

        public void performGameStateRulesApplication(MGPumApplyStateRulesSEvent sre)
        {
            //check victory conditions
            bool possibleVictory = false;

            bool recalculateStateAgain = false;
            //check if anything dies


            foreach (MGPumPlayer p in state.players)
            {
                possibleVictory = possibleVictory || p.losing || p.winning;
            }

            foreach (MGPumField f in state.fields)
            {
                if (f.unit != null)
                {
                    if (f.unit.currentHealth <= 0)
                    {
                        destroyUnit(sre, f.unit);
                        recalculateStateAgain = true;
                        possibleVictory = true;
                    }
                }
            }

            if (possibleVictory)
            {
                MGPumDetermineResultSEvent e = new MGPumDetermineResultSEvent();
                e.parent = sre;
                handler.applyEvent(e);
            }
            if (recalculateStateAgain)
            {
                this.recalculateState(sre);
            }
        }
    }

    
}
