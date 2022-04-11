namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumWinGameEvent : MGPumGameEvent, MGPumPlayerReferencable
    {
        public int playerID { get; set; }

        public MGPumWinGameEvent(int playerID) : base()
        {
            this.playerID = playerID;
        }

        


        public override void apply(MGPumGameState state)
        {
            MGPumPlayer p = state.getPlayer(playerID);
            p.winning = true;
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumWinGameEvent wge = new MGPumWinGameEvent(this.playerID);
            copyToGameEvent(wge);
            return wge;
        }

        public int getReferencablePlayerID()
        {
            return playerID;
        }
    }
}
