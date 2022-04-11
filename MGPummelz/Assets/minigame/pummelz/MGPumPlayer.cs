using System;
using System.Collections;
using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumPlayer : MGPumEntity
    {

        public bool winning { get; set; }
        public bool losing { get; set; }

        public int playerID { get; set; }

        public MGPumDestroyedZone destroyedZone { get; set; }

        public MGPumPlayer(int playerID, MGPumPlayerConfig playerConfig)
        {
            this.resetWinningLosing();

            this.playerID = playerID;
            this.id = playerID;
            this.ownerID = playerID;

            this.destroyedZone = new MGPumDestroyedZone(playerID);


            if(playerConfig != null)
            {
            }

        }

        internal MGPumPlayer deepCopy(MGPumGameState state)
        {
            
            //TODO: copy playerConfig
            MGPumPlayer newPlayer = new MGPumPlayer(this.playerID, null);
            this.copyToGameEntity(newPlayer);

            newPlayer.winning = winning;
            newPlayer.losing = losing;


            if (this.destroyedZone != null)
            {
                newPlayer.destroyedZone = destroyedZone.deepCopy(state);
            }

            return newPlayer;
        }


        public void resetWinningLosing()
        {
            this.winning = false;
            this.losing = false;
        }

        public override MGPumZoneType getZone()
        {
            return MGPumZoneType.Any;
        }

    }
}
