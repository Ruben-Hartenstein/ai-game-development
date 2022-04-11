using RelegatiaCCG.rccg.engine.state;
using RelegatiaCCG.rccg.i18n;
using System;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumSequentialEffect : MGPumMultiEffect
    {
        public const int EESTATE_BATCH_NUMBER = 0;
        public const int EESTATE_BATCH_LIST = 1;
        public const int EESTATE_SELECTIONS = 2;
        public const int EESTATE_INTERNAL_EESTATES = 3;

        internal List<MGPumEffectSequenceItem> effectSequence;


        public MGPumSequentialEffect()
        {
            effectSequence = new List<MGPumEffectSequenceItem>();
        }

        public MGPumSequentialEffect(List<MGPumEffectSequenceItem> effectSequence)
        {
            this.effectSequence = effectSequence;
        }

        public MGPumSequentialEffect and(MGPumOneTimeEffect effect)
        {
            effectSequence.Add(new MGPumEffectSequenceItem(effect, false));

            return this;
        }

        public MGPumSequentialEffect andThen(MGPumOneTimeEffect effect)
        {
            effectSequence.Add(new MGPumEffectSequenceItem(effect, true));
            return this;
        }

        private List<List<MGPumEffectSequenceItem>> getBatches()
        {
            List<List<MGPumEffectSequenceItem>> batches = new List<List<MGPumEffectSequenceItem>>();
            List<MGPumEffectSequenceItem> currentBatch = new List<MGPumEffectSequenceItem>();
            foreach (MGPumEffectSequenceItem esi in effectSequence)
            {
                if(esi.applyStateBasedBefore)
                {
                    batches.Add(currentBatch);
                    currentBatch = new List<MGPumEffectSequenceItem>();
                }
                currentBatch.Add(esi);
            }
            batches.Add(currentBatch);
            return batches;
        }

        internal override bool executeInternal(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            int batchNumber = 0;
            if(eestate.contains(EESTATE_BATCH_NUMBER))
            {
                batchNumber = eestate.get<int>(EESTATE_BATCH_NUMBER);
            }
            if (!eestate.contains(EESTATE_BATCH_LIST))
            {
                eestate.set<List<List<MGPumEffectSequenceItem>>>(EESTATE_BATCH_LIST, getBatches());
            }
            if (!eestate.contains(EESTATE_SELECTIONS))
            {
                eestate.set<List<List<MGPumEntity>>>(EESTATE_SELECTIONS, new List<List<MGPumEntity>>());
            }

            if (!eestate.contains(EESTATE_INTERNAL_EESTATES))
            {
                eestate.set<Dictionary<int, MGPumEffectExecutionState>>(EESTATE_INTERNAL_EESTATES, new Dictionary<int, MGPumEffectExecutionState>());
            }

            

            List<List<MGPumEffectSequenceItem>> batches = eestate.get<List<List<MGPumEffectSequenceItem>>>(EESTATE_BATCH_LIST);

            List<MGPumEffectSequenceItem> currentBatch = batches[batchNumber];

            List<List<MGPumEntity>> selections = eestate.get<List<List<MGPumEntity>>>(EESTATE_SELECTIONS);

            Dictionary<int, MGPumEffectExecutionState> internalEEEstates = eestate.get<Dictionary<int, MGPumEffectExecutionState>>(EESTATE_INTERNAL_EESTATES);

            foreach (MGPumEffectSequenceItem esi in currentBatch)
            {
                if(esi.effect is MGPumSingleEffect)
                {
                    MGPumSingleEffect se = (MGPumSingleEffect)esi.effect;

                    MGPumSelector sEffectSelector = se.getSelector(gc.state, source);
                    MGPumSelection sEffectSelection = null;

                    sEffectSelection = sEffectSelector.getSelection();
                    
                    se.executeInternal(parent, gc, handler, sEffectSelection, source, triggeringEvent, eestate);
                    //NTOE: do not add before execution, as a reference then may get itself (which is a problem combined with RLT)
                    selections.Add(sEffectSelection.getSelection(gc.state, source, triggeringEvent, eestate));
                }
                else if(esi.effect is MGPumMultiEffect)
                {
                    if(!internalEEEstates.ContainsKey(batchNumber))
                    {
                        internalEEEstates.Add(batchNumber, new MGPumEffectExecutionState());
                    }
                    MGPumEffectExecutionState internalEEstate = internalEEEstates[batchNumber];

                    MGPumMultiEffect me = (MGPumMultiEffect)esi.effect;

                    MGPumSelector sEffectSelector = me.getSelector(gc.state, source, internalEEstate);

                    MGPumSelection sEffectSelection = null;
                    
                    
                    sEffectSelection = sEffectSelector.getSelection();
                    
                    me.executeInternal(parent, gc, handler, sEffectSelection, source, triggeringEvent, internalEEstate);
                    //NTOE: do not add before execution, as a reference then may get itself (which is a problem combined with RLT)
                    //selections.Add(sEffectSelection.getSelection(gc.state, source, triggeringEvent, eestate));
                }
                else
                {
                    throw new NotImplementedException("Cannot nest effect type:" + esi.effect.GetType());
                }
                
            }
         

            batchNumber++;
            eestate.set<int>(EESTATE_BATCH_NUMBER, batchNumber);
            if(batchNumber >= batches.Count)
            {
                //execution has finished after last batch
                return true;
            }
            else
            {
                return false;
            }

            
        }

        public override MGPumSelector getSelector(MGPumGameState state, MGPumEntity source)
        {

            return getSelector(state, source, new MGPumEffectExecutionState());
        }

        public override MGPumSelector getSelector(MGPumGameState state, MGPumEntity source, MGPumEffectExecutionState eestate)
        {
            int batchNumber = 0;
            if (eestate.contains(EESTATE_BATCH_NUMBER))
            {
                batchNumber = eestate.get<int>(EESTATE_BATCH_NUMBER);
            }
            if (!eestate.contains(EESTATE_BATCH_LIST))
            {
                eestate.set<List<List<MGPumEffectSequenceItem>>>(EESTATE_BATCH_LIST, getBatches());
            }
            List<List<MGPumEffectSequenceItem>> batches = eestate.get<List<List<MGPumEffectSequenceItem>>>(EESTATE_BATCH_LIST);

            List<MGPumEffectSequenceItem> currentBatch = batches[batchNumber];

            List<MGPumOneTimeEffect> effects = new List<MGPumOneTimeEffect>();


            MGPumSelector result = new MGPumDummyFilter();

            return result;
        }

        internal override MGPumEffect deepCopy(MGPumGameState state)
        {
            List<MGPumEffectSequenceItem> copiedEsis = new List<MGPumEffectSequenceItem>();

            foreach(MGPumEffectSequenceItem esi in effectSequence)
            {
                copiedEsis.Add(esi.deepCopy(state));
            }

            MGPumSequentialEffect result = new MGPumSequentialEffect(copiedEsis);

            return result;
        }

        internal override MGPumEffect lookupSubEffect(MGPumEffect effect)
        {
            if (this.id == effect.id)
            {
                return this;
            }

            MGPumEffect result = null;
            foreach(MGPumEffectSequenceItem esi in effectSequence)
            {
                if(esi.effect.id == effect.id)
                {
                    result = esi.effect;
                    break;
                }
                else if(esi.effect is MGPumMultiEffect)
                {
                    MGPumEffect foundSubEffect = ((MGPumMultiEffect)esi.effect).lookupSubEffect(effect);
                    if(foundSubEffect != null)
                    {
                        result = foundSubEffect;
                        break;
                    }
                }
            }
            return result;
        }

        internal override void setEffectIDs(IDManager idm)
        {
            this.id = idm.getNextID();
            foreach (MGPumEffectSequenceItem esi in effectSequence)
            {
                esi.effect.setEffectIDs(idm);
            }
        }
    }

}
