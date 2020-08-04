using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    private readonly static float minHeight = 0.6f;

    public int x, y;
    public float scale;

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), IslandTextureGenerator(x, y, scale));
    }

    Texture2D IslandTextureGenerator(int x, int y, float scale)
    {
        return TextureGenerator.TextureFromHeightMap(Noise.GenerateRegion(x, y, scale, 0.64f, 100));
    }
}
