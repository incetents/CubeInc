using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Components
    private Player m_player;

    // Settings
    [Header("Settings")]
    public float X_Sensitivity = 1.0f;
    public float Y_Sensitivity = 1.0f;
    [Header("Raycast Collision")]
    public LayerMask ChunkMask;

    // Data
    private Vector2 m_mouseLook = new Vector2(0, 0);
    private Camera  m_mainCamera;
    private Vector3 m_lookBlockPosition = new Vector3();
    private Block   m_lookBlock = null;
    private Chunk   m_lookBlockChunk = new Chunk();
    private Vector3 m_lookNormal = new Vector3();

    void Awake()
    {
        m_player = GetComponent<Player>();
        m_mainCamera = m_player.CameraObject.GetComponent<Camera>();
    }

    void Update()
    {
        if (m_player.m_windowFocus && !m_player.m_paused)
        {
            // Raycast and check which block you are looking at
            RaycastHit hit;
            Ray ray = new Ray(m_mainCamera.transform.position, m_mainCamera.transform.forward);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ChunkMask.value))
            {
                m_lookBlockPosition = hit.point + m_mainCamera.transform.forward * 0.01f;
                m_lookNormal = hit.normal;

                m_lookBlock = ChunkManager.getBlock(m_lookBlockPosition);
                m_lookBlockChunk = ChunkManager.getChunkPosition(m_lookBlockPosition);
            }
            else
            {
                m_lookBlock = null;
                m_lookBlockChunk = null;
            }

            // Destroy Block
            if (Input.GetMouseButtonDown(0) && m_lookBlock != null && m_lookBlockChunk != null)
            {
                // Become Air
                Block block = m_lookBlock;
                block.m_data = BlockAppendix.GetData(0);
                m_lookBlock = null;
                // Dirty chunk
                m_lookBlockChunk.m_meshDirty = true;
                m_lookBlockChunk = null;
            }
            // Add Block
            if (Input.GetMouseButtonDown(1) && m_lookBlock != null && m_lookBlockChunk != null)
            {
                // Update new block
                Block newBlock = ChunkManager.getBlock(m_lookBlockPosition + m_lookNormal);
                if (newBlock != null)
                {
                    newBlock.m_data = BlockAppendix.GetData(1);
                    // Update relative chunk
                    Chunk chunk = ChunkManager.getChunkPosition(m_lookBlockPosition + m_lookNormal);
                    chunk.m_meshDirty = true;
                }
            }

            // Draw Block outline
            if (m_lookBlock != null)
            {
                m_player.m_blockOutline.SetActive(true);
                m_player.m_blockOutline.transform.position = new Vector3Int(Mathf.FloorToInt(m_lookBlockPosition.x), Mathf.FloorToInt(m_lookBlockPosition.y), Mathf.FloorToInt(m_lookBlockPosition.z)); ;
            }
            else
            {
                m_player.m_blockOutline.SetActive(false);
            }

            // Get Mouse Values
            Vector2 mouse = new Vector2(Input.GetAxis("Mouse X") * X_Sensitivity, Input.GetAxis("Mouse Y") * Y_Sensitivity);
            m_mouseLook += mouse;
            m_mouseLook.y = Mathf.Clamp(m_mouseLook.y, -89.9f, 89.9f);

            // Rotate Camera Accordingly
            m_mainCamera.transform.localRotation = Quaternion.AngleAxis(-m_mouseLook.y, Vector3.right);
            Vector3 Euler = m_mainCamera.transform.eulerAngles;
            Euler.y = m_mouseLook.x;
            m_mainCamera.transform.eulerAngles = Euler;
        }
    }
}
