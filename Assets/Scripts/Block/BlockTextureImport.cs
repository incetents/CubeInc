using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BlockTextureImport", menuName = "BlockTextureImport")]
public class BlockTextureImport : ScriptableObject
{
    // Base Settings
    public Texture2D m_texture;
    [System.NonSerialized] public uint m_id = 0; // Generated based on storage
    [System.NonSerialized] public bool m_failedToLoad = false;
}