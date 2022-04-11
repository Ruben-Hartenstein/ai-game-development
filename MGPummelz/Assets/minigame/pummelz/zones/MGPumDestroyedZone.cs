using RelegatiaCCG.rccg.engine.exceptions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace mg.pummelz
{

    [System.Serializable]
    public class MGPumDestroyedZone : MGPumZone
    {
        public int playerID;

        public List<MGPumUnit> units;

        public MGPumDestroyedZone(int playerID) : base()
        {
            this.playerID = playerID;
            this.units = new List<MGPumUnit>();
        }

        public MGPumZoneType getZoneType()
        {
            return MGPumZoneType.Destroyed;
        }

        public bool removeUnit(MGPumUnit c)
        {


            bool success = false;
           
            {
                success = units.Remove(c);
            }
            //Debug.LogError("Remove unit " + c + " fom deck " + getOwnerID() + "  " + success);
            return success;
            
        }

        public bool addUnit(MGPumUnit c)
        {

            bool success = false;
            {
                success = units.Remove(c);
            }
            //Debug.LogError("Remove unit " + c + " fom deck " + getOwnerID() + "  " + success);
            return success;

        }


        internal MGPumDestroyedZone deepCopy(MGPumGameState state)
        {
            MGPumDestroyedZone copy = new MGPumDestroyedZone(this.playerID);
           
            foreach (MGPumUnit u in units)
            {
                MGPumUnit newU = null;

                newU = state.lookupUnit(u.id);
    

                if (newU == null)
                {
                    throw new Exception("Cannot find unit " + u.id + " in new state.");
                }
                copy.units.Add(newU);
            }
            return copy;
        }

        public int getOwnerID()
        {
            return playerID;
        }

        public int count()
        {
            return units.Count;
        }
    }

}
