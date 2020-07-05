using UnityEngine;

public class PlaneGenerator : MonoBehaviour
{
    public int dimensions = 5;
    public int scale = 5;

    Mesh m;
    MeshFilter mF;
    MeshRenderer mR;

    public void Generate()
    {
        // Instantiate Components
        m = new Mesh();

        if (!gameObject.TryGetComponent<MeshFilter>(out mF))
            mF = gameObject.AddComponent<MeshFilter>();

        if (!gameObject.TryGetComponent<MeshRenderer>(out mR))
            mR = gameObject.AddComponent<MeshRenderer>();

        // Mesh Generation
        m.name = gameObject.name;
        m.vertices = GenerateVerts();   // SimpleGenerateVerts();
        m.triangles = GenerateTris();   // SimpleGenerateTris();

        // Recalculations
        m.RecalculateBounds();
        m.RecalculateNormals();
        m.RecalculateTangents();

        // Mesh Insertion
        mF.mesh = m;
    }

    private Vector3[] GenerateVerts()
    {
        // Instantiate Vertices
        Vector3[] verts = new Vector3[(dimensions+1) * (dimensions+1)];

        // Fill Up Vertices
        for (int x = 0; x <= dimensions; x++)
            for (int z = 0; z <= dimensions; z++)
                verts[index(x,z)] = new Vector3((x- (dimensions / 2)) * scale, 0, (z-(dimensions/2)) * scale);

        return verts;
    }

    private int[] GenerateTris()
    {
        int[] tris = new int[m.vertices.Length * 6];

        for (int x = 0; x < dimensions; x++)
            for (int z = 0; z < dimensions; z++)
            {
                tris[index(x, z) * 6 + 0] = index(x, z);
                tris[index(x, z) * 6 + 1] = index(x, z + 1);
                tris[index(x, z) * 6 + 2] = index(x + 1, z + 1);

                tris[index(x, z) * 6 + 3] = index(x, z);
                tris[index(x, z) * 6 + 4] = index(x + 1, z + 1);
                tris[index(x, z) * 6 + 5] = index(x + 1, z);
            }

        return tris;
    }

    public int index(int x, int z) { return ((dimensions+1) * x) + z; }
}