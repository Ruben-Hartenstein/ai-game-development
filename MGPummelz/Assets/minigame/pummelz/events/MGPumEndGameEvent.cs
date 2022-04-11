namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumEndGameEvent : MGPumGameEvent
    {
        public MGPumGameResultType result { get; set; }

    

        public MGPumEndGameEvent(MGPumGameResultType result) : base()
        {
            this.result = result;

        }

        public override void apply(MGPumGameState state)
        {
            state.result = this.result;
            state.gameRunning = false;
            state.gameFinished = true;
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumGameEvent result = new MGPumEndGameEvent(this.result);
            this.copyToGameEvent(result);
            return result;
        }

    }
}
