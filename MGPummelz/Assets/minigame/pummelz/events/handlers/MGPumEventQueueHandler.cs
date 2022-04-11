using System.Collections.Generic;
using System.Linq;

namespace mg.pummelz
{
    public class MGPumEventQueueHandler : MGPumGameEventHandler
    {
        private Queue<MGPumGameEvent> eventQueue;

        public MGPumEventQueueHandler()
        {
            this.eventQueue = new Queue<MGPumGameEvent>();
        }

        public void applyEvent(MGPumGameEvent e)
        {
            //Debug.Log("Queueing event:" + e);
            this.eventQueue.Enqueue(e);
        }

        public MGPumGameEvent getNextEvent()
        {
            if(eventQueue.Any())
            {
                return this.eventQueue.First();
            }
            else
            {
                return null;
            }
            
        }

        public bool queueEmpty()
        {
            return !eventQueue.Any();
        }

        public MGPumGameEvent consumeNextEvent(MGPumGameEventHandler handler)
        {
            MGPumGameEvent e = this.eventQueue.Dequeue();
            if(e != null)
            {
                handler.applyEvent(e);
            }
            return e;

            

        }

    }
}
