
using RelegatiaCCG.rccg.engine;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumStateCondition : MGPumCondition
    {
        

        protected List<List<MGPumSubCondition>> alternativeConditionChains;

        protected List<MGPumSubCondition> conditionChain;

        public MGPumStateCondition()
        {
            this.conditionChain = new List<MGPumSubCondition>();
            this.alternativeConditionChains = new List<List<MGPumSubCondition>>();
            this.alternativeConditionChains.Add(this.conditionChain);
        }

        public bool check(MGPumGameState state, MGPumEntity source)
        {
            //result is false unless one condition chain is true (OR)
            bool result = false;
            foreach (IEnumerable<MGPumSubCondition> conditionChain in alternativeConditionChains)
            {
                //chain result is true unless one condition in chain is false (AND)
                bool chainResult = true;
                foreach (MGPumSubCondition c in conditionChain)
                {
                    if (!c.check(state, source))
                    {
                        chainResult = false;
                        break;
                    }

                }
                if (chainResult)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }


        public MGPumStateCondition addSubCondition(MGPumSubCondition Condition)
        {
            this.conditionChain.Add(Condition);
            return this;
        }

        public MGPumStateCondition exists(MGPumFilter filterCondition)
        {
            this.addSubCondition(new MGPumCountMinCondition(filterCondition, 1));

            return this;
        }

        public MGPumStateCondition notExists(MGPumFilter filterCondition)
        {
            this.addSubCondition(new MGPumCountMaxCondition(filterCondition, 0));

            return this;
        }

        public MGPumStateCondition moreThanX(MGPumFilter filterCondition, int x)
        {
            this.addSubCondition(new MGPumCountMinCondition(filterCondition, x + 1));

            return this;
        }

      

        public MGPumStateCondition healthXOrLessThan(int max)
        {
            this.addSubCondition(new MGPumHealthXOrLessCondition(MGPumFF.source(), max));

            return this;
        }

     
        public MGPumStateCondition thisHasDamage()
        {
            this.addSubCondition(new MGPumHasDamageCondition(MGPumFF.source()));

            return this;
        }

        public MGPumStateCondition thisSurvivedDamage()
        {
            this.addSubCondition(new MGPumSurvivedDamageCondition(MGPumFF.source()));

            return this;
        }


           public MGPumStateCondition xOrMore(MGPumFilter filterCondition, int x)
        {
            this.addSubCondition(new MGPumCountMinCondition(filterCondition, x));

            return this;
        }

      

   

        public MGPumStateCondition wasLastUnit()
        {
            this.addSubCondition(new MGPumWasLastUnitCondition());

            return this;

        }

        public MGPumStateCondition or()
        {
            this.conditionChain = new List<MGPumSubCondition>();
            this.alternativeConditionChains.Add(this.conditionChain);
            return this;
        }

     
    }
}
