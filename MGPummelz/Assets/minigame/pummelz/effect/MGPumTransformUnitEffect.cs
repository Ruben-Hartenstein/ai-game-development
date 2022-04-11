using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumTransformUnitEffect : MGPumSingleEffect
    {
        private MGPumSelector selector;

        public string newFormID;

        public MGPumTransformUnitEffect(MGPumSelector unitSelector, string newFormID)
        {
            this.selector = unitSelector;
            this.newFormID = newFormID;
        }



        internal override void executeInternal(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            List<MGPumEntity> selectedEntities = selection.getSelection(gc.state, source, triggeringEvent, eestate);
            foreach(MGPumEntity e in selectedEntities)
            {
                if(e is MGPumUnit)
                {
                    gc.transform(parent, (MGPumUnit)e, newFormID);
                }
                else
                {
                    throw new GameException("Entity " + e + " is not a hero.");
                }
                
            }

            
        }

        public override MGPumSelector getSelector(MGPumGameState state, MGPumEntity source)
        {
            return selector;
        }

        internal override MGPumEffect deepCopy(MGPumGameState state)
        {
            return new MGPumTransformUnitEffect(this.selector.deepCopySelector(state), newFormID);
        }
    }
}
