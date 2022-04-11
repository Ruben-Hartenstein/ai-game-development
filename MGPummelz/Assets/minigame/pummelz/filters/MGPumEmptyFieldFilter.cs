using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumEmptyFieldFilter : MGPumSubFilter
    {

        public MGPumEmptyFieldFilter() : base()
        {

        }
        
        public override List<MGPumEntity> apply(IEnumerable<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> filteredEntities = new List<MGPumEntity>();

            foreach (MGPumEntity entity in entities)
            {
                if (entity is MGPumField)
                {
                    MGPumField f = (MGPumField)entity;
                    if (f.isEmpty())
                    {
                        filteredEntities.Add(entity);
                    }
                }


            }

            return filteredEntities;
        }


    }
}
