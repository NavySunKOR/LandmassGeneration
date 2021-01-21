using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DrawType
{ 
    whiteBlackNoise,
    color,
    mesh
}

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

    public DrawType drawType;


    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapWidth, mapHeight, seed, noiseScale, octaves, persistence,lacunarity, offset);
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawType == DrawType.color)
            display.DrawColorMap(noiseMap);
        else if(drawType == DrawType.whiteBlackNoise)
            display.DrawNoiseMap(noiseMap);
        else if(drawType == DrawType.mesh)
            display.DrawMeshMap(noiseMap);
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
