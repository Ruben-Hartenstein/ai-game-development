using UnityEngine;

using rccg.frontend;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MGTile : MonoBehaviour, IPointerClickHandler
{
    public int x;
    public int y;
    public Image image;

    public MGTileObjectMap tilemap;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Clicked tile " + x + "/" + y);
    }

    internal void initialize(int x, int y, Sprite sprite, MGTileObjectMap mGTilemap)
    {
        this.x = x;
        this.y = y;
        this.image.sprite = sprite;
        this.tilemap = mGTilemap;
    }

    internal void changeSprite(Sprite sprite)
    {
        this.image.sprite = sprite;
    }
}
