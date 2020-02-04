using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class ChunkComponent : MonoBehaviour
{
    // Components
    private Player          m_player;
    private Chunk           m_chunk;
    private ChunkManager    m_chunkManager;

    private Mesh            m_mesh;
    private MeshFilter      m_meshFilter;
    private MeshRenderer    m_meshRenderer;
    private MeshCollider    m_meshCollider;

    private GameObject m_lineHolder;
    private LineRenderer[] m_lineRenderers;

    // Data
    [System.NonSerialized] public bool m_visible;

    // Utility
    public void SetChunk(Chunk chunk)
    {
        m_chunk = chunk;
    }
    public bool CanCameraSeeChunk()
    {
        if (m_chunk == null)
            return false;

        if (
            m_chunk.m_index.x >= m_player.m_chunkIndex.x - 1 &&
            m_chunk.m_index.x <= m_player.m_chunkIndex.x + 1 &&
            m_chunk.m_index.y >= m_player.m_chunkIndex.y - 1 &&
            m_chunk.m_index.y <= m_player.m_chunkIndex.y + 1 &&
            m_chunk.m_index.z >= m_player.m_chunkIndex.z - 1 &&
            m_chunk.m_index.z <= m_player.m_chunkIndex.z + 1
            )
            return true;

        // Check if camera can see the center
        Camera camera = m_player.GetCamera();
        Vector3 cameraToChunkCenter = m_chunk.m_center - camera.transform.position;

        // Check if in front of camera (more than 90 degress from camera forward = not seen)
        if (Vector3.Dot(cameraToChunkCenter, camera.transform.forward) > 0)
            return true;

        // Cannot be seen
        return false;
    }

    public void UpdateMesh(ChunkBuilder chunkBuilder)
    {
        m_mesh = new Mesh();
        m_mesh.name = "ChunkMesh";

        m_mesh.vertices     = chunkBuilder.vertices.ToArray();
        m_mesh.uv           = chunkBuilder.uvs.ToArray();
        m_mesh.uv2          = chunkBuilder.id.ToArray();
        m_mesh.normals      = chunkBuilder.normals.ToArray();
        m_mesh.triangles    = chunkBuilder.indices.ToArray();

        m_meshFilter.mesh = m_mesh;
        m_meshCollider.sharedMesh = m_mesh;
    }

    // Start is called before the first frame update
    private void Start()
    {
        m_player = GlobalData.player;
        m_chunkManager = FindObjectOfType<ChunkManager>();

        m_meshFilter    = GetComponent<MeshFilter>();
        m_meshRenderer  = GetComponent<MeshRenderer>();
        m_meshCollider  = GetComponent<MeshCollider>();
        m_meshRenderer.material = GlobalData.material_block;

        // Line Rendering
        m_lineHolder = new GameObject();
        m_lineHolder.name = "LineHolder";
        m_lineHolder.transform.SetParent(transform);

        int LineCount = 8;
        m_lineRenderers = new LineRenderer[LineCount];
        for(int i = 0; i < LineCount; i++)
        {
            GameObject g = (GameObject)Instantiate(GlobalData.prefab_line);
            g.transform.SetParent(m_lineHolder.transform);
            m_lineRenderers[i] = g.GetComponent<LineRenderer>();
            m_lineRenderers[i].startColor = Color.red;
            m_lineRenderers[i].endColor = Color.green;
            m_lineRenderers[i].positionCount = 2;
        }
        
        // Vertical Lines
        m_lineRenderers[0].SetPosition(0, m_chunk.m_bottomExtents[0]);
        m_lineRenderers[0].SetPosition(1, m_chunk.m_topExtents[0]);
        
        m_lineRenderers[1].SetPosition(0, m_chunk.m_bottomExtents[1]);
        m_lineRenderers[1].SetPosition(1, m_chunk.m_topExtents[1]);
        
        m_lineRenderers[2].SetPosition(0, m_chunk.m_bottomExtents[2]);
        m_lineRenderers[2].SetPosition(1, m_chunk.m_topExtents[2]);
        
        m_lineRenderers[3].SetPosition(0, m_chunk.m_bottomExtents[3]);
        m_lineRenderers[3].SetPosition(1, m_chunk.m_topExtents[3]);

        // Bottom Lines
        m_lineRenderers[4].SetPosition(0, m_chunk.m_bottomExtents[0]);
        m_lineRenderers[4].SetPosition(1, m_chunk.m_bottomExtents[1]);

        m_lineRenderers[5].SetPosition(0, m_chunk.m_bottomExtents[1]);
        m_lineRenderers[5].SetPosition(1, m_chunk.m_bottomExtents[2]);
        
        m_lineRenderers[6].SetPosition(0, m_chunk.m_bottomExtents[2]);
        m_lineRenderers[6].SetPosition(1, m_chunk.m_bottomExtents[3]);
        
        m_lineRenderers[7].SetPosition(0, m_chunk.m_bottomExtents[3]);
        m_lineRenderers[7].SetPosition(1, m_chunk.m_bottomExtents[0]);
    }

    // Update is called once per frame
    private void Update()
    {
        if(m_chunk == null)
        {
            Debug.LogError("ChunkComponent missing chunk reference");
            return;
        }

        //
        m_visible = CanCameraSeeChunk();

        //
        m_lineHolder.SetActive(m_player.m_showChunkLines);

        //
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

        // Destroy Self if too far away from player camera
        if (m_chunkManager.m_generateChunks)
        {
            Vector3Int manhattanDistance = (m_chunk.m_index - m_player.m_chunkIndex);
            if (
                Mathf.Abs(manhattanDistance.x) > ChunkManager.m_ChunkDistance ||
                Mathf.Abs(manhattanDistance.y) > ChunkManager.m_ChunkDistance ||
                Mathf.Abs(manhattanDistance.z) > ChunkManager.m_ChunkDistance
                )
            {
                ChunkStorage.RemoveChunk(m_chunk);
                Destroy(this.gameObject);
            }
        }
    }
}
