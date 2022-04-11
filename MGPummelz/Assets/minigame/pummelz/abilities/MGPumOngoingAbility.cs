using RelegatiaCCG.rccg.engine.state;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumOngoingAbility : MGPumAbility
    {
        public MGPumOngoingEffect effect { get; set; }

        protected MGPumOngoingAbility()
        {

        }

        public MGPumOngoingAbility(MGPumOngoingEffect effect)
        {

            this.effect = effect;

        }

        protected void copyToOngoingAbility(MGPumOngoingAbility a)
        {
            base.copyToAbility(a);
            a.effect = effect;
        }

        public override MGPumAbility deepCopy()
        {
            MGPumOngoingAbility a = new MGPumOngoingAbility();
            this.copyToOngoingAbility(a);
            return a;
        }

        public override void setEffectIDs(IDManager idm)
        {
            this.effect.setEffectIDs(idm);
        }

        public MGPumOngoingAbility ifSC(MGPumStateCondition condition)
        {
            this.enabledCondition = condition;
            return this;
        }

      
    }
}
