using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumSourceFilter : MGPumFilter
    {

        public MGPumSourceFilter() : base()

        {
            
        }

         protected override List<MGPumEntity> applyBaseFilter(List<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> filteredEntities = new List<MGPumEntity>();

            foreach (MGPumEntity entity in entities)
            {
                //Debug.LogError("entity " + entity + " source " + source);
                if (entity.id  == source.id)
                {
                    filteredEntities.Add(entity);
                }
            }
            return filteredEntities;
        }

        protected override List<MGPumEntity> applyBaseFilter(MGPumGameState state, MGPumEntity source)
        {
            List<MGPumEntity> filteredEntities = new List<MGPumEntity>();
            filteredEntities.Add(source);
            return filteredEntities;
        }

        public override MGPumSelection deepCopy(MGPumGameState state)
        {
            MGPumSourceFilter copy = new MGPumSourceFilter();
            copyToFilter(copy);

            return copy;
        }

    }
}
