using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockDictionary
{
    public static List<BlockData> m_blockData = new List<BlockData>();
}

public class BlockData
{
    // Data
    public uint m_id;
    public uint m_subId;

    BlockData(uint _id, uint _subId)
    {
        m_id = _id;
        m_subId = _subId;
    }
}

public class Block
{
    public BlockData m_data;
    public Vector3Int m_localPosition;

    // Behaviour
    public Block(Vector3Int localPosition)
    {
        m_localPosition = localPosition;
    }
}

