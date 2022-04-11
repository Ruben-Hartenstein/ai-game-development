using RelegatiaCCG.rccg.engine.state;
using System;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumTriggeredAbility : MGPumAbility
    {
        public MGPumEffect effect { get; set; }

        public bool oneTime { get; set; }

        public bool isReplacement { get; set; }
        public MGPumEventCondition trigger { get; set; }

        public MGPumTriggeredAbility(MGPumEventCondition trigger, MGPumEffect effect)
        {
            this.trigger = trigger;
            if(effect is MGPumReplacementEffect)
            {
                this.isReplacement = true;
            }
            else if(effect is MGPumOneTimeEffect)
            {
                this.isReplacement = false;
            }
            else
            {
                throw new Exception("Unsupported effect type for triggered ability: " + effect);
            }
            
            this.effect = effect;
            this.activeZone = trigger.activeZone;
        }

        public MGPumTriggeredAbility(MGPumEventCondition trigger, MGPumStateCondition condition, MGPumEffect effect)
        {
            this.trigger = trigger;

            if (effect is MGPumReplacementEffect)
            {
                this.isReplacement = true;
            }
            else if (effect is MGPumOneTimeEffect)
            {
                this.isReplacement = false;
            }
            else
            {
                throw new Exception("Unsupported effect type for triggered ability: " + effect);
            }

            this.enabledCondition = condition;
            this.effect = effect;
            this.activeZone = trigger.activeZone;
        }

        protected MGPumTriggeredAbility()
        {
        }

        public bool isTriggeredBy(MGPumGameEvent e, MGPumGameState state, MGPumEntity source)
        {
            //Debug.LogError("this.isEnabled(state, source)" + this.isEnabled(state, source));
            //Debug.LogError("trigger.check(e, state, source)" + trigger.check(e, state, source));
            return trigger.check(e, state, source) && this.isEnabled(state, source);
        }

        protected void copyToTriggeredAbility(MGPumTriggeredAbility a)
        {
            base.copyToAbility(a);
            a.isReplacement = this.isReplacement;
            a.trigger = this.trigger;
            a.effect = this.effect;
            a.oneTime = this.oneTime;
        }

        public override MGPumAbility deepCopy()
        {
            MGPumTriggeredAbility a = new MGPumTriggeredAbility();
            this.copyToTriggeredAbility(a);
            return a;
        }

        public override void setEffectIDs(IDManager idm)
        {
            this.effect.setEffectIDs(idm);
        }

        public override MGPumAbility ifInZone(MGPumZoneType zone)
        {
            this.activeZone = zone;
            this.trigger.activeZone = zone;
            return this;
        }

        public MGPumTriggeredAbility usuallyOneTime()
        {
            this.oneTime = true;
            return this;
        }
    }
}
