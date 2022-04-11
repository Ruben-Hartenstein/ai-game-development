namespace mg.pummelz
{
    [System.Serializable]
    public abstract class MGPumCommand : MGPumGameEvent, MGPumPlayerReferencable
    {
        public int issuingPlayer { get; set; }

        public override void apply(MGPumGameState state)
        {

        }

        public void copyToCommand(MGPumCommand c)
        {
            copyToGameEvent(c);
            c.issuingPlayer = this.issuingPlayer;
        }

        public int getReferencablePlayerID()
        {
            return issuingPlayer;
        }
    }

}
