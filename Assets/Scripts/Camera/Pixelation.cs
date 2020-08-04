using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Manual Step: Change camera anti-aliasing to none.
public class Pixelation : MonoBehaviour
{
    [Range(1,40)] public int pixelate;
    public RawImage screen;

    RenderTexture rT;

    private void OnEnable()
    {
        // Create Render Texture
        rT = new RenderTexture(8 * (41-pixelate), 8 * (41-pixelate), 24);
        rT.filterMode = FilterMode.Point;
        rT.useMipMap = false;

        // Attach Camrea to Render Texture
        Camera.main.targetTexture = rT;

        // Attach Render Texture to Canvas Raw Image
        screen.gameObject.SetActive(true);
        screen.texture = rT;
    }
}
