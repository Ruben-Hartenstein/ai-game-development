using RelegatiaCCG.rccg.i18n;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace mg.pummelz
{
    [System.Serializable]
    public class MGPumAroundSourceFilter : MGPumSubFilter
    {

        public MGPumAroundSourceFilter() : base()

        {
        }

        public override List<MGPumEntity> apply(IEnumerable<MGPumEntity> entities, MGPumEntity source)
        {
            List<MGPumEntity> filteredEntities = new List<MGPumEntity>();

            Vector2Int scoords = Vector2Int.zero;

            if (source is MGPumUnit && ((MGPumUnit)source).lastKnownField != null)
            {
                
                scoords = ((MGPumUnit)source).lastKnownField.coords;
            }
            else if (source is MGPumField)
            {
                scoords = ((MGPumField)source).coords;
            }
            else
            {
                Debug.LogError("Source has no coordinates!");
                return filteredEntities;
            }


            foreach (MGPumEntity entity in entities)
            {
                Vector2Int ecoords = Vector2Int.zero;
                if (source is MGPumUnit)
                {
                    ecoords = ((MGPumUnit)entity).coords;
                }
                else if (source is MGPumField)
                {
                    ecoords = ((MGPumField)entity).coords;
                }
                else
                {
                    continue;
                }

                if(entity != source && (Math.Abs(scoords.x - ecoords.x) <= 1) && (Math.Abs(scoords.y - ecoords.y) <= 1))
                {
                    filteredEntities.Add(entity);
                }
            }
            return filteredEntities;
        }

        
    }
}
