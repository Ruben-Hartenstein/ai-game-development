using RelegatiaCCG.rccg.i18n;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumUnitSetFilter : MGPumFilter
    {

        string setID;

        internal bool verbose = true;

        public MGPumUnitSetFilter(string setID)
        {
            this.setID = setID;
        }

        protected override List<MGPumEntity> applyBaseFilter(List<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> result = new List<MGPumEntity>();
            foreach (MGPumEntity e in entities)
            {
                if (e is MGPumUnit && ((MGPumUnit)e).setID.Equals(this.setID))
                {
                    result.Add(e);
                }
            }
            return result;
        }

        protected override List<MGPumEntity> applyBaseFilter(MGPumGameState state, MGPumEntity source)
        {
            
            List<MGPumEntity> entities = new List<MGPumEntity>();
            foreach (MGPumUnit c in MGPumSetHandler.getInstance().getSetByID(this.setID).getUnits())
            {
                entities.Add(c);

            }
            return entities;
        }

        public override MGPumSelection deepCopy(MGPumGameState state)
        {
            MGPumUnitSetFilter copy = new MGPumUnitSetFilter(this.setID);
            copyToFilter(copy);

            return copy;
        }

    }
}
