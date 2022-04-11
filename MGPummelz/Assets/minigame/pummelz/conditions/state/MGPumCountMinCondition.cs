using RelegatiaCCG.rccg.i18n;
using System.Linq;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumCountMinCondition : MGPumSubCondition
    {
        private MGPumFilter filter;
        private int minCount;
        public MGPumCountMinCondition(MGPumFilter filterCondition, int minCount)
        {
            this.filter = filterCondition;
            this.minCount = minCount;
        }

        public override bool check(MGPumGameState state, MGPumEntity source)
        {
            return filter.apply(state, source).Count() >= this.minCount;
        }


    }
}
