﻿using System.Collections;
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
                    data[x, y, z] = Block.CreateLocalBlock(0, 0, new Vector3Int(x, y, z));
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

    public void Set(Block block)
    {
        // Limit Checks
        if(IsIndexTooLarge(block.m_localPosition))
            return;

        data[block.m_localPosition.x, block.m_localPosition.y, block.m_localPosition.z] = block;
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
        if (data[index.x, index.y, index.z].m_data == null)
            return true;

        return data[index.x, index.y, index.z].m_data.m_air;
    }
}

public enum BlockFace
{
    TOP = 0,
    LEFT = 1,
    RIGHT = 2,
    FRONT = 3,
    BACK = 4,
    BOTTOM = 5
}

public class BlockInfo
{
    // Data
    public uint m_id;
    public bool m_air;
    public string m_name;
    public List<uint> m_textureIDs = new List<uint>();

    public BlockInfo(uint _id)
    {
        m_id = _id;
        m_air = (_id == 0);
    }

    public void AddTextureID(BlockTextureImport info, uint fallbackID)
    {
        if (info == null || info.m_failedToLoad)
        {
            m_textureIDs.Add(fallbackID);
            return;
        }
       
        m_textureIDs.Add(info.m_id);
    }

    public uint GetTextureID(BlockFace face)
    {
        if (m_textureIDs.Count == 0)
            return 0;
        else if (m_textureIDs.Count == 1)
            return m_textureIDs[0];

        return m_textureIDs[(int)face];
    }
}

public class Block
{
    // Special Creation
    private Block() { }
    public static Block CreateLocalBlock(uint id, uint subID, Vector3Int localPosition)
    {
        Block newBlock = new Block();
        newBlock.m_data = BlockDictionary.Get(id);
        if (newBlock.m_data == null)
            Debug.LogError("err");

        newBlock.m_subId = subID;
        newBlock.m_localPosition = localPosition;
        return newBlock;
    }
    public static Block CreateWorldBlock(uint id, uint subID, Vector3Int worldPosition)
    {
        Block newBlock = new Block();
        newBlock.m_data = BlockDictionary.Get(id);
        if (newBlock.m_data == null)
            Debug.LogError("err");

        newBlock.m_subId = subID;
        newBlock.m_localPosition = Chunk.ConvertToBlockIndex(worldPosition);
        return newBlock;
    }

    public BlockInfo    m_data;
    public Vector3Int   m_localPosition;
    public uint         m_subId;

    // Behaviour
    //  public Block(uint id, uint subID, Vector3Int localPosition)
    //  {
    //      m_data = BlockDictionary.Get(id);
    //      if (m_data == null)
    //          Debug.LogError("err");
    //  
    //      m_subId = subID;
    //      m_localPosition = localPosition;
    //  }
    //  public Block(uint id, uint subID, Vector3 worldPosition)
    //  {
    //      m_data = BlockDictionary.Get(id);
    //      if (m_data == null)
    //          Debug.LogError("err");
    //  
    //      m_subId = subID;
    //      m_localPosition = Chunk.ConvertToBlockIndex(worldPosition);
    //  }
}

