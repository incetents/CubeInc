using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockDictionary
{
    private static Dictionary<uint, BlockInfo> data = new Dictionary<uint, BlockInfo>();

    // Special case for air
    private static BlockInfo air = new BlockInfo(0);

    public static BlockInfo GetData(uint id)
    {
        if (id == 0)
            return air;
        else if(data.ContainsKey(id))
            return data[id];
        else
            Debug.LogError("Getting missing BlockData, ID: " + id);

        return null;
    }
    public static void SetData(BlockInfo newData)
    {
        // Air is specific case
        if (newData.m_id == 0)
            air = newData;
        else
            data.Add(newData.m_id, newData);
    }
}

public class BlockInfo
{
    // Data
    public uint m_id;
    public bool m_air;

    public BlockInfo(uint _id)
    {
        m_id = _id;
        m_air = (_id == 0);
    }
}

// Stores data of blocks for chunk usage
public class BlockStorage
{
    public Block[,,] data = new Block[Chunk.MaxSize.x, Chunk.MaxSize.y, Chunk.MaxSize.z];

    public BlockStorage()
    {
        // Set all to air
        for (int x = 0; x < Chunk.MaxSize.x; x++)
        {
            for (int y = 0; y < Chunk.MaxSize.y; y++)
            {
                for (int z = 0; z < Chunk.MaxSize.z; z++)
                {
                    data[x, y, z] = new Block(0, new Vector3Int(x, y, z));
                }
            }
        }
    }

    public void Set(Vector3Int index, Block block)
    {
        // Limit Checks
        if(
            index.x < 0 || index.x >= Chunk.MaxSize.x ||
            index.y < 0 || index.y >= Chunk.MaxSize.y ||
            index.z < 0 || index.z >= Chunk.MaxSize.z
            )
            return;

        data[index.x, index.y, index.z] = block;
    }
    public Block Get(Vector3Int index)
    {
        // Limit Checks
        if (
            index.x < 0 || index.x >= Chunk.MaxSize.x ||
            index.y < 0 || index.y >= Chunk.MaxSize.y ||
            index.z < 0 || index.z >= Chunk.MaxSize.z
            )
            return null;

        return data[index.x, index.y, index.z];
    }
    public bool IsAir(Vector3Int index)
    {
        return data[index.x, index.y, index.z].m_data.m_air;
    }
}

public class Block
{
    public BlockInfo    m_data;
    public Vector3Int   m_localPosition;
    //public uint m_subId;

    // Behaviour
    public Block(uint id, Vector3Int localPosition)
    {
        m_data = BlockDictionary.GetData(id);
        m_localPosition = localPosition;
    }
}

