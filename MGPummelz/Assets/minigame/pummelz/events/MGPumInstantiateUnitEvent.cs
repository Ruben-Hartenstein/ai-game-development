using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumInstantiateUnitEvent : MGPumGameEvent
    {
        public string unitID;
        public int ownerID;
        public MGPumUnit instantiatedUnit;

        public MGPumInstantiateUnitEvent(string unitID, int ownerID)
        {
            this.unitID = unitID;
            this.ownerID = ownerID;
        }

        public override void apply(MGPumGameState state)
        {
            if(instantiatedUnit == null)
            {
                this.instantiatedUnit = state.createUnit(unitID, ownerID);
            }
            else
            {
                //unit has already been created in deep copy, but state needs to got through the motions
                state.createdUnit(instantiatedUnit);
            }
            
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumUnit c = state.lookupOrCreate(this.instantiatedUnit);
            MGPumInstantiateUnitEvent result = new MGPumInstantiateUnitEvent(unitID, ownerID);
            result.instantiatedUnit = c;

            this.copyToGameEvent(result);
            return result;
        }
    }
}
