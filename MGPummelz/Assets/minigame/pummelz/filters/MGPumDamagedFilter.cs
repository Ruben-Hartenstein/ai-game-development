using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumDamagedFilter : MGPumSubFilter
    {

        public MGPumDamagedFilter() : base()

        {
        }

        public override List<MGPumEntity> apply(IEnumerable<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> filteredEntities = new List<MGPumEntity>();

            foreach (MGPumEntity entity in entities)
            {
                if(entity is MGPumUnit)
                {
                    MGPumUnit t = (MGPumUnit)entity;
                    if(t.damage > 0)
                    {
                        filteredEntities.Add(entity);
                    }
                    
                }
            }
            return filteredEntities;
        }

        
    }
}
