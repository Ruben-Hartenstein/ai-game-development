using System;
using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumField : MGPumEntity, MGPumZoneURI
    {
        readonly internal Vector2Int coords;

        internal int x { get { return coords.x; } }
        internal int y { get { return coords.y; } }


        private MGPumUnit _unit { get; set; }
        public MGPumUnit unit
        {
            get { return _unit; }
            set {
                //Debug.LogError("Setting " + value + " on " + this);
                this._unit = value;
                if (this._unit != null) { this._unit.field = this; }
            }
        }

        public MGPumField(int x, int y)
        {
            this.ownerID = NEUTRAL_OWNER;
            this.coords = new Vector2Int(x, y);
        }

        public bool isEmpty()
        {
            return unit == null;
        }

        public bool neighboring(MGPumField other)
        {
            return (Math.Abs(this.x - other.x) <= 1 && Math.Abs(this.y - other.y) <= 1);
        }

        public override string ToString()
        {
            String result = "( " + x + " / " + y + ")";
            if (unit != null)
            {
                result += " " + unit.ToString();
            }
            return result;
        }

        public MGPumZoneType getZoneType()
        {
            return MGPumZoneType.Battlegrounds;
        }

        public override MGPumZoneType getZone()
        {
            return MGPumZoneType.Battlegrounds;
        }

        public MGPumZoneURI deepCopy(MGPumGameState state)
        {
            return state.getField(this);
        }

        public int getPlayerID()
        {
            return ownerID;
        }

        public MGPumUnit getUnit(MGPumGameState state)
        {
            return this.unit;
        }

        public void setUnit(MGPumUnit unit)
        {
            this.unit = unit;
        }

    }
}
