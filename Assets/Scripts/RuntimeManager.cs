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
        GameObject chunkObject = (GameObject)Instantiate(globalPrefab_Chunk);
        Chunk chunk = chunkObject.GetComponent<Chunk>();
        chunk.m_position = new Vector3Int(0, 0, 0);

        chunk.generateTest1();

    }

    void Update()
    {
        
    }
}
