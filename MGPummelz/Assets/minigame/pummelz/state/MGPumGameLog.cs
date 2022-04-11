using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumGameLog : MGPumGameEventHandler, IEnumerable<MGPumGameEvent>
    {
        private int nextNumber;

        protected List<MGPumGameEvent> entries;
        internal LinkedList<MGPumGameEvent> entriesReverse;

        protected Dictionary<int, MGPumGameEvent> eventsByNumber;

        public MGPumGameLog()
        {
            nextNumber = 0;
            entries = new List<MGPumGameEvent>();
            entriesReverse = new LinkedList<MGPumGameEvent>();
            eventsByNumber = new Dictionary<int, MGPumGameEvent>();
        }

        public bool isEmpty()
        {
            return !entries.Any();
        }
            

        public int getCurrentPosition()
        {
            return nextNumber - 1;
        }

        public List<MGPumGameEvent> getNewEvents(int lastPosition)
        {
            int firstNewPos = lastPosition + 1;
            int count = entries.Count - firstNewPos;
            return entries.GetRange(firstNewPos, count);
        }

        public int getLastReferencedPlayer()
        {
            MGPumPlayerReferencable e = (MGPumPlayerReferencable)getLastEvent<MGPumPlayerReferencable>();
            return e.getReferencablePlayerID();
        }

        public MGPumGameEvent getLastEvent<T>()
        {
            MGPumGameEvent result = null;

            foreach (MGPumGameEvent e in entriesReverse)
            {
                if (e is T)
                {
                    result = e;
                    break;
                }
            }

            return result;
        }

        public LinkedList<MGPumGameEvent> getLastEventsUntil(MGPumGameEvent lastEvent)
        {
            LinkedList<MGPumGameEvent> events = new LinkedList<MGPumGameEvent>();
            foreach (MGPumGameEvent e in entriesReverse)
            {
                events.AddFirst(e);

                if (e == lastEvent)
                {
                    break;
                }
            }
            return events;
        }

        public LinkedList<MGPumGameEvent> getLastEventsUntil<T>()
        {
            LinkedList<MGPumGameEvent> events = new LinkedList<MGPumGameEvent>();
            foreach (MGPumGameEvent e in entriesReverse)
            {
                events.AddFirst(e);

                if (e is T)
                {
                    break;
                }
            }
            return events;
        }

        public LinkedList<MGPumGameEvent> getEventsOfThisTurn()
        {
            LinkedList<MGPumGameEvent> events = new LinkedList<MGPumGameEvent>();
            foreach (MGPumGameEvent e in entriesReverse)
            {
                if (e is MGPumNextTurnSEvent)
                {
                    break;
                }

                events.AddFirst(e);
            }
            return events;
        }

        public LinkedList<MGPumGameEvent> getEventsOfThisEffect()
        {
            LinkedList<MGPumGameEvent> events = new LinkedList<MGPumGameEvent>();
            foreach (MGPumGameEvent e in entriesReverse)
            {
                events.AddFirst(e);

                if (e is MGPumEffectSEvent)
                {
                    break;
                }
            }
            return events;
        }

        public MGPumGameEvent getTriggerOfThisEffect()
        {
            MGPumGameEvent tevent = null;
            foreach (MGPumGameEvent e in entriesReverse)
            {
                if (e is MGPumDeQueueEffectEvent)
                {
                    tevent = ((MGPumDeQueueEffectEvent)e).item.trigger;
                    break;
                }
            }
            return tevent;
        }


        public LinkedList<MGPumGameEvent> getEventsOfYourLastTurn(int playerID)
        {
            LinkedList<MGPumGameEvent> events = new LinkedList<MGPumGameEvent>();

            bool inLastTurn = false;
            foreach (MGPumGameEvent e in entriesReverse)
            {
                if (e is MGPumEndTurnSEvent && ((MGPumEndTurnSEvent)e).playerID == playerID)
                {
                    inLastTurn = true;
                }
                if(inLastTurn)
                {
                    events.AddFirst(e);
                    if (e is MGPumNextTurnSEvent)
                    {
                        break;
                    }
                }
            }
            return events;
        }

        public void applyEvent(MGPumGameEvent e)
        {
            //TODO: this should equal the position in the log but is not guaranteed (add assert?)
            e.number = nextNumber++;
            this.addEvent(e);
            //Debug.Log("Logging event " + e.number + ": " + e);



        }

        protected void addEvent(MGPumGameEvent e)
        {
            //Debug.Log("Adding " + e.number);
            entries.Add(e);
            entriesReverse.AddFirst(e);
            if(eventsByNumber.ContainsKey(e.number))
            {
                Debug.LogError("GameLog already has entry: " + e.number + " " + e);
            }
            eventsByNumber.Add(e.number, e);
        }

        internal MGPumGameEvent getEventByNumber(int number)
        {
            if(number < 0 || number >= entries.Count)
            {
                Debug.LogError("Cannot get event " + number + " maximum is:" + (entries.Count - 1));
            }
            MGPumGameEvent result = eventsByNumber[number];
            Assert.AreEqual(number, result.number);
            return result;
        }

        internal MGPumGameLog deepCopyToGameLog(MGPumGameLog copiedLog, MGPumGameState gameState)
        {
            copiedLog.nextNumber = this.nextNumber;
            foreach(MGPumGameEvent e in entries)
            {
                if(e == null)
                {
                    Debug.LogError("GameLog has empty entry");
                }
                MGPumGameEvent eNew = e.deepCopy(gameState);
                copiedLog.addEvent(eNew);
            }
            return copiedLog;
        }

        public IEnumerator<MGPumGameEvent> GetEnumerator()
        {
            return ((IEnumerable<MGPumGameEvent>)entries).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<MGPumGameEvent>)entries).GetEnumerator();
        }

        internal MGPumField getLastKnownField(MGPumUnit unit)
        {
            foreach (MGPumGameEvent e in entriesReverse)
            {
                if (e is MGPumZoneChangeEvent)
                {
                    MGPumZoneChangeEvent zce = (MGPumZoneChangeEvent)e;
                    if(zce.unit.id == unit.id)
                    {
                        //destination first, then origin
                        if(zce.destination is MGPumField)
                        {
                            return (MGPumField)zce.destination;
                        }
                        if (zce.origin is MGPumField)
                        {
                            return (MGPumField)zce.origin;
                        }
                    }
                }
            }
            return null;
        }

        internal bool hasEnteredBattlegroundsLastTurn(MGPumUnit unit, int playerID)
        {
            foreach (MGPumGameEvent e in entriesReverse)
            {
                if (e is MGPumZoneChangeEvent)
                {
                    MGPumZoneChangeEvent zce = (MGPumZoneChangeEvent)e;
                    if (zce.unit.id == unit.id)
                    {
                        if (zce.destination is MGPumField)
                        {
                            return true;
                        }
                    }
                    
                }
                if (e is MGPumEndTurnSEvent && ((MGPumEndTurnSEvent)e).playerID == playerID)
                {
                    break;
                }
            }
            

            return false;
        }

    }
}
