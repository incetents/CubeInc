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

    private static Vector3Int vecRight = new Vector3Int(+1, 0, 0);
    private static Vector3Int vecLeft = new Vector3Int(-1, 0, 0);
    private static Vector3Int vecUp = new Vector3Int(0, +1, 0);
    private static Vector3Int vecDown = new Vector3Int(0, -1, 0);
    private static Vector3Int vecForward = new Vector3Int(0, 0, +1);
    private static Vector3Int vecBack = new Vector3Int(0, 0, -1);

    public ChunkBuilder(Vector3Int index)
    {
        Chunk chunk = ChunkStorage.GetChunk(index);
        if (chunk == null)
        {
            Debug.LogError("Missing Chunk for building: " + index);
            return;
        }

        BlockStorage blocks = chunk.m_blocks;
        Vector3 position = index * Chunk.MaxSize;

        for (int x = 0; x < Chunk.MaxSize.x; x++)
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

                    Vector3Int RightCheck   = localPos + vecRight;
                    Vector3Int LeftCheck    = localPos + vecLeft;
                    Vector3Int UpCheck      = localPos + vecUp;
                    Vector3Int DownCheck    = localPos + vecDown;
                    Vector3Int FrontCheck   = localPos + vecForward;
                    Vector3Int BackCheck    = localPos + vecBack;

                    // LEFT // Out of bounds check
                    if (LeftCheck.x < 0)
                    {
                        Chunk other = ChunkStorage.GetChunk(index + vecLeft);
                        if (other == null || (other != null && other.m_blocks.IsAir(new Vector3Int(Chunk.MaxSize.x - 1, y, z))))
                            AddFace_Left();
                    }
                    // LEFT // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(LeftCheck))
                            AddFace_Left();
                    }

                    // RIGHT // Out of bounds check
                    if (RightCheck.x > Chunk.MaxSize.x - 1)
                    {
                        Chunk other = ChunkStorage.GetChunk(index + vecRight);
                        if (other == null || (other != null && other.m_blocks.IsAir(new Vector3Int(0, y, z))))
                            AddFace_Right();
                    }
                    // RIGHT // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(RightCheck))
                            AddFace_Right();
                    }

                    // DOWN // Out of bounds check
                    if (DownCheck.y < 0)
                    {
                        Chunk other = ChunkStorage.GetChunk(index + vecDown);
                        if (other == null || (other != null && other.m_blocks.IsAir(new Vector3Int(x, Chunk.MaxSize.y - 1, z))))
                            AddFace_Down();
                    }
                    // DOWN // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(DownCheck))
                            AddFace_Down();
                    }

                    // UP // Out of bounds check
                    if (UpCheck.y > Chunk.MaxSize.y - 1)
                    {
                        Chunk other = ChunkStorage.GetChunk(index + vecUp);
                        if (other == null || (other != null && other.m_blocks.IsAir(new Vector3Int(x, 0, z))))
                            AddFace_Up();
                    }
                    // UP // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(UpCheck))
                            AddFace_Up();
                    }

                    // BACK // Out of bounds check
                    if (BackCheck.z < 0)
                    {
                        Chunk other = ChunkStorage.GetChunk(index + vecBack);
                        if (other == null || (other != null && other.m_blocks.IsAir(new Vector3Int(x, y, Chunk.MaxSize.z - 1))))
                            AddFace_Back();
                    }
                    // BACK // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(BackCheck))
                            AddFace_Back();
                    }

                    // FRONT // Out of bounds check
                    if (FrontCheck.z > Chunk.MaxSize.z - 1)
                    {
                        Chunk other = ChunkStorage.GetChunk(index + vecForward);
                        if (other == null || (other != null && other.m_blocks.IsAir(new Vector3Int(x, y, 0))))
                            AddFace_Front();
                    }
                    // FRONT // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(FrontCheck))
                            AddFace_Front();
                    }

                }
            }
        }
    }
}