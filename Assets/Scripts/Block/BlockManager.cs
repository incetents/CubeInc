using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockDictionary
{
    public static Dictionary<uint, Texture2D> textures = new Dictionary<uint, Texture2D>();

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

public class BlockManager : MonoBehaviour
{
    // Import
    public int blockPixelSize = 16;

    [System.NonSerialized] public static Texture2DArray blockTextureArray = null;

    // Behaviour
    private void Awake()
    {
        // Air added manually
        BlockInfo airBlock = new BlockInfo(0);
        airBlock.m_name = "air";
        BlockDictionary.Set(airBlock);

        // Debug Texture
        //  Texture2D perlinTest = new Texture2D(100, 100);
        //  
        //  for(int x = 0; x < 100; x++)
        //  {
        //      for (int y = 0; y < 100; y++)
        //      {
        //          float perlin = Mathf.PerlinNoise((float)x / 10.0f, (float)y / 10.0f);
        //          Debug.Log(perlin);
        //          perlinTest.SetPixel(x, y, new Color(perlin, perlin, perlin));
        //      }
        //  }
        //  
        //  perlinTest.Apply();
        //  perlinTest.filterMode = FilterMode.Point;
        //  perlinTest.wrapMode = TextureWrapMode.Clamp;
        //  
        //  GlobalData.material_debug.SetTexture("_MainTex", perlinTest);

        // Get all block data
        BlockInfoImport[] importBlockInfo = Resources.LoadAll<BlockInfoImport>("Blocks");
        BlockTextureImport[] importBlockTextures = Resources.LoadAll< BlockTextureImport>("BlockTextures");

        // Import Textures
        List<Texture2D> blockTextures = new List<Texture2D>();

        foreach (BlockTextureImport info in importBlockTextures)
        {
            if (info == null)
            {
                Debug.LogError("BlockInfoImport has a Null value");
                continue;
            }

            if (info.m_texture == null)
            {
                Debug.LogError("BlockInfoImport missing Base Texture: " + info.name);
                info.m_failedToLoad = true;
                continue;
            }

            // Make sure all textures have correct size
            if(info.m_texture.width != blockPixelSize)
            {
                Debug.LogError("BlockInfoImport Size does not match specified size, name = " + info.name);
                info.m_failedToLoad = true;
                continue;
            }

            Debug.Log("Import: " + info.name);

            info.m_id = (uint)blockTextures.Count;
            blockTextures.Add(info.m_texture);

        }

        // Finish 2D Texture Array
        if (blockTextures.Count > 0)
        {
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

        // Import all Blocks
        foreach (BlockInfoImport info in importBlockInfo)
        {
            // Error if duplicate ID
            if (BlockDictionary.Has(info.m_id))
            {
                Debug.LogError("BlockInfoImport Duplicate Info:\nName: " + info.name + ", ID= " + info.m_id.ToString());
                continue;
            }

            // Add block to dictionary
            BlockInfo block = new BlockInfo(info.m_id);
            block.m_name = info.name;

            bool hasBaseTexture = !info.m_baseTextureID.m_failedToLoad;
            uint fallbackID = hasBaseTexture ? info.m_baseTextureID.m_id : 0;

            // Add 1 global Texture
            if (!info.m_textureOverride)
            {
                // Base Texture
                block.AddTextureID(info.m_baseTextureID, fallbackID);
            }
            // Add custom texture slots
            else
            {
                block.AddTextureID(info.m_texture_top, fallbackID);
                block.AddTextureID(info.m_texture_left, fallbackID);
                block.AddTextureID(info.m_texture_right, fallbackID);
                block.AddTextureID(info.m_texture_front, fallbackID);
                block.AddTextureID(info.m_texture_back, fallbackID);
                block.AddTextureID(info.m_texture_bottom, fallbackID);
                //
            }

            BlockDictionary.Set(block);
        }

        Debug.Log("BLOCK TEXTURES/DATA IMPORT COMPLETE");
    }
}
