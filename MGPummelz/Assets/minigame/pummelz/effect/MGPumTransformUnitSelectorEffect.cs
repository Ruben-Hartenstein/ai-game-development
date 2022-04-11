using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumTransformUnitSelectorEffect : MGPumSingleEffect
    {
        private MGPumSelector unitSelector;

        public MGPumAutoSelect newFormFilter;

        public MGPumTransformUnitSelectorEffect(MGPumSelector unitSelector, MGPumAutoSelect newFormSelector)
        {
            this.unitSelector = unitSelector;
            this.newFormFilter = newFormSelector;
        }



        internal override void executeInternal(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            List<MGPumEntity> selectedEntities = selection.getSelection(gc.state, source, triggeringEvent, eestate);
            foreach(MGPumEntity e in selectedEntities)
            {

                if (e is MGPumUnit)
                {
                    List<MGPumEntity> copyTarget = newFormFilter.getSelection(gc.state, source, triggeringEvent, eestate);
                    if(copyTarget.Count > 0 && copyTarget[0] is MGPumUnit)
                    {
                        //gc.transform(parent, (MGPumUnit)e, ((MGPumUnit)copyTarget[0]).unitID);
                        throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new GameException("Entity " + e + " is not a hero.");
                }
                
            }

            
        }

        public override MGPumSelector getSelector(MGPumGameState state, MGPumEntity source)
        {
            return unitSelector;
        }

        internal override MGPumEffect deepCopy(MGPumGameState state)
        {
            return new MGPumTransformUnitSelectorEffect(this.unitSelector.deepCopySelector(state), (MGPumAutoSelect)newFormFilter.deepCopy(state));
        }
    }
}
