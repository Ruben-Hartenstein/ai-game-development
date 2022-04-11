using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace mg.pummelz
{
    public abstract class MGPumSet
    {
        public virtual string id { get { return ""; } }

        public virtual bool isDebug { get { return false; } }

        protected Dictionary<string, MGPumUnit> unitsByName;
        protected Dictionary<string, MGPumUnit> unitsByID;
        protected Dictionary<string, string> searchTexts;
        protected List<MGPumUnit> units;

        public MGPumSet()
        {
            unitsByName = new Dictionary<string, MGPumUnit>();
            unitsByID = new Dictionary<string, MGPumUnit>();
            searchTexts = new Dictionary<string, string>();
            units = new List<MGPumUnit>();
        }


        protected void addUnit(MGPumUnit unit)
        {
            unit.resetToPrinted();

            if (unitsByID.ContainsKey(unit.unitID))
            {
                Debug.LogError("Unit " + unit.unitID + " already in set.");
            }
            unitsByID[unit.unitID] = unit;
            if (unitsByName.ContainsKey(unit.name))
            {
                Debug.LogError("Unit " + unit.name + " already in set.");
            }
            unitsByName[unit.name] = unit;

            units.Add(unit);

            unit.setID = this.id;
        }


      

        public void createSearchTexts()
        {
            foreach(MGPumUnit unit in this.units)
            {
                searchTexts.Add(unit.unitID, unit.toI18nedSearchString().ToLower());
            }

        }

        public MGPumUnit getUnitByID(String id)
        {
            if(!unitsByID.ContainsKey(id))
            {
                Debug.LogError("Can't get unknown unit:" + id);
            }
            return unitsByID[id];
        }

        public string getSearchText(String id)
        {
            if (!unitsByID.ContainsKey(id))
            {
                Debug.LogError("Can't get unknown unit:" + id);
            }
            return searchTexts[id];
        }

        public MGPumUnit getUnitByName(String name)
        {
            return unitsByName[name];
        }

        public IEnumerable<MGPumUnit> getUnits()
        {
            return unitsByID.Values;
        }

        public List<MGPumUnit> getUnits(MGPumFilter filter)
        {
            //casting to game entity then result back to unit and list
            List<MGPumEntity> entities = filter.apply(units.Cast<MGPumEntity>(), null);
            return entities.ConvertAll(instance => (MGPumUnit)instance);
        }

    }
}
