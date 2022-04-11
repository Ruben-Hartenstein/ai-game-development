using UnityEngine;

using rccg.frontend;
using UnityEngine.UI;

public class MGTileSelector
{

    

    int[,] tileIndizes;

    bool borderIsNeighbor = false;

    Vector2Int up = new Vector2Int(0, 1);
    Vector2Int down = new Vector2Int(0, -1);
    Vector2Int left = new Vector2Int(-1, 0);
    Vector2Int right = new Vector2Int(1, 0);

    //Vector2Int upleft = new Vector2Int(-1, -1);
    //Vector2Int upright = new Vector2Int(1, -1);
    //Vector2Int downleft = new Vector2Int(-1, 1);
    //Vector2Int downright = new Vector2Int(1, 1);





    bool hasNeighbor(int x, int y, Vector2Int direction)
    {
        int goalX = x + direction.x;
        int goalY = y + direction.y;

        //Debug.LogError("" + goalX + "/" + goalY);

        if (goalX < 0 || goalX > tileIndizes.GetUpperBound(0) || goalY < 0 || goalY > tileIndizes.GetUpperBound(1)) {
            return borderIsNeighbor;
        }
        else
        {
            return tileIndizes[x, y] == tileIndizes[goalX, goalY];
        }
    }

    public void copyToTexture(int x, int y, int[,] tileIndizes, Sprite[] tileset, Texture2D tilemapTexture, int tilesize)
    {
        this.tileIndizes = tileIndizes;
        Sprite sprite = tileset[tileIndizes[x, y]];




        //tilemapTexture.SetPixels(x * tilesize, y * tilesize, tilesize, tilesize, pixels);

        {
            int tile = selectTile(x, y, 0, left, up);
            copySpriteTile(tileset[tile], tilemapTexture, x * tilesize, y * tilesize + 4);
        }
        {
            int tile = selectTile(x, y, 1, right, up);
            copySpriteTile(tileset[tile], tilemapTexture, x * tilesize + 4, y * tilesize + 4);
        }
        {
            int tile = selectTile(x, y, 2, left, down);
            copySpriteTile(tileset[tile], tilemapTexture, x * tilesize, y * tilesize);
        }
        {
            int tile = selectTile(x, y, 3, right, down);
            copySpriteTile(tileset[tile], tilemapTexture, x * tilesize + 4, y * tilesize);
        }


    }


    public int selectTile(int x, int y, int offset, Vector2Int horizontal, Vector2Int vertical)
    {
        bool hvertical = hasNeighbor(x, y, vertical);
        bool hhorizontal = hasNeighbor(x, y, horizontal);
        bool hdiagonal = hasNeighbor(x, y, horizontal + vertical);

        int tile = 0;

        if (!hvertical && hhorizontal)
        {
            tile = 4;
        }
        else if (hvertical && !hhorizontal)
        {
            tile = 8;
        }
        else if (hvertical && hhorizontal && !hdiagonal)
        {
            tile = 12;
        }
        else if (hvertical && hhorizontal && hdiagonal)
        {
            tile = 16;
        }


        return tile + offset;
    }

    //public int upleftTile(int x, int y)
    //{
    //    bool hup = hasNeighbor(x, y, up);
    //    bool hleft = hasNeighbor(x, y, left);
    //    bool hupleft = hasNeighbor(x, y, upleft);

    //    int tile = 0;

    //    if (!hup && hleft)
    //    {
    //        tile = 4;
    //    }
    //    else if (hup && !hleft)
    //    {
    //        tile = 8;
    //    }
    //    else if (!hup && !hleft && hupleft)
    //    {
    //        tile = 12;
    //    }
    //    else if (hup && hleft && hupleft)
    //    {
    //        tile = 16;
    //    }
        

    //    return tile;
    //}


    protected void copySpriteTile(Sprite sprite, Texture2D texture, int x, int y)
    {
        Color[] pixels = sprite.texture.GetPixels((int)sprite.rect.x,
                                            (int)sprite.rect.y,
                                            
                                            (int)sprite.rect.width,
                                            (int)sprite.rect.height);

        texture.SetPixels(x , y , (int)sprite.rect.width, (int)sprite.rect.height, pixels);
    }
        
}
