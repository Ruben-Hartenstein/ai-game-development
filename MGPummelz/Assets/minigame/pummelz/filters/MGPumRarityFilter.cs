using RelegatiaCCG.rccg.i18n;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumRarityFilter : MGPumSubFilter
    {
        private MGPumCRT[] rarities;

        public MGPumRarityFilter(params MGPumCRT[] rarities) : base()

        {
            this.rarities = rarities;
        }

        public override List<MGPumEntity> apply(IEnumerable<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> filteredEntities = new List<MGPumEntity>();

            foreach (MGPumEntity entity in entities)
            {
                if(entity is MGPumUnit)
                {
                    MGPumUnit c = (MGPumUnit)entity;
                    bool result = false;
                    foreach(MGPumCRT rarity in rarities)
                    {
                        if (c.rarity == rarity)
                        {
                            result = true;
                            break;
                        }
                    }
                    if(result)
                    {
                        filteredEntities.Add(entity);
                    }
                    
                }
            }
            return filteredEntities;
        }

        
    }
}
