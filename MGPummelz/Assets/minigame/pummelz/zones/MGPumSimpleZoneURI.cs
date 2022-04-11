using System;
using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumSimpleZoneURI : MGPumZoneURI
    {
        public MGPumZoneType getZoneType()
        {
            return zoneType;
        }

        public int getPlayerID()
        {
            return playerID;
        }

        public MGPumZoneType zoneType {get; set;}
        public int playerID { get; set; }
        public MGPumUnit unit { get; set; }



        public MGPumSimpleZoneURI(int playerID, MGPumZoneType zoneType)
        {
            if(playerID == 2)
            {
                Debug.LogError("Player ID is 2");
            }
            this.playerID = playerID;
            this.zoneType = zoneType;
            this.unit = null;
        }


        public MGPumSimpleZoneURI(int playerID, MGPumZoneType zoneType, MGPumUnit unit)
        {
            if (playerID == 2)
            {
                Debug.LogError("Player ID is 2 " + unit);
            }
            this.playerID = playerID;
            this.zoneType = zoneType;
            this.unit = unit;
        }

        public MGPumZoneURI deepCopy(MGPumGameState state)
        {
            MGPumUnit c = state.lookupOrCreate(unit);

            return new MGPumSimpleZoneURI(this.playerID, this.zoneType, c);
        }

        public MGPumUnit getUnit(MGPumGameState state)
        {
            return this.unit;
        }

        public void setUnit(MGPumUnit unit)
        {
            this.unit = unit;
        }

        public override string ToString()
        {
            String result = zoneType.ToString() + " " + playerID;
            if (unit != null)
            {
                result += " " + unit.ToString();
            }
            return result;
        }

    }
}
