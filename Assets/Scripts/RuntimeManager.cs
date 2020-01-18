using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    public static GameObject prefab_chunk;
    public static Material material_default;

    public static void NullCheck(Object obj)
    {
        if (obj == null)
            Debug.LogError("Missing obj");
    }
}


public class RuntimeManager : MonoBehaviour
{
    // Global Data
    public GameObject globalPrefab_Chunk;
    public Material globalMaterial_Default;

    private Dictionary<int, Dictionary<int, Dictionary<int, Chunk>>> m_chunks;

    void Awake()
    {
        GlobalData.prefab_chunk = globalPrefab_Chunk;
        GlobalData.material_default = globalMaterial_Default;
    }

    void Start()
    {
        Random.InitState(0);

        // Set
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    GameObject chunkObject = (GameObject)Instantiate(globalPrefab_Chunk);
                    Chunk chunk = chunkObject.GetComponent<Chunk>();
                    chunk.m_position = new Vector3Int(x, y, z);
                    ChunkManager.add(chunk);

                    chunk.generateTest();
                }
            }
        }
        // Build
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    Chunk chunk = ChunkManager.getChunk(new Vector3Int(x, y, z));
                    chunk.buildMesh();
                }
            }
        }
    }

    void Update()
    {
        
    }
}
