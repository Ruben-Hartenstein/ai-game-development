using RelegatiaCCG.rccg.engine.state;
using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumConditionalEffect : MGPumMultiEffect
    {
        private const int EESTATE_MODE = 0;
        private const int EESTATE_INTERNAL_EESTATE = 1;

        private MGPumStateCondition condition;
        private MGPumOneTimeEffect ifEffect;
        private MGPumOneTimeEffect elseEffect;

        public MGPumConditionalEffect(MGPumStateCondition condition, MGPumOneTimeEffect ifEffect)
        {
            this.condition = condition;
            this.ifEffect = ifEffect;
        }


        public MGPumConditionalEffect(MGPumStateCondition condition, MGPumOneTimeEffect ifEffect, MGPumOneTimeEffect elseEffect)
        {
            this.condition = condition;
            this.ifEffect = ifEffect;
            this.elseEffect = elseEffect;
        }


        public override MGPumSelector getSelector(MGPumGameState state, MGPumEntity source)
        {
            if(condition.check(state, source))
            {
                return ifEffect.getSelector(state, source);
            }
            else if(elseEffect != null)
            {
                return elseEffect.getSelector(state, source);
            }
            else
            {
                return new MGPumDummyFilter();
            }
        }

        public override MGPumSelector getSelector(MGPumGameState state, MGPumEntity source, MGPumEffectExecutionState eestate)
        {
            int mode = -1;
            MGPumEffectExecutionState internalState = null;
            if (eestate.contains(EESTATE_MODE))
            {
                mode = eestate.get<int>(EESTATE_MODE);
            }
            if (eestate.contains(EESTATE_INTERNAL_EESTATE))
            {
                internalState = eestate.get<MGPumEffectExecutionState>(EESTATE_INTERNAL_EESTATE);
            }
            if (mode == 1)
            {
                return ifEffect.getSelector(state, source, internalState);
            }
            else if (mode == 0)
            {
                if(elseEffect != null)
                {
                    return elseEffect.getSelector(state, source, internalState);
                }
                else
                {
                    return new MGPumDummyFilter();
                }
                
            }
            else
            {
                //we cannot modify the execution state, so we dafault to now state
                return getSelector(state,source);
            }
        }

        internal override MGPumEffect deepCopy(MGPumGameState state)
        {
            if(elseEffect != null)
            {
                return new MGPumConditionalEffect(condition, (MGPumOneTimeEffect)ifEffect.deepCopy(state), (MGPumOneTimeEffect)elseEffect.deepCopy(state));
            }
            else
            {
                return new MGPumConditionalEffect(condition, (MGPumOneTimeEffect)ifEffect.deepCopy(state));
            }
        }

        internal override bool executeInternal(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            int mode = -1;
            MGPumEffectExecutionState internalState = null;
            if (eestate.contains(EESTATE_MODE))
            {
                mode = eestate.get<int>(EESTATE_MODE);
            }
            if (eestate.contains(EESTATE_INTERNAL_EESTATE))
            {
                internalState = eestate.get<MGPumEffectExecutionState>(EESTATE_INTERNAL_EESTATE);
            }
            if(mode == -1)
            {
                if(condition.check(gc.state, source))
                {
                    mode = 1;
                }
                else
                {
                    mode = 0;
                }
                eestate.set<int>(EESTATE_MODE, mode);
            }
            if(internalState == null)
            {
                internalState = new MGPumEffectExecutionState();
                eestate.set<MGPumEffectExecutionState>(EESTATE_INTERNAL_EESTATE, internalState);
            }

            if (mode == 1)
            {
                return ifEffect.execute(parent, gc, handler, selection, source, triggeringEvent, internalState);
            }
            else
            {
                if (elseEffect != null)
                {
                    return elseEffect.execute(parent, gc, handler, selection, source, triggeringEvent, internalState);
                }
                else
                {
                    //no else, so we are finished
                    return true;
                }
            }
            
        }

        internal override MGPumEffect lookupSubEffect(MGPumEffect effect)
        {
            if(effect.id == ifEffect.id)
            {
                return ifEffect;
            }
            else if(elseEffect != null && effect.id == elseEffect.id)
            {
                return elseEffect;
            }
            else
            {
                if (ifEffect is MGPumMultiEffect)
                {
                    MGPumEffect result = ((MGPumMultiEffect)ifEffect).lookupSubEffect(effect);
                    if (result != null)
                    {
                        return result;
                    }
                }

                if (elseEffect != null && elseEffect is MGPumMultiEffect)
                {
                    MGPumEffect result = ((MGPumMultiEffect)elseEffect).lookupSubEffect(effect);
                    if (result != null)
                    {
                        return result;
                    }
                }
                return null;
            }
        }


        internal override void setEffectIDs(IDManager idm)
        {
            this.id = idm.getNextID();
            ifEffect.setEffectIDs(idm);
            if(elseEffect != null)
            {
                elseEffect.setEffectIDs(idm);
            }
        }
    }
}
