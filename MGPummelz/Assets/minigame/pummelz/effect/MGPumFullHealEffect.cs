using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumFullHealEffect : MGPumSingleEffect
    {
        private MGPumSelector selector;


        public MGPumFullHealEffect(MGPumSelector selector)
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
                    MGPumUnit c = (MGPumUnit)e;

                    gc.healUnit(parent, source, c, c.damage);

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
            return new MGPumFullHealEffect(this.selector.deepCopySelector(state));
        }
    }
}
