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

    [System.NonSerialized] public BlockAtlasGenerator atlasMaker;

    // Start is called before the first frame update
    void Start()
    {
        // Create atlas of all imported blocks
        atlasMaker = new BlockAtlasGenerator(blocks, 16, 8);

        // Give Chunk this texture
        GlobalData.material_default.mainTexture = atlasMaker.GetAtlasTexture();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
