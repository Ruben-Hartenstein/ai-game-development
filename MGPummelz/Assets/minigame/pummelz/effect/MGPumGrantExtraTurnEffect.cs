using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumGrantExtraTurnEffect : MGPumSingleEffect
    {
        private MGPumSelector selector;


        public MGPumGrantExtraTurnEffect(MGPumSelector selector)
        {
            this.selector = selector;
        }



        internal override void executeInternal(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            List<MGPumEntity> selectedEntities = selection.getSelection(gc.state, source, triggeringEvent, eestate);

            foreach (MGPumEntity e in selectedEntities)
            {
                if(e is MGPumPlayer)
                {
                    MGPumPlayer p = (MGPumPlayer)e;
                    gc.grantExtraTurn(parent, p.playerID);
                }
                else
                {
                    throw new GameException("Entity " + e + " is not a player.");
                }
                
            }

            
        }

        public override MGPumSelector getSelector(MGPumGameState state, MGPumEntity source)
        {
            return selector;
        }

        internal override MGPumEffect deepCopy(MGPumGameState state)
        {
            return new MGPumGrantExtraTurnEffect(this.selector.deepCopySelector(state));
        }
    }
}
