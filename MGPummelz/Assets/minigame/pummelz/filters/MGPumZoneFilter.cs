using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;
using System.Linq;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumZoneFilter : MGPumFilter
    {
        private MGPumZoneType zone;

        public MGPumZoneFilter(MGPumZoneType zone)
        {
            this.zone = zone;
        }

        public override bool test(MGPumGameState state, MGPumEntity entity, MGPumEntity source)
        {
            if(zone == MGPumZoneType.Any)
            {
                return test(entity, source);
            }
            else
            {
                List<MGPumEntity> entities = apply(state, source);
                return entities.Contains(entity);
            }
        }

        protected override List<MGPumEntity> applyBaseFilter(List<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> result = new List<MGPumEntity>();

            foreach(MGPumEntity e in entities)
            {
                if (e is MGPumUnit)
                {
                    MGPumUnit c = (MGPumUnit)e;
                    if(c.zone == zone||zone == MGPumZoneType.Any)
                    {
                        result.Add(c);
                    }
                }
            }
            return result;
            
        }

        protected override List<MGPumEntity> applyBaseFilter(MGPumGameState state, MGPumEntity source)
        {
            return new List<MGPumEntity>(state.getAllUnitsInZone(zone).Cast<MGPumEntity>());
        }

        public override MGPumSelection deepCopy(MGPumGameState state)
        {
            MGPumZoneFilter copy = new MGPumZoneFilter(zone);
            copyToFilter(copy);

            return copy;
        }

    }
}
