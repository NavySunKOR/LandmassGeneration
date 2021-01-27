using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DrawType
{ 
    whiteBlackNoise,
    color,
    mesh
}
[System.Serializable]
public class LayerOfLand
{
    public string label;
    public LandType landType;
    public float value;
    public Color color;

}

public struct MapData
{
    public float[,] heightMap;
    public Color[] colorMap;

    public MapData(float[,] heightMap, Color[] colorMap)
    {
        this.heightMap = heightMap;
        this.colorMap = colorMap;
    }
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
    public LayerOfLand[] layerOfLands;


    public MapData GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale, octaves, persistence,lacunarity, offset);
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        Color[] colour = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int i = 0; i < layerOfLands.Length; i++)
                {
                    if (noiseMap[x, y] <= layerOfLands[i].value)
                    {
                        colour[y * width + x] = layerOfLands[i].color;
                        break;
                    }
                }
            }
        }
        return new MapData(noiseMap, colour);
    }

   public void DrawMapInEditor()
    {
        MapData mapData = GenerateMap();
        MapDisplay display = FindObjectOfType<MapDisplay>();
        if (drawType == DrawType.color)
            display.DrawColorMap(mapData.heightMap, mapData.colorMap);
        else if (drawType == DrawType.whiteBlackNoise)
            display.DrawNoiseMap(mapData.heightMap);
        else if (drawType == DrawType.mesh)
            display.DrawMeshMap(mapData.heightMap, mapData.colorMap, maxHeightPx, landCurve, levelOfDetails);
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
