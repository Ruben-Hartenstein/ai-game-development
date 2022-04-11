using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumMoveUnitEvent : MGPumGameEvent, MGPumUnitReferencable
    {
        public MGPumUnit movingUnit { get; set; }
        public MGPumField originField { get; set; }
        public MGPumField targetField { get; set; }


        public MGPumMoveUnitEvent(MGPumUnit movingUnit, MGPumField originField, MGPumField targetField) : base()
        {
            this.movingUnit = movingUnit;
            this.originField = originField;
            this.targetField = targetField;
        }

        public override void apply(MGPumGameState state)
        {
            if(targetField.unit != null)
            {
                Debug.LogError("Unit " + movingUnit + " moving to occupied field " + targetField);
            }
            movingUnit.field.unit = null;
            targetField.unit = movingUnit;
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumUnit mu = (MGPumUnit)state.lookupOrCreate(this.movingUnit);
            MGPumField of = (MGPumField)state.lookupEntity(this.originField);
            MGPumField tf = (MGPumField)state.lookupEntity(this.targetField);

            MGPumGameEvent result = new MGPumMoveUnitEvent(mu, of, tf);
            this.copyToGameEvent(result);
            return result;
        }

        public MGPumUnit getReferencableUnit()
        {
            return movingUnit;
        }
    }
}
