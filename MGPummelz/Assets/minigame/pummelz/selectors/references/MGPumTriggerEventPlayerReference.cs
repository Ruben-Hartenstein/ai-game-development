using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumTriggerEventPlayerReference : MGPumReference
    {
        public override List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource)
        {
            throw new GameException("Cannot get reference selection without triggering event.");
        }

        public override List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource, MGPumGameEvent triggeringEvent)
        {
            List<MGPumEntity> result = new List<MGPumEntity>();

            if (triggeringEvent is MGPumPlayerReferencable)
            {
                result.Add(state.getPlayer(((MGPumPlayerReferencable)triggeringEvent).getReferencablePlayerID()));
            }
            return result;
        }

        public override List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            return getSelection(state, selectionSource, triggeringEvent);
        }

    }
}
