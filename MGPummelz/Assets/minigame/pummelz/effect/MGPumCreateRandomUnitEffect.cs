using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumCreateRandomUnitEffect : MGPumSingleEffect
    {
        private MGPumRandomSelect unitSelect;
        private MGPumSelector selector;


        public MGPumCreateRandomUnitEffect(MGPumRandomSelect unitSelect, MGPumSelector selector)
        {
            this.unitSelect = unitSelect;
            this.selector = selector;
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
                        //different random draw for each field
                        string unitID = ((MGPumUnit)(unitSelect.getSelection().getSelection(gc.state, source, triggeringEvent, eestate).First())).unitID;
                        lastCTE = gc.createUnit(parent, unitID, f.ownerID, f, source);
                        lastCTE.recalculateStateAfter = false;
                    }
                }
                else
                {
                    Debug.LogError("Entity " + e + " is not a field.");
                }
                
            }
            if (lastCTE != null)
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
            return new MGPumCreateRandomUnitEffect((MGPumRandomSelect)unitSelect.deepCopy(state), this.selector.deepCopySelector(state));
        }
    }
}
