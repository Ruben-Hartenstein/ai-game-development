using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumSurvivedDamageCondition : MGPumSubCondition
    {
        private MGPumFilter filter;
        public MGPumSurvivedDamageCondition(MGPumFilter selection)
        {
            this.filter = selection;
        }

        public override bool check(MGPumGameState state, MGPumEntity source)
        {
            bool result = true;
            foreach(MGPumEntity e in filter.apply(state, source))
            {
                if(!(e is MGPumUnit && ((MGPumUnit)e).zone == MGPumZoneType.Battlegrounds))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }


    }
}
