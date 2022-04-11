using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;
using System.Linq;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumOwnerReference : MGPumReference
    {
        private string snum;

        public MGPumOwnerReference(string snum)
        {
            this.snum = snum;
        }

        public override List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource)
        {
            throw new GameException("Cannot get reference selection without execution state.");
        }

        public override List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource, MGPumGameEvent triggeringEvent)
        {
            throw new GameException("Cannot get reference selection without execution state.");
        }

        public override List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            List<List<MGPumEntity>> selections = eestate.get<List<List<MGPumEntity>>>(MGPumSequentialEffect.EESTATE_SELECTIONS);
            if(selections == null || !selections.Any())
            {
                throw new GameException("There is no previous selection for owner reference.");
            }
            List<MGPumEntity> result = new List<MGPumEntity>();

            foreach(MGPumEntity e in selections.Last())
            {
                MGPumPlayer p = state.getPlayer(e.ownerID);
                if(!result.Contains(p))
                {
                    result.Add(p);
                }
            }

            return result;
        }

   
    }
}
