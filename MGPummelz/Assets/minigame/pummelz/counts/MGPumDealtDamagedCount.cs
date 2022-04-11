using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumDealtDamageCount : MGPumCount
    {
        public MGPumDealtDamageCount()
        {
        }

        public override int count(MGPumGameState state, int playerID, MGPumEntity source)
        {
            return count(state, source);
        }

        public override int count(MGPumGameState state, MGPumEntity source)
        {
            int count = 0;
            foreach(MGPumGameEvent e in state.log.getEventsOfThisEffect())
            {
                

                if (e is MGPumDamageUnitEvent)
                {
                    MGPumDamageUnitEvent dce = (MGPumDamageUnitEvent)e;
                    
                    count += dce.damage;
                    
                }
            }

            MGPumGameEvent trigger = state.log.getTriggerOfThisEffect();

            if (trigger is MGPumDamageUnitEvent)
            {
                MGPumDamageUnitEvent dce = (MGPumDamageUnitEvent)trigger;

                count += dce.damage;
            }

            return count / divider;
        }

    }
}
