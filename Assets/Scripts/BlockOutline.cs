using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockOutline : MonoBehaviour
{
    // Import
    public GameObject m_mesh;

    // References
    private Player m_player;

    // Data
    [System.NonSerialized] public Block   m_block = null;
    [System.NonSerialized] public Vector3 m_position = new Vector3();
    [System.NonSerialized] public Vector3 m_normal = new Vector3();

    private void Awake()
    {
        m_player = FindObjectOfType<Player>();
    }

    public void Update()
    {
        // Update Raycast
        Camera camera = m_player.CameraObject.GetComponent<Camera>();
        Debug.Log(camera.transform.position);

        // Raycast and check which block you are looking at
        RaycastHit hit;
        Ray ray = new Ray(camera.transform.position, camera.transform.forward);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_player.m_chunkMask.value))
        {
            m_position = hit.point + camera.transform.forward * 0.01f;
            m_normal = hit.normal;
            Debug.Log(m_position);

            m_block = ChunkManager.getBlock(m_position);
        }
        else
        {
            m_block = null;
        }

        // Draw Block outline
        if (m_block != null)
        {
            m_mesh.SetActive(true);
            m_mesh.transform.position = new Vector3Int(
                Mathf.FloorToInt(m_position.x),
                Mathf.FloorToInt(m_position.y),
                Mathf.FloorToInt(m_position.z)
                );
        }
        else
        {
            m_mesh.SetActive(false);
        }
    }
}
