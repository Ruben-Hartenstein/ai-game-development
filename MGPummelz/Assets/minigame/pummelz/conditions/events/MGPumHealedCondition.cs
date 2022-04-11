using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumHealedCondition : MGPumEventCondition
    {
        internal MGPumFilter unitFilter;

        private int minHeal = 1;

        public MGPumHealedCondition(MGPumFilter unitFilter) : base(typeof(MGPumHealUnitEvent), null)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.unitFilter = unitFilter;
        }

        public MGPumHealedCondition(MGPumFilter unitFilter, int minHeal) : base(typeof(MGPumHealUnitEvent), null)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.unitFilter = unitFilter;
            this.minHeal = minHeal;
        }

        public MGPumHealedCondition(MGPumFilter unitFilter, MGPumStateCondition stateCondition) : base(typeof(MGPumHealUnitEvent), stateCondition)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.unitFilter = unitFilter;
        }

        protected override bool checkInternal(MGPumGameEvent gameEvent, MGPumGameState state, MGPumEntity source)
        {
            MGPumHealUnitEvent dtse = (MGPumHealUnitEvent)gameEvent;
            //Debug.LogError("dtse.damage " + dtse.damage);
            return unitFilter.test(state, dtse.healedUnit, source) && dtse.amount >= minHeal;
        }
        
    }
}
