using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumHealCountEffect : MGPumSingleEffect
    {
        private MGPumSelector selector;

        public int healPerCount { get; set; }

        public MGPumCount count;

        protected MGPumHealCountEffect()
        {
        }

        


        public MGPumHealCountEffect(MGPumCount count, MGPumSelector selector) : this()
        {
            this.healPerCount = 1;
            this.count = count;
            this.selector = selector;
        }

        public MGPumHealCountEffect(int healthBuffPerCount, MGPumCount count, MGPumSelector selector) : this()
        {
            this.healPerCount = healthBuffPerCount;
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

                    int amount = count.count(gc.state, source) * healPerCount;

                    gc.healUnit(parent, source, c, amount);

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
            MGPumHealCountEffect be = new MGPumHealCountEffect(healPerCount, count, this.selector.deepCopySelector(state));
            return be;
        }
    }
}
