using RelegatiaCCG.rccg.engine.exceptions;
using System.Collections.Generic;
using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumLoseGameEffect : MGPumSingleEffect
    {
        private MGPumSelector selector;


        public MGPumLoseGameEffect(MGPumSelector selector)
        {
            this.selector = selector;
        }



        internal override void executeInternal(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            List<MGPumEntity> selectedEntities = selection.getSelection(gc.state, source, triggeringEvent, eestate);
            foreach(MGPumEntity e in selectedEntities)
            {
                if(e is MGPumPlayer)
                {
                    MGPumPlayer p = (MGPumPlayer)e;
                    gc.loseGame(parent, p);
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
            return new MGPumLoseGameEffect(this.selector.deepCopySelector(state));
        }
    }
}
