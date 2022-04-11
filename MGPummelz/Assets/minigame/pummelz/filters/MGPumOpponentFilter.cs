using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumOpponentFilter : MGPumFilter
    {

        public MGPumOpponentFilter() : base()

        {
            
        }

         protected override List<MGPumEntity> applyBaseFilter(List<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> filteredEntities = new List<MGPumEntity>();

            foreach (MGPumEntity entity in entities)
            {
                if (entity is MGPumPlayer && ((MGPumPlayer)entity).playerID != source.ownerID)
                {
                    filteredEntities.Add(entity);
                }
            }
            return filteredEntities;
        }

        protected override List<MGPumEntity> applyBaseFilter(MGPumGameState state, MGPumEntity source)
        {
            List<MGPumEntity> filteredEntities = new List<MGPumEntity>();
            foreach(MGPumPlayer p in state.players)
            {
                if(MGPumPST.Opponent.isSelected(p.playerID, source))
                {
                    filteredEntities.Add(p);
                }
                
            }
            
            return filteredEntities;
        }

        public override MGPumSelection deepCopy(MGPumGameState state)
        {
            MGPumOpponentFilter copy = new MGPumOpponentFilter();
            copyToFilter(copy);

            return copy;
        }


    }
}
