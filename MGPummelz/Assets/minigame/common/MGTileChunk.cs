using UnityEngine;

using rccg.frontend;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MGTileChunk
{
    public int minX;
    public int minY;
    public int maxX;
    public int maxY;

    public int tile;

    public MGTileChunk(int x, int y, int tile)
    {
        minX = x;
        maxX = x;
        minY = y;
        maxY = y;
        this.tile = tile;
    }

    internal bool canMerge(MGTileChunk chunk)
    {
        if(this.tile != chunk.tile)
        {
            return false;
        }
        //try vertically
        if(minX == chunk.minX && maxX == chunk.maxX)
        {
            if((minY - 1) == chunk.maxY || (maxY + 1) == chunk.minY)
            {
                return true;
            }
        }
        //try horizontally
        if (minY == chunk.minY && maxY == chunk.maxY)
        {
            if ((minX - 1) == chunk.maxX || (maxX + 1) == chunk.minX)
            {
                return true;
            }
        }
        return false;
    }

    internal void merge(MGTileChunk chunk)
    {
        this.minX = Math.Min(minX, chunk.minX);
        this.maxX = Math.Max(maxX, chunk.maxX);
        this.minY = Math.Min(minY, chunk.minY);
        this.maxY = Math.Max(maxY, chunk.maxY);
    }
}
