using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InfiniteTerrain : MonoBehaviour
{
    public const float maxViewDistance = 450f;
    public Transform viewer;
    public static Vector2 viewerPosition;
    static MapGen mapGen;
    
    int chunkSize;
    int chunksVisibleInViewDist;
    Dictionary<Vector2, TerrainChunk> terrainChunkDic = new Dictionary<Vector2, TerrainChunk>();
    List<TerrainChunk> terrainChunksLastVisible = new List<TerrainChunk>();



    public class TerrainChunk
    {
        private GameObject meshObject;
        private Vector2 pos;
        private Bounds bounds;

        MapData mapData;
        MeshRenderer meshRenderer;
        MeshFilter meshFilter;

        public TerrainChunk(Vector2 coord, int size)
        {
            pos = coord * size;
            bounds = new Bounds(pos, Vector2.one * size);
            Vector3 posV3 = new Vector3(pos.x, 0, pos.y);

            meshObject = new GameObject("TerrainChunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshObject.transform.position = posV3;
            meshObject.transform.localScale = Vector3.one * size / 10f;
            SetVisible(false);
            mapGen.RequestMapData(OnMapDataReceived);
        }

        void OnMapDataReceived(MapData mapData)
        {
            print("MAP DATA");
        }

        void OnMeshDataReceived(MeshData meshData)
        {
            meshFilter.mesh = meshData.CreateMesh();
        }

        public void Update()
        {
            float viewerDistFromNearestEdge = Mathf.Sqrt(bounds.SqrDistance(viewerPosition));
            bool visible = viewerDistFromNearestEdge <= maxViewDistance;
            SetVisible(visible);
        }
        public void SetVisible(bool isVisible)
        {
            meshObject.SetActive(isVisible);
        }

        public bool IsVisible()
        {
            return meshObject.activeSelf;
        }
    }

    private void Start()
    {
        chunkSize = MapGen.mapChunkSize - 1;
        mapGen = FindObjectOfType<MapGen>();
        chunksVisibleInViewDist = Mathf.RoundToInt(maxViewDistance / chunkSize);
    }

    private void Update()
    {
        viewerPosition = new Vector2(viewer.position.x, viewer.position.z);
        UpdateVisibleChunks();
    }

    private void UpdateVisibleChunks()
    {
        for (int i = 0; i < terrainChunksLastVisible.Count; i++)
        {
            terrainChunksLastVisible[i].SetVisible(false);
        }
        terrainChunksLastVisible.Clear();

        int currentChunkCoordX = Mathf.RoundToInt(viewerPosition.x / chunkSize);
        int currentChunkCoordY = Mathf.RoundToInt(viewerPosition.y / chunkSize);

        for(int yOffset = -chunksVisibleInViewDist; yOffset <= chunksVisibleInViewDist; yOffset++ )
        {
            for(int xOffset= -chunksVisibleInViewDist; xOffset <= chunksVisibleInViewDist; xOffset++)
            {
                Vector2 viewedChunkCoord = new Vector2(currentChunkCoordX + xOffset, currentChunkCoordY + yOffset);

                if(!terrainChunkDic.ContainsKey(viewedChunkCoord))
                {
                    terrainChunkDic.Add(viewedChunkCoord, new TerrainChunk(viewedChunkCoord,chunkSize));
                }
                else
                {
                    terrainChunkDic[viewedChunkCoord].Update();
                    if(terrainChunkDic[viewedChunkCoord].IsVisible())
                    {
                        terrainChunksLastVisible.Add(terrainChunkDic[viewedChunkCoord]);
                    }
                }
            }
        }
    }

}
