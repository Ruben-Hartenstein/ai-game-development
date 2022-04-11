namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumGrantExtraTurnEvent : MGPumGameEvent
    {
        public int playerID { get; set; }

        public MGPumGrantExtraTurnEvent(int playerID) : base()
        {
            this.playerID = playerID;
        }

        public override void apply(MGPumGameState state)
        {
            state.extraTurnQueue.Enqueue(playerID);
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumGrantExtraTurnEvent result = new MGPumGrantExtraTurnEvent(this.playerID);
            this.copyToGameEvent(result);
            return result;
        }
    }
}
