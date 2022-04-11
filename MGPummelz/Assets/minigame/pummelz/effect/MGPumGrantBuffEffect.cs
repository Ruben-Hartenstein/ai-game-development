using RelegatiaCCG.rccg.engine.exceptions;
using RelegatiaCCG.rccg.i18n;
using System;
using System.Collections.Generic;
using System.Linq;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumGrantBuffEffect : MGPumSingleEffect
    {
        private MGPumSelector selector;

        public MGPumBuffOEffect buffEffect;
        public MGPumCondition duration;
        public MGPumOngoingAbility ongoingAbility;

        public MGPumGrantBuffEffect(MGPumBuffOEffect buffEffect, MGPumCondition duration, MGPumSelector selector)
        {
            this.buffEffect = buffEffect;
            this.duration = duration;
            ongoingAbility = (MGPumOngoingAbility)new MGPumOngoingAbility(buffEffect).withDuration(duration);
            this.selector = selector;
        }



        internal override void executeInternal(MGPumGameEvent parent, MGPumGameCommands gc, MGPumGameEventHandler handler, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {

            List<MGPumEntity> selectedEntities = selection.getSelection(gc.state, source, triggeringEvent, eestate);
            foreach(MGPumEntity e in selectedEntities)
            {
                if(e is MGPumUnit)
                {
                    
                    MGPumUnit c = (MGPumUnit)e;

                    MGPumAbility a =  ongoingAbility.deepCopy();
                    //Player selection for duration depends on who grants the ability and who get's it granted
                    //e.g. until end of your turn needs to be "reversed" if ability is granted to enemy unit
                    if(a.durationCondition is MGPumConditionWithPST)
                    {
                        MGPumConditionWithPST adcWithPST = (MGPumConditionWithPST)a.durationCondition;

                        if(adcWithPST.getPST() == MGPumPST.GrantSource)
                        {
                            if(c.ownerID == source.ownerID)
                            {
                                adcWithPST.setPST(MGPumPST.Source);
                            }
                            else
                            {
                                adcWithPST.setPST(MGPumPST.Opponent);
                            }
                        }
                        else if (adcWithPST.getPST() == MGPumPST.GrantOpponent)
                        {
                            if (c.ownerID == source.ownerID)
                            {
                                adcWithPST.setPST(MGPumPST.Opponent);
                            }
                            else
                            {
                                adcWithPST.setPST(MGPumPST.Source);
                            }
                        }
                    }
                    //Don't grant id here, it will lead to a state mismatch
                    //a.id = gc.state.idm.getNextID();
                    //gc.grantAbility(parent, c, a);
                    throw new NotImplementedException();
                }
                else
                {
                    throw new GameException("Entity " + e + " is not a unit.");
                }
                
            }

            
        }

        public override MGPumSelector getSelector(MGPumGameState state, MGPumEntity source)
        {
            return selector;
        }

        internal override MGPumEffect deepCopy(MGPumGameState state)
        {
            return new MGPumGrantBuffEffect((MGPumBuffOEffect)this.buffEffect.deepCopy(state), this.duration, this.selector.deepCopySelector(state));
        }
    }
}
