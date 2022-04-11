using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumDamageUnitCountEffect : MGPumSingleEffect
    {
        private MGPumSelector selector;

        private MGPumCount count;

        public int damagePerCount = 1;
        
        public MGPumDamageUnitCountEffect(MGPumSelector selector, MGPumCount count, int damagePerCount)
        {
            this.selector = selector;
            this.count = count;
            this.damagePerCount = damagePerCount;
        }



        internal override void executeInternal(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            List<MGPumEntity> selectedEntities = selection.getSelection(gc.state, source, triggeringEvent, eestate);
            foreach(MGPumEntity e in selectedEntities)
            {
                if(e is MGPumUnit)
                {
                    MGPumUnit c = (MGPumUnit)e;
                    int damage = count.count(gc.state, source);
                    damage *= damagePerCount;

                    gc.damageUnit(parent, source, c, damage, false);

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
            return new MGPumDamageUnitCountEffect(this.selector.deepCopySelector(state), this.count, damagePerCount);
        }
    }
}
