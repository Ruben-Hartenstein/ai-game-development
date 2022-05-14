using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace mg.pummelz
{
    public class MGPumBreadthFirstSearch 
    {
        public bool[,] queued;
        public Queue<Vector2Int> tilesToVisit;
        public Dictionary<Vector2Int, Vector2Int> predecessors;

        public List<Vector2Int> getNeighbors(Vector2Int pos, MGPumFields fields)
        {
            List<Vector2Int> neighbourFields = new List<Vector2Int>();
            MGPumField home = fields.getField(pos);
            foreach(MGPumField field in fields.fieldArray) 
            {
                if(home.neighboring(field))
                    neighbourFields.Add(field.coords);
            }
            return neighbourFields;
        }

        public List<Vector2Int> findPathInternal(MGPumUnit unit, MGPumField targetField, MGPumFields fields)
        {
            queued = new bool[fields.dimSize, fields.dimSize];
            tilesToVisit = new Queue<Vector2Int>();
            predecessors = new Dictionary<Vector2Int, Vector2Int>();

            tilesToVisit.Enqueue(unit.field.coords);
            queued[unit.field.x, unit.field.y] = true;
            
            int recursion = 0;
            int maxRecursion = 500;
            while(tilesToVisit.Count > 0)
            {
                recursion++;
                if(recursion > maxRecursion)
                    break;
                
                Vector2Int position = tilesToVisit.Dequeue();

                if (position == targetField.coords)
                {
                    List<Vector2Int> path = new List<Vector2Int>();
                    path.Add(position);

                    //reconstruct path in reverse
                    while(predecessors.ContainsKey(path[0]))
                    {
                        path.Insert(0, predecessors[path[0]]);
                    }
                    return path;
                }

                if (fields.fieldArray[position.x, position.y] == null || (!fields.getField(position).isEmpty() && position != unit.field.coords)){
                    continue; 
                }

                foreach (Vector2Int neighbor in getNeighbors(position, fields))
                {
                    if(!queued[neighbor.x, neighbor.y])
                    {
                        queued[neighbor.x, neighbor.y] = true;
                        tilesToVisit.Enqueue(neighbor);
                        predecessors.Add(neighbor, position);
                    }
                }
            }
            return null;     
        }
    }
}