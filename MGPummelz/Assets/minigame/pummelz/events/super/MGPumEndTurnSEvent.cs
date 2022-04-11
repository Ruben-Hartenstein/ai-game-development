namespace mg.pummelz
{
    [System.Serializable]
    class MGPumEndTurnSEvent : MGPumSuperEvent, MGPumPlayerReferencable
    {
        internal int playerID;

        public MGPumEndTurnSEvent(int playerID) : base()
        {
            this.playerID = playerID;
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumGameEvent result = new MGPumEndTurnSEvent(playerID);
            this.copyToGameEvent(result);
            return result;
        }

        public int getReferencablePlayerID()
        {
            return playerID;
        }
    }
}
