using UnityEngine;

namespace mg.pummelz
{

    [System.Serializable]
    public class MGPumMoveCommand : MGPumCommand
    {


        public MGPumFieldChain chain;

        public MGPumUnit mover;

        public MGPumMoveCommand(int issuingPlayer, MGPumFieldChain chain, MGPumUnit mover)
        {
            this.issuingPlayer = issuingPlayer;
            this.chain = chain;
            this.mover = mover;
        }

        public override string ToString()
        {
            return "Move( " + mover + " -> " + chain.last + " )";
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumMoveCommand mc = new MGPumMoveCommand(this.issuingPlayer, this.chain.deepCopy(state), state.lookupUnit(mover.id));
           
            copyToCommand(mc);
            return mc;
        }
    }
}
