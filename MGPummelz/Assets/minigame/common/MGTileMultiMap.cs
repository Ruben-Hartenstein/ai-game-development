using UnityEngine;

using rccg.frontend;
using UnityEngine.UI;
using System.Collections.Generic;

public class MGTileMultiMap : MonoBehaviour
{
    public MGTileMap tileMap;

    internal MGTileChunk[,] tileChunks;

    internal List<MGTileChunk> tileChunkList;

    public void createChunks()
    {
        tileChunks = new MGTileChunk[tileMap.tilesXLength, tileMap.tilesYLength];
        tileChunkList = new List<MGTileChunk>();
        //create initial chunks
        for (int x = 0; x < tileMap.tilesXLength; x++)
        {
            for (int y = 0; y < tileMap.tilesYLength; y++)
            {
                MGTileChunk chunk = new MGTileChunk(x, y, tileMap.tileIndizes[x, y]);
                tileChunks[x, y] = chunk;
                tileChunkList.Add(chunk);
            }
        }

        Debug.LogError("Chunks initial: " + tileChunkList.Count);

        //merge chunks horizontally
        for (int x = 1; x < tileMap.tilesXLength; x++)
        {
            for (int y = 0; y < tileMap.tilesYLength; y++)
            {
                if(tileChunks[x - 1, y].canMerge(tileChunks[x, y]))
                {
                    tileChunks[x - 1, y].merge(tileChunks[x, y]);
                    tileChunkList.Remove(tileChunks[x, y]);
                    tileChunks[x, y] = tileChunks[x - 1, y];
                }
            }
        }

        Debug.LogError("Chunks first pass: " + tileChunkList.Count);

        //merge chunks vertically
        for (int y = 1; y < tileMap.tilesXLength; y++)
        {
            for (int x = 0; x < tileMap.tilesYLength; x++)
            {
                if (tileChunks[x, y - 1] != tileChunks[x, y] && tileChunks[x, y - 1].canMerge(tileChunks[x, y]))
                {
                    tileChunks[x, y - 1].merge(tileChunks[x, y]);
                    tileChunkList.Remove(tileChunks[x, y]);
                    tileChunks[x, y] = tileChunks[x, y - 1];
                }
            }
        }

        Debug.LogError("Chunks final: " + tileChunkList.Count);


    }

}
