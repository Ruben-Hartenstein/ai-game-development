using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumDestroyedUnitCount: MGPumCount
    {
        private MGPumFilter filter;

        public MGPumDestroyedUnitCount()
        {
            this.filter = MGPumFF.any();
        }

        public MGPumDestroyedUnitCount(MGPumFilter filter)
        {
            this.filter = filter;
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
                if (e is MGPumDestroyUnitSEvent)
                {
                    MGPumDestroyUnitSEvent dtse = (MGPumDestroyUnitSEvent)e;
                    if(filter.test(state, dtse.unit, source))
                    {
                        count++;
                    }
                }
            }
            return count / divider;
        }

    }
}
