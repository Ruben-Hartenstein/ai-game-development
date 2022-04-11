using UnityEngine;
using UnityEngine.Assertions;

namespace mg.pummelz
{
    [System.Serializable]
    class MGPumEffectSEvent : MGPumSuperEvent
    {
        public MGPumEntity source { get; set; }
        public MGPumGameEvent triggeringEvent { get; set; }
        public MGPumOneTimeEffect effect { get; set; }
        public MGPumSelection selection { get; set; }


        public MGPumEffectSEvent(MGPumOneTimeEffect effect, MGPumSelection selection, MGPumEntity source, MGPumGameEvent triggeringEvent) : base()
        {
            Assert.IsNotNull(effect);

            this.effect = effect;
            this.selection = selection;
            this.source = source;
            this.triggeringEvent = triggeringEvent;
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumEntity en = state.lookupEntity(source);

            
            MGPumGameEvent ev = null;
            //triggering event may be null if effect was not caused by a trigger
            if (this.triggeringEvent != null)
            {
                ev = state.lookupEvent(triggeringEvent);
            }

            MGPumEffect lookedUpEffect = state.lookupEffect(effect, en);

            MGPumOneTimeEffect ef = null;
            if (lookedUpEffect is MGPumOneTimeEffect)
            {
                ef = (MGPumOneTimeEffect)lookedUpEffect;
            }
            else
            {
                ef = (MGPumOneTimeEffect)effect.deepCopy(state);
            }

            //effect may not exist yet, e.g. a created "deadly" ability effect
            if(ef == null)
            {
                ef = (MGPumOneTimeEffect)effect.deepCopy(state);
            }



            MGPumSelection s = selection.deepCopy(state);

            MGPumGameEvent result = new MGPumEffectSEvent(ef, s, en, ev);
            this.copyToGameEvent(result);
            return result;
        }
    }
}
