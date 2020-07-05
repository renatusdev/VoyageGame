using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator : MonoBehaviour
{
    [Range(0, 100)] public int fallOff;
    [Range(0, 1)] public float fallOffInterpolation;

    [Range(0, 1)] public float size;
    [Range(1, 25)] public int padding;
    [Range(0.1f, 0.5f)] public float intHeight;
    [Range(0.1f, 0.5f)] public float extHeight;
    [Range(10, 100)] public float intScale;
    [Range(2, 100)] public float extScale;
    [Range(1, 100)]  public int height;

    public float xOff;
    public float yOff;

    public AnimationCurve heightCurve;
    public Material mat;

    public Texture2D texture;

    MeshFilter mF;
    MeshRenderer mR;

    void Start()
    {
        mF = gameObject.AddComponent<MeshFilter>();
        mR = gameObject.AddComponent<MeshRenderer>();
        Generate(texture, padding, Random.Range(0,300), Random.Range(0, 300), extHeight, intHeight, extScale, intScale, size);
    }

    // Island terrain from 1:1 texture alpha. Has subdivided perlin noise for exterior and interior.//
    public void Generate(Texture2D txt, int padding, float xOff, float yOff, float extHeight, float intHeight, float extScale, float intScale, float size)
    {
        float[,] heightMap = Noise.Generate(txt, padding, xOff, yOff, extHeight, intHeight, extScale, intScale);
        MeshFromHeightMap(heightMap, txt.width + (padding * 2) + 1, size);
    }

    // Terrain From Perlin Noise //
    public void Generate(int area, float xOff, float yOff, float noise, float size)
    {
        // The reason its area + 1 is cuz later we generate a plane, which uses whichever dimensions
        // you want + 1 as the amount of vertices to plot. So since our heightMap is for verts, we do + 1 as well.
        float[,] heightMap = Noise.Generate(area+1, area+1, xOff, yOff, noise, 1);
        MeshFromHeightMap(heightMap, area, size);
    }

    private void MeshFromHeightMap(float[,] heightMap, int area, float size)
    {
        Mesh plane = Plane.Generate(area, false);
        Vector3[] verts = plane.vertices;

        for (int y = 0; y <= area; y++)
            for (int x = 0; x <= area; x++)
            {
                int i = ((area+1) * x) + y;
                float sample = heightMap[x, y];
                
                verts[i] = new Vector3(verts[i].x, heightCurve.Evaluate(sample) * height, verts[i].z) * size;
            }

        FallOff(verts, area);
        
        plane.vertices = verts;
        plane.RecalculateBounds();
        plane.RecalculateNormals();
        plane.RecalculateTangents();

        mF.sharedMesh = plane;
        mR.sharedMaterial = mat;
    }

    private void FallOff(Vector3[] verts, int area)
    {
        int fallOffDistance = fallOff + 15;

        for (int i = area; i >= 0 ; i--)
        {
            // i

            verts[i].y = Mathf.Lerp(verts[i].y, -fallOff, fallOffInterpolation);
            verts[i].x -= fallOffDistance;

            // (area+1) * x + i

            int index = (area + 1) * area + i;

            verts[index].y = Mathf.Lerp(verts[i].y, -fallOff, fallOffInterpolation);
            verts[index].x += fallOffDistance;

            // (area+1) * i

            index = (area + 1) * i;

            verts[index].y = Mathf.Lerp(verts[i].y, -fallOff, fallOffInterpolation);
            verts[index].z -= fallOffDistance;

            // (area+1) * i + area

            index = (area + 1) * i + area;

            verts[index].y = Mathf.Lerp(verts[i].y, -fallOff, fallOffInterpolation);
            verts[index].z += fallOffDistance;
        }
    }

}
