using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumRemoveAbilitiesOEffect : MGPumOngoingEffect
    {

        public MGPumRemoveAbilitiesOEffect(MGPumFilter affectedEntities)
        {

            this.affectedEntityFilter = affectedEntities;
        }

        protected override void applyInternal(MGPumGameState state, MGPumSelection affectedEntities, MGPumEntity source)
        {
            foreach (MGPumEntity affectedEntity in affectedEntities.getSelection(state, source))
            {
                if (affectedEntity is MGPumUnit)
                {
                    MGPumUnit c = (MGPumUnit)affectedEntity;
                    c.abilityCurrent = null;
                }
            }

        }

        internal override MGPumEffect deepCopy(MGPumGameState state)
        {
            return new MGPumRemoveAbilitiesOEffect(this.affectedEntityFilter);
        }
    }
}
