namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumAttackSEvent : MGPumSuperEvent
    {
        public MGPumFieldChain chain;

        public int damage;

        public MGPumUnit attacker;
        public MGPumUnit defender;

        public int playerID;


        public MGPumAttackSEvent(int playerID, MGPumFieldChain chain, int damage, MGPumUnit attacker, MGPumUnit target) : base()
        {
            this.playerID = playerID;
            this.chain = chain;
            this.attacker = attacker;
            this.defender = target;
            this.damage = damage;
            
        }

        public override void apply(MGPumGameState state)
        {
            this.attacker.attacksThisTurn += 1;
        }


        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {

            MGPumGameEvent result = new MGPumAttackSEvent(playerID, chain.deepCopy(state), damage, (MGPumUnit)state.lookupOrCreate(attacker), (MGPumUnit)state.lookupOrCreate(defender));
            
            this.copyToGameEvent(result);

            return result;

        }
    }
}
