namespace mg.pummelz
{

    [System.Serializable]
    public abstract class MGPumSequentialCGameEvent : MGPumGameEvent
    {
        protected MGPumSequentialCGameEvent(MGPumEntity source) : base()
        {
            this.source = source;
        }

        public MGPumEntity source;

        public abstract void execute(MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection);

        public abstract MGPumSelector getSelector(MGPumGameState state, MGPumEntity source);
    }
}
