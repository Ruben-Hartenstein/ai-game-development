using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumUnitIDFilter : MGPumSubFilter
    {

        string unitID;

        public MGPumUnitIDFilter(string unitID)
        {
            this.unitID = unitID;
        }

        public override List<MGPumEntity> apply(IEnumerable<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> result = new List<MGPumEntity>();
            foreach (MGPumEntity e in entities)
            {
                if (e is MGPumUnit && ((MGPumUnit)e).unitID.Equals(this.unitID))
                {
                    result.Add(e);
                }
            }
            return result;
        }
    }
}
