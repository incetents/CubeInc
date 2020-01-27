using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockDictionary
{
    public static Dictionary<uint, BlockInfo> data = new Dictionary<uint, BlockInfo>();

    public static BlockInfo Get(uint id)
    {
        if (data.ContainsKey(id))
            return data[id];

        return null;
    }
    public static void Set(BlockInfo newData)
    {
        if (data.ContainsKey(newData.m_id))
            Debug.LogError("Setting BlockData ID that already exists: " + newData.m_id);
        else
            data.Add(newData.m_id, newData);
    }
    public static bool Has(uint id)
    {
        return data.ContainsKey(id);
    }
}

[CreateAssetMenu(fileName = "New BlockInfoImport", menuName = "BlockInfoImport")]
public class BlockInfoImport : ScriptableObject
{
    // Settings
    public new string   name;
    public uint         m_id;
    public Texture2D    m_texture;
}

public class BlockManager : MonoBehaviour
{
    // Import
    public BlockInfoImport[] blocks;
    public int blockPixelSize = 16;

    [System.NonSerialized] public BlockAtlasGenerator atlasMaker;

    // Behaviour
    void Start()
    {
        // Create atlas of all imported blocks
        atlasMaker = new BlockAtlasGenerator(blocks, 16, 8);

        // Give Chunk this texture
        GlobalData.material_block.mainTexture = atlasMaker.GetAtlasTexture();
    }
}
