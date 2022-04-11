using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumFieldFilter : MGPumFilter
    {
        private MGPumPST playerSelection;

        public MGPumFieldFilter()
        {
            this.playerSelection = MGPumPST.Any;
        }

        public MGPumFieldFilter(MGPumPST playerSelection)
        {
            this.playerSelection = playerSelection;
        }

        protected override List<MGPumEntity> applyBaseFilter(List<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> result = new List<MGPumEntity>();

            foreach(MGPumEntity e in entities)
            {
                if (e is MGPumField)
                {
                    MGPumField f = (MGPumField)e;
                    if(playerSelection.isSelected(f.ownerID, source))
                    {
                        
                        result.Add(f);
                    }
                }
            }
            return result;
            
        }

        protected override List<MGPumEntity> applyBaseFilter(MGPumGameState state, MGPumEntity source)
        {
            
            List<MGPumEntity> entities = new List<MGPumEntity>();
            foreach(MGPumPlayer p in state.players)
            {
                if(playerSelection.isSelected(p.playerID, source))
                {
                    foreach (MGPumField f in state.fields)
                    {
                        entities.Add(f);
                    }
                }
            }
            
            return entities;
        }

        public override MGPumSelection deepCopy(MGPumGameState state)
        {
            MGPumFieldFilter copy = new MGPumFieldFilter(playerSelection);
            copyToFilter(copy);

            return copy;
        }

    }
}
