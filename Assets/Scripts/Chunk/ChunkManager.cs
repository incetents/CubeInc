using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    // Reference
    private Player m_player;

    // Global
    public static int m_ChunkDistance = 1;

    // Data
    private Chunk m_constructingChunk = null;

    // Settings
    [System.NonSerialized] public bool m_generateChunks = true;

    private void Start()
    {
        m_player = GlobalData.player;
    }

    void Update()
    {
        if (m_player.m_menuState == MenuState.NONE)
        {
            if (Input.GetKeyDown(KeyCode.L))
                m_generateChunks = !m_generateChunks;
        }

        if (m_generateChunks)
        {
            // Create Chunks at necessary spots
            for (int x = -m_ChunkDistance; x <= m_ChunkDistance; x++)
            {
                for (int y = -m_ChunkDistance; y <= m_ChunkDistance; y++)
                {
                    for (int z = -m_ChunkDistance; z <= m_ChunkDistance; z++)
                    {
                        Vector3Int chunkSpot = m_player.m_chunkIndex + new Vector3Int(x, y, z);
                        //chunkSpot.y = 0;

                        Chunk chunkCheck = ChunkStorage.GetChunk(chunkSpot);
                        if (chunkCheck == null)
                        {
                            // Create chunk at location
                            GameObject chunkObject = (GameObject)Instantiate(GlobalData.prefab_chunk);
                            chunkObject.name = "Chunk: " + chunkSpot.x.ToString() + ", " + chunkSpot.y.ToString() + ", " + chunkSpot.z.ToString();
                            ChunkComponent chunkComponent = chunkObject.GetComponent<ChunkComponent>();

                            Chunk chunk = new Chunk(chunkComponent, chunkSpot);
                            
                            // Add New Chunk
                            ChunkStorage.SetChunk(chunk);
                            
                            //chunk.GenerateTest();
                            chunk.GenerateTerrain();
                            chunk.MakeDirty();

                            // Redraw all nearby chunks that may be modified from the new chunk existing
                            Chunk[] nearbyChunks = ChunkStorage.GetNeighborChunks(chunk);
                            foreach (Chunk c in nearbyChunks)
                            {
                                if (c != null)
                                    c.MakeDirty();
                            }
                        }
                    }
                }
            }
        }

        // Update all existing chunks
        //Chunk[] chunks = FindObjectsOfType<Chunk>();
        Chunk[] chunks = ChunkStorage.allData.ToArray();
        // Make sure chunks update based on distance from player

        chunks = chunks.OrderBy(
            x => Vector3.Distance(m_player.transform.position, x.GetCenter())
            ).ToArray();

        foreach(Chunk chunk in chunks)
        {
            // Reload Chunks
            if(Input.GetKeyDown(KeyCode.G))
            {
                chunk.GenerateTerrain();
                chunk.MakeDirty();
            }

            if(chunk.IsDirty())
            {
                if(chunk.IsMeshConstructed())
                {
                   // Debug.Log("Complete");
                    chunk.EndMeshConstruction();
                    chunk.MakeClean();
                    // Only create 1 Mesh per frame
                    break;
                }
                else
                {
                    chunk.BeginMeshConstruction();

                    //  if (chunk.BeginMeshConstruction())
                    //      Debug.Log("Build");
                    //  else
                    //      Debug.Log("Wait");
                }
            }
        
        }
    }
}
