using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArchipelagoTextureGenerator
{
    private static Region ocean;

    public static Texture2D GenerateTexture(Region[] regions, int width, int height, int waterRate)
    {
        Color[] colormap = new Color[width * height];

        foreach (Region r in regions)
            if (r.name.Equals("Ocean"))
            {
                ocean = r;

                if (ocean.spawnWeight > 1)
                    ocean.spawnWeight -= waterRate;
                else
                    ocean.spawnWeight += waterRate;
            }

        GenerateRegions(regions, colormap, width, height);

        return TextureGenerator.TextureFromColorMap(colormap, width, height);
    }

    static void GenerateRegions(Region[] regions, Color[] colormap, int width, int height)
    {
        float w = 0;

        //Finds the total value of all weights.
        for (int i = 0; i < regions.Length; i++)
            w += regions[i].spawnWeight;

        w += ArchipelagoGenerator.waterRate;

        // Loops through archipelago map.
        for (int y = 1; y < height - 1; y++)
        {
            for (int x = 1; x < width - 1; x++)
            {
                int index = (width * y) + x;

                Region regionToSpawn = regions[WeightedRandomizer(w, regions)];

                if (regionToSpawn.name.EndsWith("Island"))
                    GenerateIsland(regionToSpawn, colormap, index, width, x, y);
                else if (regionToSpawn.name.Equals("Infested Waters"))
                    GenerateInfestedWaters(regionToSpawn, colormap, index, width, x, y);
                else
                    colormap[index] = Color.black;
            }
        }
    }

    static int WeightedRandomizer(float totalWeight, Region[] regions)
    {
        float w = totalWeight;

        for (int i = 0; i < regions.Length; i++)
        {
            if (Random.Range(0, w) < regions[i].spawnWeight)
            {
                return i;
            }
            w -= regions[i].spawnWeight;
        }

        return regions.Length - 1;
    }

    static void GenerateIsland(Region r, Color[] map, int i, int width, int x, int y)
    {
        // Stops an island from being generated next to another, and generates ocean instead.
        if (map[i - 1].Equals(r.color))
            map[i - 1] = Color.black;
        if (map[(width * (y - 1)) + x].Equals(r.color))
            map[(width * (y - 1)) + x] = Color.black;

        map[i] = r.color;
    }

    static void GenerateInfestedWaters(Region r, Color[] map, int i, int width, int x, int y)
    {
        // Center
        map[i] = r.color;

        // Left and Right
        map[i - 1] = r.color;
        map[i + 1] = r.color;

        // Top and Bottom
        map[(width * (y - 1)) + x] = r.color;
        map[(width * (y + 1)) + x] = r.color;

        // Top Left and Top Right
        map[(width * (y + 1)) + (x-1)] = r.color;
        map[(width * (y + 1)) + (x+1)] = r.color;

        // Bottom left and Bottom Right
        map[(width * (y - 1)) + (x - 1)] = r.color;
        map[(width * (y - 1)) + (x + 1)] = r.color;
    }
}