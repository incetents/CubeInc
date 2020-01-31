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
    public static bool SetBlock(Vector3 position, Block block)
    {
        Vector3Int chunkIndex = Chunk.ConvertToChunkIndex(position);
        Chunk chunk = GetChunk(chunkIndex);
        if (chunk == null)
            return false;

        // Check if block exists in chunk
        chunk.m_blocks.Set(block);
        return true;
    }

    // Acquire block from world space position
    public static Block GetBlock(Vector3 position)
    {
        // Check if chunk exists
        Vector3Int chunkIndex = Chunk.ConvertToChunkIndex(position);
        Chunk chunk = GetChunk(chunkIndex);
        if (chunk == null)
            return null;

        // Check if block exists in chunk
        Vector3Int blockIndex = Chunk.ConvertToBlockIndex(position);
        return chunk.m_blocks.Get(blockIndex);
    }
    // Get all adjacent blocks from position (includes position block)
    public static Block[] GetBlocksNearPosition(Vector3 position)
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
    public static HashSet<Chunk> GetChunksNearPosition(Vector3 position)
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
    // Get all chunks around given chunk
    public static Chunk[] GetNeighborChunks(Chunk chunk)
    {
        Chunk[] chunks =
        {
            GetChunk(chunk.m_index + Vector3Int.right),
            GetChunk(chunk.m_index + Vector3Int.left),
            GetChunk(chunk.m_index + Vector3Int.up),
            GetChunk(chunk.m_index + Vector3Int.down),
            GetChunk(chunk.m_index + new Vector3Int(0,0,1)),
            GetChunk(chunk.m_index + new Vector3Int(0,0,-1))
        };

        return chunks;
    }

    // Updates block and adjacent blocks (and their chunks)
    public static void MakePositionDirty(Vector3 position)
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

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Chunk : MonoBehaviour
{
    // Constants
    public static readonly Vector3Int MaxSize = new Vector3Int(16, 16, 16);

    // Object
    private ChunkManager m_chunkManager;
    private Player m_player;

    // Data
    [System.NonSerialized] public  Vector3Int  m_index;
    [System.NonSerialized] private Vector3     m_center; // Center of chunk position (world space)
    [System.NonSerialized] private Vector3[]   m_bottomExtents;
    [System.NonSerialized] private Vector3[]   m_topExtents;


    [System.NonSerialized] public BlockStorage m_blocks;
    [System.NonSerialized] private bool        m_visible = true; // Can camera see this chunk
    [System.NonSerialized] private bool        m_meshDirty = false;

    // Components
    private Mesh         m_mesh;
    private MeshFilter   m_meshFilter;
    private MeshRenderer m_meshRenderer;
    private MeshCollider m_meshCollider;

    // Special
    private ChunkBuilder m_chunkBuilder = null;
    private bool m_isSetup = false;
    private LineRenderer[] m_lineRenderers;

    // Utility [local]
    public void UpdateMesh()
    {
        if (m_chunkBuilder == null)
        {
            Debug.LogError("Copying mesh data from null chunkbuilder");
            return;
        }

        m_mesh = new Mesh();
        m_mesh.name = "ChunkMesh";

        m_mesh.vertices = m_chunkBuilder.vertices.ToArray();
        m_mesh.uv = m_chunkBuilder.uvs.ToArray();
        m_mesh.uv2 = m_chunkBuilder.id.ToArray();
        m_mesh.normals = m_chunkBuilder.normals.ToArray();
        m_mesh.triangles = m_chunkBuilder.indices.ToArray();

        m_meshFilter.mesh = m_mesh;
        m_meshCollider.sharedMesh = m_mesh;
    }

    // Utility
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
        if(m_chunkBuilder == null)
        {
            m_chunkBuilder = new ChunkBuilder(m_index);
            return true;
        }
        return false;
    }
    public bool IsMeshConstructed()
    {
        if (m_chunkBuilder == null)
            return false;

        return m_chunkBuilder.isComplete();
    }
    public void EndMeshConstruction()
    {
        m_chunkBuilder = null;
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
                            SetBlock(new Block(1, new Vector3Int(x, y, z)));
                        else
                            SetBlock(new Block(2, new Vector3Int(x, y, z)));

                    }
                    else
                    {
                        SetBlock(new Block(0, new Vector3Int(x, y, z)));
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
                    Block block = new Block(id, new Vector3Int(x, y, z));

                    SetBlock(block);
                }
            }
        }
    }
    public bool CanCameraSeeChunk()
    {
        if (
            m_index.x >= m_player.m_chunkIndex.x - 1 &&
            m_index.x <= m_player.m_chunkIndex.x + 1 &&

            m_index.y >= m_player.m_chunkIndex.y - 1 &&
            m_index.y <= m_player.m_chunkIndex.y + 1 &&

            m_index.z >= m_player.m_chunkIndex.z - 1 &&
            m_index.z <= m_player.m_chunkIndex.z + 1
            )
            return true;

        // Check if camera can see the center
        Camera camera = m_player.GetCamera();
        Vector3 cameraToChunkCenter = m_center - camera.transform.position;

        // Check if in front of camera (more than 90 degress from camera forward = not seen)
        if (Vector3.Dot(cameraToChunkCenter, camera.transform.forward) > 0)
            return true;

        // Cannot be seen
        return false;
    }

    // Behaviour
    public void Setup(Vector3Int index)
    {
        //
        m_index = index;

        // Data
        m_blocks = new BlockStorage(); //
        m_player = GlobalData.player;
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_meshCollider = GetComponent<MeshCollider>();
        m_meshRenderer.material = GlobalData.material_block;

        // Update Center
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

        // Line Rendering
        m_lineRenderers = new LineRenderer[4];
        for(int i = 0; i < 4; i++)
        {
            GameObject g = (GameObject)Instantiate(GlobalData.prefab_line);
            g.transform.SetParent(transform);
            m_lineRenderers[i] = g.GetComponent<LineRenderer>();
            m_lineRenderers[i].startColor = Color.red;
            m_lineRenderers[i].endColor = Color.green;
            m_lineRenderers[i].positionCount = 2;
        }
        
        m_lineRenderers[0].SetPosition(0, m_bottomExtents[0]);
        m_lineRenderers[0].SetPosition(1, m_topExtents[0]);
        
        m_lineRenderers[1].SetPosition(0, m_bottomExtents[1]);
        m_lineRenderers[1].SetPosition(1, m_topExtents[1]);

        m_lineRenderers[2].SetPosition(0, m_bottomExtents[2]);
        m_lineRenderers[2].SetPosition(1, m_topExtents[2]);

        m_lineRenderers[3].SetPosition(0, m_bottomExtents[3]);
        m_lineRenderers[3].SetPosition(1, m_topExtents[3]);

        // done
        m_isSetup = true;
    }


    //  private void OnDrawGizmos()
    //  {
    //      if (!m_isSetup)
    //          return;
    //  
    //      // Draw Around Extents of Chunk
    //      DebugExtension.DrawLocalCube(Matrix4x4.Translate(m_center), Chunk.MaxSize);
    //  }

    private void Awake()
    {
        m_chunkManager = FindObjectOfType<ChunkManager>();
    }

    private void Update()
    {
        if (!m_isSetup)
            return;

        // Destroy Self if too far away from player camera
        if (m_chunkManager.m_generateChunks)
        {
            Vector3Int manhattanDistance = (m_index - m_player.m_chunkIndex);
            if (Mathf.Abs(manhattanDistance.x) > ChunkManager.m_ChunkDistance)
            {
                Destroy(this.gameObject);
            }
            else if (Mathf.Abs(manhattanDistance.y) > ChunkManager.m_ChunkDistance)
            {
                Destroy(this.gameObject);
            }
            else if (Mathf.Abs(manhattanDistance.z) > ChunkManager.m_ChunkDistance)
            {
                Destroy(this.gameObject);
            }
        }

        // Check if camera can see chunk
        m_visible = CanCameraSeeChunk();

        if (m_visible)
        {
            m_meshRenderer.enabled = true;

            // Wireframe Mode
            if (m_player.m_wireframeMode)
                m_meshRenderer.material = GlobalData.material_wireframe;
            else
                m_meshRenderer.material = GlobalData.material_block;
        }
        else
        {
            m_meshRenderer.enabled = false;
            //m_meshRenderer.material = GlobalData.material_wireframe;
        }

    }
}
