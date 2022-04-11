using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumTriggeringUnitHealthCount : MGPumCount
    {
        public MGPumTriggeringUnitHealthCount()
        {
        }

        public override int count(MGPumGameState state, int playerID, MGPumEntity source)
        {
            return count(state, source);
        }

        public override int count(MGPumGameState state, MGPumEntity source)
        {
            int count = 0;
            MGPumGameEvent trigger = state.log.getTriggerOfThisEffect();
            if(trigger is MGPumUnitReferencable)
            {
                MGPumUnit c = ((MGPumUnitReferencable)trigger).getReferencableUnit();
                if(c is MGPumUnit)
                {
                    count = ((MGPumUnit)c).currentHealth;
                }
            }
            return count / divider;
        }

    }
}
