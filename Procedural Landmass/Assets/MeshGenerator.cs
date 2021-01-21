using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{

    public static Mesh CreateMesh(float[,] pNoiseMap,float pLandMaxHeight,AnimationCurve landCurve,int levelOfDetails)
    {
        int totalGridWidth = pNoiseMap.GetLength(0);
        int totalGridHeight = pNoiseMap.GetLength(1);
        Vector3[] vertices = new Vector3[totalGridHeight * totalGridWidth];
        Vector2[] uvs = new Vector2[totalGridWidth * totalGridHeight];
        int[] triangles = new int[(totalGridWidth - 1) * (totalGridHeight - 1) * 3 * 2];
        int verticesIndex = 0;
        int triangleIndex = 0;

        float topLeftX = (totalGridWidth - 1) / -2f;
        float topLeftY = (totalGridHeight - 1) / 2f;


        int meshSimplificationIncrement = (levelOfDetails <=0)?1 : levelOfDetails * 2;
        int verticesPerLine = (totalGridWidth - 1) / meshSimplificationIncrement + 1;


        for (int x = 0; x < totalGridWidth; x += meshSimplificationIncrement)
        {
            for (int y = 0; y < totalGridHeight; y += meshSimplificationIncrement)
            {
                vertices[verticesIndex] = new Vector3( x + topLeftX, landCurve.Evaluate(pNoiseMap[x, y])* pLandMaxHeight,  y - topLeftY); // 정확하게 center에서 scaling을 하기 위함이다. 궁금하면 topLeft들을 빼고 돌려보면 된다.
                uvs[verticesIndex] = new Vector2(x / (float)totalGridWidth, y / (float)totalGridHeight); // 노멀은 0~1까지이므로

                if(x < totalGridWidth - 1 && y < totalGridHeight - 1) // x가 만약에 width -1 이 되면 이 이상으로는 triangleIndex가 out of bounds가 되므로 그 전에 끊어줘야함.
                {
                    triangles[triangleIndex] = verticesIndex;
                    triangles[triangleIndex + 1] = verticesIndex + verticesPerLine + 1; // 배열을 한줄을 끝까지 채우면 + width가 되어야 한다. lod가 적용되므로써 위에서 lod에 따라 계산한 지수를 넣어줘야한다.
                    triangles[triangleIndex + 2] = verticesIndex + verticesPerLine; // 
                    triangleIndex += 3;

                    triangles[triangleIndex] = verticesIndex + verticesPerLine + 1;  // 
                    triangles[triangleIndex + 1] = verticesIndex;
                    triangles[triangleIndex + 2] = verticesIndex + 1;
                    triangleIndex += 3;
                }
                verticesIndex++;
            }
        }


        Mesh mesh = new Mesh();

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }
}
