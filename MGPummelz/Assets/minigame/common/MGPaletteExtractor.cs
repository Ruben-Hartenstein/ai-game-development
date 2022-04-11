using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using rccg.frontend;
using System.IO;
using System;

public class MGPaletteExtractor : MonoBehaviour
{

    public MGMinigameBitmapLevel mbl;

    public void Start()
    {
        mbl = new MGMinigameBitmapLevel("test", "m3palette_extended");
        mbl.load();

        Dictionary<Color, List<Vector2Int>> pieces = new Dictionary<Color, List<Vector2Int>>();

        HashSet<string> palette = new HashSet<string>();

        string filePath = Application.dataPath + "/palettem3.txt";

        string content = "";

        for (int x = 0; x < 240; x++)
        {
            for (int y = 0; y < 8; y++)
            {
                Color c = mbl.get(x, y);

                string color = ColorUtility.ToHtmlStringRGB(c);

                string colordec = Int32.Parse(color.Substring(0, 2), System.Globalization.NumberStyles.HexNumber) + " " + Int32.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) + " " + Int32.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);

                if (!palette.Contains(color))
                {
                    content += "\n" + colordec;
                }
                else
                {
                    Debug.LogError(color);
                }
                palette.Add(color);




            

      


}



        }
        File.WriteAllText(filePath, content);

       

    }

}
