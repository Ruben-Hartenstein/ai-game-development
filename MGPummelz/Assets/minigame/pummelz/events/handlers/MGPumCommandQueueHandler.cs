using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumCommandQueueHandler : MGPumGameEventHandler
    {
        private Queue<MGPumGameEvent> commandQueue;

        public MGPumCommandQueueHandler()
        {
            this.commandQueue = new Queue<MGPumGameEvent>();
        }

        public void applyEvent(MGPumGameEvent e)
        {
            this.commandQueue.Enqueue(e);
        }

        public MGPumGameEvent getNextEvent()
        {
            if(commandQueue.Any())
            {
                return this.commandQueue.First();
            }
            else
            {
                return null;
            }
            
        }

        public bool queueEmpty()
        {
            return !commandQueue.Any();
        }

        public MGPumGameEvent consumeNextEvent(MGPumGameEventHandler handler)
        {
            MGPumGameEvent e = this.commandQueue.Dequeue();
            if(e != null)
            {
                handler.applyEvent(e);
            }
            return e;

            

        }

        public void disunitNextEvent()
        {
            this.commandQueue.Dequeue();
        }

    }
}
