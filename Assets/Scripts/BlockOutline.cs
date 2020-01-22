using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockOutline : MonoBehaviour
{
    // Import
    public GameObject m_mesh;

    // Components
    private Player m_player;
    private Camera m_camera;

    // Data
    [System.NonSerialized] public Block   m_block = null;
    [System.NonSerialized] public Vector3 m_position = new Vector3();
    [System.NonSerialized] public Vector3 m_normal = new Vector3();

    private void Awake()
    {
        m_player = FindObjectOfType<Player>();
        m_camera = m_player.CameraObject.GetComponent<Camera>();
    }

    public void Update()
    {
        // Raycast and check which block you are looking at
        RaycastHit hit;
        Ray ray = new Ray(m_camera.transform.position, m_camera.transform.forward);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, m_player.m_chunkMask.value))
        {
            m_position = hit.point + m_camera.transform.forward * 0.01f;
            m_normal = hit.normal;
            Debug.Log(m_position);

            m_block = ChunkStorage.GetBlock(m_position);
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
