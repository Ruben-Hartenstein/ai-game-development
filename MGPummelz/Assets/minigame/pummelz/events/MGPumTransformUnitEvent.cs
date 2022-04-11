using System;
using System.Collections;
using System.Collections.Generic;

namespace mg.pummelz 
{
    [System.Serializable]
    public class MGPumTransformUnitEvent : MGPumGameEvent
    {
        public MGPumUnit unit { get; set; }

        public string transformUnitID;

        public MGPumTransformUnitEvent(MGPumUnit unit, string transformUnitID) : base()
        {
            this.unit = unit;
            this.transformUnitID = transformUnitID;
        }



        private static void copyPrinted(MGPumUnit unit, MGPumUnit target)
        {
            unit.unitID = target.unitID;
            unit.name = target.name;
            unit.artID = target.artID;

            unit.setID = target.setID;

            unit.rarity = target.rarity;

            unit.subTypePrinted = target.subTypePrinted;

            unit.printedStats = (int[])target.printedStats.Clone();

            if(target.abilityPrinted != null)
            {
                unit.abilityPrinted = target.abilityPrinted.deepCopy();
                unit.abilityPrinted.owner = unit;
            }
            else
            {
                unit.abilityPrinted = null;
            }
            
        }


        private static void copyBaseCurrent(MGPumUnit unit, MGPumUnit target)
        {
            unit.subTypeBase = target.subTypeBase;
            unit.subTypeNow = target.subTypeNow;

            unit.baseStats = (int[])target.baseStats.Clone();
            unit.currentStats = (int[])target.currentStats.Clone();

            if (target.abilityBase != null)
            {
                unit.abilityBase = target.abilityBase.deepCopy();
                unit.abilityBase.owner = unit;
            }
            else
            {
                unit.abilityBase = null;
            }
            if (target.abilityCurrent != null)
            {
                unit.abilityCurrent = target.abilityCurrent.deepCopy();
                unit.abilityCurrent.owner = unit;
            }
            else
            {
                unit.abilityCurrent = null;
            }
        }

       

        public override void apply(MGPumGameState state)
        {
            MGPumUnit target = (MGPumUnit)MGPumSetHandler.getInstance().getNewUnit(state.idm.getNextID(), transformUnitID, unit.ownerID, state.idm);
            MGPumUnit sourceBefore = (MGPumUnit)unit.deepCopy();
            copyPrinted(unit, target);
            unit.setBaseToPrinted();
            copyBaseCurrent(unit, target);
            unit.setCurrentToBase();
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumUnit u = (MGPumUnit)state.lookupOrCreate(this.unit);

            MGPumGameEvent result = new MGPumTransformUnitEvent(u, transformUnitID);
            this.copyToGameEvent(result);
            return result;

        }
    }


}
