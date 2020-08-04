using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArchipelagoGenerator : MonoBehaviour
{
    public static ArchipelagoGenerator gen;

    public bool showMap;
    public Transform player;
    
    [Range(5, 10)] public readonly static int waterRate = 7;

    [Header("Map Settings")]
    [Range(1, 128)] public int width = 64;
    [Range(1, 128)] public int height = 64;

    public Region[] regions;

    public int regionSize;

    static Texture2D archipelagoMap;
    static Transform archipelago;

    private void Awake()
    {
        if (gen == null)
            gen = this;
    }

    private void Start()
    {
        Generate();
    }

    void Generate()
    {
        if(!Mathf.IsPowerOfTwo(width) || !Mathf.IsPowerOfTwo(height))
        {
            Debug.LogError("ARCHIPELAGO SIZE IS NOT A POWER OF TWO. --- STOPPING GENERATION ---");
            return;
        }

        archipelagoMap = ArchipelagoTextureGenerator.GenerateTexture(regions, width, height, waterRate);
        archipelago = new GameObject("Archipelago").GetComponent<Transform>();

        gameObject.AddComponent<ChunkSerialization>();
        ChunkSerialization.Reset();
        ChunkSerialization.chunkSize = regionSize;        
        ChunkSerialization.viewer = player;

        Color[] colorMap = archipelagoMap.GetPixels();

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = (width * z) + x;
                Vector3 pos = new Vector3(x - Mathf.RoundToInt(width / 2), 0, z - Mathf.RoundToInt(height / 2)) * regionSize;
                GameObject obj;

                foreach (Region r in regions)
                {
                    if (Mathf.Abs(r.color.r - colorMap[index].r) <= 0.009f)
                    {
                        obj = Instantiate(r.region, pos, Quaternion.identity, archipelago);
                        ChunkSerialization.chunks.Add(pos/regionSize, new ChunkSerialization.TerrainChunk(pos, obj));
                    }
                }
            }
        }

        Destroy(this);
    }

    private void OnGUI()
    {
        if(Input.GetKeyDown(KeyCode.Return))
            archipelagoMap = ArchipelagoTextureGenerator.GenerateTexture(regions, width, height, waterRate);

        if (showMap)
            GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), archipelagoMap, ScaleMode.ScaleToFit);
    }
}

[System.Serializable]
public class Region
{
    public string name;
    [Range(0,1)] public float spawnWeight;
    public Color color;
    public GameObject region;
}