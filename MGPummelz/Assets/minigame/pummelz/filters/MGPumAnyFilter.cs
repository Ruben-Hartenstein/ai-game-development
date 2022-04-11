using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;
using System.Linq;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumAnyFilter : MGPumFilter
    {

        public override bool test(MGPumGameState state, MGPumEntity entity, MGPumEntity source)
        {
            return test(entity, source);
        }

        protected override List<MGPumEntity> applyBaseFilter(List<MGPumEntity> entities, MGPumEntity source)
        {
            return entities;
        }

        protected override List<MGPumEntity> applyBaseFilter(MGPumGameState state, MGPumEntity source)
        {
            throw new GameException("Cannot apply any filter on state.");
        }

        public override MGPumSelection deepCopy(MGPumGameState state)
        {
            MGPumAnyFilter copy = new MGPumAnyFilter();
            copyToFilter(copy);

            return copy;
        }

       
    }
}
