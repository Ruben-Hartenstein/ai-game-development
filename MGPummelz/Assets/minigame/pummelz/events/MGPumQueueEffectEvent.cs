using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumQueueEffectEvent : MGPumGameEvent
    {
        public MGPumOneTimeEffect effect { get; private set; }
        public MGPumEntity source { get; private set; }
        public MGPumGameEvent trigger { get; private set; }

        public MGPumQueueEffectEvent(MGPumOneTimeEffect effect, MGPumGameEvent trigger, MGPumEntity source) : base()
        {
            this.effect = effect;
            this.trigger = trigger;
            this.source = source;

            if (trigger == null)
            {
                Debug.LogError("Trigger is null");
            }
            if (source == null)
            {
                Debug.LogError("Source is null");
            }
        }

        

        public override void apply(MGPumGameState state)
        {
            state.queueEffect(new MGPumEffectQueueItem(effect, trigger, source));
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            if(trigger == null)
            {
                Debug.LogError("Trigger is null");
            }
            if (source == null)
            {
                Debug.LogError("Source is null");
            }

            MGPumGameEvent t = state.lookupEvent(trigger);
            MGPumEntity s = state.lookupEntity(source);

            MGPumOneTimeEffect e = null;

            MGPumEffect ceffect = state.lookupOrCreateEffect(effect, state, s);

            if(ceffect is MGPumOneTimeEffect)
            {
                e = (MGPumOneTimeEffect)ceffect;
            }
            else
            {
                e = (MGPumOneTimeEffect)effect.deepCopy(state);
            }

            MGPumGameEvent result = new MGPumQueueEffectEvent(e, t, s);
            this.copyToGameEvent(result);

            return result;

        }
    }
}
