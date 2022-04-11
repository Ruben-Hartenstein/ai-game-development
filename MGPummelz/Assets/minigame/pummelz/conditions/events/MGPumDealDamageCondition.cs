using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumDealDamageCondition : MGPumEventCondition
    {
        public MGPumFilter sourceFilter;

        private int minDamage = 1;

        public MGPumDealDamageCondition(MGPumFilter sourceFilter) : base(typeof(MGPumDamageUnitEvent), null)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.sourceFilter = sourceFilter;
        }

        public MGPumDealDamageCondition(MGPumFilter unitFilter, int minDamage) : base(typeof(MGPumDamageUnitEvent), null)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.sourceFilter = unitFilter;
            this.minDamage = minDamage;
        }

        public MGPumDealDamageCondition(MGPumFilter unitFilter, MGPumStateCondition stateCondition) : base(typeof(MGPumDamageUnitEvent), stateCondition)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.sourceFilter = unitFilter;
        }

        protected override bool checkInternal(MGPumGameEvent gameEvent, MGPumGameState state, MGPumEntity source)
        {
            MGPumDamageUnitEvent dtse = (MGPumDamageUnitEvent)gameEvent;
            //Debug.LogError("dtse.damage " + dtse.damage);
            return sourceFilter.test(state, dtse.source, source) && dtse.damage >= minDamage;
        }
        
    }
}
