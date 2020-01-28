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

    [System.NonSerialized] public Texture2DArray blockTextureArray;

    // Behaviour
    void Start()
    {
        List<Texture2D> blockTextures = new List<Texture2D>();

        foreach (BlockInfoImport info in blocks)
        {
            if (info == null)
            {
                Debug.LogError("BlockInfoImport has a Null value");
                continue;
            }

            Debug.Log("Import: " + info.name);

            if (info.m_texture == null)
            {
                Debug.LogError("BlockInfoImport missing Texture: " + info.name);
                continue;
            }

            // Error if duplicate ID
            if (BlockDictionary.Has(info.m_id))
            {
                Debug.LogError("BlockInfoImport duplicate ID: " + info.name + ", ID= " + info.m_id.ToString());
                continue;
            }

            Debug.Assert(info.m_texture.width == blockPixelSize,
                "Block Texture Size does not match specified size (Block Size= " + info.m_texture.width.ToString() + ")(Target Size: " + blockPixelSize.ToString()
                );

            // Add block to dictionary
            BlockInfo block = new BlockInfo(info.m_id);
            block.m_name = info.name;
            block.m_texture = info.m_texture;
            block.m_textureID = (uint)blockTextures.Count;

            // Store Texture
            blockTextures.Add(info.m_texture);

            BlockDictionary.Set(block);

        }

        // Create Texture Array
        blockTextureArray = new Texture2DArray(16, 16, blockTextures.Count, TextureFormat.RGB24, true, false);
        blockTextureArray.filterMode = FilterMode.Point;
        blockTextureArray.wrapMode = TextureWrapMode.Clamp;

        // Store Textures
        for (int i = 0; i < blockTextures.Count; i++)
        {
            blockTextureArray.SetPixels(blockTextures[i].GetPixels(0), i, 0);
        }

        //
        blockTextureArray.Apply();

        // Apply
        GlobalData.material_block.SetTexture("_TextureArray", blockTextureArray);
    }
}
