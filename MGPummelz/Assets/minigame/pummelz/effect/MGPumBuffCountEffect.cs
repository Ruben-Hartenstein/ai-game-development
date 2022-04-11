using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumBuffCountEffect : MGPumSingleEffect
    {
        private MGPumSelector selector;

        public int healthBuffPerCount { get; set; }

        public MGPumCount count;

           protected MGPumBuffCountEffect()
        {
           
        }

        


        public MGPumBuffCountEffect(MGPumCount count, MGPumSelector selector) : this()
        {
            this.healthBuffPerCount = 1;
            this.count = count;
            this.selector = selector;
        }

        public MGPumBuffCountEffect(int healthBuffPerCount, MGPumCount count, MGPumSelector selector) : this()
        {
            this.healthBuffPerCount = healthBuffPerCount;
            this.count = count;
            this.selector = selector;
        }

     


        internal override void executeInternal(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            List<MGPumEntity> selectedEntities = selection.getSelection(gc.state, source, triggeringEvent, eestate);
            foreach(MGPumEntity e in selectedEntities)
            {
                if(e is MGPumUnit)
                {
                    MGPumUnit c = (MGPumUnit)e;

                    int amount = count.count(gc.state, source) * healthBuffPerCount;

                    gc.buffUnit(parent, c, amount);

                  


                }
                else
                {
                    throw new GameException("Entity " + e + " is not a unit.");
                }
                
            }

            
        }

        public override MGPumSelector getSelector(MGPumGameState state, MGPumEntity source)
        {
            return selector;
        }

        internal override MGPumEffect deepCopy(MGPumGameState state)
        {
            MGPumBuffCountEffect be = new MGPumBuffCountEffect(healthBuffPerCount, count, this.selector.deepCopySelector(state));
            return be;
        }
    }
}
