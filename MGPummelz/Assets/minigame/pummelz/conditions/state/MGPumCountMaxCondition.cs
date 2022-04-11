using RelegatiaCCG.rccg.i18n;
using System.Linq;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumCountMaxCondition : MGPumSubCondition
    {
        private MGPumFilter filter;
        private int maxCount;
        public MGPumCountMaxCondition(MGPumFilter filterCondition, int maxCount)
        {
            this.filter = filterCondition;
            this.maxCount = maxCount;
        }

        public override bool check(MGPumGameState state, MGPumEntity source)
        {
            return filter.apply(state, source).Count() <= this.maxCount;
        }

    }
}
