using System;

namespace mg.pummelz
{
    [System.Serializable]
    public abstract class MGPumEventCondition : MGPumCondition
    {
        public Type eventType { get; set; }

        public MGPumZoneType activeZone;

        protected MGPumStateCondition stateCondition;

        public MGPumEventCondition()
        {
            this.eventType = null;
            this.stateCondition = null;
            this.activeZone = MGPumZoneType.Battlegrounds;
        }

        public MGPumEventCondition(Type eventTypeName, MGPumStateCondition stateCondition) 
        {
            
            this.eventType = eventTypeName;
            this.stateCondition = stateCondition;
        }

        public bool check(MGPumGameEvent gameEvent, MGPumGameState state, MGPumEntity source)
        {
            bool result = true;
            result &= eventType.IsAssignableFrom(gameEvent.GetType());

            if (result && (source.getZone() != activeZone) && activeZone != MGPumZoneType.Any)
            {
                result = false;
            }

            if (result)
            {
                result &= this.checkInternal(gameEvent, state, source);
            }

            if (result && stateCondition != null)
            {
                result &= stateCondition.check(state, source);
            }

            return result;
        }

        protected abstract bool checkInternal(MGPumGameEvent gameEvent, MGPumGameState state, MGPumEntity source);
        
    }
}
