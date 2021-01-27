using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LandType
{ 
    water, ground, mountain
}




public class MapDisplay : MonoBehaviour
{
    public Renderer textureRender;
    

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

    public void DrawColorMap(float[,] noiseMap,Color[] colorMap)
    {
        Texture2D texture = TextureGenerator.DrawTexture(colorMap, noiseMap.GetLength(0) - 1, noiseMap.GetLength(1) - 1);
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(noiseMap.GetLength(0) - 1, 1, noiseMap.GetLength(1) - 1);
    }

    public void DrawMeshMap(float[,] noiseMap,Color[] colorMap,float pMaxHeight,AnimationCurve landCurve,int levelOfDetails)
    {
        Texture2D texture = TextureGenerator.DrawTexture(colorMap, noiseMap.GetLength(0), noiseMap.GetLength(1));
        MeshData mesh = MeshGenerator.CreateMesh(noiseMap,pMaxHeight, landCurve, levelOfDetails);
        meshFilter.mesh = mesh.CreateMesh();
        meshRender.sharedMaterial.SetTexture("_MainTex", texture);
    }
}
