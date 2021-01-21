using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGen : MonoBehaviour
{
    public int mapWidth;
    public int mapHeight;
    public float noiseScale;
    public int seed;
    public int octaves;
    public Vector2 offset;
    public float persistence;
    public float lacunarity;
    public bool autoUpdate;

    public bool isColor;


    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistence,lacunarity, offset);
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (isColor)
            display.DrawColorMap(noiseMap);
        else
            display.DrawNoiseMap(noiseMap);
    }

    private void OnValidate()
    {
        if(mapWidth < 1)
        {
            mapWidth = 1;
        }
        if(mapHeight < 1)
        {
            mapHeight = 1;
        }
        if(lacunarity < 1)
        {
            lacunarity = 1;
        }

        if(octaves<0)
        {
            octaves = 0;
        }
    }
}
