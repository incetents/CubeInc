using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;

public class ChunkBuilder
{
    // Static
    private static Vector3Int vecRight = new Vector3Int(+1, 0, 0);
    private static Vector3Int vecLeft = new Vector3Int(-1, 0, 0);
    private static Vector3Int vecUp = new Vector3Int(0, +1, 0);
    private static Vector3Int vecDown = new Vector3Int(0, -1, 0);
    private static Vector3Int vecForward = new Vector3Int(0, 0, +1);
    private static Vector3Int vecBack = new Vector3Int(0, 0, -1);

    // Data
    private Vector3Int m_chunkIndex = new Vector3Int();
    private Thread m_buildThread = null;
    private bool m_threadComplete = false;

    // Read Data
    public List<Vector3> vertices = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();
    public List<Vector3> normals = new List<Vector3>();
    public List<int> indices = new List<int>();
    public int quadCount = 0;

    // Temp Data
    private float atlasSize;

    // Utility
    public void AddFace_Up(Vector3 position, Vector4 uvData)
    {
        // +Y Face
        vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 0)); // 3
        vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 1)); // 4
        vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 1)); // 2
        vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 0)); // 1

        uvs.Add(new Vector2(uvData.x, uvData.z)); // 4
        uvs.Add(new Vector2(uvData.x, uvData.w)); // 2
        uvs.Add(new Vector2(uvData.y, uvData.w)); // 1
        uvs.Add(new Vector2(uvData.y, uvData.z)); // 3

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
    public void AddFace_Down(Vector3 position, Vector4 uvData)
    {
        // -Y Face
        vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 0)); // 3
        vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 1)); // 4
        vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 1)); // 2
        vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 0)); // 1

        uvs.Add(new Vector2(uvData.x, uvData.z)); // 4
        uvs.Add(new Vector2(uvData.x, uvData.w)); // 2
        uvs.Add(new Vector2(uvData.y, uvData.w)); // 1
        uvs.Add(new Vector2(uvData.y, uvData.z)); // 3

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
    public void AddFace_Right(Vector3 position, Vector4 uvData)
    {
        // +X Face
        vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 0)); // 3
        vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 1)); // 4
        vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 1)); // 2
        vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 0)); // 1

        uvs.Add(new Vector2(uvData.y, uvData.z)); // 3
        uvs.Add(new Vector2(uvData.x, uvData.z)); // 4
        uvs.Add(new Vector2(uvData.x, uvData.w)); // 2
        uvs.Add(new Vector2(uvData.y, uvData.w)); // 1

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
    public void AddFace_Left(Vector3 position, Vector4 uvData)
    {
        // -X Face
        vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 0)); // 3
        vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 1)); // 4
        vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 1)); // 2
        vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 0)); // 1

        uvs.Add(new Vector2(uvData.y, uvData.z)); // 3
        uvs.Add(new Vector2(uvData.x, uvData.z)); // 4
        uvs.Add(new Vector2(uvData.x, uvData.w)); // 2
        uvs.Add(new Vector2(uvData.y, uvData.w)); // 1

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
    public void AddFace_Front(Vector3 position, Vector4 uvData)
    {
        // +Z Face
        vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 1)); // 3
        vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 1)); // 4
        vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 1)); // 2
        vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 1)); // 1

        uvs.Add(new Vector2(uvData.y, uvData.z)); // 3
        uvs.Add(new Vector2(uvData.x, uvData.z)); // 4
        uvs.Add(new Vector2(uvData.x, uvData.w)); // 2
        uvs.Add(new Vector2(uvData.y, uvData.w)); // 1

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
    public void AddFace_Back(Vector3 position, Vector4 uvData)
    {
        // -Z Face
        vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 0));
        vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 0));
        vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 0));
        vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 0));

        uvs.Add(new Vector2(uvData.x, uvData.w)); // 2
        uvs.Add(new Vector2(uvData.y, uvData.w)); // 1
        uvs.Add(new Vector2(uvData.x, uvData.z)); // 4
        uvs.Add(new Vector2(uvData.y, uvData.z)); // 3

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

    public bool isComplete()
    {
        return m_threadComplete;
    }

    private void Run()
    {
        Chunk chunk = ChunkStorage.GetChunk(m_chunkIndex);
        if (chunk == null)
        {
            Debug.LogError("Missing Chunk for building: " + m_chunkIndex);
            m_threadComplete = true;
            return;
        }

        BlockStorage blocks = chunk.m_blocks;
        Vector3 globalPos = m_chunkIndex * Chunk.MaxSize;

        for (int x = 0; x < Chunk.MaxSize.x; x++)
        {
            for (int y = 0; y < Chunk.MaxSize.y; y++)
            {
                for (int z = 0; z < Chunk.MaxSize.z; z++)
                {
                    Block block = blocks.data[x, y, z];

                    // Broken Block Check
                    if (block.m_data == null)
                        return;
                    if (block.m_data.m_air)
                        continue;

                    Vector3Int localPos = new Vector3Int(x, y, z);
                    Vector3 position = localPos + globalPos;

                    Vector3Int RightCheck = localPos + vecRight;
                    Vector3Int LeftCheck = localPos + vecLeft;
                    Vector3Int UpCheck = localPos + vecUp;
                    Vector3Int DownCheck = localPos + vecDown;
                    Vector3Int FrontCheck = localPos + vecForward;
                    Vector3Int BackCheck = localPos + vecBack;

                    // LEFT // Out of bounds check
                    if (LeftCheck.x < 0)
                    {
                        Chunk other = ChunkStorage.GetChunk(m_chunkIndex + vecLeft);
                        if (other == null || (other != null && other.m_blocks.IsAir(new Vector3Int(Chunk.MaxSize.x - 1, y, z))))
                            AddFace_Left(position, block.m_data.m_atlasLocation);
                    }
                    // LEFT // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(LeftCheck))
                            AddFace_Left(position, block.m_data.m_atlasLocation);
                    }

                    // RIGHT // Out of bounds check
                    if (RightCheck.x > Chunk.MaxSize.x - 1)
                    {
                        Chunk other = ChunkStorage.GetChunk(m_chunkIndex + vecRight);
                        if (other == null || (other != null && other.m_blocks.IsAir(new Vector3Int(0, y, z))))
                            AddFace_Right(position, block.m_data.m_atlasLocation);
                    }
                    // RIGHT // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(RightCheck))
                            AddFace_Right(position, block.m_data.m_atlasLocation);
                    }

                    // DOWN // Out of bounds check
                    if (DownCheck.y < 0)
                    {
                        Chunk other = ChunkStorage.GetChunk(m_chunkIndex + vecDown);
                        if (other == null || (other != null && other.m_blocks.IsAir(new Vector3Int(x, Chunk.MaxSize.y - 1, z))))
                            AddFace_Down(position, block.m_data.m_atlasLocation);
                    }
                    // DOWN // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(DownCheck))
                            AddFace_Down(position, block.m_data.m_atlasLocation);
                    }

                    // UP // Out of bounds check
                    if (UpCheck.y > Chunk.MaxSize.y - 1)
                    {
                        Chunk other = ChunkStorage.GetChunk(m_chunkIndex + vecUp);
                        if (other == null || (other != null && other.m_blocks.IsAir(new Vector3Int(x, 0, z))))
                            AddFace_Up(position, block.m_data.m_atlasLocation);
                    }
                    // UP // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(UpCheck))
                            AddFace_Up(position, block.m_data.m_atlasLocation);
                    }

                    // BACK // Out of bounds check
                    if (BackCheck.z < 0)
                    {
                        Chunk other = ChunkStorage.GetChunk(m_chunkIndex + vecBack);
                        if (other == null || (other != null && other.m_blocks.IsAir(new Vector3Int(x, y, Chunk.MaxSize.z - 1))))
                            AddFace_Back(position, block.m_data.m_atlasLocation);
                    }
                    // BACK // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(BackCheck))
                            AddFace_Back(position, block.m_data.m_atlasLocation);
                    }

                    // FRONT // Out of bounds check
                    if (FrontCheck.z > Chunk.MaxSize.z - 1)
                    {
                        Chunk other = ChunkStorage.GetChunk(m_chunkIndex + vecForward);
                        if (other == null || (other != null && other.m_blocks.IsAir(new Vector3Int(x, y, 0))))
                            AddFace_Front(position, block.m_data.m_atlasLocation);
                    }
                    // FRONT // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(FrontCheck))
                            AddFace_Front(position, block.m_data.m_atlasLocation);
                    }

                }
            }
        }

        m_threadComplete = true;
    }

    public ChunkBuilder(Vector3Int index)
    {
        m_chunkIndex = index;

        m_buildThread = new Thread(Run);
        m_buildThread.Start();
    }
}