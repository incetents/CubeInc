using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalData
{
    public static Player player;
    public static GameObject prefab_chunk;
    public static GameObject prefab_blockOutline;
    public static GameObject prefab_line;
    public static Material material_block;
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
    public Material globalMaterial_Block;
    public Material globalMaterial_Wireframe;

    void Awake()
    {
        // Air added manually
        BlockDictionary.Set(new BlockInfo(0));

        Random.InitState(0);

        GlobalData.player = globalPlayer;
        GlobalData.prefab_chunk = globalPrefab_Chunk;
        GlobalData.prefab_blockOutline = globalPrefab_BlockOutline;
        GlobalData.prefab_line = globalPrefab_Line;
        GlobalData.material_block = globalMaterial_Block;
        GlobalData.material_wireframe = globalMaterial_Wireframe;
    }
}
