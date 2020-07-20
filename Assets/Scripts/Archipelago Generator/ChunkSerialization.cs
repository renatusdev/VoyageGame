using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkSerialization : MonoBehaviour
{
    public static Transform viewer;                         // Viewer
    public static int rD = 18;                              // Render Distance
    public static int chunkSize;                            // Scale Of A Chunk
    public static Dictionary<Vector3, TerrainChunk> chunks; // Chunk Container Of All Chunks
    private static List<TerrainChunk> lastChunks;           // Chunk Container Of Visible Chunks @ Last Frame

    public static void Reset()
    {
        chunks = new Dictionary<Vector3, TerrainChunk>();
        lastChunks = new List<TerrainChunk>();
    }

    void Update()
    {
        UpdateVisibleChunks();
    }

    private void UpdateVisibleChunks()
    {
        for (int i = 0; i < lastChunks.Count; i++)
            lastChunks[i].ChunkUpdate();
        lastChunks.Clear();
        
        int xPos = Mathf.RoundToInt(viewer.position.x / chunkSize);
        int zPos = Mathf.RoundToInt(viewer.position.z / chunkSize);

        for (int z = -rD; z <= rD; z++)
        {
            for (int x = -rD; x <= rD; x++)
            {
                // Chunk Position
                Vector3 cP = new Vector3(x + xPos, 0, z + zPos);

                if (chunks.ContainsKey(cP))
                {
                    lastChunks.Add(chunks[cP]);
                }
            }
        }
    }

    public class TerrainChunk
    {
        Vector3 coords;
        GameObject obj;

        public TerrainChunk(Vector3 coords, GameObject obj)
        {
            this.coords = coords;
            this.obj = obj;

            SetVisible(false);
        }
        
        public void ChunkUpdate()
        {
            SetVisible(Vector3.Distance(coords, viewer.position) <= rD * chunkSize);
        }

        public void SetVisible(bool state)
        {
            obj.SetActive(state);
        }
    }
}