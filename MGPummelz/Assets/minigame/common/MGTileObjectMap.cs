using UnityEngine;

using rccg.frontend;

public class MGTileObjectMap : MonoBehaviour
{

    int[,] tileIndizes;

    public MGTile[,] tiles;



    Sprite[] tileset;

    int tilesize = 8;

    int resolutionX = 160;
    int resolutionY = 160;

    int tilesXLength = 20;
    int tilesYLength = 20;

    string tilesetName;



    public void initialize()
    {
        tilesXLength = resolutionX / tilesize;
        tilesYLength = resolutionY / tilesize;
        tileIndizes = new int[resolutionX / tilesize, resolutionY / tilesize];
        tiles = new MGTile[resolutionX / tilesize, resolutionY / tilesize];
        loadTileset();

        UnityEngine.Object prefab = GUIResourceLoader.getResourceLoaderInstance().loadMinigamePrefab("MGTile");

        for (int x = 0; x < tilesXLength; x++)
        {
            for (int y = 0; y < tilesYLength; y++)
            {
                GameObject tileGO = Instantiate(prefab) as GameObject;
                tileGO.transform.SetParent(this.transform);
                tileGO.transform.localScale = this.transform.localScale;
                MGTile tile = tileGO.GetComponent<MGTile>();
                tile.initialize(x, y, tileset[tileIndizes[x, y]], this);
                tiles[x, y] = tile;
            }
        }
    }

    public void rerender(int[,] tileIndizes)
    {
        this.tileIndizes = tileIndizes;
        rerender();
    }

    public void rerender()
    {
        Debug.LogError("rerender");
        for(int x = 0; x < tilesXLength; x++)
        {
            for (int y = 0; y < tilesYLength; y++)
            {
                tiles[x, y].changeSprite(tileset[tileIndizes[x, y]]);
            }
        }

    }

    public void loadTileset()
    {
        if(this.tileset == null)
        {
            this.tileset = GUIResourceLoader.getResourceLoaderInstance().loadMinigameTileset(tilesetName);
        }
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
