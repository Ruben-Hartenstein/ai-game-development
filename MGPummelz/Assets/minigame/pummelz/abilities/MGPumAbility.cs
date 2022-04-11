using RelegatiaCCG.rccg.engine.state;
using RelegatiaCCG.rccg.i18n;
using System;

namespace mg.pummelz
{
    [System.Serializable]
    public abstract class MGPumAbility : MGPumEntity, I18nizable
    {
        public MGPumEntity owner { get; set; }

        public MGPumStateCondition enabledCondition { get; set; }
        public MGPumCondition durationCondition { get; set; }

        public MGPumZoneType activeZone = MGPumZoneType.Battlegrounds;

        public String text;

        public override MGPumZoneType getZone()
        {
            return MGPumZoneType.Destroyed;
        }

        public bool isEnabled(MGPumGameState state, MGPumEntity source)
        {
            if(activeZone == source.getZone() || activeZone == MGPumZoneType.Any)
            {
                if (enabledCondition == null)
                {
                    return true;
                }
                else
                {
                    return enabledCondition.check(state, source);
                }
            }
            else
            {
                return false;
            }
            
            
        }

        protected void copyToAbility(MGPumAbility a)
        {
            copyToGameEntity(a);
            //if anyone copies something, they are responsible for updating the owner of all abilities
            a.owner = null;
            a.enabledCondition = enabledCondition;
            a.activeZone = this.activeZone;
            a.durationCondition = durationCondition;
            a.text = this.text;
        }

        public MGPumAbility withDuration(MGPumCondition durationCondition)
        {
            this.durationCondition = durationCondition;
            return this;
        }

        public MGPumAbility withText(String text)
        {
            this.text = text;
            return this;
        }

        public MGPumAbility withEnabled(MGPumStateCondition enabledCondition)
        {
            this.enabledCondition = enabledCondition;
            return this;
        }

        public abstract void setEffectIDs(IDManager idm);


        public abstract MGPumAbility deepCopy();

        public string toI18nedString()
        {
            return text;
        }

        public virtual MGPumAbility ifInZone(MGPumZoneType zone)
        {
            this.activeZone = zone;
            return this;
        }
    }
}
