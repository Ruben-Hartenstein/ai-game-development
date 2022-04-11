using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace mg.pummelz
{
    public class MGPumInputManager : MonoBehaviour
    {

        public LineRenderer line;
        public MGPummelz mainController;

        public MGPumUnitToolTip unitToolTip;

        internal MGPumFieldChain currentChain;
        internal MGPumFieldChainMatcher currentMatcher;

        internal List<Vector3> linePositions;

        internal int playingPlayerID = -1;



        public void endTurnButtonclicked()
        {
            if(playingPlayerID == mainController.stateManager.knownState.activePlayer)
            {
                MGPumEndTurnCommand etc = new MGPumEndTurnCommand(playingPlayerID);
                bool result = mainController.gameController.acceptCommand(etc);
                if (!result)
                {
                    Debug.LogError("GameController didn't accept command from GUI: " + etc);
                }
            }
        }

        public void fieldPointerDown(MGPumFieldRender fr, PointerEventData.InputButton button)
        {

            if(PointerEventData.InputButton.Middle == button)
            {
                if(fr.unitRender != null)
                {
                    unitToolTip.show(fr.unitRender, fr.gameObject);
                }
            }
            else 
            {
                if (currentChain != null)
                {
                    stopChain();
                }
                if (currentChain == null)
                {
                    startChain(fr, PointerEventData.InputButton.Right == button);
                }
            }
        }

        public void fieldPointerUp(MGPumFieldRender fr)
        {
            if (currentChain != null)
            {
                if(currentChain.getLength() <= 1)
                {
                    stopChain();
                }
                else
                {
                    finishChain();
                }
            }
            if(unitToolTip.gameObject.activeSelf)
            {
                unitToolTip.hide();
            }
        }

        public void fieldPointerEnter(MGPumFieldRender fr)
        {
            if (currentChain != null)
            {
                if(currentChain.getSecondToLast() == fr.field)
                {
                    removeLastFromChain();
                }
                else
                {
                    addToChain(fr);
                }
                
            }
        }

        public void fieldPointerExit(MGPumFieldRender fr)
        {
            //nothing to do yet
        }




        public void startChain(MGPumFieldRender fr, bool rightClick)
        {
            if(fr.unitRender != null)
            {
                currentMatcher = getChainMatcher(fr.unitRender.unit, playingPlayerID, rightClick);
                if (currentMatcher != null)
                {
                    currentChain = new MGPumFieldChain(playingPlayerID, currentMatcher);
                    startLine();
                    addToChain(fr);
                }
            }
        }

        public void addToChain(MGPumFieldRender fr)
        {
            if(currentChain.canAdd(fr.field))
            {
                currentChain.add(fr.field);
                addToLine(fr);
                updateTooltip(true);
            }
        }

        internal bool highlightUnit(MGPumUnit unitShown)
        {
            return unitShown.ownerID == playingPlayerID
                && mainController.stateManager.shownState.activePlayer == unitShown.ownerID;
        }

        public void removeLastFromChain()
        {
            currentChain.removeLast();
            removeLastFromLine();
            updateTooltip(true);
        }

        public MGPumFieldChainMatcher getChainMatcher(MGPumUnit unit, int playingPLayerID, bool rightClick)
        {

            MGPumUnit unitKnownState = mainController.stateManager.knownState.lookupUnit(unit.id);

            if (unit.ownerID != playingPLayerID)
            {
                return null;
            }
            else
            {
                if(rightClick)
                {
                    if(mainController.stateManager.knownStateOracle.canAttack(unitKnownState))
                    {
                        return unit.getAttackMatcher();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (mainController.stateManager.knownStateOracle.canMove(unitKnownState))
                    {
                        return unit.getMoveMatcher();
                    }
                    else
                    {
                        return null;
                    }
                    
                }
            }
        }

        public void finishChain()
        {
            if (this.currentMatcher.isValid(currentChain))
            {
                executeChainCommand();
            }

            this.currentMatcher = null;
            this.currentChain = null;
            stopLine();
            updateTooltip(false);
        }

        public void executeChainCommand()
        {
            MGPumFieldChain chainInKnownState = currentChain.deepCopy(mainController.stateManager.knownState);

            MGPumCommand command = null;
            MGPumOracleBoolResult commandResult = null;

            if (currentMatcher is MGPumAttackChainMatcher)
            {
                command = new MGPumAttackCommand(chainInKnownState.ownerID, chainInKnownState, chainInKnownState.getFirst().unit);
                commandResult = mainController.stateManager.knownStateOracle.checkAttackCommandDetailed((MGPumAttackCommand)command);
            }
            else if (currentMatcher is MGPumMoveChainMatcher)
            {
                command = new MGPumMoveCommand(chainInKnownState.ownerID, chainInKnownState, chainInKnownState.getFirst().unit);
                commandResult = mainController.stateManager.knownStateOracle.checkMoveCommandDetailed((MGPumMoveCommand)command);
            }
            else
            {
                Debug.LogError("Unknown matcher: " + currentMatcher);
            }


            if (commandResult.result)
            {
                if(((MGPumGUIPlayerController)mainController.playerControllers[command.issuingPlayer]).checkCommandFromGUI(command, true))
                {
                    bool result = mainController.gameController.acceptCommand(command);
                    if (!result)
                    {
                        Debug.LogError("GameController didn't accept command from GUI: " + command);
                    }
                }
            }
            else
            {
                mainController.textField.setText(commandResult.reason);
            }
        }

        public void stopChain()
        {
            this.currentMatcher = null;
            this.currentChain = null;
            stopLine();
            updateTooltip(false);
        }

        

        public void startLine()
        {
            linePositions = new List<Vector3>();
            this.line.gameObject.SetActive(true);
        }

        public void stopLine()
        {
            lineUpdated(false);
            this.line.gameObject.SetActive(false);
        }

        public void addToLine(MGPumFieldRender fr)
        {
            //make sure line is sorted correctly
            Vector3 newPos = fr.transform.position;
            newPos.z = -1;

            linePositions.Add(newPos);
            

            lineUpdated(true);
        }

        public void lineUpdated(bool show)
        {
            if (show)
            {
                this.line.positionCount = linePositions.Count;
                this.line.SetPositions(linePositions.ToArray());
            }
            else
            {
                //this.line.gameObject.SetActive(false);
            }
        }

        public void removeLastFromLine()
        {
            linePositions.RemoveAt(linePositions.Count - 1);

            lineUpdated(true);
        }

        public void updateTooltip(bool show)
        {
            
            if(show && currentChain != null && currentChain.last != null)
            {
                MGPumFieldRender lastFR = mainController.getFieldRender(currentChain.last);

                if (currentMatcher is MGPumAttackChainMatcher)
                {
                    if(currentChain.last.unit != null && currentChain.last.unit != currentMatcher.startUnit)
                    {
                        mainController.toolTip.show(currentChain.last.unit.currentHealth + "H - " + currentMatcher.startUnit.currentPower + "P", lastFR);
                    }
                    else
                    {
                        mainController.toolTip.show(currentMatcher.startUnit.currentPower + "P", lastFR);
                    }
                }
                else if (currentMatcher is MGPumMoveChainMatcher)
                {
                    mainController.toolTip.show((currentChain.getLength() - 1) + "/" + currentMatcher.startUnit.currentSpeed + "S", lastFR);
                }
                else
                {
                    Debug.LogError("Unknown matcher: " + currentMatcher);
                }
            }
            else
            {
                mainController.toolTip.hide();
            }
        }
    }
}
