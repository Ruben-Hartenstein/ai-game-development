using RelegatiaCCG.rccg.engine;
using System.Collections;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumFieldChain : MGPumEntity, IEnumerable<MGPumField>
    {
        public List<MGPumField> chain;

        public MGPumField last;

        public MGPumFieldChainMatcher matcher;

        public MGPumFieldChain(int ownerID, MGPumFieldChainMatcher matcher)
        {
            this.matcher = matcher;
            this.chain = new List<MGPumField>();
            this.ownerID = ownerID;
            last = null;
        }

        public int getLength()
        {
            return chain.Count;
        }

        public MGPumField getFirst()
        {
            if(chain.Count > 0)
            {
                return chain[0];
            }
            else
            {
                return null;
            }
        }

        public MGPumField getLast()
        {
            return last;
        }

        public void add(MGPumField field)
        {
            chain.Add(field);
            last = field;
        }

        public void removeLast()
        {
            chain.RemoveAt(chain.Count - 1);
            if(chain.Count >= 1)
            {
                last = chain[chain.Count - 1];
            }
            else
            {
                last = null;
            }
            
        }

        public void removeFirst()
        {
            chain.RemoveAt(0);
            if(chain.Count == 0)
            {
                last = null;
            }
        }

        public MGPumField getSecondToLast()
        {
            if(chain.Count > 1)
            {
                return chain[chain.Count - 2];
            }
            else
            {
                return null;
            }
            
        }

        public bool canAdd(MGPumField field)
        {
            return matcher.matchesChain(field, this) && !chain.Contains(field);
        }

        public bool isValidChain()
        {
            return matcher.isValid(this);
        }


        public MGPumFieldChain deepCopy(MGPumGameState state)
        {
            
            MGPumFieldChain copy = new MGPumFieldChain(ownerID, matcher);

            foreach (MGPumField c in chain)
            {
                copy.chain.Add((MGPumField)state.lookupEntity(c));
                
            }
            copy.last = ((MGPumField)state.lookupEntity(this.last));
            copyToGameEntity(copy);

            return copy;
        }

        public override MGPumZoneType getZone()
        {
            return MGPumZoneType.Destroyed;
        }

   

        public IEnumerator<MGPumField> GetEnumerator()
        {
            return ((IEnumerable<MGPumField>)chain).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<MGPumField>)chain).GetEnumerator();
        }

        //internal MGPumFieldChain getReverseChain()
        //{
        //    MGPumFieldChain rcopy = this.deepCopy(state);
        //    rcopy.chain.Reverse();
        //    rcopy.last = this.getFirst();
        //    return rcopy;
        //}

        public override string ToString()
        {
            string t = "";
            foreach (MGPumField c in chain)
            {
                t += c.ToString();
                
            }

            return t;
        }
    }



}

