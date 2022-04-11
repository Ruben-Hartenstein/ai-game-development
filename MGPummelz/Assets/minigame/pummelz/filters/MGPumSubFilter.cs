using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public abstract class MGPumSubFilter
    {
        public abstract List<MGPumEntity> apply(IEnumerable<MGPumEntity> entities, MGPumEntity source);

    }
}
