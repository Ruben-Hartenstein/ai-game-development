using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumHealthFilter : MGPumSubFilter
    {
        private int minHealth;

        private int maxHealth;

        public const int UNINITIALIZED = -1;

        public MGPumHealthFilter(int minHealth)
        {
            this.minHealth = minHealth;
            this.maxHealth = UNINITIALIZED;
        }

        public MGPumHealthFilter(int minHealth, int maxHealth) : base()
        {
            this.minHealth = minHealth;
            this.maxHealth = maxHealth;
        }

        public override List<MGPumEntity> apply(IEnumerable<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> filteredEntities = new List<MGPumEntity>();

            foreach (MGPumEntity entity in entities)
            {
                if(entity is MGPumUnit)
                {
                    MGPumUnit c = (MGPumUnit)entity;
                    int totalAmount = c.currentHealth;
                    bool result = true;

                    if (totalAmount < minHealth && minHealth != UNINITIALIZED)
                    {
                        result = false;
                    }
                    if (totalAmount > maxHealth && maxHealth != UNINITIALIZED)
                    {
                        result = false;
                    }
                    if(result)
                    {
                        filteredEntities.Add(c);
                    }
                }
            }
            return filteredEntities;
        }

        
    }
}
