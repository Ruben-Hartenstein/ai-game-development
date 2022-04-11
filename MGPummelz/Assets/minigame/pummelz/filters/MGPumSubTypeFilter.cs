using RelegatiaCCG.rccg.i18n;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumSubTypeFilter : MGPumSubFilter
    {
        private MGPumST[] types;

        public MGPumSubTypeFilter(params MGPumST[] types) : base()

        {
            this.types = types;
        }

        public override List<MGPumEntity> apply(IEnumerable<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> filteredEntities = new List<MGPumEntity>();

            foreach (MGPumEntity entity in entities)
            {
                if(entity is MGPumUnit)
                {
                    MGPumUnit c = (MGPumUnit)entity;
                    bool result = true;
                    foreach(MGPumST subtype in types)
                    {
                        if (!c.hasSubtype(subtype))
                        {
                            result = false;
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
