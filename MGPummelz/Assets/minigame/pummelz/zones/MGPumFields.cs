using RelegatiaCCG.rccg.engine.state;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace mg.pummelz
{

    [System.Serializable]
    public class MGPumFields : IEnumerable<MGPumField>, MGPumZone
    {
        public int dimSize = 8;

        public int ownerID;

        public MGPumField[,] fieldArray;

        //for deepcopying
        private MGPumFields(int dimSize)
        {
            this.ownerID = MGPumEntity.NEUTRAL_OWNER;

            fieldArray = new MGPumField[dimSize, dimSize];
        }

        public MGPumFields(IDManager idm)
        {
            this.ownerID = MGPumEntity.NEUTRAL_OWNER;

            fieldArray = new MGPumField[dimSize, dimSize];

            //initialize all fields
            for (int x = 0; x < dimSize; x++)
            {
                for (int y = 0; y < dimSize; y++)
                {
                    fieldArray[x, y] = new MGPumField(x, y);
                    fieldArray[x, y].id = idm.getNextID();
                }
            }
        }

        public MGPumField getField(int x, int y)
        {
            return fieldArray[x, y];
        }

        public MGPumField getField(Vector2Int coords)
        {
            return fieldArray[coords.x, coords.y];
        }

        public bool inBounds(Vector2Int coords)
        {
            return coords.x >= 0 && coords.x < fieldArray.GetLength(0) && coords.y >= 0 && coords.y < fieldArray.GetLength(1);
        }

        public IEnumerator<MGPumField> GetEnumerator()
        {
            return fieldArray.Cast<MGPumField>().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public MGPumZoneType getZoneType()
        {
            return MGPumZoneType.Battlegrounds;
        }

        public int count()
        {
            int count = 0;

            for (int x = 0; x < dimSize; x++)
            {
                for (int y = 0; y < dimSize; x++)
                {
                    if (fieldArray[x, y].unit != null)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        public int getOwnerID()
        {
            return ownerID;
        }

        public MGPumFields deepCopy(MGPumGameState state)
        {
            MGPumFields copy = new MGPumFields(dimSize);

            //initialize all fields
            for (int x = 0; x < dimSize; x++)
            {
                for (int y = 0; y < dimSize; y++)
                {
                    copy.fieldArray[x, y] = new MGPumField(x, y);
                    copy.fieldArray[x, y].id = this.fieldArray[x, y].id;
                    copy.fieldArray[x, y].unit = state.lookupOrCreate(this.fieldArray[x, y].unit);
                }
            }
            return copy;
        }

    }
}
