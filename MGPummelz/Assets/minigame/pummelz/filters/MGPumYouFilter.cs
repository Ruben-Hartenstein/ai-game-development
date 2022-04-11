using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumYouFilter : MGPumFilter
    {

        public MGPumYouFilter() : base()

        {
            
        }

         protected override List<MGPumEntity> applyBaseFilter(List<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> filteredEntities = new List<MGPumEntity>();

            foreach (MGPumEntity entity in entities)
            {
                if (entity is MGPumPlayer && ((MGPumPlayer)entity).playerID == source.ownerID)
                {
                    filteredEntities.Add(entity);
                }
            }
            return filteredEntities;
        }

        protected override List<MGPumEntity> applyBaseFilter(MGPumGameState state, MGPumEntity source)
        {
            List<MGPumEntity> filteredEntities = new List<MGPumEntity>();
            filteredEntities.Add(state.getPlayer(source.ownerID));
            return filteredEntities;
        }

        public override MGPumSelection deepCopy(MGPumGameState state)
        {
            MGPumYouFilter copy = new MGPumYouFilter();
            copyToFilter(copy);

            return copy;
        }


    }
}
