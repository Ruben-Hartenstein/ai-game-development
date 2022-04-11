using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumHasDamageCondition : MGPumSubCondition
    {
        private MGPumFilter filter;
        public MGPumHasDamageCondition(MGPumFilter selection)
        {
            this.filter = selection;
        }

        public override bool check(MGPumGameState state, MGPumEntity source)
        {
            bool result = true;
            foreach(MGPumEntity e in filter.apply(state, source))
            {
                if(e is MGPumUnit && ((MGPumUnit)e).damage <= 0)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

    }
}
