namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumStartGameRunningEvent : MGPumGameEvent
    {

        public MGPumStartGameRunningEvent() : base()
        {
            
        }

        public override void apply(MGPumGameState state)
        {
            state.gameRunning = true;
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumGameEvent result = new MGPumStartGameRunningEvent();
            this.copyToGameEvent(result);
            return result;

        }
    }
}
