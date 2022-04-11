namespace mg.pummelz
{

    [System.Serializable]
    public abstract class MGPumComplexGameEvent : MGPumGameEvent
    {
        public bool recalculateStateAfter = true;

        protected MGPumComplexGameEvent(MGPumEntity source) : base()
        {
            this.source = source;
        }

        public MGPumEntity source;

        public abstract void execute(MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection);

        public abstract MGPumSelector getSelector(MGPumGameState state, MGPumEntity source);

        public void copyToComplexGameEvent(MGPumGameEvent e)
        {
            ((MGPumComplexGameEvent)e).recalculateStateAfter = this.recalculateStateAfter;
            this.copyToGameEvent(e);
        }

    }
}
