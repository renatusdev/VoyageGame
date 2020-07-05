using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    [Range(2, 100)] public int scale;
    [Range(1,100)]  public int area;

    public float xOff;
    public float yOff;

    Texture2D txt;

    private void OnGUI()
    {
        if (txt == null)
            return;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), TextureGenerator.TextureFromHeightMap(Noise.Generate(area, area, xOff, yOff, scale,1)));
    }

    public void SendTexture(Texture2D txt)
    {
        this.txt = txt;
    }

}
