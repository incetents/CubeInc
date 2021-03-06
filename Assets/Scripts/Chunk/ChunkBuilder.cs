﻿using System.Collections;
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
    private Vector3Int m_chunkIndex;
    private Thread m_buildThread = null;
    private bool m_threadComplete = false;
    private bool m_threadFailed = false;

    // Read Data
    public List<Vector3> vertices = new List<Vector3>();
    public List<Vector2> uvs = new List<Vector2>();
    public List<Vector2> id = new List<Vector2>();
    public List<Vector3> normals = new List<Vector3>();
    public List<int> indices = new List<int>();
    public int quadCount = 0;

    // Temp Data
    private float atlasSize;

    // Utility
    public void AddFace_Up(Vector3 position, uint texID)
    {
        // +Y Face
        vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 0)); // 3
        vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 1)); // 4
        vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 1)); // 2
        vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 0)); // 1

        uvs.Add(new Vector2(0, 0)); // 4
        uvs.Add(new Vector2(0, 1)); // 2
        uvs.Add(new Vector2(1, 1)); // 1
        uvs.Add(new Vector2(1, 0)); // 3

        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));

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
    public void AddFace_Down(Vector3 position, uint texID)
    {
        // -Y Face
        vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 0)); // 3
        vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 1)); // 4
        vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 1)); // 2
        vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 0)); // 1

        uvs.Add(new Vector3(0, 0)); // 4
        uvs.Add(new Vector3(0, 1)); // 2
        uvs.Add(new Vector3(1, 1)); // 1
        uvs.Add(new Vector3(1, 0)); // 3

        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));

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
    public void AddFace_Right(Vector3 position, uint texID)
    {
        // +X Face
        vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 0)); // 3
        vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 1)); // 4
        vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 1)); // 2
        vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 0)); // 1

        uvs.Add(new Vector3(1, 0)); // 3
        uvs.Add(new Vector3(0, 0)); // 4
        uvs.Add(new Vector3(0, 1)); // 2
        uvs.Add(new Vector3(1, 1)); // 1

        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));

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
    public void AddFace_Left(Vector3 position, uint texID)
    {
        // -X Face
        vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 0)); // 3
        vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 1)); // 4
        vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 1)); // 2
        vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 0)); // 1

        uvs.Add(new Vector3(1, 0)); // 3
        uvs.Add(new Vector3(0, 0)); // 4
        uvs.Add(new Vector3(0, 1)); // 2
        uvs.Add(new Vector3(1, 1)); // 1

        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));

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
    public void AddFace_Front(Vector3 position, uint texID)
    {
        // +Z Face
        vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 1)); // 3
        vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 1)); // 4
        vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 1)); // 2
        vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 1)); // 1

        uvs.Add(new Vector3(1, 0)); // 3
        uvs.Add(new Vector3(0, 0)); // 4
        uvs.Add(new Vector3(0, 1)); // 2
        uvs.Add(new Vector3(1, 1)); // 1

        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));

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
    public void AddFace_Back(Vector3 position, uint texID)
    {
        // -Z Face
        vertices.Add(new Vector3(position.x + 0, position.y + 1, position.z + 0));
        vertices.Add(new Vector3(position.x + 1, position.y + 1, position.z + 0));
        vertices.Add(new Vector3(position.x + 0, position.y + 0, position.z + 0));
        vertices.Add(new Vector3(position.x + 1, position.y + 0, position.z + 0));

        uvs.Add(new Vector3(0, 1)); // 2
        uvs.Add(new Vector3(1, 1)); // 1
        uvs.Add(new Vector3(0, 0)); // 4
        uvs.Add(new Vector3(1, 0)); // 3

        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));
        id.Add(new Vector2(texID, 0));

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
        return m_threadComplete && !m_threadFailed;
    }
    public bool hasFailed()
    {
        return m_threadFailed;
    }

    private void Run(Vector3Int chunkIndex)
    {
        m_threadComplete = false;
        m_chunkIndex = chunkIndex;

        // Current Chunk
        Chunk chunk = ChunkStorage.GetChunkFromIndex(m_chunkIndex);
        if (chunk == null)
        {
            //Debug.LogWarning("Missing Chunk for building: " + m_chunkIndex);
            m_threadComplete = true;
            m_threadFailed = true;
            return;
        }
        // Nearby Chunks
        Chunk chunk_left = ChunkStorage.GetChunkFromIndex(m_chunkIndex + vecLeft);
        Chunk chunk_right = ChunkStorage.GetChunkFromIndex(m_chunkIndex + vecRight);
        Chunk chunk_up = ChunkStorage.GetChunkFromIndex(m_chunkIndex + vecUp);
        Chunk chunk_down = ChunkStorage.GetChunkFromIndex(m_chunkIndex + vecDown);
        Chunk chunk_forward = ChunkStorage.GetChunkFromIndex(m_chunkIndex + vecForward);
        Chunk chunk_back = ChunkStorage.GetChunkFromIndex(m_chunkIndex + vecBack);

        BlockStorage blocks = chunk.m_blocks;
        if(blocks == null)
        {
            //Debug.LogError("Building Chunk with missing blocks");
            m_threadComplete = true;
            m_threadFailed = true;
            return;
        }
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
                        continue;
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
                        if (chunk_left == null || (chunk_left != null && chunk_left.m_blocks.IsAir(new Vector3Int(Chunk.MaxSize.x - 1, y, z))))
                            AddFace_Left(position, block.m_data.GetTextureID(BlockFace.LEFT));
                    }
                    // LEFT // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(LeftCheck))
                            AddFace_Left(position, block.m_data.GetTextureID(BlockFace.LEFT));
                    }

                    // RIGHT // Out of bounds check
                    if (RightCheck.x > Chunk.MaxSize.x - 1)
                    {
                        if (chunk_right == null || (chunk_right != null && chunk_right.m_blocks.IsAir(new Vector3Int(0, y, z))))
                            AddFace_Right(position, block.m_data.GetTextureID(BlockFace.RIGHT));
                    }
                    // RIGHT // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(RightCheck))
                            AddFace_Right(position, block.m_data.GetTextureID(BlockFace.RIGHT));
                    }

                    // DOWN // Out of bounds check
                    if (DownCheck.y < 0)
                    {
                        if (chunk_down == null || (chunk_down != null && chunk_down.m_blocks.IsAir(new Vector3Int(x, Chunk.MaxSize.y - 1, z))))
                            AddFace_Down(position, block.m_data.GetTextureID(BlockFace.BOTTOM));
                    }
                    // DOWN // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(DownCheck))
                            AddFace_Down(position, block.m_data.GetTextureID(BlockFace.BOTTOM));
                    }

                    // UP // Out of bounds check
                    if (UpCheck.y > Chunk.MaxSize.y - 1)
                    {
                        if (chunk_up == null || (chunk_up != null && chunk_up.m_blocks.IsAir(new Vector3Int(x, 0, z))))
                            AddFace_Up(position, block.m_data.GetTextureID(BlockFace.TOP));
                    }
                    // UP // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(UpCheck))
                            AddFace_Up(position, block.m_data.GetTextureID(BlockFace.TOP));
                    }

                    // BACK // Out of bounds check
                    if (BackCheck.z < 0)
                    {
                        if (chunk_back == null || (chunk_back != null && chunk_back.m_blocks.IsAir(new Vector3Int(x, y, Chunk.MaxSize.z - 1))))
                            AddFace_Back(position, block.m_data.GetTextureID(BlockFace.BACK));
                    }
                    // BACK // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(BackCheck))
                            AddFace_Back(position, block.m_data.GetTextureID(BlockFace.BACK));
                    }

                    // FRONT // Out of bounds check
                    if (FrontCheck.z > Chunk.MaxSize.z - 1)
                    {
                        if (chunk_forward == null || (chunk_forward != null && chunk_forward.m_blocks.IsAir(new Vector3Int(x, y, 0))))
                            AddFace_Front(position, block.m_data.GetTextureID(BlockFace.FRONT));
                    }
                    // FRONT // Inbounds Check
                    else
                    {
                        if (blocks.IsAir(FrontCheck))
                            AddFace_Front(position, block.m_data.GetTextureID(BlockFace.FRONT));
                    }

                }
            }
        }

        m_threadComplete = true;
        m_threadFailed = false;
    }

    public ChunkBuilder(Vector3Int index)
    {
        m_buildThread = new Thread(() => Run(index));
        m_buildThread.Start();
    }

    public void Restart()
    {
        if(m_threadComplete && m_threadFailed)
        {
            Debug.Log("Fixing Mesh: " + m_chunkIndex);
            m_buildThread = new Thread(() => Run(m_chunkIndex));
            m_buildThread.Start();
        }
    }
}