namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumLoseGameEvent : MGPumGameEvent, MGPumPlayerReferencable
    {
        public int playerID { get; set; }

        public MGPumLoseGameEvent(int playerID) : base()
        {
            this.playerID = playerID;
        }

        


        public override void apply(MGPumGameState state)
        {
            MGPumPlayer p = state.getPlayer(playerID);
            p.losing = true;
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            return new MGPumLoseGameEvent(this.playerID);
        }

        public int getReferencablePlayerID()
        {
            return playerID;
        }
    }
}
