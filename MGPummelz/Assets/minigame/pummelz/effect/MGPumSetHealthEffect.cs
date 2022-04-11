using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumSetHealthEffect : MGPumSingleEffect
    {
        private MGPumSelector selector;

        private int amount;

        public MGPumSetHealthEffect(MGPumSelector selector, int amount)
        {
            this.selector = selector;
            this.amount = amount;
        }



        internal override void executeInternal(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            List<MGPumEntity> selectedEntities = selection.getSelection(gc.state, source, triggeringEvent, eestate);
            foreach(MGPumEntity e in selectedEntities)
            {
                if(e is MGPumUnit)
                {
                    MGPumUnit c = (MGPumUnit)e;

                    gc.removeDamageFromUnit(parent, source, c);
                    gc.setHealth(parent, c, amount);

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
            return new MGPumSetHealthEffect(this.selector.deepCopySelector(state), this.amount);
        }
    }
}
