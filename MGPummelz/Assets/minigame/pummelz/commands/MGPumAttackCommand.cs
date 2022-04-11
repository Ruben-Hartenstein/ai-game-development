using UnityEngine;

namespace mg.pummelz
{

    [System.Serializable]
    public class MGPumAttackCommand : MGPumCommand
    {


        public MGPumFieldChain chain;

        public MGPumUnit attacker;

        public MGPumUnit defender;

        public MGPumAttackCommand(int issuingPlayer, MGPumFieldChain chain, MGPumUnit attacker)
        {
            this.issuingPlayer = issuingPlayer;
            this.chain = chain;
            this.attacker = attacker;
            this.defender = chain.getLast().unit;
        }

        public override string ToString()
        {
            return "Attack( " + attacker + " -> " + defender + " )";
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumAttackCommand ac = new MGPumAttackCommand(this.issuingPlayer, this.chain.deepCopy(state), state.lookupUnit(attacker.id));
           
            copyToCommand(ac);
            return ac;
        }
    }
}
