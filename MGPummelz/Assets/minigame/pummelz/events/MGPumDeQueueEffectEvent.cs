using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumDeQueueEffectEvent : MGPumGameEvent
    {
        public MGPumEffectQueueItem item;

        public MGPumDeQueueEffectEvent() : base()
        {
        }

        

        public override void apply(MGPumGameState state)
        {
            this.item = state.dequeueNextQueuedEffect();
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {

            MGPumDeQueueEffectEvent result = new MGPumDeQueueEffectEvent();
            this.copyToGameEvent(result);
            if(item != null)
            {
                result.item = this.item.deepCopy(state);
            }

            return result;

        }
    }
}
