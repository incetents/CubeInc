using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockAppendix
{
    private static Dictionary<uint, BlockData> data = new Dictionary<uint, BlockData>();

    // Special case for air
    private static BlockData air = new BlockData(0);

    public static BlockData GetData(uint id)
    {
        if (id == 0)
            return air;
        else if(data.ContainsKey(id))
            return data[id];
        else
            Debug.LogError("Getting missing BlockData, ID: " + id);

        return null;
    }
    public static void SetData(BlockData newData)
    {
        // Air is specific case
        if (newData.m_id == 0)
            air = newData;
        else
            data.Add(newData.m_id, newData);
    }
}

public class BlockData
{
    // Data
    public uint m_id;
    public bool m_air;

    public BlockData(uint _id)
    {
        m_id = _id;
        m_air = (_id == 0);
    }
}

public class Block
{
    public BlockData    m_data;
    public Vector3Int   m_localPosition;
    public Chunk        m_chunk;
    //public uint m_subId;

    // Behaviour
    public Block(uint id, Vector3Int localPosition, Chunk chunk)
    {
        m_data = BlockAppendix.GetData(id);
        m_localPosition = localPosition;
        m_chunk = chunk;
    }
}

