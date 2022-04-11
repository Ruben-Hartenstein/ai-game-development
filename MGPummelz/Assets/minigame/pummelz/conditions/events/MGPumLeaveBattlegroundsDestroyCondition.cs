using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumLeaveBattlegroundsDestroyCondition : MGPumEventCondition
    {
        private MGPumFilter leaveFilter;

        public MGPumLeaveBattlegroundsDestroyCondition(MGPumFilter enterFilter) : base(typeof(MGPumZoneChangeEvent), null)
        {
            if(enterFilter is MGPumSourceFilter)
            {
                this.activeZone = MGPumZoneType.Any;
            }
            else
            {
                this.activeZone = MGPumZoneType.Battlegrounds;
            }
            this.leaveFilter = enterFilter;
        }

        public MGPumLeaveBattlegroundsDestroyCondition(MGPumFilter enterFilter, MGPumStateCondition stateCondition) : base(typeof(MGPumZoneChangeEvent), stateCondition)
        {
            if (enterFilter is MGPumSourceFilter)
            {
                this.activeZone = MGPumZoneType.Any;
            }
            else
            {
                this.activeZone = MGPumZoneType.Battlegrounds;
            }
            this.leaveFilter = enterFilter;
        }

        protected override bool checkInternal(MGPumGameEvent gameEvent, MGPumGameState state, MGPumEntity source)
        {
            MGPumZoneChangeEvent zce = (MGPumZoneChangeEvent)gameEvent;
            if(zce.destination.getZoneType() == MGPumZoneType.Destroyed && zce.parent is MGPumDestroyUnitSEvent)
            {
                return leaveFilter.test(state, zce.unit, source);
            }
            else
            {
                return false;
            }
            
        }
        
    }
}
