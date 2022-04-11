using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumDestroyCondition : MGPumEventCondition
    {
        private MGPumFilter unitFilter;

        public MGPumDestroyCondition(MGPumFilter unitFilter) : base(typeof(MGPumDestroyUnitSEvent), null)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.unitFilter = unitFilter;
        }

        public MGPumDestroyCondition(MGPumFilter enterFilter, MGPumStateCondition stateCondition) : base(typeof(MGPumDestroyUnitSEvent), stateCondition)
        {
            this.activeZone = MGPumZoneType.Battlegrounds;
            this.unitFilter = enterFilter;
        }

        protected override bool checkInternal(MGPumGameEvent gameEvent, MGPumGameState state, MGPumEntity source)
        {
            MGPumDestroyUnitSEvent dtse = (MGPumDestroyUnitSEvent)gameEvent;
            return unitFilter.test(state, dtse.unit, source);

        }
        
    }
}
