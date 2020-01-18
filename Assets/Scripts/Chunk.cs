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
        

        Chunk chunk = ChunkManager.getChunk(index);
        if (chunk == null)
            return;

        ChunkData blocks = chunk.m_blocks;
        Vector3 position = index * Chunk.MaxSize;

        foreach (var XYZSection in blocks.data)
        {
            foreach (var YZSection in XYZSection.Value)
            {
                foreach (var ZSection in YZSection.Value)
                {
                    Block block = ZSection.Value;

                    offset = new Vector3(
                        block.m_localPosition.x,
                        block.m_localPosition.y,
                        block.m_localPosition.z
                        ) + position;

                    Vector3Int RightCheck = block.m_localPosition + vecRight;
                    Vector3Int LeftCheck  = block.m_localPosition + vecLeft;
                    Vector3Int UpCheck    = block.m_localPosition + vecUp;
                    Vector3Int DownCheck  = block.m_localPosition + vecDown;
                    Vector3Int FrontCheck = block.m_localPosition + vecForward;
                    Vector3Int BackCheck  = block.m_localPosition + vecBack;

                    // LEFT // Out of bounds check
                    if (LeftCheck.x < 0)
                    {
                        Chunk other = ChunkManager.getChunk(index + vecLeft);
                        if (other == null || (other != null && other.m_blocks.checkEmpty(new Vector3Int(Chunk.MaxSize - 1, block.m_localPosition.y, block.m_localPosition.z))))
                            AddFace_Left();
                    }
                    // LEFT // Inbounds Check
                    else
                    {
                        if (blocks.checkEmpty(LeftCheck))
                            AddFace_Left();
                    }

                    // RIGHT // Out of bounds check
                    if (RightCheck.x > Chunk.MaxSize - 1)
                    {
                        Chunk other = ChunkManager.getChunk(index + vecRight);
                        if (other == null || (other != null && other.m_blocks.checkEmpty(new Vector3Int(0, block.m_localPosition.y, block.m_localPosition.z))))
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
                        Chunk other = ChunkManager.getChunk(index + vecDown);
                        if (other == null || (other != null && other.m_blocks.checkEmpty(new Vector3Int(block.m_localPosition.x, Chunk.MaxSize - 1, block.m_localPosition.z))))
                            AddFace_Down();
                    }
                    // DOWN // Inbounds Check
                    else
                    {
                        if (blocks.checkEmpty(DownCheck))
                            AddFace_Down();
                    }

                    // UP // Out of bounds check
                    if (UpCheck.y > Chunk.MaxSize - 1)
                    {
                        Chunk other = ChunkManager.getChunk(index + vecUp);
                        if (other == null || (other != null && other.m_blocks.checkEmpty(new Vector3Int(block.m_localPosition.x, 0, block.m_localPosition.z))))
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
                         Chunk other = ChunkManager.getChunk(index + vecBack);
                         if (other == null || (other != null && other.m_blocks.checkEmpty(new Vector3Int(block.m_localPosition.x, block.m_localPosition.y, Chunk.MaxSize - 1))))
                             AddFace_Back();
                     }
                     // BACK // Inbounds Check
                     else
                     {
                         if (blocks.checkEmpty(BackCheck))
                             AddFace_Back();
                     }
                     
                     // FRONT // Out of bounds check
                     if (FrontCheck.z > Chunk.MaxSize - 1)
                     {
                         Chunk other = ChunkManager.getChunk(index + vecForward);
                         if (other == null || (other != null && other.m_blocks.checkEmpty(new Vector3Int(block.m_localPosition.x, block.m_localPosition.y, 0))))
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
    public Dictionary<int, Dictionary<int, Dictionary<int, Block>>> data = new Dictionary<int, Dictionary<int, Dictionary<int, Block>>>(); //

    public void add(Block block)
    {
        Vector3Int index = block.m_localPosition;

        // Check if X section is missing
        if (!data.ContainsKey(index.x))
            data.Add(index.x, new Dictionary<int, Dictionary<int, Block>>());

        // Check if Y section is missing
        if (!data[index.x].ContainsKey(index.y))
            data[index.x].Add(index.y, new Dictionary<int, Block>());

        // Edit Z Section
        data[index.x][index.y][index.z] = block;
    }

    public Block getBlock(Vector3Int index)
    {
        if (!checkEmpty(index))
            return data[index.x][index.y][index.z];

        return null;
    }

    public bool checkEmpty(Vector3Int index)
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

public static class ChunkManager
{
    public static Dictionary<int, Dictionary<int, Dictionary<int, Chunk>>> data = new Dictionary<int, Dictionary<int, Dictionary<int, Chunk>>>(); //

    public static void add(Chunk chunk)
    {
        Vector3Int index = chunk.m_position;

        // Check if X section is missing
        if (!data.ContainsKey(index.x))
            data.Add(index.x, new Dictionary<int, Dictionary<int, Chunk>>());

        // Check if Y section is missing
        if (!data[index.x].ContainsKey(index.y))
            data[index.x].Add(index.y, new Dictionary<int, Chunk>());

        // Edit Z Section
        data[index.x][index.y][index.z] = chunk;

        //Debug.Log("new chunk:" + index);
    }

    public static Chunk getChunk(Vector3Int index)
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
    public static readonly int MaxSize = 5;

    // Data
    public ChunkData m_blocks = new ChunkData(); //
    public Vector3Int m_position = new Vector3Int(0, 0, 0);

    // Components
    Mesh m_mesh;
    MeshFilter m_meshFilter;
    MeshRenderer m_meshRenderer;
    MeshCollider m_meshCollider;

    // Utility
    public void add(Block block)
    {
        m_blocks.add(block);
    }
    public void generateTest()
    {
        for (int x = 0; x < MaxSize; x++)
        {
            for (int z = 0; z < MaxSize; z++)
            {
                int randomHeight = Random.Range(0, 6);

                for (int y = 0; y < randomHeight; y++)
                //for (int y = 0; y < MaxSize; y++)
                {
                    add(new Block(new Vector3Int(x, y, z)));
                }
            }
        }
    }

    public void buildMesh()
    {
        ChunkBuilder builder = new ChunkBuilder(m_position);

        m_mesh.vertices = builder.vertices.ToArray();
        m_mesh.uv = builder.uvs.ToArray();
        m_mesh.normals = builder.normals.ToArray();
        m_mesh.triangles = builder.indices.ToArray();

        m_meshFilter.mesh = m_mesh;
        m_meshRenderer.material = GlobalData.material_default;
        m_meshCollider.sharedMesh = m_mesh;
    }

    // Behaviour
    private void Awake()
    {
        m_mesh = new Mesh();
        m_mesh.name = "ChunkMesh";
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();
        m_meshCollider = GetComponent<MeshCollider>();
    }

    private void OnDrawGizmos()
    {
        Vector3 Size = new Vector3(MaxSize, MaxSize, MaxSize);
        float HalfMaxSize = MaxSize * 0.5f;
        Vector3 Position = new Vector3(
            m_position.x * MaxSize + HalfMaxSize,
            m_position.y * MaxSize + HalfMaxSize,
            m_position.z * MaxSize + HalfMaxSize
            );
        // Draw Around Extents of Chunk
        //DebugExtension.DrawLocalCube(Matrix4x4.Translate(Position), Size);
    }
}
