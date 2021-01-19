using UnityEngine;
using System.Collections;

public static class Noise{

    public static float[,] GenerateNoiseMap(int mapWidth,int mapHeight,int seed,float scale,int octaves,float persistence, float lacunarity,Vector2 offset){
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];
        for(int i = 0; i < octaveOffsets.Length;i++)
        {
            float offsetX = prng.Next(-10000, 10000) + offset.x;
            float offsetY = prng.Next(-10000, 10000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        float minNoiseHeight = float.MinValue;
        float maxNoiseHeight = float.MaxValue;

        float halfWidth = mapWidth / 2f;
        float halfHeight = mapHeight / 2f;

        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x<mapWidth; x++){

                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves;i++){
                    float sampleX = (x- halfWidth) / scale * frequency + octaveOffsets[i].x; //밸류가 작을수록 느슨해지고 밸류가 클수록 커진다. 여기서 주기를 곱해지면 숫자가 점점 커지므로 촘촘해진다.
                    float sampleY = (y-halfHeight) / scale * frequency + octaveOffsets[i].y;// 스케일이 커지면  공간이 느슨해진다. 

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) *2 - 1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistence; // 숫자 2 이상부터 유효
                    frequency *= lacunarity; // 숫자 2이상부터 유효
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                }
                else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        for(int y = 0; y < mapHeight; y++){
            for(int x = 0; x < mapWidth; x++){
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]); //정규화하기 위하여 사용됨 - 0.1,0.2,0.3,0.4 이렇게 있으면 0.1 0.4 이렇게 두개만 나오게 할 수 있게 한다는 뜻
            }
        }

        return noiseMap;
    }
}
