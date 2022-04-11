using UnityEngine;

using rccg.frontend;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class MGResizableObject : MGObject
{
    public void resize(MGTileChunk chunk)
    {
        Vector2Int objExtents = new Vector2Int(8 * (chunk.maxX - chunk.minX + 1), 8 * (chunk.maxY - chunk.minY + 1));
        Vector2Int objCenter = new Vector2Int(objExtents.x / 2 + chunk.minX * 8, objExtents.y / 2 + chunk.minY * 8);

        RectTransform rt = this.GetComponent<RectTransform>();
        if(rt != null)
        {
            rt.sizeDelta = objExtents;
            transform.localPosition = new Vector3(objCenter.x, objCenter.y, transform.localPosition.z);
        }
        BoxCollider2D collider2D = this.GetComponent<BoxCollider2D>();
        if(collider2D != null)
        {
            collider2D.size = objExtents;
        }
    }


}
