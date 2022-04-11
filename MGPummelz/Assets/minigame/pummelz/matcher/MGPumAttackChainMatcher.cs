using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumAttackChainMatcher : MGPumFieldChainMatcher
    {

        public MGPumAttackChainMatcher(MGPumUnit unit) : base(unit)
        {
        }

        protected override bool isValidExtension(MGPumFieldChain chain, MGPumField first, MGPumField last)
        {
            //has to attack a non-friendly unit
            bool result = last.unit != null && last.unit.ownerID != startUnit.ownerID;

            return result;
        }

        public override bool matchesChain(MGPumField field, MGPumFieldChain chain)
        {
            int rangeSoFar = chain.getLength() - 1;
            if(chain.last == null)
            {
                //chains have to start at the unit
                return field == startUnit.field;
            }
            else
            {
                return (
                    //you cannot skip fields
                    (chain.last.neighboring(field)) &&
                    //cannot go farther than range
                    rangeSoFar < startUnit.currentRange &&
                    //cannot attack over other troops
                    (chain.last.unit == null || chain.last.unit == startUnit))
                    //shots can be angled but cannot zigzag            
                    && checkShotDirection(field, chain);
            }

            
        }

        
        private bool checkShotDirection(MGPumField field, MGPumFieldChain chain)
        {
            int xdir = 0;
            int ydir = 0;

            foreach (MGPumField f in chain) {
                if(f.x > startUnit.field.x)
                {
                    xdir = 1;
                }
                else if (f.x < startUnit.field.x)
                {
                    xdir = -1;
                }

                if (f.y > startUnit.field.y)
                {
                    ydir = 1;
                }
                else if (f.y < startUnit.field.y)
                {
                    ydir = -1;
                }
            }

            return (xdir == 0 || (xdir == 1 && field.x >= chain.last.x) || (xdir == -1 && field.x <= chain.last.x)) &&
                (ydir == 0 || (ydir == 1 && field.y >= chain.last.y) || (ydir == -1 && field.y <= chain.last.y));

        }
    }



}

