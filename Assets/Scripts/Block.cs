using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public static bool IsIndexTooLarge(Vector3Int index)
    {
        return
            (index.x < 0 || index.x >= Chunk.MaxSize.x ||
            index.y < 0 || index.y >= Chunk.MaxSize.y ||
            index.z < 0 || index.z >= Chunk.MaxSize.z
            );
    }

    public void Set(Vector3Int index, Block block)
    {
        // Limit Checks
        if(IsIndexTooLarge(index))
            return;

        data[index.x, index.y, index.z] = block;
    }
    public Block Get(Vector3Int index)
    {
        // Limit Checks
        if (IsIndexTooLarge(index))
            return null;

        return data[index.x, index.y, index.z];
    }
    public bool IsAir(Vector3Int index)
    {
        return data[index.x, index.y, index.z].m_data.m_air;
    }
}

public class BlockInfo
{
    // Data
    public uint m_id;
    public bool m_air;
    public string m_name;
    public Texture2D m_texture;
    public Vector4 m_atlasLocation; // u1,u2,v1,v2 on textureatlas

    public BlockInfo(uint _id)
    {
        m_id = _id;
        m_air = (_id == 0);
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
        m_data = BlockDictionary.Get(id);
        m_localPosition = localPosition;
    }
}

