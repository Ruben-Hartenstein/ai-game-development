using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumRedirectDamageREffect : MGPumReplacementEffect
    {
        public override MGPumGameEvent replace(MGPumGameEvent eventToReplace, MGPumGameState state, MGPumEntity source)
        {
            MGPumDamageUnitEvent gEvent = (MGPumDamageUnitEvent)eventToReplace.deepCopy(state);
            gEvent.damagedUnit = (MGPumUnit)source;
            return gEvent;
        }


        internal override MGPumEffect deepCopy(MGPumGameState state)
        {
            return new MGPumRedirectDamageREffect();
        }

    }
}
