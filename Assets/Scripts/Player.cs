using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // References
    public GameObject CameraObject;

    // Objects to control
    private BlockOutline m_blockOutline;

    // Components
    private PlayerMovement m_playerMovement;
    private PlayerCamera m_playerCamera;
    private CharacterController m_controller;

    // Settings
    [Header("Settings")]
    public KeyCode m_key_forward = KeyCode.W;
    public KeyCode m_key_backwards = KeyCode.S;
    public KeyCode m_key_left = KeyCode.A;
    public KeyCode m_key_right = KeyCode.D;
    public KeyCode m_key_jump = KeyCode.Space;
    public KeyCode m_key_crouch = KeyCode.LeftControl;
    public KeyCode m_key_run = KeyCode.LeftShift;
    [Header("Raycast Collision")]
    public LayerMask m_chunkMask;

    // Data
    [System.NonSerialized] public bool m_windowFocus = true;
    [System.NonSerialized] public bool m_paused = false;
    [System.NonSerialized] public bool m_noclip = false;
    [System.NonSerialized] public bool m_debugMenu = true;
    [System.NonSerialized] public bool m_wireframeMode = false;

    // Behaviour
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        m_playerMovement = GetComponent<PlayerMovement>();
        m_playerCamera = GetComponent<PlayerCamera>();
        m_controller = GetComponent<CharacterController>();
    }
    private void Start()
    {
        GameObject gameObject = (GameObject)Instantiate(GlobalData.prefab_blockOutline);
        m_blockOutline = gameObject.GetComponent<BlockOutline>();
        //m_blockOutline.gameObject.SetActive(false);
    }

    private void Update()
    {
        // Destroy Block
        if (Input.GetMouseButtonDown(0) && m_blockOutline.m_block != null)
        {
            // Become Air
            m_blockOutline.m_block.m_data = BlockAppendix.GetData(0);
            // Dirty chunk
            ChunkManager.updateBlocksNearby(m_blockOutline.m_position);
        }
        // Add Block
        if (Input.GetMouseButtonDown(1) && m_blockOutline.m_block != null)
        {
            // Update new block
            Block newBlock = ChunkManager.getBlock(m_blockOutline.m_position + m_blockOutline.m_normal);
            if (newBlock != null)
            {
                newBlock.m_data = BlockAppendix.GetData(1);
                // Update relative chunk
                ChunkManager.updateBlocksNearby(m_blockOutline.m_position + m_blockOutline.m_normal);
            }
        }


        // Toggle Debugmenu
        if (Input.GetKeyDown(KeyCode.Tab))
            m_debugMenu = !m_debugMenu;

        // Toggle Wireframe
        if (Input.GetKeyDown(KeyCode.F1))
            m_wireframeMode = !m_wireframeMode;

        // Toggle Noclip
        if (Input.GetKeyDown(KeyCode.N))
            m_noclip = !m_noclip;

        // Toggle Pause
        if (Input.GetKeyUp(KeyCode.Escape))
            m_paused = !m_paused;

        // Lock Mouse
        if (m_windowFocus && !m_paused)
            Cursor.lockState = CursorLockMode.Locked;
        else
            Cursor.lockState = CursorLockMode.None;

        // Collision
        m_controller.enabled = !m_noclip;

    }

    void OnApplicationFocus(bool hasFocus)
    {
        m_windowFocus = hasFocus;
    }
}
