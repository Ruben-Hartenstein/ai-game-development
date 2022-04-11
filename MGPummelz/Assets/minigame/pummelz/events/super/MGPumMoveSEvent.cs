namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumMoveSEvent : MGPumSuperEvent
    {
        public MGPumFieldChain chain;

        public MGPumUnit mover;

        public int playerID;


        public MGPumMoveSEvent(int playerID, MGPumFieldChain chain, MGPumUnit mover) : base()
        {
            this.playerID = playerID;
            this.chain = chain;
            this.mover = mover;
        }

        public override void apply(MGPumGameState state)
        {
            this.mover.movesThisTurn += 1;
        }


        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {

            MGPumGameEvent result = new MGPumMoveSEvent(playerID, chain.deepCopy(state), (MGPumUnit)state.lookupOrCreate(mover));
            
            this.copyToGameEvent(result);

            return result;

        }
    }
}
