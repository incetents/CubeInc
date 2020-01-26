using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    public static Player player;
    public static GameObject prefab_chunk;
    public static GameObject prefab_blockOutline;
    public static GameObject prefab_line;
    public static Material material_default;
    public static Material material_wireframe;

    public static void NullCheck(Object obj)
    {
        if (obj == null)
            Debug.LogError("Missing obj");
    }
}


public class GlobalDataManager : MonoBehaviour
{
    // Global Data
    public Player globalPlayer;
    public GameObject globalPrefab_Chunk;
    public GameObject globalPrefab_BlockOutline;
    public GameObject globalPrefab_Line;
    public Material globalMaterial_Default;
    public Material globalMaterial_Wireframe;

    void Awake()
    {
        Random.InitState(0);

        // Block
        BlockDictionary.Set(new BlockInfo(1));

        GlobalData.player = globalPlayer;
        GlobalData.prefab_chunk = globalPrefab_Chunk;
        GlobalData.prefab_blockOutline = globalPrefab_BlockOutline;
        GlobalData.prefab_line = globalPrefab_Line;
        GlobalData.material_default = globalMaterial_Default;
        GlobalData.material_wireframe = globalMaterial_Wireframe;
        //  
        //  GameObject chunkObject = (GameObject)Instantiate(globalPrefab_Chunk);
        //  Chunk chunk = chunkObject.GetComponent<Chunk>();
        //  chunk.m_position = new Vector3Int(0, 0, 0);
        //  ChunkManager.add(chunk);
        //  
        //  chunk.generateTest();
        //  chunk.m_meshDirty = true;

        // Set
        //  for (int x = -1; x <= 1; x++)
        //  {
        //      for (int y = -1; y <= 1; y++)
        //      {
        //          for (int z = -1; z <= 1; z++)
        //          {
        //              GameObject chunkObject = (GameObject)Instantiate(globalPrefab_Chunk);
        //              Chunk chunk = chunkObject.GetComponent<Chunk>();
        //              chunk.m_index = new Vector3Int(x, y, z);
        //              ChunkStorage.SetChunk(chunk);
        //  
        //              chunk.GenerateTest();
        //              chunk.MakeDirty();
        //          }
        //      }
        //  }
    }

    void Update()
    {
        
    }
}
