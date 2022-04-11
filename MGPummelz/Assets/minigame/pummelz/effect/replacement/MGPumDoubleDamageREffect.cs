using RelegatiaCCG.rccg.i18n;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumDoubleDamageREffect : MGPumReplacementEffect
    {
        public override MGPumGameEvent replace(MGPumGameEvent eventToReplace, MGPumGameState state, MGPumEntity source)
        {
            MGPumDamageUnitEvent gEvent = (MGPumDamageUnitEvent)eventToReplace.deepCopy(state);
            if(gEvent.damage > 0)
            {
                gEvent.damage *= 2;
            }
            return gEvent;
        }

        internal override MGPumEffect deepCopy(MGPumGameState state)
        {
            return new MGPumDoubleDamageREffect();
        }

    }
}
