using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public abstract class MGPumFieldChainMatcher
    {

        internal MGPumUnit startUnit;

        protected MGPumFieldChainMatcher(MGPumUnit unit)
        {
            this.startUnit = unit;
        }

        public abstract bool matchesChain(MGPumField crystal, MGPumFieldChain chain);

        protected abstract bool isValidExtension(MGPumFieldChain chain, MGPumField first, MGPumField last);

        public bool isValid(MGPumFieldChain chain)
        {
            

            bool valid = true;
            if(chain == null)
            {
                return false;
            }

            //TODO: no field may be contained twice
            List<MGPumField> previousCs = new List<MGPumField>();

            //crystals must be neighboring
            MGPumField first = null;
            MGPumField last = null;
            foreach(MGPumField f in chain)
            {
                if(first == null) {
                    first = f;
                }

                if(last != null)
                {
                    if(!last.neighboring(f))
                    {
                        valid = false;
                        break;
                    }
                }

                if (previousCs.Contains(f))
                {
                    valid = false;
                    break;
                }
                previousCs.Add(f);
                

                last = f;
            }
            
            if(valid)
            {
                valid = valid && first.unit != null && first.unit.id == startUnit.id && first.unit.ownerID == chain.ownerID;
            }

            //this might be a bit redundant but we need to make sure student AIs cannot cheat
            MGPumFieldChain copy = new MGPumFieldChain(chain.ownerID, this);
            foreach (MGPumField f in chain)
            {
                if(copy.canAdd(f))
                {
                    copy.add(f);
                }
                else
                {
                    valid = false;
                    break;
                }
            }

            //Debug.LogError("Base Valid=" + valid);
            return valid && isValidExtension(chain, first, last);

        }
    }



}

