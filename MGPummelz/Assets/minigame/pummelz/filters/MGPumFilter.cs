using RelegatiaCCG.rccg.engine;
using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;
using System.Linq;

namespace mg.pummelz
{
    [System.Serializable]
    public abstract class MGPumFilter : MGPumAutoSelect
    {
        protected abstract List<MGPumEntity> applyBaseFilter(MGPumGameState state, MGPumEntity source);

        protected abstract List<MGPumEntity> applyBaseFilter(List<MGPumEntity> entities, MGPumEntity source);

        protected List<List<MGPumSubFilter>> alternativeFilterChains;

        internal List<MGPumSubFilter> filterChain;

        protected MGPumFilter()
        {
            this.filterChain = new List<MGPumSubFilter>();
            this.alternativeFilterChains = new List<List<MGPumSubFilter>>();
            this.alternativeFilterChains.Add(this.filterChain);
        }

        protected List<MGPumEntity> apply(List<MGPumEntity> entities, MGPumEntity source, bool useBaseFilterOnEntities)
        {
            List<MGPumEntity> baseFilteredEntities = null;
            if (useBaseFilterOnEntities)
            {
                baseFilteredEntities = applyBaseFilter(entities, source);
            }
            else
            {
                baseFilteredEntities = entities;
            }
            List<MGPumEntity> filteredEntities = baseFilteredEntities;
            if (alternativeFilterChains.Count == 1)
            {
                foreach (MGPumSubFilter f in filterChain)
                {
                    filteredEntities = f.apply(filteredEntities, source);
                }
            }
            else
            {
                List<MGPumEntity> filteredEntityList = new List<MGPumEntity>();
                List<MGPumEntity> remainingEntities = new List<MGPumEntity>(baseFilteredEntities);

                //we need to try with each filter
                foreach (IEnumerable<MGPumSubFilter> filterChain in alternativeFilterChains)
                {
                    //run remaining entities through all filters in chain
                    filteredEntities = remainingEntities;

                    foreach (MGPumSubFilter f in filterChain)
                    {
                        filteredEntities = f.apply(filteredEntities, source);

                    }
                    //all surviving entities can be added to result - as this is an alternative they do not need to be run through the other filterchains
                    foreach (MGPumEntity entity in filteredEntities)
                    {
                        if (!filteredEntityList.Contains(entity))
                        {
                            filteredEntityList.Add(entity);
                            remainingEntities.Remove(entity);
                        }
                    }
                }
                filteredEntities = filteredEntityList;
            }

            return filteredEntities;
        }



        public virtual bool test(MGPumGameState state, MGPumEntity entity, MGPumEntity source)
        {
            List<MGPumEntity> entities = apply(state, source);
            return entities.Contains(entity);

            //List<MGPumEntity> testList = new List<MGPumEntity>();
            //testList.Add(entity);
            //return apply(testList, source, true).Count() > 0;
        }

        public virtual bool test(MGPumEntity entity, MGPumEntity source)
        {
            List<MGPumEntity> testList = new List<MGPumEntity>();
            testList.Add(entity);
            return apply(testList, source, true).Count() > 0;
        }

        public List<MGPumEntity> apply(IEnumerable<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> entityList = new List<MGPumEntity>(entities);
            return apply(entityList, source, true);
        }

        public List<MGPumEntity> apply(List<MGPumEntity> entities, MGPumEntity source)
        {
            return apply(entities, source, true);

        }

        public List<MGPumEntity> apply(MGPumGameState state, MGPumEntity source)
        {
            return apply(applyBaseFilter(state, source), source, false);
        }


        public MGPumFilter addMGPumSubFilter(MGPumSubFilter filter)
        {
            this.filterChain.Add(filter);
            return this;
        }



        public MGPumFilter or()
        {
            this.filterChain = new List<MGPumSubFilter>();
            this.alternativeFilterChains.Add(this.filterChain);
            return this;
        }

        public virtual List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource)
        {
            return apply(state, selectionSource);
        }

        public virtual List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource, MGPumGameEvent triggeringEvent)
        {
            return apply(state, selectionSource);
        }

        public virtual List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            return apply(state, selectionSource);
        }

        public MGPumSelection getSelection()
        {
            return this;
        }



        public virtual MGPumSelection deepCopy(MGPumGameState state)
        {
            //Debug.LogWarning("Filter is not really deepcopied");
            return this;
        }

        public MGPumSelector deepCopySelector(MGPumGameState state)
        {
            return (MGPumSelector)(this.deepCopy(state));
        }

        protected void copyToFilter(MGPumFilter copy)
        {
            copy.filterChain = new List<MGPumSubFilter>();
            foreach (MGPumSubFilter f in filterChain)
            {
                copy.filterChain.Add(f);
            }

            copy.alternativeFilterChains = new List<List<MGPumSubFilter>>();

            foreach (List<MGPumSubFilter> afc in alternativeFilterChains)
            {
                List<MGPumSubFilter> copiedAFC = new List<MGPumSubFilter>();
                foreach (MGPumSubFilter sf in afc)
                {
                    copiedAFC.Add(sf);
                }
                copy.alternativeFilterChains.Add(copiedAFC);
            }

        }

        public MGPumFilter emptyField()
        {
            this.addMGPumSubFilter(new MGPumEmptyFieldFilter());
            return this;
        }

       
        public MGPumFilter other()
        {
            this.addMGPumSubFilter(new MGPumNotSourceFilter());
            return this;
        }

        public MGPumFilter allied()
        {
            this.addMGPumSubFilter(new MGPumAlliedFilter());
            return this;
        }

        public MGPumFilter enemy()
        {
            this.addMGPumSubFilter(new MGPumEnemyFilter());
            return this;
        }

        public MGPumFilter damaged()
        {
            this.addMGPumSubFilter(new MGPumDamagedFilter());
            return this;
        }

        internal MGPumFilter withSubType(MGPumST subtype)
        {
            this.addMGPumSubFilter(new MGPumSubTypeFilter(subtype));
            return this;
        }

        internal MGPumFilter withUnitID(string unitID)
        {
            this.addMGPumSubFilter(new MGPumUnitIDFilter(unitID));
            return this;
        }



        internal MGPumFilter withRarity(params MGPumCRT[] rarities)
        {
            this.addMGPumSubFilter(new MGPumRarityFilter(rarities));
            return this;
        }

      

        internal MGPumFilter withMinHealth(int minHealth)
        {
            this.addMGPumSubFilter(new MGPumHealthFilter(minHealth));
            return this;
        }

        internal MGPumFilter withMaxHealth(int maxHealth)
        {
            this.addMGPumSubFilter(new MGPumHealthFilter(MGPumHealthFilter.UNINITIALIZED, maxHealth));
            return this;
        }

        internal MGPumFilter aroundSource()
        {
            this.addMGPumSubFilter(new MGPumAroundSourceFilter());
            return this;
        }




    }
}
