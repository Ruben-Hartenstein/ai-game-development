using System;
using System.Collections.Generic;

namespace mg.pummelz
{

    [System.Serializable]
    public class MGPumEncounter
    {
        public enum Terrain
        {
            Grass,
            Flowers,
            Dirt,
            Desert,
            Ice,
            Obsidian,
            Marble,
            Chess
        }


        public MGPumEncounter(string id, int level)
        {
            this.id = id;
            this.level = level;

            this.setups = new List<MGPumSetup>();
        }

        public Terrain terrain;

        public string id;

        public int level;

        public string[] battlegrounds;

        public Dictionary<char, String> unitDictionary;



        public const int AI_ATTACK_ONLY = 0;
        public const int AI_GREEDY = 1;
        public const int AI_SKIP_TURN = 2;
        public const int AI_ACTIVATE_ABILITY = 3;

        public int aiType = 0;

        public List<MGPumSetup> setups;


        public MGPumPlayerConfig getPlayerConfig()
        {
            MGPumPlayerConfig cfg = new MGPumPlayerConfig();
            return cfg;
        }

        public void addSetup(MGPumSetup setup)
        {
            this.setups.Add(setup);
        }

        internal MGPumEncounter deepCopy()
        {
            //keep what will not be changed
            MGPumEncounter copy = new MGPumEncounter(this.id, this.level);
            copy.terrain = this.terrain;
            return copy;
        }

        
    }

        

}
