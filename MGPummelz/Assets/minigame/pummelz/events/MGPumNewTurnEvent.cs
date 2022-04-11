using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumNewTurnEvent : MGPumGameEvent
    {
        public int playerID { get; set; }

        public MGPumNewTurnEvent(int playerID) : base()
        {
            this.playerID = playerID;
        }

        public override void apply(MGPumGameState state)
        {
            //only do an extra turn if there is any AND we had a regular turn
            if(state.extraTurnQueue.Count > 0 && state.regularTurnPlayer == this.playerID)
            {
                int testID = state.extraTurnQueue.Dequeue();
                if(testID != this.playerID)
                {
                    Debug.LogError("Extra turn, but new turn player id " + this.playerID + " does not match queued player ID" + testID);
                }
            }
            else //no extra turn was consumed so regular turn player switches
            {
                state.regularTurnPlayer = this.playerID;
            }

            state.turnNumber++;
            state.activePlayer = this.playerID;
            state.playerTurnNumber[this.playerID] = state.playerTurnNumber[this.playerID] + 1;

            foreach (MGPumUnit c in state.getAllUnitsInZone(MGPumZoneType.Battlegrounds))
            {
                c.attacksThisTurn = 0;
                c.movesThisTurn = 0;
            }

            //draw game if no change happened for 50 turns
            if(state.turnNumber > state.lastChangeTurnNumber + 50)
            {

                foreach (MGPumPlayer p in state.players)
                {
                    p.losing = true;
                }
            }

        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            MGPumGameEvent result = new MGPumNewTurnEvent(this.playerID);
            this.copyToGameEvent(result);
            return result;
        }
    }
}
