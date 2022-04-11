using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumTriggerEventUnitReference : MGPumReference
    {

        public MGPumTriggerEventUnitReference()
        {
        }


        public override List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource)
        {
            throw new GameException("Cannot get reference selection without triggering event.");
        }

        public override List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource, MGPumGameEvent triggeringEvent)
        {
            List<MGPumEntity> result = new List<MGPumEntity>();

            if (triggeringEvent is MGPumUnitReferencable)
            {
                result.Add(((MGPumUnitReferencable)triggeringEvent).getReferencableUnit());
            }
            return result;
        }

        public override List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            return getSelection(state, selectionSource, triggeringEvent);
        }

    }
}
