using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public abstract class MGPumReference : MGPumAutoSelect
    {

    

      

        public MGPumSelection deepCopy(MGPumGameState state)
        {
            return this;
        }

        public MGPumSelector deepCopySelector(MGPumGameState state)
        {
            return (MGPumSelector)(this.deepCopy(state));
        }

        public MGPumSelection getSelection()
        {
            return this;
        }

        public abstract List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource);

        public abstract List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource, MGPumGameEvent triggeringEvent);

        public abstract List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate);

    }
}
