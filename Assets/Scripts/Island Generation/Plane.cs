using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Plane 
{
    static int area;

    public static Mesh Generate(int area, bool recalculate)
    {   
        Plane.area = area;

        Vector3[] verts = new Vector3[(area + 1) * (area + 1)];
        int[] tris = new int[verts.Length * 6];

        for (int x = 0; x <= area; x++)
            for (int z = 0; z <= area; z++)
                verts[index(x, z)] = new Vector3(x - (area / 2), 0, z - ( area / 2 ));

        for (int x = 0; x < area; x++)
            for (int z = 0; z < area; z++)
            {
                tris[index(x, z) * 6 + 0] = index(x, z);
                tris[index(x, z) * 6 + 1] = index(x, z + 1);
                tris[index(x, z) * 6 + 2] = index(x + 1, z + 1);

                tris[index(x, z) * 6 + 3] = index(x, z);
                tris[index(x, z) * 6 + 4] = index(x + 1, z + 1);
                tris[index(x, z) * 6 + 5] = index(x + 1, z);
            }

        Mesh m = new Mesh() { vertices = verts, triangles = tris };

        if (recalculate)
        {
            m.RecalculateTangents();
            m.RecalculateNormals();
            m.RecalculateBounds();
        }

        return m;
    }

    static int index(int x, int z) { return (((area + 1) * x) + z); }
}
