﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    // Reference
    private Player m_player;

    // Global
    public static int m_ChunkDistance = 4;

    // Data
    private Chunk m_constructingChunk = null;

    private void Start()
    {
        m_player = GlobalData.player;
    }

    void Update()
    {
        // Nearby chunks that are being check
        for (int x = -m_ChunkDistance; x <= m_ChunkDistance; x++)
        {
            for (int z = -m_ChunkDistance; z <= m_ChunkDistance; z++)
            {
                Vector3Int chunkSpot = m_player.m_chunkIndex + new Vector3Int(x, 0, z);

                Chunk chunkCheck = ChunkStorage.GetChunk(chunkSpot);
                if (chunkCheck == null)
                {
                    // Create chunk at location
                    GameObject chunkObject = (GameObject)Instantiate(GlobalData.prefab_chunk);
                    chunkObject.name = "Chunk: " + chunkSpot.x.ToString() + ", " + chunkSpot.z.ToString();
                    Chunk chunk = chunkObject.GetComponent<Chunk>();
                    chunk.Setup(chunkSpot);

                    ChunkStorage.SetChunk(chunk);
                    chunk.GenerateTest();
                    chunk.MakeDirty();
                }
            }
        }

        // Update all existing chunks
        Chunk[] chunks = FindObjectsOfType<Chunk>();
        foreach(Chunk chunk in chunks)
        {
            if(chunk.IsDirty())
            {
                if(chunk.IsMeshConstructed())
                {
                    Debug.Log("Complete");
                    chunk.UpdateMesh();
                    chunk.EndMeshConstruction();
                    chunk.MakeClean();
                    // Only create 1 Mesh per frame
                    break;
                }
                else
                {
                    if(chunk.BeginMeshConstruction())
                        Debug.Log("Build");
                    else
                        Debug.Log("Wait");
                }
            }
        
        }
    }
}