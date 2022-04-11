using RelegatiaCCG.rccg.engine.state;

namespace mg.pummelz
{
    [System.Serializable]
    public abstract class MGPumOngoingEffect : MGPumEffect
    {
        public static int PRIORITY_NORMAL = 5;
        public static int PRIORITY_LATE_BEFORE_BONI = 6;
        public static int PRIORITY_LATE_AFTER_BONI = 7;

        public int priority = PRIORITY_NORMAL;

        public MGPumFilter affectedEntityFilter { get; set; }

        public void apply(MGPumGameState state, MGPumEntity sourceEntity)
        {
            applyInternal(state, affectedEntityFilter, sourceEntity);
        }

        protected abstract void applyInternal(MGPumGameState state, MGPumSelection affectedEntities, MGPumEntity source);

        internal override void setEffectIDs(IDManager idm)
        {
            this.id = idm.getNextID();
        }

        internal MGPumOngoingEffect withPriority(int priority)
        {
            this.priority = priority;
            return this;
        }

    }
}
