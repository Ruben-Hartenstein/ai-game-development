using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumEnemyFilter : MGPumSubFilter
    {

        public MGPumEnemyFilter() : base()
        {

        }

        public override List<MGPumEntity> apply(IEnumerable<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> filteredEntities = new List<MGPumEntity>();

            foreach (MGPumEntity entity in entities)
            {
                if (entity.ownerID != source.ownerID)
                {
                    filteredEntities.Add(entity);
                }
            }
            return filteredEntities;
        }


    }
}
