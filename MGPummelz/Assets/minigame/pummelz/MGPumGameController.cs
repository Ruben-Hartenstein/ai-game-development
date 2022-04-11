using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumGameController
    {
        public const int PLAYER0 = 0;
        public const int PLAYER1 = 1;

        private MGPumCommandQueueHandler commandQueueHandler;
        private MGPumEventQueueHandler eventQueueHandler;
        private MGPumEventTriggerHandler eventTriggerHandler;

        private List<MGPumGameResultCallback> gameResultCallbacks;

        public MGPumGameStateHandler stateHandler;
        public MGPumGameState state
        {
            get { return stateHandler.state; }
        }

        public MGPumGameStateOracle oracle;

        private MGPumGameConfig config;
        private MGPumGameCommands gc;
        private MGPumPlayerCommands pc;
        private MGPumGameStateRuleApplier gstaterapplier;


        private MGPumPlayerController[] playerControllers;

        public MGPumGameController(MGPumPlayerController player0, MGPumPlayerController player1, MGPumGameConfig config)
        {

            this.config = config;
            this.playerControllers = new MGPumPlayerController[] { player0, player1 };
            player0.gameController = this;
            player1.gameController = this;

            this.gameResultCallbacks = new List<MGPumGameResultCallback>();


            this.stateHandler = new MGPumGameStateHandler(config);



            foreach (MGPumSetup setup in config.setups)
            {
                setup.apply(this.stateHandler.state);
            }

            this.eventQueueHandler = new MGPumEventQueueHandler();
            this.commandQueueHandler = new MGPumCommandQueueHandler();
            this.eventTriggerHandler = new MGPumEventTriggerHandler(stateHandler.state, this.eventQueueHandler);

            this.oracle = new MGPumGameStateOracle(stateHandler.state);
            this.gc = new MGPumGameCommands(commandQueueHandler, eventTriggerHandler, stateHandler.state, oracle);
            this.pc = new MGPumPlayerCommands(gc, eventTriggerHandler, oracle);
            this.gstaterapplier = new MGPumGameStateRuleApplier(gc);

            player0.setInitialState(stateHandler.state.getInitialStateForPlayer(0));
            player1.setInitialState(stateHandler.state.getInitialStateForPlayer(1));
        }

        internal MGPumGameController(MGPumPlayerController player0, MGPumPlayerController player1, MGPumGameState initialState)
        {

            this.config = null;
            this.playerControllers = new MGPumPlayerController[] { player0, player1 };
            player0.gameController = this;
            player1.gameController = this;

            this.gameResultCallbacks = new List<MGPumGameResultCallback>();

            this.stateHandler = new MGPumGameStateHandler(initialState);
            this.eventQueueHandler = new MGPumEventQueueHandler();
            this.commandQueueHandler = new MGPumCommandQueueHandler();
            this.eventTriggerHandler = new MGPumEventTriggerHandler(stateHandler.state, this.eventQueueHandler);

            this.oracle = new MGPumGameStateOracle(stateHandler.state);
            this.gc = new MGPumGameCommands(commandQueueHandler, eventTriggerHandler, stateHandler.state, oracle);
            this.pc = new MGPumPlayerCommands(gc, eventTriggerHandler, oracle);
            this.gstaterapplier = new MGPumGameStateRuleApplier(gc);

            player0.setInitialState(initialState);
            player1.setInitialState(initialState);
        }

        public void registerGameResultCallback(MGPumGameResultCallback callback)
        {
            this.gameResultCallbacks.Add(callback);
        }

        public void startGame()
        {
            //TODO: determine starting player
            gc.startGame(PLAYER0);

            continueToNextDecision();
        }

        private void gameLoop()
        {
            continueToNextDecision();
        }

        private void notifyPlayers()
        {
            foreach (MGPumPlayerController controller in playerControllers)
            {
                List<MGPumGameEvent> copiedEvents = new List<MGPumGameEvent>();

                try
                {
                    foreach (MGPumGameEvent e in stateHandler.gameLog.getNewEvents(controller.logPosition))
                    {
                        copiedEvents.Add(e);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogException(ex);
                    throw ex;
                }

                controller.updateState(copiedEvents);
                controller.logPosition = stateHandler.gameLog.getCurrentPosition();
            }
        }

        private void nextAction()
        {

            playerControllers[stateHandler.state.activePlayer].allowCommand();
        }


        public void concede(int playerID)
        {
            this.stateHandler.state.concede(playerID);
            finishGame();
        }


        //allow accepting of move from a different thread
        private MGPumCommand queuedCommand;

        public bool acceptCommandAsynchronous(MGPumCommand command)
        {
            if (queuedCommand == null)
            {
                MGPumCommand moveInState = (MGPumCommand)command.deepCopy(stateHandler.state);
                if (oracle.checkCommand(moveInState, true))
                {
                    queuedCommand = command;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public void checkForAsynchronousCommand()
        {
            if (queuedCommand != null)
            {
                MGPumCommand command = queuedCommand;
                queuedCommand = null;
                acceptCommand(command);
            }
        }

        /// <summary>
        /// Accepting a command from the player Controller
        /// </summary>
        /// <param name="cFromPlayer"></param>
        /// <returns></returns>
        public bool acceptCommand(MGPumCommand cFromPlayer)
        {
            if(cFromPlayer == null)
            {
                Debug.LogError("Received null command");
                return false;
            }

            MGPumCommand cNew = (MGPumCommand)cFromPlayer.deepCopy(stateHandler.state);

            bool result = pc.processCommand(cNew);

            if (result)
            {
                gc.recalculateState(cNew);
                continueToNextDecision();
            }

            return result;
        }

        public bool checkCommand(MGPumCommand c, bool checkChain)
        {
            MGPumCommand cNew = (MGPumCommand)c.deepCopy(stateHandler.state);

            return pc.checkCommand(cNew, checkChain);
        }

        private bool executingEffect = false;
        private MGPumEffectSEvent currentEffect = null;
        private MGPumEffectExecutionState currentEffectExecutionState = null;

        public void continueToNextDecision()
        {

            try
            {

                while ((!stateHandler.state.gameFinished) && (!eventQueueHandler.queueEmpty() || !(stateHandler.state.effectQueueEmpty()) || !(commandQueueHandler.queueEmpty()) || executingEffect))
                {

                    //do the event queue
                    MGPumGameEvent nextEvent = eventQueueHandler.getNextEvent();
                    bool eventQueueEmpty = false;



                    MGPumProcessQueuedEffectsSEvent pqese = null;
                    if ((nextEvent == null))
                    {
                        eventQueueEmpty = true;
                    }
                    else
                    {
                        //Debug.Log("Before " + eventQueueHandler.getNextEvent()); 
                        //Debug.Log(stateHandler.state.players[1].getField(MGPumFieldType.Top).unit);
                        eventQueueHandler.consumeNextEvent(stateHandler);


                        if (nextEvent is MGPumEffectSEvent)
                        {//effect is executed as soon as its queue position is reached
                            MGPumEffectSEvent ese = (MGPumEffectSEvent)nextEvent;
                            MGPumEffectExecutionState eestate = new MGPumEffectExecutionState();
                            bool finished = ese.effect.execute(ese, gc, eventTriggerHandler, ese.selection, ese.source, ese.triggeringEvent, eestate);
                            gc.recalculateState(ese, eventTriggerHandler);
                            //we continue execution if necessary
                            if (!finished)
                            {
                                executingEffect = true;
                                currentEffect = ese;
                                currentEffectExecutionState = eestate;
                                //even if the event is not finished, we continue for now, as everything needs to be processed before the next step of the effect can be processed
                            }


                        }
                        if (nextEvent is MGPumComplexGameEvent)
                        {
                            MGPumComplexGameEvent cge = (MGPumComplexGameEvent)nextEvent;

                            //if we have choices we need to interrupt for now
                            MGPumSelector selector = cge.getSelector(stateHandler.state, cge.source);
                            
                            {
                                MGPumSelection selection = selector.getSelection();
                                cge.execute(gc, eventTriggerHandler, selection);
                                if (cge.recalculateStateAfter)
                                {
                                    gc.recalculateState(currentCGE, eventTriggerHandler);
                                }
                            }
                            
                        }

                        if (nextEvent is MGPumProcessQueuedEffectsSEvent)
                        {
                            eventQueueEmpty = true;
                            pqese = (MGPumProcessQueuedEffectsSEvent)nextEvent;

                        }
                        else if (nextEvent is MGPumApplyStateRulesSEvent)
                        {
                            gc.performGameStateRulesApplication((MGPumApplyStateRulesSEvent)nextEvent);
                        }
                        else if (nextEvent is MGPumDetermineResultSEvent)
                        {
                            gc.determineResult((MGPumDetermineResultSEvent)nextEvent);
                        }
                        else if (nextEvent is MGPumNextTurnSEvent)
                        {
                            gc.startTurn(nextEvent, oracle.determineNextTurnPlayer());
                        }


                        if (stateHandler.state.gameFinished)
                        {
                            break;
                        }

                    }




                    if (eventQueueEmpty)
                    {
                        //we processed all queued events, BUT we may still be processing an effect
                        if (executingEffect)
                        {



                            MGPumSelector selector = currentEffect.effect.getSelector(stateHandler.state, currentEffect.source, currentEffectExecutionState);

                            MGPumSelection selection = selector.getSelection();

                            bool finished = currentEffect.effect.execute(currentEffect, gc, eventTriggerHandler, selection, currentEffect.source, currentEffect.triggeringEvent, currentEffectExecutionState);
                            gc.recalculateState(currentEffect, eventTriggerHandler);

                            if (finished)
                            {
                                executingEffect = false;
                            }
                            //if we have not finished, queued events so far will be processed and a new step for this execution has to be made afterwards
                        }
                        else
                        {


                            if (!(stateHandler.state.effectQueueEmpty()))
                            {
                                if (pqese == null)
                                {
                                    pqese = new MGPumProcessQueuedEffectsSEvent();
                                    stateHandler.applyEvent(pqese);
                                }

                                MGPumEffectQueueItem eqi = stateHandler.state.getNextEffect();

                                MGPumDeQueueEffectEvent dqee = new MGPumDeQueueEffectEvent();
                                stateHandler.applyEvent(dqee);
                                dqee.parent = pqese;

                                MGPumSelector selector = eqi.effect.getSelector(stateHandler.state, eqi.source);
                                
                                MGPumSelection selection = selector.getSelection();

                                gc.recalculateState(pqese, eventTriggerHandler);
                                eqi.effect.execute(pqese, gc, eventTriggerHandler, selection, eqi.source, eqi.trigger);
                                
                                

                                

                            }
                        }




                    }
                    //Third, if both other queues are empty, put the next command in the event queue
                    //we queue all children as well
                    //(it will be handled in the next loop iteration)
                    if (!commandQueueHandler.queueEmpty() && !executingEffect && eventQueueHandler.queueEmpty() && stateHandler.state.effectQueueEmpty())
                    {
                        MGPumGameEvent nextCommand = commandQueueHandler.getNextEvent();

                        bool fizzleNextCommand = false;
                        if (nextCommand is MGPumCommandQueueExecutable)
                        {
                            fizzleNextCommand = ((MGPumCommandQueueExecutable)nextCommand).fizzle(stateHandler.state);
                        }
                        if (fizzleNextCommand)
                        {
                            commandQueueHandler.disunitNextEvent();
                        }
                        else
                        {
                            commandQueueHandler.consumeNextEvent(eventTriggerHandler);
                            //first we execute any additional stuff
                            if (nextCommand is MGPumCommandQueueExecutable)
                            {
                                ((MGPumCommandQueueExecutable)nextCommand).execute(gc, stateHandler.state, eventTriggerHandler);
                            }
                            List<MGPumGameEvent> parents = new List<MGPumGameEvent>();
                            parents.Add(nextCommand);
                            //then we queue all other child events
                            while (commandQueueHandler.getNextEvent() != null && parents.Contains(commandQueueHandler.getNextEvent().parent))
                            {
                                parents.Add(commandQueueHandler.getNextEvent());
                                commandQueueHandler.consumeNextEvent(eventTriggerHandler);

                                if (stateHandler.state.gameFinished)
                                {
                                    break;
                                }
                            }
                            //finally we recalculate the state
                            gc.recalculateState(nextCommand, eventTriggerHandler);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error during dequeing loop:" + e);
                Debug.LogError(e);
                throw e;
            }
            notifyPlayers();

            if (stateHandler.state.gameFinished)
            {
                this.finishGame();
            }
            else
            {
                nextAction();
            }
        }

        private void finishGame()
        {
            MGPumGameResult result = new MGPumGameResult(stateHandler.state.result, config);
            result.conceded = this.stateHandler.state.gameConceded;
            foreach (MGPumPlayerController pc in playerControllers)
            {
                pc.finishGame(result);
            }
            foreach(MGPumGameResultCallback grcb in gameResultCallbacks)
            {
                grcb.gameEnded(result);
            }

        }

        private MGPumEffectQueueItem currentEQItem;
        private MGPumProcessQueuedEffectsSEvent currentPQESE;
        private MGPumComplexGameEvent currentCGE;

    }
}
