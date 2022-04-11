using RelegatiaCCG.rccg.engine.state;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace mg.pummelz
{
    public class MGPumSetHandler
    {
        /// <summary>
        /// Has to be three letters
        /// </summary>
        public virtual string id { get { return ""; } }

        private Dictionary<string, MGPumSet> setsByID;


        private MGPumSetHandler()
        {
            setsByID = new Dictionary<string, MGPumSet>();
            
            {
                MGPumSet s = new MGPumDebugSet();
                setsByID[s.id] = s;
            }

            {
                MGPumSet s = new MGPumUnitTestSet();
                setsByID[s.id] = s;
            }

            {
                MGPumSet s = new MGPumBaseSet();
                setsByID[s.id] = s;
            }

            
            //debug functions - will crash when called from title screen
            //outputSetStatistics();
            //generateI18nFiles();
            //generateMetaFiles();
            //generateConstants();
            //generateSFXFiles();
        }

        private static MGPumSetHandler instance = null;

        public static MGPumSetHandler getInstance()
        {
            if (instance == null)
            {
                instance = new MGPumSetHandler();
                //this is done afterwards, as search texts may reference the set handler (i.e. for create unit effects)
                foreach(MGPumSet set in instance.setsByID.Values)
                {
                    set.createSearchTexts();
                }
            }

            return instance;
        }


        public MGPumUnit getUnitByID(String unitID)
        {
            string setID = unitID.Substring(0, 3);
            if(!setsByID.ContainsKey(setID))
            {
                Debug.LogError("Cannot find set " + setID);
            }
            return setsByID[unitID.Substring(0, 3)].getUnitByID(unitID);
        }

        public string getSearchTextByID(String unitID)
        {
            string setID = unitID.Substring(0, 3);
            if (!setsByID.ContainsKey(setID))
            {
                Debug.LogError("Cannot find set " + setID);
            }
            return setsByID[unitID.Substring(0, 3)].getSearchText(unitID);
        }

        public MGPumUnit getNewUnit(string unitID, bool isFoil)
        {
            MGPumUnit template = this.getUnitByID(unitID);
            MGPumUnit u = template.deepCopy();
            return u;
        }

        public MGPumUnit getNewUnit(int id, string unitID, int ownerID, IDManager idm)
        {
            MGPumUnit template = this.getUnitByID(unitID);
            MGPumUnit u = template.deepCopy();

            if(u.abilityPrinted != null)
            {
                u.abilityPrinted.id = idm.getNextID();
                u.abilityPrinted.setEffectIDs(idm);
            }

            u.ownerID = ownerID;
            u.id = id;
            u.resetToPrinted();
            return u;
        }

       

        public IEnumerable<MGPumSet> getSets()
        {
            return setsByID.Values;
        }

        public MGPumSet getSetByID(String id)
        {
            return setsByID[id];
        }


    }
}
