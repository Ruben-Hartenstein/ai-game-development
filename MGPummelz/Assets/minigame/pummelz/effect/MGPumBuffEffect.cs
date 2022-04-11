using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumBuffEffect : MGPumSingleEffect
    {
        private MGPumSelector selector;

        public int healthBuff { get; set; }



        protected MGPumBuffEffect()
        {
         }

        
        public MGPumBuffEffect(int healthBuff, MGPumSelector selector) : this()
        {
            this.healthBuff = healthBuff;
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

                    gc.buffUnit(parent, c, healthBuff);

                  

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
            MGPumBuffEffect be = new MGPumBuffEffect(healthBuff, this.selector.deepCopySelector(state));

            return be;
        }
    }
}
