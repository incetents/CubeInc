using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkBuilder
{
    // Core Data
    public List<Vector3> vertices = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();
    public List<Vector3> normals = new List<Vector3>();
    public List<int> indices = new List<int>();
    public int quadCount = 0;

    // Internal Data
    private Vector3 offset = new Vector3(0, 0, 0); // Position Offset

    public void AddFace_Up()
    {
        // +Y Face
        vertices.Add(new Vector3(offset.x + 0, offset.y + 1, offset.z + 0)); // 3
        vertices.Add(new Vector3(offset.x + 0, offset.y + 1, offset.z + 1)); // 4
        vertices.Add(new Vector3(offset.x + 1, offset.y + 1, offset.z + 1)); // 2
        vertices.Add(new Vector3(offset.x + 1, offset.y + 1, offset.z + 0)); // 1

        uvs.Add(new Vector2(0, 0)); // 4
        uvs.Add(new Vector2(0, 1)); // 2
        uvs.Add(new Vector2(1, 1)); // 1
        uvs.Add(new Vector2(1, 0)); // 3

        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);
        normals.Add(Vector3.up);

        indices.Add(0 + quadCount * 4);
        indices.Add(1 + quadCount * 4);
        indices.Add(2 + quadCount * 4);
        indices.Add(0 + quadCount * 4);
        indices.Add(2 + quadCount * 4);
        indices.Add(3 + quadCount * 4);

        quadCount++;
    }
    public void AddFace_Down()
    {
        // -Y Face
        vertices.Add(new Vector3(offset.x + 0, offset.y + 0, offset.z + 0)); // 3
        vertices.Add(new Vector3(offset.x + 0, offset.y + 0, offset.z + 1)); // 4
        vertices.Add(new Vector3(offset.x + 1, offset.y + 0, offset.z + 1)); // 2
        vertices.Add(new Vector3(offset.x + 1, offset.y + 0, offset.z + 0)); // 1

        uvs.Add(new Vector2(0, 0)); // 4
        uvs.Add(new Vector2(0, 1)); // 2
        uvs.Add(new Vector2(1, 1)); // 1
        uvs.Add(new Vector2(1, 0)); // 3

        normals.Add(Vector3.down);
        normals.Add(Vector3.down);
        normals.Add(Vector3.down);
        normals.Add(Vector3.down);

        indices.Add(0 + quadCount * 4);
        indices.Add(2 + quadCount * 4);
        indices.Add(1 + quadCount * 4);
        indices.Add(0 + quadCount * 4);
        indices.Add(3 + quadCount * 4);
        indices.Add(2 + quadCount * 4);

        quadCount++;
    }
    public void AddFace_Right()
    {
        // +X Face
        vertices.Add(new Vector3(offset.x + 1, offset.y + 0, offset.z + 0)); // 3
        vertices.Add(new Vector3(offset.x + 1, offset.y + 0, offset.z + 1)); // 4
        vertices.Add(new Vector3(offset.x + 1, offset.y + 1, offset.z + 1)); // 2
        vertices.Add(new Vector3(offset.x + 1, offset.y + 1, offset.z + 0)); // 1

        uvs.Add(new Vector2(0, 0)); // 3
        uvs.Add(new Vector2(1, 0)); // 4
        uvs.Add(new Vector2(1, 1)); // 2
        uvs.Add(new Vector2(0, 1)); // 1

        normals.Add(Vector3.right);
        normals.Add(Vector3.right);
        normals.Add(Vector3.right);
        normals.Add(Vector3.right);

        indices.Add(0 + quadCount * 4);
        indices.Add(2 + quadCount * 4);
        indices.Add(1 + quadCount * 4);
        indices.Add(0 + quadCount * 4);
        indices.Add(3 + quadCount * 4);
        indices.Add(2 + quadCount * 4);

        quadCount++;
    }
    public void AddFace_Left()
    {
        // -X Face
        vertices.Add(new Vector3(offset.x + 0, offset.y + 0, offset.z + 0)); // 3
        vertices.Add(new Vector3(offset.x + 0, offset.y + 0, offset.z + 1)); // 4
        vertices.Add(new Vector3(offset.x + 0, offset.y + 1, offset.z + 1)); // 2
        vertices.Add(new Vector3(offset.x + 0, offset.y + 1, offset.z + 0)); // 1

        uvs.Add(new Vector2(1, 0)); // 3
        uvs.Add(new Vector2(0, 0)); // 4
        uvs.Add(new Vector2(0, 1)); // 2
        uvs.Add(new Vector2(1, 1)); // 1

        normals.Add(Vector3.left);
        normals.Add(Vector3.left);
        normals.Add(Vector3.left);
        normals.Add(Vector3.left);

        indices.Add(0 + quadCount * 4);
        indices.Add(1 + quadCount * 4);
        indices.Add(2 + quadCount * 4);
        indices.Add(0 + quadCount * 4);
        indices.Add(2 + quadCount * 4);
        indices.Add(3 + quadCount * 4);

        quadCount++;
    }
    public void AddFace_Front()
    {
        // +Z Face
        vertices.Add(new Vector3(offset.x + 0, offset.y + 0, offset.z + 1)); // 3
        vertices.Add(new Vector3(offset.x + 1, offset.y + 0, offset.z + 1)); // 4
        vertices.Add(new Vector3(offset.x + 1, offset.y + 1, offset.z + 1)); // 2
        vertices.Add(new Vector3(offset.x + 0, offset.y + 1, offset.z + 1)); // 1

        uvs.Add(new Vector2(1, 0)); // 3
        uvs.Add(new Vector2(0, 0)); // 4
        uvs.Add(new Vector2(0, 1)); // 2
        uvs.Add(new Vector2(1, 1)); // 1

        normals.Add(Vector3.forward);
        normals.Add(Vector3.forward);
        normals.Add(Vector3.forward);
        normals.Add(Vector3.forward);

        indices.Add(0 + quadCount * 4);
        indices.Add(1 + quadCount * 4);
        indices.Add(2 + quadCount * 4);
        indices.Add(0 + quadCount * 4);
        indices.Add(2 + quadCount * 4);
        indices.Add(3 + quadCount * 4);

        quadCount++;
    }
    public void AddFace_Back()
    {
        // -Z Face
        vertices.Add(new Vector3(offset.x + 0, offset.y + 1, offset.z + 0));
        vertices.Add(new Vector3(offset.x + 1, offset.y + 1, offset.z + 0));
        vertices.Add(new Vector3(offset.x + 0, offset.y + 0, offset.z + 0));
        vertices.Add(new Vector3(offset.x + 1, offset.y + 0, offset.z + 0));

        uvs.Add(new Vector2(0, 1));
        uvs.Add(new Vector2(1, 1));
        uvs.Add(new Vector2(0, 0));
        uvs.Add(new Vector2(1, 0));

        normals.Add(Vector3.back);
        normals.Add(Vector3.back);
        normals.Add(Vector3.back);
        normals.Add(Vector3.back);

        indices.Add(0 + quadCount * 4);
        indices.Add(1 + quadCount * 4);
        indices.Add(2 + quadCount * 4);
        indices.Add(2 + quadCount * 4);
        indices.Add(1 + quadCount * 4);
        indices.Add(3 + quadCount * 4);

        quadCount++;
    }

    private static Vector3Int vecRight      = new Vector3Int(+1, 0, 0);
    private static Vector3Int vecLeft       = new Vector3Int(-1, 0, 0);
    private static Vector3Int vecUp         = new Vector3Int(0, +1, 0);
    private static Vector3Int vecDown       = new Vector3Int(0, -1, 0);
    private static Vector3Int vecForward    = new Vector3Int(0, 0, +1);
    private static Vector3Int vecBack       = new Vector3Int(0, 0, -1);

    public ChunkBuilder(Vector3Int index)
    {
        Chunk chunk = ChunkManager.getChunkIndex(index);
        if (chunk == null)
        {
            Debug.LogError("Missing Chunk for building: " + index);
            return;
        }

        ChunkData blocks = chunk.m_blocks;
        Vector3 position = index * Chunk.MaxSize;

        for(int x = 0; x < Chunk.MaxSize.x; x++)
        {
            for (int y = 0; y < Chunk.MaxSize.y; y++)
            {
                for (int z = 0; z < Chunk.MaxSize.z; z++)
                {
                    Block block = blocks.data[x, y, z];
                    if (block.m_data.m_air)
                        continue;

                    Vector3Int localPos = new Vector3Int(x, y, z);

                    offset = localPos + position;

                    Vector3Int RightCheck = localPos + vecRight;
                    Vector3Int LeftCheck  = localPos + vecLeft;
                    Vector3Int UpCheck    = localPos + vecUp;
                    Vector3Int DownCheck  = localPos + vecDown;
                    Vector3Int FrontCheck = localPos + vecForward;
                    Vector3Int BackCheck  = localPos + vecBack;

                    // LEFT // Out of bounds check
                    if (LeftCheck.x < 0)
                    {
                        Chunk other = ChunkManager.getChunkIndex(index + vecLeft);
                        if (other == null || (other != null && other.m_blocks.checkEmpty(new Vector3Int(Chunk.MaxSize.x - 1, y, z))))
                            AddFace_Left();
                    }
                    // LEFT // Inbounds Check
                    else
                    {
                        if (blocks.checkEmpty(LeftCheck))
                            AddFace_Left();
                    }

                    // RIGHT // Out of bounds check
                    if (RightCheck.x > Chunk.MaxSize.x - 1)
                    {
                        Chunk other = ChunkManager.getChunkIndex(index + vecRight);
                        if (other == null || (other != null && other.m_blocks.checkEmpty(new Vector3Int(0, y, z))))
                            AddFace_Right();
                    }
                    // RIGHT // Inbounds Check
                    else
                    {
                        if (blocks.checkEmpty(RightCheck))
                            AddFace_Right();
                    }

                    // DOWN // Out of bounds check
                    if (DownCheck.y < 0)
                    {
                        Chunk other = ChunkManager.getChunkIndex(index + vecDown);
                        if (other == null || (other != null && other.m_blocks.checkEmpty(new Vector3Int(x, Chunk.MaxSize.y - 1, z))))
                            AddFace_Down();
                    }
                    // DOWN // Inbounds Check
                    else
                    {
                        if (blocks.checkEmpty(DownCheck))
                            AddFace_Down();
                    }

                    // UP // Out of bounds check
                    if (UpCheck.y > Chunk.MaxSize.y - 1)
                    {
                        Chunk other = ChunkManager.getChunkIndex(index + vecUp);
                        if (other == null || (other != null && other.m_blocks.checkEmpty(new Vector3Int(x, 0, z))))
                            AddFace_Up();
                    }
                    // UP // Inbounds Check
                    else
                    {
                        if (blocks.checkEmpty(UpCheck))
                            AddFace_Up();
                    }

                     // BACK // Out of bounds check
                     if (BackCheck.z < 0)
                     {
                         Chunk other = ChunkManager.getChunkIndex(index + vecBack);
                         if (other == null || (other != null && other.m_blocks.checkEmpty(new Vector3Int(x, y, Chunk.MaxSize.z - 1))))
                             AddFace_Back();
                     }
                     // BACK // Inbounds Check
                     else
                     {
                         if (blocks.checkEmpty(BackCheck))
                             AddFace_Back();
                     }
                     
                     // FRONT // Out of bounds check
                     if (FrontCheck.z > Chunk.MaxSize.z - 1)
                     {
                         Chunk other = ChunkManager.getChunkIndex(index + vecForward);
                         if (other == null || (other != null && other.m_blocks.checkEmpty(new Vector3Int(x, y, 0))))
                             AddFace_Front();
                     }
                     // FRONT // Inbounds Check
                     else
                     {
                         if (blocks.checkEmpty(FrontCheck))
                             AddFace_Front();
                     }

                }
            }
        }
    }
}

public class ChunkData
{
    public Block[,,] data = new Block[Chunk.MaxSize.x, Chunk.MaxSize.y, Chunk.MaxSize.z];

    public ChunkData(Chunk chunk)
    {
        for(int x = 0; x < Chunk.MaxSize.x; x++)
        {
            for (int y = 0; y < Chunk.MaxSize.y; y++)
            {
                for (int z = 0; z < Chunk.MaxSize.z; z++)
                {
                    data[x, y, z] = new Block(0, new Vector3Int(x, y, z), chunk);
                }
            }
        }
    }

    public void add(Vector3Int index, Block block)
    {
        data[index.x, index.y, index.z] = block;
    }

    public Block getBlock(Vector3Int index)
    {
        return data[index.x, index.y, index.z];
    }

    public bool checkEmpty(Vector3Int index)
    {
        if (data[index.x, index.y, index.z].m_data.m_air)
            return true;

        return false;
    }
}

public static class ChunkManager
{
    public static Dictionary<int, Dictionary<int, Dictionary<int, Chunk>>> data = new Dictionary<int, Dictionary<int, Dictionary<int, Chunk>>>(); //

    public static void add(Chunk chunk)
    {
        Vector3Int index = chunk.m_index;

        // Check if X section is missing
        if (!data.ContainsKey(index.x))
            data.Add(index.x, new Dictionary<int, Dictionary<int, Chunk>>());

        // Check if Y section is missing
        if (!data[index.x].ContainsKey(index.y))
            data[index.x].Add(index.y, new Dictionary<int, Chunk>());

        // Edit Z Section
        data[index.x][index.y][index.z] = chunk;
    }
    public static void updateBlocksNearby(Vector3 position)
    {
        // Get all adjacent blocks
        Block[] blocks =
        {
            getBlock(position),
            getBlock(position + Vector3.right),
            getBlock(position + Vector3.left),
            getBlock(position + Vector3.up),
            getBlock(position + Vector3.down),
            getBlock(position + Vector3.forward),
            getBlock(position + Vector3.back)
        };

        // Update all chunks they are part of
        for(int i = 0; i < blocks.Length; i++)
        {
            //Debug.Log(blocks[i]);
            if(blocks[i] != null)
            {
                Chunk chunk = blocks[i].m_chunk;
                chunk.makeDirty();
                //Debug.Log(chunk.m_position);
            }
        }
    }

    public static Block getBlock(Vector3 position)
    {
        // Convert to int
        Vector3Int index = new Vector3Int(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y), Mathf.FloorToInt(position.z));

        int chunkX = (index.x < 0) ? Mathf.FloorToInt(index.x) - Chunk.MaxSize.x + 1 : Mathf.FloorToInt(index.x);
        int chunkY = (index.y < 0) ? Mathf.FloorToInt(index.y) - Chunk.MaxSize.y + 1 : Mathf.FloorToInt(index.y);
        int chunkZ = (index.z < 0) ? Mathf.FloorToInt(index.z) - Chunk.MaxSize.z + 1 : Mathf.FloorToInt(index.z);

        // Find Chunk
        Vector3Int chunkIndex = new Vector3Int(
            chunkX / Chunk.MaxSize.x,
            chunkY / Chunk.MaxSize.y,
            chunkZ / Chunk.MaxSize.z
            );

        Chunk chunk = getChunkIndex(chunkIndex);
        if (chunk == null)
            return null;

        int modLocalX = (index.x < 0) ? (Chunk.MaxSize.x - (Mathf.Abs(index.x) % Chunk.MaxSize.x)) : index.x;
        int modLocalY = (index.y < 0) ? (Chunk.MaxSize.y - (Mathf.Abs(index.y) % Chunk.MaxSize.y)) : index.y;
        int modLocalZ = (index.z < 0) ? (Chunk.MaxSize.z - (Mathf.Abs(index.z) % Chunk.MaxSize.z)) : index.z;

        Vector3Int localIndex = new Vector3Int(
            modLocalX % Chunk.MaxSize.x,
            modLocalY % Chunk.MaxSize.y,
            modLocalZ % Chunk.MaxSize.z
            );

        return chunk.m_blocks.getBlock(localIndex);
    }

    public static Chunk getChunkPosition(Vector3 position)
    {
        Vector3Int index = new Vector3Int(
            Mathf.FloorToInt(position.x / Chunk.MaxSize.x),
            Mathf.FloorToInt(position.y / Chunk.MaxSize.y),
            Mathf.FloorToInt(position.z / Chunk.MaxSize.z)
            );
        return getChunkIndex(index);
    }
    public static Chunk getChunkIndex(Vector3Int index)
    {
        if (!checkEmpty(index))
            return data[index.x][index.y][index.z];
    
        return null;
    }

    public static bool checkEmpty(Vector3Int index)
    {
        if (!data.ContainsKey(index.x))
            return true;

        if (!data[index.x].ContainsKey(index.y))
            return true;

        if (!data[index.x][index.y].ContainsKey(index.z))
            return true;

        return false;
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
    public Vector3Int m_index = new Vector3Int(0, 0, 0);
    public ChunkData  m_blocks;
    private bool      m_meshDirty = false;

    // Components
    private Mesh m_mesh;
    private MeshFilter m_meshFilter;
    private MeshRenderer m_meshRenderer;
    private MeshCollider m_meshCollider;

    // Utility [local]
    private void buildMesh()
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
    public void add(Vector3Int index, Block block)
    {
        m_blocks.add(index, block);
    }
    public void makeDirty()
    {
        m_meshDirty = true;
    }
    public void generateTest()
    {
        for (int x = 0; x < MaxSize.x; x++)
        {
            for (int z = 0; z < MaxSize.z; z++)
            {
                int randomHeight = Random.Range(0, 6);
                //for (int y = 0; y < randomHeight; y++)
                for (int y = 0; y < MaxSize.y; y++)
                {
                    add(new Vector3Int(x, y, z), new Block(1, new Vector3Int(x, y, z), this));
                }
            }
        }
    }

    // Behaviour
    private void Awake()
    {
        m_blocks = new ChunkData(this); //

        m_player = FindObjectOfType<Player>();

        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_meshCollider = GetComponent<MeshCollider>();

        m_meshRenderer.material = GlobalData.material_default;
    }

    private void OnDrawGizmos()
    {
        Vector3 Size = new Vector3(MaxSize.x, MaxSize.y, MaxSize.z);
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
            buildMesh();
            m_meshDirty = false;
        }
    }
}
