using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace mg.pummelz
{



    [System.Serializable]
    public class MGPumCampaign : IEnumerable<MGPumEncounter>
    {
        private static MGPumCampaign instance;

        public List<MGPumEncounter> encounters;

        public Dictionary<string, MGPumEncounter> extraEncounters;

        protected string campaignID;

        private Dictionary<char, string> unitDictionary;

        private void initializeDictionaries()
        {
            //
            // Units
            //
            unitDictionary = new Dictionary<char, string>();
            unitDictionary.Add('p', MGPumBaseSet.BELLIE);
            unitDictionary.Add('h', MGPumBaseSet.HOPPEL);
            unitDictionary.Add('s', MGPumBaseSet.SNEIP);
            unitDictionary.Add('w', MGPumBaseSet.WOLLI);
            unitDictionary.Add('c', MGPumBaseSet.CHILLY);
            unitDictionary.Add('k', MGPumBaseSet.KILLY);
            unitDictionary.Add('b', MGPumBaseSet.BUMMZ);
            unitDictionary.Add('z', MGPumBaseSet.CZAREMIR);
            unitDictionary.Add('u', MGPumBaseSet.LINK);
            unitDictionary.Add('m', MGPumBaseSet.MAMPFRED);
            unitDictionary.Add('f', MGPumBaseSet.FROMMLI);
            unitDictionary.Add('a', MGPumBaseSet.ANGLI);
            unitDictionary.Add('y', MGPumBaseSet.BUFFY);
            unitDictionary.Add('l', MGPumBaseSet.HALEY);

        }
        
        public static MGPumCampaign getInstance()
        {
            if (instance == null)
            {
                instance = new MGPumCampaign("main");
            }
            return instance;
        }

        protected void addEncounter(MGPumEncounter encounter)
        {
            addEncounter(encounter, true);
        }

        protected void addExtraEncounter(MGPumEncounter encounter)
        {
            addEncounter(encounter, false);
        }

        public MGPumEncounter getEncounter(string key)
        {
            if (!extraEncounters.ContainsKey(key))
            {
                Debug.LogError("Unknown encounter " + key);
            }
            return extraEncounters[key];
        }

        protected void addEncounter(MGPumEncounter encounter, bool regularEncounter)
        {
            encounter.unitDictionary = this.unitDictionary;

            if (regularEncounter)
            {
                this.encounters.Add(encounter);
            }

            this.extraEncounters.Add(encounter.id, encounter);
        }

        public IEnumerator<MGPumEncounter> GetEnumerator()
        {
            return ((IEnumerable<MGPumEncounter>)encounters).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<MGPumEncounter>)encounters).GetEnumerator();
        }

        public const string TEST = "PUM_TEST";

        //three without special abilities
        public const string FIRST = "PUM_FIRST";
        public const string FOUR_CORNERS = "PUM_FOUR_CORNERS";
        public const string BIG_BRAWL = "PUM_BIG_BRAWL";

        //basic special abilities
        public const string CHILLORDER = "PUM_CHILLORDER";
        public const string HERDING = "PUM_HERDING";
        public const string KINGSLAYER = "PUM_KINGSLAYER";

        //advanced/extreme encounters
        public const string BURNING_FUSE = "PUM_BURNING_FUSE";
        //public const string BUMMPITZ = "PUM_BUMMPITZ";
        public const string A_LINK_TO_THE_BLAST = "PUM_A_LINK_TO_THE_BLAST";
        public const string CAMPERS_PARADISE = "PUM_CAMPERS_PARADISE";
        
        //everything at once
        public const string CHESS = "PUM_CHESS";
        
        

        protected MGPumCampaign()
        {

        }


        private MGPumCampaign(string campaignID)
        {
            this.campaignID = campaignID;

            encounters = new List<MGPumEncounter>();
            extraEncounters = new Dictionary<string, MGPumEncounter>();

            initializeDictionaries();

            //Student Encounters
            {


                {
                    MGPumEncounter encounter = new MGPumEncounter(FIRST, 1);

                    encounter.battlegrounds = new string[] {
                       //01234567
                        "..WHS...",//0
                        "..PPP...",//1
                        "........",//2
                        "........",//3
                        "........",//4
                        "........",//5
                        "...ppp..",//6
                        "...shw.." //7
                    };

                    encounter.terrain = MGPumEncounter.Terrain.Grass;

                    {//always the same
                        addEncounter(encounter);
                    }
                }


                {
                    MGPumEncounter encounter = new MGPumEncounter(FOUR_CORNERS, 1);

                    encounter.battlegrounds = new string[] {
                       //01234567
                        "swp..PHS",//0
                        "hp....PW",//1
                        "p......P",//2
                        "........",//3
                        "........",//4
                        "P......p",//5
                        "WP....ph",//6
                        "SHP..pws" //7
                    };

                    encounter.terrain = MGPumEncounter.Terrain.Dirt;

                    {//always the same
                        addEncounter(encounter);
                    }
                }

                {
                    MGPumEncounter encounter = new MGPumEncounter(BIG_BRAWL, 1);

                    encounter.battlegrounds = new string[] {
                       //01234567
                        "hshshshs",//0
                        "wpwpwpwp",//1
                        "........",//2
                        "........",//3
                        "........",//4
                        "........",//5
                        "PWPWPWPW",//6
                        "SHSHSHSH" //7
                    };

                    encounter.terrain = MGPumEncounter.Terrain.Desert;

                    {//always the same
                        addEncounter(encounter);
                    }
                }


                {
                    MGPumEncounter encounter = new MGPumEncounter(HERDING, 1);

                    encounter.battlegrounds = new string[] {
                       //01234567
                        "PLPYPLPY",//0
                        "PPPPPPPP",//1
                        "........",//2
                        "........",//3
                        "........",//4
                        "........",//5
                        "pppppppp",//6
                        "yplpyplp" //7
                    };

                    encounter.terrain = MGPumEncounter.Terrain.Flowers;

                    {//always the same
                        addEncounter(encounter);
                    }
                }

                {
                    MGPumEncounter encounter = new MGPumEncounter(CHILLORDER, 1);

                    encounter.battlegrounds = new string[] {
                       //01234567
                        "..CMFS..",//0
                        "..PPPP..",//1
                        "........",//2
                        "........",//3
                        "........",//4
                        "........",//5
                        "..pppp..",//6
                        "..sfmc.." //7
                    };

                    encounter.terrain = MGPumEncounter.Terrain.Ice;

                    {//always the same
                        addEncounter(encounter);
                    }
                }

                

                {
                    MGPumEncounter encounter = new MGPumEncounter(KINGSLAYER, 1);

                    encounter.battlegrounds = new string[] {
                       //01234567
                        "..PHZLP.",//0
                        "...PWP..",//1
                        "....P...",//2
                        "........",//3
                        "........",//4
                        "...p....",//5
                        "..pwp...",//6
                        ".plzhp.." //7
                    };

                    encounter.terrain = MGPumEncounter.Terrain.Marble;

                    {//always the same
                        addEncounter(encounter);
                    }
                }


                {
                    MGPumEncounter encounter = new MGPumEncounter(BURNING_FUSE, 1);

                    encounter.battlegrounds = new string[] {
                        //01234567
                        ".PCCFFP.",//0
                        ".BFFCCB.",//1
                        "..BPPB..",//2
                        "........",//3
                        "........",//4
                        "..bppb..",//5
                        ".bccffb.",//6
                        ".pffccp." //7
                    };

                    encounter.terrain = MGPumEncounter.Terrain.Obsidian;

                    {//always the same
                        addEncounter(encounter);
                    }
                }


              

                {
                    MGPumEncounter encounter = new MGPumEncounter(A_LINK_TO_THE_BLAST, 1);

                    encounter.battlegrounds = new string[] {
                       //01234567
                        "..UUU.BZ",//0
                        "......BB",//1
                        "u.......",//2
                        "u......U",//3
                        "u......U",//4
                        ".......U",//5
                        "bb......",//6
                        "zb.uuu.." //7
                    };

                    encounter.terrain = MGPumEncounter.Terrain.Dirt;

                    {//always the same
                        addEncounter(encounter);
                    }
                }

                {
                    MGPumEncounter encounter = new MGPumEncounter(CAMPERS_PARADISE, 1);

                    encounter.battlegrounds = new string[] {
                       //01234567
                        "....UYSS",//0
                        ".....LYS",//1
                        "......LY",//2
                        ".......L",//3
                        "l.......",//4
                        "yl......",//5
                        "syl.....",//6
                        "ssyu...." //7
                    };

                    encounter.terrain = MGPumEncounter.Terrain.Dirt;

                    {//always the same
                        addEncounter(encounter);
                    }
                }

                {
                    MGPumEncounter encounter = new MGPumEncounter(CHESS, 1);

                    encounter.battlegrounds = new string[] {
                       //01234567
                        "hwfyzlcs",//0
                        "pppppppp",//1
                        "........",//2
                        "........",//3
                        "........",//4
                        "........",//5
                        "PPPPPPPP",//6
                        "SCLZYFWH",//7
                    };

                    encounter.terrain = MGPumEncounter.Terrain.Chess;

                    {//always the same
                        addEncounter(encounter);
                    }
                }

                //test encounters
                {
                    MGPumEncounter encounter = new MGPumEncounter("uiuiuiui", 1);

                    encounter.battlegrounds = new string[] {
                       //01234567
                        "...TT...",//0
                        "t..CC..t",//1
                        "h..cc..H",//2
                        "m......M",//3
                        "p......P",//4
                        "........",//5
                        "........",//6
                        "........" //7
                    };

                    {//always the same
                        addEncounter(encounter);
                    }
                }
                
                {
                    MGPumEncounter encounter = new MGPumEncounter("template", 1);

                    encounter.battlegrounds = new string[] {
                       //01234567
                        "........",//0
                        "........",//1
                        "........",//2
                        "........",//3
                        "........",//4
                        "........",//5
                        "........",//6
                        "........" //7
                    };

                    encounter.terrain = MGPumEncounter.Terrain.Grass;

                    {//always the same
                        addEncounter(encounter);
                    }
                }

                {
                    MGPumEncounter encounter = new MGPumEncounter(TEST, 1);

                    encounter.battlegrounds = new string[] {
                       //01234567
                        "hswckzum",//0
                        "faflb.uu",//1
                        "kBBk...u",//2
                        "bfBf...u",//3
                        "FFFF...U",//4
                        ".......U",//5
                        "FAYLB.UU",//6
                        "HSWCKZUM",//7
                    };

                    encounter.terrain = MGPumEncounter.Terrain.Flowers;

                    {//always the same
                        addEncounter(encounter);
                    }
                }
            }
        }


    }


}




