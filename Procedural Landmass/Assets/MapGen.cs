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
    public const int mapChunkSize = 241;
    public int levelOfDetails;
    public float noiseScale;
    public int seed;
    public int octaves;
    public Vector2 offset;
    public float persistence;
    public float lacunarity;
    public float maxHeightPx;
    public bool autoUpdate;

    public DrawType drawType;
    public AnimationCurve landCurve;


    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistence,lacunarity, offset);
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawType == DrawType.color)
            display.DrawColorMap(noiseMap);
        else if(drawType == DrawType.whiteBlackNoise)
            display.DrawNoiseMap(noiseMap);
        else if(drawType == DrawType.mesh)
            display.DrawMeshMap(noiseMap,maxHeightPx,landCurve, levelOfDetails);
    }

    private void OnValidate()
    {
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
