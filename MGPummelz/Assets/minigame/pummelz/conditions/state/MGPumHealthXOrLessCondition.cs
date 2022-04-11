using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumHealthXOrLessCondition : MGPumSubCondition
    {
        private MGPumFilter filter;
        private int maxHealth;
        public MGPumHealthXOrLessCondition(MGPumFilter selection, int maxHealth)
        {
            this.filter = selection;
            this.maxHealth = maxHealth;
        }

        public override bool check(MGPumGameState state, MGPumEntity source)
        {
            bool result = true;

            foreach(MGPumEntity e in filter.apply(state, source))
            {
                if (!(e is MGPumUnit && ((MGPumUnit)e).currentHealth <= maxHealth))
                {
                    result = false;
                    break;
                }
            }

            return result;
        }

    }
}
