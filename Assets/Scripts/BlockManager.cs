using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Start is called before the first frame update
    void Start()
    {
        // How many blocks on each side
        int atlasBlockLength = Mathf.CeilToInt(Mathf.Sqrt(blocks.Length));

        // Block Size
        GlobalData.texture_blockPixelSize = blockPixelSize;
        // Texture Size
        GlobalData.texture_blockAtlasSize = blockPixelSize * atlasBlockLength;
        // Texel Size
        float texelSize = 1.0f / (float)GlobalData.texture_blockAtlasSize;

        // Create Texture Atlas
        GlobalData.texture_blockAtlas = new Texture2D(
            GlobalData.texture_blockAtlasSize,
            GlobalData.texture_blockAtlasSize,
            TextureFormat.RGB24,
            true
            );
        //
        GlobalData.texture_blockAtlas.filterMode = FilterMode.Point;
        GlobalData.texture_blockAtlas.wrapMode = TextureWrapMode.Clamp;

        int storedBlocks = 0;
        // Copy Textures onto atlas
        foreach (BlockInfoImport info in blocks)
        {
            if (info.m_texture == null)
            {
                Debug.LogError("BlockInfoImport missing Texture: " + info.name);
                continue;
            }

            // Location on atlas
            int x = (storedBlocks % atlasBlockLength);
            int y = Mathf.FloorToInt(storedBlocks / atlasBlockLength);
             
            GlobalData.texture_blockAtlas.SetPixels(
                x * blockPixelSize, y * blockPixelSize,
                blockPixelSize, blockPixelSize,
                info.m_texture.GetPixels()
                );

            // Add block to dictionary
            BlockInfo block = new BlockInfo(info.m_id);
            block.m_name = info.name;
            block.m_texture = info.m_texture;
            block.m_atlasLocation = new Vector4(
                ((float)x / (float)atlasBlockLength),
                ((float)(x+1) / (float)atlasBlockLength),
                ((float)y / (float)atlasBlockLength),
                ((float)(y+1) / (float)atlasBlockLength)
                );

            Debug.Assert(
                blockPixelSize == info.m_texture.width && blockPixelSize == info.m_texture.height,
                "Imported Block Texture does not match blockPixelSize setting"
                );

            BlockDictionary.Set(block);
            //

            // Flag
            storedBlocks++;
        }

        GlobalData.texture_blockAtlas.Apply(true);

        // Give Chunk this texture
        GlobalData.material_default.mainTexture = GlobalData.texture_blockAtlas;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
