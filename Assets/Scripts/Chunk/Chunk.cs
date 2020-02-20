using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChunkStorage
{
    public static Dictionary<int, Dictionary<int, Dictionary<int, Chunk>>> data = new Dictionary<int, Dictionary<int, Dictionary<int, Chunk>>>(); //
    public static HashSet<Chunk> allData = new HashSet<Chunk>();

    public static void SetChunk(Chunk chunk)
    {
        Vector3Int index = chunk.m_index;

        // Check if X section is missing
        if (!data.ContainsKey(index.x))
            data.Add(index.x, new Dictionary<int, Dictionary<int, Chunk>>());

        // Check if Y section is missing
        if (!data[index.x].ContainsKey(index.y))
            data[index.x].Add(index.y, new Dictionary<int, Chunk>());

        // Edit at Z section
        data[index.x][index.y][index.z] = chunk;
    }
    public static void RemoveChunk(Chunk chunk)
    {
        Vector3Int index = chunk.m_index;

        // Check if X section is missing
        if (!data.ContainsKey(index.x))
            data.Add(index.x, new Dictionary<int, Dictionary<int, Chunk>>());

        // Check if Y section is missing
        if (!data[index.x].ContainsKey(index.y))
            data[index.x].Add(index.y, new Dictionary<int, Chunk>());

        // Edit at Z section
        data[index.x][index.y][index.z] = null;
    }
    public static Chunk GetChunkFromIndex(Vector3Int index)
    {
        if (IsEmpty(index))
            return null;
    
        return data[index.x][index.y][index.z];
    }
    public static Chunk GetChunkFromPosition(Vector3Int position)
    {
        Vector3Int index = Chunk.ConvertToChunkIndex(position);

        if (IsEmpty(index))
            return null;

        return data[index.x][index.y][index.z];
    }
   
    public static bool IsEmpty(Vector3Int index)
    {
        // Check if X section is missing
        if (!data.ContainsKey(index.x))
            return true;

        // Check if Y section is missing
        if (!data[index.x].ContainsKey(index.y))
            return true;

        // Check if Z section is missing
        if (!data[index.x][index.y].ContainsKey(index.z))
            return true;

        return false;
    }

    // Modify an existing block from world space position (no update)
    public static bool SetBlock(Vector3Int position, Block block)
    {
        Chunk chunk = GetChunkFromPosition(position);
        if (chunk == null)
            return false;

        // Check if block exists in chunk
        chunk.m_blocks.Set(block);
        return true;
    }

    // Acquire block from world space position
    public static Block GetBlock(Vector3Int position)
    {
        // Check if chunk exists
        Chunk chunk = GetChunkFromPosition(position);
        if (chunk == null)
            return null;

        // Check if block exists in chunk
        Vector3Int blockIndex = Chunk.ConvertToBlockIndex(position);
        return chunk.m_blocks.Get(blockIndex);
    }
    // Get all adjacent blocks from position (includes position block)
    public static Block[] GetBlocksNearPosition(Vector3Int position)
    {
        Block[] blocks =
        {
            GetBlock(position),
            GetBlock(position + Vector3Int.right),
            GetBlock(position + Vector3Int.left),
            GetBlock(position + Vector3Int.up),
            GetBlock(position + Vector3Int.down),
            GetBlock(position + new Vector3Int(0, 0, +1)),
            GetBlock(position + new Vector3Int(0, 0, -1))
        };

        return blocks;
    }
    // Check all adjacent spots and return all unique Chunks
    public static HashSet<Chunk> GetChunksNearPosition(Vector3Int position)
    {
        Chunk[] chunks =
        {
            GetChunkFromPosition(position),
            GetChunkFromPosition(position + Vector3Int.right),
            GetChunkFromPosition(position + Vector3Int.left),
            GetChunkFromPosition(position + Vector3Int.up),
            GetChunkFromPosition(position + Vector3Int.down),
            GetChunkFromPosition(position + new Vector3Int(0, 0, +1)),
            GetChunkFromPosition(position + new Vector3Int(0, 0, -1))
        };

        return new HashSet<Chunk>(chunks);
    }
    // Get all chunks around given chunk
    public static Chunk[] GetNeighborChunks(Chunk chunk)
    {
        Chunk[] chunks =
        {
            GetChunkFromIndex(chunk.m_index + Vector3Int.right),
            GetChunkFromIndex(chunk.m_index + Vector3Int.left),
            GetChunkFromIndex(chunk.m_index + Vector3Int.up),
            GetChunkFromIndex(chunk.m_index + Vector3Int.down),
            GetChunkFromIndex(chunk.m_index + new Vector3Int(0,0,1)),
            GetChunkFromIndex(chunk.m_index + new Vector3Int(0,0,-1))
        };

        return chunks;
    }

    // Updates block and adjacent blocks (and their chunks)
    public static void MakePositionDirty(Vector3Int position)
    {
        // Make all chunks near block dirty
        HashSet<Chunk> chunks = GetChunksNearPosition(position);
        foreach(Chunk chunk in chunks)
        {
            if(chunk != null)
                chunk.MakeDirty();
        }
    }
}

public class Chunk
{
    // Constants
    public static readonly Vector3Int MaxSize = new Vector3Int(16, 16, 16);

    // Object
    private ChunkManager m_chunkManager;
    private Player m_player;
    private ChunkComponent m_component;

    // Construction
    private ChunkBuilder m_chunkBuilder = null;

    // Data
    [System.NonSerialized] public Vector3Int  m_index;
    [System.NonSerialized] public Vector3     m_center; // Center of chunk position (world space)
    [System.NonSerialized] public Vector3[]   m_bottomExtents;
    [System.NonSerialized] public Vector3[]   m_topExtents;

    [System.NonSerialized] public BlockStorage m_blocks;
    [System.NonSerialized] private bool        m_meshDirty = false;

    // Utility //

    // Index Conversion
    public static Vector3Int ConvertToChunkIndex(Vector3Int position)
    {
        Vector3Int index = new Vector3Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z));

        // Account for negative indices
        int chunkX = (index.x < 0) ? Mathf.FloorToInt(index.x) - Chunk.MaxSize.x + 1 : Mathf.FloorToInt(index.x);
        int chunkY = (index.y < 0) ? Mathf.FloorToInt(index.y) - Chunk.MaxSize.y + 1 : Mathf.FloorToInt(index.y);
        int chunkZ = (index.z < 0) ? Mathf.FloorToInt(index.z) - Chunk.MaxSize.z + 1 : Mathf.FloorToInt(index.z);

        // Result
        return new Vector3Int(
            chunkX / Chunk.MaxSize.x,
            chunkY / Chunk.MaxSize.y,
            chunkZ / Chunk.MaxSize.z
            );
    }
    // Index Conversion
    public static Vector3Int ConvertToBlockIndex(Vector3 position)
    {
        Vector3Int index = new Vector3Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z));

        int modLocalX = (index.x < 0) ? (Chunk.MaxSize.x - (Mathf.Abs(index.x) % Chunk.MaxSize.x)) : index.x;
        int modLocalY = (index.y < 0) ? (Chunk.MaxSize.y - (Mathf.Abs(index.y) % Chunk.MaxSize.y)) : index.y;
        int modLocalZ = (index.z < 0) ? (Chunk.MaxSize.z - (Mathf.Abs(index.z) % Chunk.MaxSize.z)) : index.z;

        return new Vector3Int(
            modLocalX % Chunk.MaxSize.x,
            modLocalY % Chunk.MaxSize.y,
            modLocalZ % Chunk.MaxSize.z
            );
    }
    // Convert Local To World
    public Vector3Int ConvertFromLocalToWorld(Vector3Int localPosition)
    {
        return localPosition + (m_index * Chunk.MaxSize);
    }

    public Vector3 GetCenter()
    {
        return m_center;
    }
    public Vector3Int GetIndex()
    {
        return m_index;
    }
    public void SetBlock(Block block)
    {
        m_blocks.Set(block);
    }
    public void MakeDirty()
    {
        m_meshDirty = true;
    }
    public void MakeClean()
    {
        m_meshDirty = false;
    }
    public bool IsDirty()
    {
        return m_meshDirty;
    }
    public bool BeginMeshConstruction()
    {
        // Check if object is destroyed
        if (m_component == null)
            return false;

        // Init Chunk Builder
        if (m_chunkBuilder == null)
        {
            m_chunkBuilder = new ChunkBuilder(m_index);
            return true;
        }

        // Check if failed
        if(m_chunkBuilder.hasFailed())
        {
            m_chunkBuilder.Restart();
        }

        return false;
    }
    public bool IsMeshConstructed()
    {
        // Check if object is destroyed
        if (m_component == null)
            return false;

        if (m_chunkBuilder == null)
            return false;

        return m_chunkBuilder.isComplete();
    }
    public void EndMeshConstruction()
    {
        // Check if object is destroyed
        if (m_component == null)
            return;

        if (m_chunkBuilder != null)
        {
            m_component.UpdateMesh(m_chunkBuilder);
            m_chunkBuilder = null;
        }
    }

    public void GenerateTerrain()
    {
        for (int x = 0; x < MaxSize.x; x++)
        {
            for (int y = 0; y < MaxSize.z; y++)
            {
                for (int z = 0; z < MaxSize.z; z++)
                {
                    Vector3 WorldPos = ConvertFromLocalToWorld(new Vector3Int(x, y, z));
                    Vector3 WorldPosUp = new Vector3(WorldPos.x, WorldPos.y + 1, WorldPos.z);
                    WorldPos /= m_player.PerlinScale;
                    WorldPosUp /= m_player.PerlinScale;

                    float perlin = Utility.Perlin3D(WorldPos);
                    if (perlin > m_player.PerlinAlpha)
                    {
                        float perlinUp = Utility.Perlin3D(WorldPosUp);

                        if (perlinUp > m_player.PerlinAlpha)
                            SetBlock(Block.CreateLocalBlock(1, 0, new Vector3Int(x, y, z)));
                        else
                            SetBlock(Block.CreateLocalBlock(2, 0, new Vector3Int(x, y, z)));

                    }
                    else
                    {
                        SetBlock(Block.CreateLocalBlock(0, 0, new Vector3Int(x, y, z)));
                    }
                }
            }
        }
    }
    public void GenerateTest()
    {
        for (int x = 0; x < MaxSize.x; x++)
        {
            for (int z = 0; z < MaxSize.z; z++)
            {
                int randomHeight = Random.Range(1, 10);
                //for (int y = 0; y < randomHeight; y++)
                //for (int y = 0; y < MaxSize.y; y++)
                for (int y = 0; y < x + 1; y++)
                {
                    //uint id = (uint)((x % 4) + 1);
                    uint id = 1;
                    Block block = Block.CreateLocalBlock(id, 0, new Vector3Int(x, y, z));

                    SetBlock(block);
                }
            }
        }
    }
    

    // Behaviour
    public Chunk(ChunkComponent component, Vector3Int index)
    {
        // Remember master list of chunks used
        ChunkStorage.allData.Add(this);

        // References stored
        component.SetChunk(this);
        m_component = component;

        // Data
        m_index = index;
        m_blocks = new BlockStorage(); //
        m_player = GlobalData.player;

        m_center = new Vector3(
            (float)m_index.x * (float)MaxSize.x + (float)MaxSize.x * 0.5f,
            (float)m_index.y * (float)MaxSize.y + (float)MaxSize.y * 0.5f,
            (float)m_index.z * (float)MaxSize.z + (float)MaxSize.z * 0.5f
            );
        m_bottomExtents = new Vector3[4]
        {
            m_index * MaxSize,
            m_index * MaxSize + new Vector3(MaxSize.x, 0, 0),
            m_index * MaxSize + new Vector3(MaxSize.x, 0, MaxSize.z),
            m_index * MaxSize + new Vector3(0, 0, MaxSize.z)
        };
        m_topExtents = new Vector3[4]
        {
            m_index * MaxSize + new Vector3(0, MaxSize.y, 0),
            m_index * MaxSize + new Vector3(MaxSize.x, MaxSize.y, 0),
            m_index * MaxSize + new Vector3(MaxSize.x, MaxSize.y, MaxSize.z),
            m_index * MaxSize + new Vector3(0, MaxSize.y, MaxSize.z)
        };

    }
    ~Chunk()
    {
        ChunkStorage.allData.Remove(this);
    }

}
