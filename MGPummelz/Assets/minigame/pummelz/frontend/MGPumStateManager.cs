using rccg.frontend.common;
using System.Collections.Generic;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumStateManager : MonoBehaviour
    {
        public MGPummelz controller;

        public MGPumLineAnimator lineAnimator;

        public float animSpeed = 0.7f;

        internal MGPumGameState shownState;
        internal MGPumGameStateHandler shownStateHandler;
        internal MGPumGameStateOracle shownStateOracle;
        internal MGPumGameState knownState;
        internal MGPumGameStateHandler knownStateHandler;
        internal MGPumGameStateOracle knownStateOracle;
        



        Queue<MGPumGameEvent> eventsToVisualize;

        MGPumGameEvent currentEvent;

        internal void setInitialState(MGPumGameState state)
        {
            shownState = state;
            shownStateOracle = new MGPumGameStateOracle(shownState);
            shownStateHandler = new MGPumGameStateHandler(shownState);
            knownState = state.deepCopy();
            knownStateOracle = new MGPumGameStateOracle(knownState);
            knownStateHandler = new MGPumGameStateHandler(knownState);
        }







        internal void init()
        {
            currentEvent = null;
            eventsToVisualize = new Queue<MGPumGameEvent>();
        }

        internal void enqueueEvents(List<MGPumGameEvent> events)
        {
            foreach (MGPumGameEvent ev in events)
            {
                {
                    //immediately apply to known state
                    
                    MGPumGameEvent ksev = ev.deepCopy(knownState);
                    knownStateHandler.applyEvent(ksev);
                }

                eventsToVisualize.Enqueue(ev);
            }
            if (currentEvent == null)
            {
                visualizeNext();
            }
        }



        internal void visualizeNext()
        {
            bool visualized = false;
            if (currentEvent == null && eventsToVisualize.Count > 0)
            {
                currentEvent = eventsToVisualize.Dequeue();

                if (currentEvent is MGPumNewTurnEvent)
                {
                    MGPumNewTurnEvent nte = (MGPumNewTurnEvent)currentEvent;
                    controller.textField.setText(MGPummelz.getColorForPlayer(nte.playerID) + " moves!");
                    controller.endTurnButton.switchTurn(nte.playerID, controller.playerControllers[nte.playerID] is MGPumGUIPlayerController);
                    visualized = true;
                }
                else if (currentEvent is MGPumEndGameEvent)
                {
                    MGPumEndGameEvent nte = (MGPumEndGameEvent)currentEvent;
                    if(nte.result == MGPumGameResultType.WinPlayer0|| nte.result == MGPumGameResultType.WinPlayer1)
                    {
                        controller.textField.setText(MGPummelz.getColorForPlayer((int)nte.result) + " wins!");
                    }
                    else
                    {
                        controller.textField.setText("Draw...");
                    }
                    visualized = true;
                }
                else if (currentEvent is MGPumDamageUnitEvent)
                {


                    MGPumDamageUnitEvent de = (MGPumDamageUnitEvent)currentEvent;
                    MGPumUnitRender unitR = controller.getUnitRender(de.damagedUnit.id);

                    unitR.showHealthbarAnim = true;

                    float maxHealth = unitR.unit.currentMaxHealth;
                    float healthBefore = unitR.unit.currentHealth;
                    float healthAfter = healthBefore - de.damage;

                    unitR.healthBar.setBar(healthBefore / maxHealth);
                    unitR.healthBar.moveBarTo(healthAfter / maxHealth);
                    StartCoroutine(unitR.healthBar.startAnimation(eventVisualized, 0));
                }
                else if (currentEvent is MGPumHealUnitEvent)
                {


                    MGPumHealUnitEvent de = (MGPumHealUnitEvent)currentEvent;
                    MGPumUnitRender unitR = controller.getUnitRender(de.healedUnit.id);

                    unitR.showHealthbarAnim = true;

                    float maxHealth = unitR.unit.currentMaxHealth;
                    float healthBefore = unitR.unit.currentHealth;
                    float healthAfter = healthBefore + de.amount;



                    unitR.healthBar.setBar(healthBefore / maxHealth);
                    unitR.healthBar.moveBarTo(healthAfter / maxHealth);
                    StartCoroutine(unitR.healthBar.startAnimation(eventVisualized, 0));
                }
                else if (currentEvent is MGPumDestroyUnitSEvent)
                {
                    MGPumDestroyUnitSEvent de = (MGPumDestroyUnitSEvent)currentEvent;
                    MGPumUnitRender unitR = controller.getUnitRender(de.unit.id);
                    MGPumFieldRender fieldR = controller.getFieldRender(de.unitBefore.field);
                    fieldR.unitRender = null;
                    unitR.showHighlights = false;

                    unitR.dissolveAnimator.setCurrentAsHome();
                    unitR.dissolveAnimator.setCurve(GUICurves.Linear);
                    unitR.dissolveAnimator.setFixedSpeed(Mathf.Max(animSpeed - 0.2f, 0.1f));
                    unitR.dissolveAnimator.dissolve();
                    StartCoroutine(unitR.dissolveAnimator.startAnimation(eventVisualized, 0));
                }
                else if (currentEvent is MGPumCreateUnitCEvent)
                {
                    MGPumCreateUnitCEvent cuce = (MGPumCreateUnitCEvent)currentEvent;
                    MGPumUnitRender unitR = controller.createUnitRender(shownState.lookupUnit(cuce.ice.instantiatedUnit.id), (MGPumField)shownState.lookupEntity(cuce.destination));
                    MGPumFieldRender fieldR = controller.getFieldRender(cuce.destination);
                    fieldR.unitRender = unitR;
                    unitR.dissolveAnimator.setInvisibleImmediately();
                    unitR.dissolveAnimator.setCurve(GUICurves.Linear);
                    unitR.dissolveAnimator.setFixedSpeed(Mathf.Max(animSpeed + 0.3f, 0.1f));
                    unitR.dissolveAnimator.appear();
                    StartCoroutine(unitR.dissolveAnimator.startAnimation(eventVisualized, 0));
                }
                else if (currentEvent is MGPumMoveUnitEvent)
                {
                    MGPumMoveUnitEvent me = (MGPumMoveUnitEvent)currentEvent;
                    MGPumUnitRender unitR = controller.getUnitRender(me.movingUnit);
                    MGPumFieldRender oldFieldR = controller.getFieldRender(me.originField);
                    MGPumFieldRender newFieldR = controller.getFieldRender(me.targetField);
                    oldFieldR.unitRender = null;
                    newFieldR.unitRender = unitR;
                    unitR.showHighlights = false;


                    //we set as last sibling of board to be rendered in front of all other pieces
                    unitR.transform.SetParent(controller.board, true);
                    unitR.imageAnimator.setCurrentAsHomeOldNew();
                    unitR.imageAnimator.setCurve(GUICurves.CubicEaseInOut);
                    unitR.imageAnimator.setFixedSpeed(Mathf.Max(animSpeed, 0.1f));
                    unitR.imageAnimator.moveToPosition(newFieldR.transform.localPosition);
                    StartCoroutine(unitR.imageAnimator.startAnimation(eventVisualized, 0));
                }
                else if (currentEvent is MGPumAttackSEvent)
                {
                    MGPumAttackSEvent me = (MGPumAttackSEvent)currentEvent;
                    MGPumUnitRender attackerR = controller.getUnitRender(me.attacker);
                    MGPumUnitRender defenderR = controller.getUnitRender(me.defender);

                    lineAnimator.setCurve(GUICurves.QuadEaseInOut);
                    lineAnimator.setFixedSpeed(Mathf.Max(animSpeed, 0.1f));
                    lineAnimator.showLine(attackerR.transform, defenderR.transform);

                    StartCoroutine(lineAnimator.startAnimation(eventVisualized, 0));
                }
                else if (currentEvent is MGPumStartGameSEvent
                    || currentEvent is MGPumStartGameRunningEvent
                    || currentEvent is MGPumStartTurnSEvent
                    || currentEvent is MGPumInitialSpawnSEvent
                    || currentEvent is MGPumRecalculateStateEvent
                    || currentEvent is MGPumApplyStateRulesSEvent
                    || currentEvent is MGPumQueueEffectEvent
                    || currentEvent is MGPumDeQueueEffectEvent
                    || currentEvent is MGPumProcessQueuedEffectsSEvent
                    || currentEvent is MGPumEffectSEvent
                    || currentEvent is MGPumDetermineResultSEvent
                    || currentEvent is MGPumZoneChangeEvent
                    )

                {
                    visualized = true;
                }
                else
                {
                    //Debug.LogWarning("Unknown event: " + currentEvent);
                    visualized = true;
                }
               

                if (visualized)
                {
                    
                    eventVisualized();
                }
                    
            }
            
        }

        internal void eventVisualized(int dummy)
        {
            eventVisualized();
        }

        internal void eventVisualized()
        {
            
            if (currentEvent != null)
            {
                {
                    //apply to shown state AFTER animation
                    MGPumGameEvent ssev = currentEvent.deepCopy(shownState);
                    shownStateHandler.applyEvent(ssev);
                }

                //post state apply actions
                if (currentEvent is MGPumMoveUnitEvent)
                {
                    MGPumMoveUnitEvent me = (MGPumMoveUnitEvent)currentEvent;
                    MGPumUnitRender unitR = controller.getUnitRender(me.movingUnit);
                    MGPumFieldRender newFieldR = controller.getFieldRender(me.targetField);
                    unitR.transform.SetParent(newFieldR.transform, true);
                    unitR.showHighlights = true;
                }
                if (currentEvent is MGPumDestroyUnitSEvent)
                {
                    MGPumDestroyUnitSEvent me = (MGPumDestroyUnitSEvent)currentEvent;
                    MGPumUnitRender unitR = controller.getUnitRender(me.unit);
                    unitR.gameObject.SetActive(false);

                }
                else if (currentEvent is MGPumHealUnitEvent)
                {
                    MGPumHealUnitEvent he = (MGPumHealUnitEvent)currentEvent;
                    MGPumUnitRender unitR = controller.getUnitRender(he.healedUnit);
                    if(he.healedUnit.damage == 0)
                    {
                        unitR.showHealthbarAnim = false;
                    }
                }
                else if (currentEvent is MGPumRecalculateStateEvent|| currentEvent is MGPumNewTurnEvent)
                {
                    foreach(MGPumUnitRender unitR in controller.unitrenders.Values) {
                        unitR.updateUnitRender();
                    }
                }
                else if (currentEvent is MGPumUnitReferencable)
                {
                    MGPumUnitReferencable ur = (MGPumUnitReferencable)currentEvent;
                    MGPumUnitRender unitR = controller.getUnitRender(ur.getReferencableUnit());
                    unitR.updateUnitRender();
                }
                //Debug.Log(currentEvent);

                currentEvent = null;
                
            }
        }


        public void Update()
        {
            visualizeNext();
        }


    }
}
