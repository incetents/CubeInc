using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BlockInfoImport", menuName = "BlockInfoImport")]
public class BlockInfoImport : ScriptableObject
{
    // Base Settings
    [Header("Base")]
    public new string name;
    public uint m_id;

    public BlockTextureImport m_baseTextureID;
    // Special Overrides
    public bool m_textureOverride;
    public BlockTextureImport m_texture_top = null;
    public BlockTextureImport m_texture_left = null;
    public BlockTextureImport m_texture_right = null;
    public BlockTextureImport m_texture_front = null;
    public BlockTextureImport m_texture_back = null;
    public BlockTextureImport m_texture_bottom = null;
}