using RelegatiaCCG.rccg.i18n;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumRandomSelect : MGPumAutoSelect
    {
        internal MGPumAutoSelect filter;

        private int amount = 1;

        public MGPumRandomSelect(int amount, MGPumAutoSelect filter) : base()
        {
            this.amount = amount;
            this.filter = filter;
        }

        public MGPumSelector deepCopySelector(MGPumGameState state)
        {
            //Debug.LogWarning("RandomSelector is not deepcopied");
            return this;
        }

        public string getI18nSNum()
        {
            if(amount > 1)
            {
                return "SNUM_SOME";
            }
            else
            {
                return "SNUM_ONE";
            }
        }

        public MGPumSelection getSelection()
        {
            return this;
        }

        public List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource)
        {
            return getSelection(state, selectionSource, null);
        }

        public List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource, MGPumGameEvent triggeringEvent)
        {
            return getSelection(state, selectionSource, triggeringEvent, null);
        }

        public List<MGPumEntity> getSelection(MGPumGameState state, MGPumEntity selectionSource, MGPumGameEvent triggeringEvent, MGPumEffectExecutionState eestate)
        {
            
            List<MGPumEntity> selectedEntities = new List<MGPumEntity>();
            List<MGPumEntity> possibleEntities = filter.getSelection(state, selectionSource, triggeringEvent, eestate);
            for (int i = 0; i < amount; i++)
            {
                if (possibleEntities != null && possibleEntities.Any())
                {
                    int selectIndex = 0;
                    if (state != null)
                    {
                        selectIndex = state.rng.Next(0, possibleEntities.Count);
                    }
                    else
                    {
                        selectIndex = new System.Random().Next(0, possibleEntities.Count);
                    }
                    
                    MGPumEntity selected = possibleEntities[selectIndex];
                    possibleEntities.Remove(selected);
                    selectedEntities.Add(selected);
                }
            }
            return selectedEntities;
        }

        public MGPumSelection deepCopy(MGPumGameState state)
        {
            //Debug.LogWarning("RandomSelector is not deepcopied");
            return this;
        }

      
    }
}
