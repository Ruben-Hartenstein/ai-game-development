using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumEffectQueueItem
    {
        public MGPumOneTimeEffect effect { get; private set; }
        public MGPumEntity source { get; private set; }
        public MGPumGameEvent trigger { get; private set; }

        public MGPumEffectQueueItem(MGPumOneTimeEffect effect, MGPumGameEvent trigger, MGPumEntity source) : base()
        {
            this.effect = effect;
            this.trigger = trigger;
            this.source = source;
        }

        internal MGPumEffectQueueItem deepCopy(MGPumGameState state)
        {
            if(effect == null)
            {
                Debug.LogError("Null Effect is queued");
            }
            MGPumOneTimeEffect copiedEffect = (MGPumOneTimeEffect)effect.deepCopy(state);
            MGPumEntity copiedSource = state.lookupEntity(source);
            MGPumGameEvent copiedTrigger = state.log.getEventByNumber(trigger.number);

            return new MGPumEffectQueueItem(copiedEffect, copiedTrigger, copiedSource);
        }
    }
}
