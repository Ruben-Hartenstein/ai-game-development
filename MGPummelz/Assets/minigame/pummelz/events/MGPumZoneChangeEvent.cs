using RelegatiaCCG.rccg.engine.exceptions;
using System;
using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumZoneChangeEvent : MGPumGameEvent, MGPumUnitReferencable
    {
        protected MGPumUnit _unit;
        public virtual MGPumUnit unit { get {return _unit; } set {_unit = value; } }
        public MGPumZoneURI origin { get;  set; }
        public MGPumZoneURI destination { get; set; }

    
        public MGPumZoneChangeEvent(MGPumZoneURI origin, MGPumZoneURI destination) : base()
        {
            this.unit = null;
            this.origin = origin;
            this.destination = destination;

        }

        public MGPumZoneChangeEvent(MGPumUnit unit, MGPumZoneURI origin, MGPumZoneURI destination) : base()
        {
            this.unit = unit;
            this.origin = origin;
            this.destination = destination;

        }

        public override void apply(MGPumGameState state)
        {
            state.lastChangeTurnNumber = state.turnNumber;
            this.changeZone(state);
        }

        protected void changeZone(MGPumGameState state)
        {
            //Debug.LogError("Changing zone " + this.unit + " from " + origin + " to " + destination);

            //if (this.origin.getZoneType() == MGPumZoneType.None && this.destination.getZoneType() == MGPumZoneType.Battlegrounds)
            //{
            //    Debug.LogError("Moving from None to Battlegrounds player:" + this.destination.getPlayerID());
            //    Debug.LogError("Moving from None to Battlegrounds unit before:" + this.unit);
            //    Debug.LogError("Moving from None to Battlegrounds count before:" + state.getPlayer(this.destination.getPlayerID()).hand.count());
            //}

            MGPumUnit unit = removeFromZone(this.origin, this.unit, state);
            //set unit if it was not known beforehand
            if (this.unit == null)
            {
                this.unit = unit;
            }
            //if ((!(unit is UnknownUnit)) && (unit == null || unit != this.unit))
            //{
            //    throw new GameException("Zone change failed! " + this.unit + " was not in " + this.origin);
            //}



            this.unit.resetToPrinted();
                        
            addToZone(this.destination, this.unit, state);

        }

        private void addToZone(MGPumZoneURI uri, MGPumUnit unit, MGPumGameState state)
        {

            if(unit == null)
            {
                throw new GameException("Unit is null");
            }
            unit.zone = uri.getZoneType();

            if (uri is MGPumField)
            {
                MGPumField f = (MGPumField)uri;
                //Debug.LogError("Putting " + unit + " on " + f);
                if (f.unit != null)
                {
                    throw new GameException("Cannot add. Unit " + f.unit + " in " + f + " already");
                }

                MGPumUnit pUnit = (MGPumUnit)unit;
                f.unit = pUnit;

            }
            else if (uri is MGPumSimpleZoneURI)
            {
                MGPumSimpleZoneURI suri = (MGPumSimpleZoneURI)uri;
                MGPumPlayer p = state.getPlayer(suri.playerID);

                if (suri.getZoneType() == MGPumZoneType.Destroyed)
                {
                    if (p.destroyedZone.units.IndexOf(unit) > 0)
                    {
                        p.destroyedZone.removeUnit(unit);
                    }

                }
                else if (suri.getZoneType() == MGPumZoneType.Battlegrounds)
                {
                    throw new GameException("TODO:Not yet implemented.");
                }
            }
            else if (uri is MGPumNoneZoneURI)
            {
                //nothing to be done
            }
            else
            {
                throw new GameException("TODO:Not yet implemented.");
            }

        }


        private MGPumUnit removeFromZone(MGPumZoneURI uri, MGPumUnit unit, MGPumGameState state)
        {
            MGPumUnit result = null;
            if (uri is MGPumField)
            {
                MGPumField f = (MGPumField)uri;
                if (f.unit == null)
                {
                    throw new GameException("Cannot remove" + unit.unitID + ". No unit in " + f );
                }
                else
                {
                    result = f.unit;
                    f.unit.field = null;
                    f.unit = null;
                }
            }
           
            else if (uri is MGPumSimpleZoneURI)
            {
                MGPumSimpleZoneURI suri = (MGPumSimpleZoneURI)uri;
                MGPumPlayer p = state.getPlayer(suri.playerID);

                if (suri.getZoneType() == MGPumZoneType.Destroyed)
                {
                    if (suri.unit == null)
                    {
                        throw new GameException("Cannot remove from destroyed. No unit specified in URI.");
                    }

                    if(p.destroyedZone.units.Contains(unit))//for the reveal in transit case unit in origin might be unknown, but it is revealed in transit
                    {
                        result = unit;
                        p.destroyedZone.removeUnit(unit);
                    }

                }
                else
                {
                    throw new GameException("TODO:Not yet implemented.");
                }
            }
            else if (uri is MGPumNoneZoneURI)
            {
                //nothing to be done
            }
            else
            {
                throw new GameException("TODO:Not yet implemented:" + uri.GetType());
            }
            return result;
        }

        protected void copyToZoneChangeEvent(MGPumZoneChangeEvent result, MGPumGameState state)
        {
            this.copyToGameEvent(result);
        }

        public override MGPumGameEvent deepCopy(MGPumGameState state)
        {
            
            MGPumUnit c = state.lookupOrCreate(this.unit);
            MGPumZoneURI o = origin.deepCopy(state);
            MGPumZoneURI d = destination.deepCopy(state);

            MGPumZoneChangeEvent result = new MGPumZoneChangeEvent(c, o, d);
            copyToZoneChangeEvent(result, state);

            return result;
        }

        public override string ToString()
        {
            String result = "MGPumZoneChangeEvent " + this.unit + "  " + this.origin + "->" + this.destination;
           
            return result;
        }

        public MGPumUnit getReferencableUnit()
        {
            return unit;
        }
       
    }

  
}
