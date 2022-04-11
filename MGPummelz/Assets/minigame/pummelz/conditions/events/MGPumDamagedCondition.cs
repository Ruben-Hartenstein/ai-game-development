using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumDamagedCondition : MGPumEventCondition
    {
        internal MGPumFilter unitFilter;

        private int minDamage = 1;

        public MGPumDamagedCondition(MGPumFilter unitFilter) : base(typeof(MGPumDamageUnitEvent), null)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.unitFilter = unitFilter;
        }

        public MGPumDamagedCondition(MGPumFilter unitFilter, int minDamage) : base(typeof(MGPumDamageUnitEvent), null)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.unitFilter = unitFilter;
            this.minDamage = minDamage;
        }

        public MGPumDamagedCondition(MGPumFilter unitFilter, MGPumStateCondition stateCondition) : base(typeof(MGPumDamageUnitEvent), stateCondition)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.unitFilter = unitFilter;
        }
        protected override bool checkInternal(MGPumGameEvent gameEvent, MGPumGameState state, MGPumEntity source)
        {
            MGPumDamageUnitEvent dtse = (MGPumDamageUnitEvent)gameEvent;
            return unitFilter.test(state, dtse.damagedUnit, source) && dtse.damage >= minDamage;
        }
        
    }
}
