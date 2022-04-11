using System.Collections.Generic;

namespace mg.pummelz
{
    public interface MGPumSelection
    {
        List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource);

        List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource, MGPumGameEvent triggeringEvent);

        List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate);

        MGPumSelection deepCopy(MGPumGameState state);
    }
}
