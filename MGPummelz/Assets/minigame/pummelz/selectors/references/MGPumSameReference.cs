using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;
using System.Linq;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumSameReference : MGPumReference
    {
        private string snum;

        public MGPumSameReference(string snum)
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
                throw new GameException("There is no previous selection for same reference.");
            }
            return selections.Last();
        }

      
    }
}
