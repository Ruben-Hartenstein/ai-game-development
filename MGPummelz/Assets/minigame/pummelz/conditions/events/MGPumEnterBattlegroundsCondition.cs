using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumEnterBattlegroundsCondition : MGPumEventCondition
    {
        private MGPumFilter enterFilter;

        public MGPumEnterBattlegroundsCondition(MGPumFilter enterFilter) : base(typeof(MGPumZoneChangeEvent), null)
        {
            if(enterFilter is MGPumSourceFilter)
            {
                this.activeZone = MGPumZoneType.Any;
            }
            else
            {
                this.activeZone = MGPumZoneType.Battlegrounds;
            }
            this.enterFilter = enterFilter;
        }

        public MGPumEnterBattlegroundsCondition(MGPumFilter enterFilter, MGPumStateCondition stateCondition) : base(typeof(MGPumZoneChangeEvent), stateCondition)
        {
            if (enterFilter is MGPumSourceFilter)
            {
                this.activeZone = MGPumZoneType.Any;
            }
            else
            {
                this.activeZone = MGPumZoneType.Battlegrounds;
            }
            this.enterFilter = enterFilter;
        }

        protected override bool checkInternal(MGPumGameEvent gameEvent, MGPumGameState state, MGPumEntity source)
        {
            MGPumZoneChangeEvent zce = (MGPumZoneChangeEvent)gameEvent;
            if(zce.destination.getZoneType() == MGPumZoneType.Battlegrounds)
            {
                return enterFilter.test(state, zce.unit, source);
            }
            else
            {
                return false;
            }
            
        }
        
    }
}
