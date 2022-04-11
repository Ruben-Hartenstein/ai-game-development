using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumTriggerEventUnitFieldReference : MGPumReference
    {

        public MGPumTriggerEventUnitFieldReference()
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
                MGPumUnit ru = ((MGPumUnitReferencable)triggeringEvent).getReferencableUnit();
                if(ru != null && ru.lastKnownField != null)
                {
                    result.Add(ru.lastKnownField);
                }
            }
            return result;
        }

        public override List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            return getSelection(state, selectionSource, triggeringEvent);
        }

    

    }
}
