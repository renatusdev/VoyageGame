using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D TextureFromHeightMap(float[,] map)
    {
        int w = map.GetLength(0);
        int h = map.GetLength(1);
        Color[] colorMap = new Color[w * h];

        for (int y = 0; y < h; y++)
            for (int x = 0; x < w; x++)
                colorMap[(w * y) + x] = Color.Lerp(Color.black, Color.white, map[x, y]);

        return TextureFromColorMap(colorMap, w, h);
    }

    public static Texture2D TextureFromColorMap(Color[] colorMap, int width, int height)
    {
        Texture2D txt = new Texture2D(width, height) { wrapMode = TextureWrapMode.Clamp, filterMode = FilterMode.Point };
        txt.SetPixels(colorMap);
        txt.Apply();

        return txt;
    }
}
