using RelegatiaCCG.rccg.engine;
using System.Collections.Generic;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumGameConfig
    {
        public int lines = 8;
        public int columns = 8;

        public List<MGPumSetup> setups;

        public MGPumEncounter encounter;

        private MGPumPlayerConfig[] playerConfigs;

        public MGPumPlayerConfig getPlayerConfig(int playerID) {
            return playerConfigs[playerID];
        }
        public MGPumPlayerConfig getOpponentConfig(int playerID)
        {
            if(playerID == 0)
            {
                return playerConfigs[1];
            }
            else
            {
                return playerConfigs[0];
            }
            
        }


        public MGPumGameConfig(MGPumPlayerConfig pc0, MGPumPlayerConfig pc1)
        {
            playerConfigs = new MGPumPlayerConfig[2];
            playerConfigs[0] = pc0;
            playerConfigs[1] = pc1;

            this.setups = new List<MGPumSetup>();
        }

        public MGPumGameConfig(bool dummy)
        {
            this.setups = new List<MGPumSetup>();

            playerConfigs = new MGPumPlayerConfig[2];


            MGPumPlayerConfig p0conf = new MGPumPlayerConfig();



            playerConfigs[0] = p0conf;

            MGPumPlayerConfig p1conf = new MGPumPlayerConfig();

         

            this.encounter = new MGPumEncounter("test", 13);
         
            playerConfigs[1] = p1conf;

        }

        public void addSetup(MGPumSetup setup)
        {
            this.setups.Add(setup);
        }

        public MGPumGameConfig deepCopy()
        {
            //keep stuff that does not change
            MGPumGameConfig copy = new MGPumGameConfig(playerConfigs[0].deepCopy(), playerConfigs[1].deepCopy());
            copy.setups = this.setups;
            if(this.encounter != null)
            {
                copy.encounter = encounter.deepCopy();
            }

            return copy;
        }

       
    }
}
