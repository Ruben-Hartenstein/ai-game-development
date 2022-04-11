using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumAllPlayersFilter : MGPumFilter
    {

        public MGPumAllPlayersFilter() : base()
        {
            
        }

         protected override List<MGPumEntity> applyBaseFilter(List<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> filteredEntities = new List<MGPumEntity>();

            foreach (MGPumEntity entity in entities)
            {
                if (entity is MGPumPlayer)
                {
                    filteredEntities.Add(entity);
                }
            }
            return filteredEntities;
        }

        protected override List<MGPumEntity> applyBaseFilter(MGPumGameState state, MGPumEntity source)
        {
            List<MGPumEntity> filteredEntities = new List<MGPumEntity>();

            filteredEntities.AddRange(state.players);
            return filteredEntities;
        }

        public override MGPumSelection deepCopy(MGPumGameState state)
        {
            MGPumAllPlayersFilter copy = new MGPumAllPlayersFilter();
            copyToFilter(copy);

            return copy;
        }

    }
}
