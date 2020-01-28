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

[CreateAssetMenu(fileName = "New BlockInfoImport", menuName = "BlockInfoImport")]
public class BlockInfoImport : ScriptableObject
{
    // Base Settings
    [Header("Base")]
    public new string   name;
    public uint         m_id;

    [Header("Base Texture")]
    public BlockTextureImport m_baseTextureID;
    // Special Overrides
    [Header("Texture Override")]
    public bool m_textureOverride;
    public BlockTextureImport m_texture_top    = null;
    public BlockTextureImport m_texture_left   = null;
    public BlockTextureImport m_texture_right  = null;
    public BlockTextureImport m_texture_front  = null;
    public BlockTextureImport m_texture_back   = null;
    public BlockTextureImport m_texture_bottom = null;
}

[CreateAssetMenu(fileName = "New BlockTextureImport", menuName = "BlockTextureImport")]
public class BlockTextureImport : ScriptableObject
{
    // Base Settings
    [Header("Base")]
    public new string   name;
    public Texture2D    m_texture;
    [System.NonSerialized ] public uint m_id = 0; // Generated based on storage
    [System.NonSerialized] public bool m_failedToLoad = false;
}

public class BlockManager : MonoBehaviour
{
    // Import
    public BlockInfoImport[] blockInfo;
    public BlockTextureImport[] blockTextures;
    public int blockPixelSize = 16;

    [System.NonSerialized] public Texture2DArray blockTextureArray;

    // Behaviour
    private void Awake()
    {
        // Air added manually
        BlockDictionary.Set(new BlockInfo(0));
    }

    void Start()
    {
        // Import all textures
        //BlockTextureImport[] test = FindObjectsOfType<BlockTextureImport>();
        //Debug.Log(test.Length);

        List<Texture2D> textureList = new List<Texture2D>();

        foreach (BlockTextureImport info in blockTextures)
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

            info.m_id = (uint)textureList.Count;
            textureList.Add(info.m_texture);

        }

        // Finish 2D Texture Array
        if (textureList.Count > 0)
        {

            // Create Texture Array
            blockTextureArray = new Texture2DArray(16, 16, textureList.Count, TextureFormat.RGB24, true, false);
            blockTextureArray.filterMode = FilterMode.Point;
            blockTextureArray.wrapMode = TextureWrapMode.Clamp;

            // Store Textures
            for (int i = 0; i < textureList.Count; i++)
            {
                blockTextureArray.SetPixels(textureList[i].GetPixels(0), i, 0);
            }
            //
            blockTextureArray.Apply();

            // Apply
            GlobalData.material_block.SetTexture("_TextureArray", blockTextureArray);
        }

        // Import all Blocks
        foreach (BlockInfoImport info in blockInfo)
        {
            // Error if duplicate ID
            if (BlockDictionary.Has(info.m_id))
            {
                Debug.LogError("BlockInfoImport duplicate ID: " + info.name + ", ID= " + info.m_id.ToString());
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
                Debug.Log("fallback: " + fallbackID);
                //
                foreach (uint id in block.m_textureIDs)
                    Debug.Log(id);
            }

            BlockDictionary.Set(block);
        }
    }
}
