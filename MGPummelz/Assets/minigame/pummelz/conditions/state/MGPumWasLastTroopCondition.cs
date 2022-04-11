using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumWasLastUnitCondition : MGPumSubCondition
    {
        public MGPumWasLastUnitCondition()
        {
        }

        public override bool check(MGPumGameState state, MGPumEntity source)
        {
            int count = 0;
            foreach(MGPumField f in state.fields)
            {
                if(!f.isEmpty() && f.unit is MGPumUnit)
                {
                    count++;
                }
            }

            return count <= 1;
        }

    }
}
