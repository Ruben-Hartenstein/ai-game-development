namespace mg.pummelz
{
    [System.Serializable]
    class MGPumStartTurnSEvent : MGPumSuperEvent, MGPumPlayerReferencable
    {
        public int playerID { get; set; }

        public MGPumStartTurnSEvent(int playerID) : base()
        {
            this.playerID = playerID;
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumGameEvent result = new MGPumStartTurnSEvent(this.playerID);
            this.copyToGameEvent(result);
            return result;
        }

        public int getReferencablePlayerID()
        {
            return playerID;
        }
    }
}
