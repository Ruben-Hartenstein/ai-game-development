using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumMoveChainMatcher : MGPumFieldChainMatcher
    {

        public MGPumMoveChainMatcher(MGPumUnit unit) : base(unit)
        {
        }

        protected override bool isValidExtension(MGPumFieldChain chain, MGPumField first, MGPumField last)
        {
            //can only move over empty fields
            bool valid = true;
            foreach(MGPumField f in chain)
            {
                if(f != first && f.unit != null)
                {
                    valid = false;
                }
            }
            
            return valid;
        }

        public override bool matchesChain(MGPumField field, MGPumFieldChain chain)
        {
            int speedSoFar = chain.getLength() - 1;
            
            return (field.unit == null || field.unit == startUnit) && ((chain.last == null || chain.last.neighboring(field)) && speedSoFar < startUnit.currentSpeed);
        }
    }



}

