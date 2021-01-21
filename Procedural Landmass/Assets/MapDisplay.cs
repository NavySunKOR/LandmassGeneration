using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LandType
{ 
    water, ground, mountain
}


[System.Serializable]
public class LayerOfLand
{
    public string label;
    public LandType landType;
    public float value;
    public Color color;

}


public class MapDisplay : MonoBehaviour
{
    public Renderer textureRender;
    public LayerOfLand[] layerOfLands;

    public MeshRenderer meshRender;
    public MeshFilter meshFilter;


    public void DrawNoiseMap(float[,] noiseMap)
    {
        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        Color[] colour = new Color[width * height];
        for(int y = 0; y < height; y++)
        {
            for(int x = 0; x < width; x++)
            {
                colour[y * width + x] = Color.Lerp(Color.black, Color.white, noiseMap[x, y]);
            }
        }

        Texture2D texture = TextureGenerator.DrawTexture(colour, width, height);
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(width, 1, height);
    }

    public void DrawColorMap(float[,] noiseMap)
    {

        int width = noiseMap.GetLength(0);
        int height = noiseMap.GetLength(1);
        Color[] colour = new Color[width * height];
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                for(int i = 0; i < layerOfLands.Length; i++)
                {
                    if(noiseMap[x,y] <= layerOfLands[i].value)
                    {
                        colour[y * width + x] = layerOfLands[i].color;
                        break;
                    }
                }
            }
        }

        Texture2D texture = TextureGenerator.DrawTexture(colour, width, height);
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(width, 1, height);
    }

    public void DrawMeshMap(float[,] noiseMap)
    {
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
        Texture2D texture = TextureGenerator.DrawTexture(colour, width, height);


        Mesh mesh = MeshGenerator.CreateMesh(noiseMap);
        meshFilter.mesh = mesh;
        meshRender.sharedMaterial.SetTexture("_MainTex", texture);
    }
}
