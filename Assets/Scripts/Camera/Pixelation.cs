using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Manual Step: Change camera anti-aliasing to none.
public class Pixelation : MonoBehaviour
{
    RenderTexture rT;

    private void OnGUI()
    {
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Camera.main.activeTexture, ScaleMode.StretchToFill);
    }

    //private void OnDisable()
    //{
    //    // Unstable
    //    Camera.main.targetTexture = null;
    //}

    private void OnEnable()
    {
        // Create Render Texture
        rT = new RenderTexture(256, 256, 24);
        rT.filterMode = FilterMode.Point;
        rT.useMipMap = false;

        // Attach Render Texture
        Camera.main.targetTexture = rT;
    }
}
