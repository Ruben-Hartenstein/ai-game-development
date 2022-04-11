using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;
using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumDestroyUnitEffect : MGPumSingleEffect
    {
        private MGPumSelector selector;


        public MGPumDestroyUnitEffect(MGPumSelector selector)
        {
            this.selector = selector;
        }



        internal override void executeInternal(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            List<MGPumEntity> selectedEntities = selection.getSelection(gc.state, source, triggeringEvent, eestate);
            foreach(MGPumEntity e in selectedEntities)
            {
                if(e is MGPumUnit)
                {
                    MGPumUnit t = (MGPumUnit)e;
                    //only destroy if on the battlegrounds
                    if(t.zone == MGPumZoneType.Battlegrounds)
                    {
                        gc.destroyUnit(parent, t);
                    }
                    

                }
                else
                {
                    Debug.LogError("Entity " + e + " is not a unit.");
                }
                
            }

            
        }

        public override MGPumSelector getSelector(MGPumGameState state, MGPumEntity source)
        {
            return selector;
        }

        internal override MGPumEffect deepCopy(MGPumGameState state)
        {
            return new MGPumDestroyUnitEffect(this.selector.deepCopySelector(state));
        }
    }
}
