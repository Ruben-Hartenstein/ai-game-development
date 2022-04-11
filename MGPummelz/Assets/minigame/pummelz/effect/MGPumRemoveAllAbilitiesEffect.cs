using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumRemoveAllAbilitiesEffect : MGPumSingleEffect
    {
        private MGPumSelector selector;

        private bool removeAbilities = false;
     

        public MGPumRemoveAllAbilitiesEffect(MGPumSelector selector, bool removeAbilities)
        {
            this.selector = selector;
            this.removeAbilities = removeAbilities;
       }



        internal override void executeInternal(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            List<MGPumEntity> selectedEntities = selection.getSelection(gc.state, source, triggeringEvent, eestate);
            foreach(MGPumEntity e in selectedEntities)
            {
                if(e is MGPumUnit)
                {
                    MGPumUnit p = (MGPumUnit)e;

                    if(removeAbilities)
                    {
                        gc.removeAbilities(parent, p);
                    }
                   
                    

                }
                else
                {
                    throw new GameException("Entity " + e + " is not a permanent.");
                }
                
            }

            
        }

        public override MGPumSelector getSelector(MGPumGameState state, MGPumEntity source)
        {
            return selector;
        }

        internal override MGPumEffect deepCopy(MGPumGameState state)
        {
            return new MGPumRemoveAllAbilitiesEffect(this.selector.deepCopySelector(state), removeAbilities);
        }
    }
}
