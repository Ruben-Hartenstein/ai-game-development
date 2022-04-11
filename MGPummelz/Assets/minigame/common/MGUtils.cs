using UnityEngine;

public static class MGUtils
{

    //position must be center position for this to work
    public static void keepWithinBounds(GameObject gObject, Vector3 position, Transform boundTransform)
    {
        float height = Screen.height;
        float width = Screen.width;

        //Debug.LogError("screen " + Screen.width + "*" + Screen.height);

        float minX = 0;
        float minY = 0;
        float maxX = width;
        float maxY = height;

        //float minX = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).x;
        //float minY = Camera.main.ScreenToWorldPoint(new Vector2(0f, 0f)).y;
        //float maxX = Camera.main.ScreenToWorldPoint(new Vector2(width, height)).x;
        //float maxY = Camera.main.ScreenToWorldPoint(new Vector2(width, height)).y;

        float newX = Camera.main.WorldToScreenPoint(position).x;
        float newY = Camera.main.WorldToScreenPoint(position).y;

        //Debug.LogError("position " + newX + "*" + newY);


        //Debug.LogError("Between " + minX + "/" + maxX + "     " + minY + "/" + maxY);

        //Debug.LogError(position);

        //Debug.LogError(newX + " / " + newY);



        //keep tooltip fully visible to camera
        Vector3 extents = new Vector3(144.0f, 200.0f, 0.0f);

        Vector3[] worldCorners = new Vector3[4];
        gObject.GetComponent<RectTransform>().GetWorldCorners(worldCorners); //new Vector3(  , 200.0f, 0.0f);


        float lengthX = Mathf.Abs(Camera.main.WorldToScreenPoint(worldCorners[0]).x - Camera.main.WorldToScreenPoint(worldCorners[2]).x);
        float lengthY = Mathf.Abs(Camera.main.WorldToScreenPoint(worldCorners[0]).y - Camera.main.WorldToScreenPoint(worldCorners[1]).y);

        //Debug.LogError("length " + lengthX + "*" + lengthY);

        //if (screenSpaceOverlay)
        //{
        //    lengthX = gObject.GetComponent<RectTransform>().sizeDelta.x;
        //    lengthY = gObject.GetComponent<RectTransform>().sizeDelta.y;
        //}


        //Debug.LogError("Extents:" + lengthX + " / " + lengthY);
        extents = new Vector3(lengthX / 2.0f, lengthY / 2.0f, 0f);

        if (newX + extents.x > maxX)
        {
            newX = maxX - extents.x;
        }
        else if (newX - extents.x < minX)
        {
            newX = minX + extents.x;
        }

        if (newY + extents.y > maxY)
        {
            newY = maxY - extents.y;
        }
        else if (newY - extents.y < minY)
        {
            newY = minY + extents.y;
        }

        //Debug.LogError("new " + newX + "*" + newY);

        //Debug.LogError(newX + " / " + newY);
        //Debug.LogError(new Vector3(Camera.main.ScreenToWorldPoint(new Vector2(newX, newY)).x, Camera.main.ScreenToWorldPoint(new Vector2(newX, newY)).y, gObject.transform.position.z));

       
        gObject.transform.position = new Vector3(newX, newY, gObject.transform.position.z);
       


    }
}
