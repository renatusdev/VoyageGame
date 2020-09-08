using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandGenerator : MonoBehaviour
{
    #region Region Generation Parameters
    private readonly static float rMinHeight = 0.65f;
    private readonly static int rArea = 45;
    private readonly static int rScale = 15;
    #endregion

    public int seed;
    public float height;
    public Material mat;


    MeshFilter mF;
    MeshRenderer mR;

    private void Start()
    {
        mF = gameObject.AddComponent<MeshFilter>();
        mR = gameObject.AddComponent<MeshRenderer>();
        GenerateMesh(seed, Noise.GenerateRegion(seed, seed, rScale, rMinHeight, rArea));

    }

    private void Update()
    {
        GenerateMesh(seed, Noise.GenerateRegion(seed, seed, rScale, rMinHeight, rArea));
    }

    void GenerateMesh(int seed, float[,] heightMap)
    {
        Vector3[] verts = new Vector3[(rArea + 1) * (rArea + 1)];
        int[] tris = new int[verts.Length * 6];

        for (int x = 0; x < rArea; x++)
            for (int z = 0; z < rArea; z++)
            {
                float sample = heightMap[x, z];

                verts[index(x, z)] = new Vector3(x - (rArea / 2), sample * height, z - (rArea / 2));

                if (sample <= 0)
                    continue;

                tris[index(x, z) * 6 + 0] = index(x, z);
                tris[index(x, z) * 6 + 1] = index(x, z + 1);
                tris[index(x, z) * 6 + 2] = index(x + 1, z + 1);

                tris[index(x, z) * 6 + 3] = index(x, z);
                tris[index(x, z) * 6 + 4] = index(x + 1, z + 1);
                tris[index(x, z) * 6 + 5] = index(x + 1, z);
            }

        for (int x = 0; x < rArea; x++)
            for (int z = 0; z < rArea; z++)
            {


                tris[index(x, z) * 6 + 0] = index(x, z);
                tris[index(x, z) * 6 + 1] = index(x, z + 1);
                tris[index(x, z) * 6 + 2] = index(x + 1, z + 1);

                tris[index(x, z) * 6 + 3] = index(x, z);
                tris[index(x, z) * 6 + 4] = index(x + 1, z + 1);
                tris[index(x, z) * 6 + 5] = index(x + 1, z);
            }

        Mesh m = new Mesh() { vertices = verts, triangles = tris };

        m.RecalculateTangents();
        m.RecalculateNormals();
        m.RecalculateBounds();

        mF.sharedMesh = m;
        mR.sharedMaterial = mat;
    }

    static int index(int x, int z) { return (((rArea + 1) * x) + z); }
}
