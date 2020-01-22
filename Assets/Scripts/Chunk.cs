using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChunkStorage
{
    public static Dictionary<int, Dictionary<int, Dictionary<int, Chunk>>> data = new Dictionary<int, Dictionary<int, Dictionary<int, Chunk>>>(); //

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
        data[chunk.m_index.x][chunk.m_index.y][chunk.m_index.z] = chunk;
    }
    public static Chunk GetChunk(Vector3Int index)
    {
        if (IsEmpty(index))
            return null;

        return data[index.x][index.y][index.z];
    }
    public static Chunk GetChunk(Vector3 position)
    {
        Vector3Int index = ConvertToChunkIndex(position);

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

    // Index Conversion
    public static Vector3Int ConvertToChunkIndex(Vector3 position)
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

    // Modify an existing block from world space position (no update)
    public static bool SetBlock(Vector3 position, Block block)
    {
        Vector3Int chunkIndex = ChunkStorage.ConvertToChunkIndex(position);
        Chunk chunk = GetChunk(chunkIndex);
        if (chunk == null)
            return false;

        // Check if block exists in chunk
        Vector3Int blockIndex = ConvertToBlockIndex(position);
        chunk.m_blocks.Set(blockIndex, block);
        return true;
    }

    // Acquire block from world space position
    public static Block GetBlock(Vector3 position)
    {
        // Check if chunk exists
        Vector3Int chunkIndex = ConvertToChunkIndex(position);
        Chunk chunk = GetChunk(chunkIndex);
        if (chunk == null)
            return null;

        // Check if block exists in chunk
        Vector3Int blockIndex = ConvertToBlockIndex(position);
        return chunk.m_blocks.Get(blockIndex);
    }
    // Get all adjacent blocks from position (includes position block)
    public static Block[] GetNearbyBlocks(Vector3 position)
    {
        Block[] blocks =
        {
            GetBlock(position),
            GetBlock(position + Vector3.right),
            GetBlock(position + Vector3.left),
            GetBlock(position + Vector3.up),
            GetBlock(position + Vector3.down),
            GetBlock(position + Vector3.forward),
            GetBlock(position + Vector3.back)
        };

        return blocks;
    }
    // Check all adjacent spots and return all unique Chunks
    public static HashSet<Chunk> GetNearbyChunks(Vector3 position)
    {
        Chunk[] chunks =
        {
            GetChunk(position),
            GetChunk(position + Vector3.right),
            GetChunk(position + Vector3.left),
            GetChunk(position + Vector3.up),
            GetChunk(position + Vector3.down),
            GetChunk(position + Vector3.forward),
            GetChunk(position + Vector3.back)
        };

        return new HashSet<Chunk>(chunks);
    }
    // Updates block and adjacent blocks (and their chunks)
    public static void UpdateAtPosition(Vector3 position)
    {
        HashSet<Chunk> chunks = GetNearbyChunks(position);
        foreach(Chunk chunk in chunks)
        {
            if(chunk != null)
                chunk.MakeDirty();
        }
    }
}

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Chunk : MonoBehaviour
{
    // Constants
    public static readonly Vector3Int MaxSize = new Vector3Int(5, 5, 5);

    // Object
    private Player m_player;

    // Data
    public Vector3Int   m_index = new Vector3Int(0, 0, 0);
    public BlockStorage m_blocks;
    private bool        m_meshDirty = false;

    // Components
    private Mesh         m_mesh;
    private MeshFilter   m_meshFilter;
    private MeshRenderer m_meshRenderer;
    private MeshCollider m_meshCollider;

    // Utility [local]
    private void BuildMesh()
    {
        ChunkBuilder builder = new ChunkBuilder(m_index);

        m_mesh = new Mesh();
        m_mesh.name = "ChunkMesh";

        m_mesh.vertices = builder.vertices.ToArray();
        m_mesh.uv = builder.uvs.ToArray();
        m_mesh.normals = builder.normals.ToArray();
        m_mesh.triangles = builder.indices.ToArray();

        m_meshFilter.mesh = m_mesh;
        m_meshCollider.sharedMesh = m_mesh;
    }

    // Utility
    public void SetBlock(Vector3Int index, Block block)
    {
        m_blocks.Set(index, block);
    }
    public void MakeDirty()
    {
        m_meshDirty = true;
    }
    public void GenerateTest()
    {
        for (int x = 0; x < MaxSize.x; x++)
        {
            for (int z = 0; z < MaxSize.z; z++)
            {
                int randomHeight = Random.Range(0, 6);
                //for (int y = 0; y < randomHeight; y++)
                for (int y = 0; y < MaxSize.y; y++)
                {
                    SetBlock(new Vector3Int(x, y, z), new Block(1, new Vector3Int(x, y, z)));
                }
            }
        }
    }

    // Behaviour
    private void Awake()
    {
        m_blocks = new BlockStorage(); //

        m_player = FindObjectOfType<Player>();

        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_meshCollider = GetComponent<MeshCollider>();

        m_meshRenderer.material = GlobalData.material_default;
    }

    private void OnDrawGizmos()
    {
        Vector3 Position = new Vector3(
            m_index.x * MaxSize.x + MaxSize.x * 0.5f,
            m_index.y * MaxSize.y + MaxSize.y * 0.5f,
            m_index.z * MaxSize.z + MaxSize.z * 0.5f
            );
        // Draw Around Extents of Chunk
        //DebugExtension.DrawLocalCube(Matrix4x4.Translate(Position), Size);
    }

    private void Update()
    {
        // Wireframe Mode
        if(m_player.m_wireframeMode)
            m_meshRenderer.material = GlobalData.material_wireframe;
        else
            m_meshRenderer.material = GlobalData.material_default;

        // Regenerate
        if (m_meshDirty)
        {
            BuildMesh();
            m_meshDirty = false;
        }
    }
}
