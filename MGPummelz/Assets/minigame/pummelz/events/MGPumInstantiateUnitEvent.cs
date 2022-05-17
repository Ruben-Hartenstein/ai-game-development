using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumInstantiateUnitEvent : MGPumGameEvent
    {
        public string unitID;
        public int ownerID;
        public MGPumUnit instantiatedUnit;
        public MGPumUnit instantiatedUnitCopy;

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
                //we keep a pristine copy in case the created unit gets changed later
                this.instantiatedUnitCopy = this.instantiatedUnit.deepCopy();
            }
            else
            {
                //unit has already been created in deep copy, but state needs to got through the motions
                state.createdUnit(instantiatedUnit);
            }
            
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumUnit c = state.lookupOrCreate(this.instantiatedUnitCopy);
            MGPumInstantiateUnitEvent result = new MGPumInstantiateUnitEvent(unitID, ownerID);
            result.instantiatedUnit = c;
            result.instantiatedUnitCopy = c.deepCopy();

            this.copyToGameEvent(result);
            return result;
        }
    }
}
