using RelegatiaCCG.rccg.i18n;
using System.Linq;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumFilterCount: MGPumCount
    {
        private MGPumFilter filter;

        public MGPumFilterCount(MGPumFilter filter)
        {
            this.filter = filter;
        }

        public override int count(MGPumGameState state, MGPumEntity source)
        {
            return filter.apply(state, source).Count() / divider;
        }

        public override int count(MGPumGameState state, int playerID, MGPumEntity source)
        {
            return count(state, source);
        }

    }
}
