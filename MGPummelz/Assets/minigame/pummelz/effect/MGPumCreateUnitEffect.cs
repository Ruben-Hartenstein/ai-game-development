using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;
using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumCreateUnitEffect : MGPumSingleEffect
    {
        private MGPumSelector selector;

        internal string unitID;

        public MGPumCreateUnitEffect(string unitID, MGPumSelector selector)
        {
            this.selector = selector;
            this.unitID = unitID;
        }



        internal override void executeInternal(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            List<MGPumEntity> selectedEntities = selection.getSelection(gc.state, source, triggeringEvent, eestate);

            MGPumCreateUnitCEvent lastCTE = null;
            foreach (MGPumEntity e in selectedEntities)
            {
                if(e is MGPumField)
                {
                    MGPumField f = (MGPumField)e;

                    if (f.isEmpty())
                    {
                        lastCTE = gc.createUnit(parent, unitID, source.ownerID, f, source);
                        lastCTE.recalculateStateAfter = false;
                    }
                }
                else
                {
                    Debug.LogError("Entity " + e + " is not a field.");
                }
                
            }
            if(lastCTE != null)
            {
                lastCTE.recalculateStateAfter = true;
            }

            
        }

        public override MGPumSelector getSelector(MGPumGameState state, MGPumEntity source)
        {
            return selector;
        }

        internal override MGPumEffect deepCopy(MGPumGameState state)
        {
            return new MGPumCreateUnitEffect(unitID, this.selector.deepCopySelector(state));
        }
    }
}
