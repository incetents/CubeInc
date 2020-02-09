using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState
{
    NONE,
    PAUSED,
    COMMAND
}

public enum ToolType
{
    PENCIL,
    BALL,
    VOXEL,
    LINE
}

public class EditorTool
{
    public ToolType m_toolType;
    public uint     m_size = 1;
    public uint     m_blockID = 0;
    public uint     m_blockSubID = 0;
}

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

    public float PerlinScale = 30.0f;
    [Range(0.0f, 1.0f)]
    public float PerlinAlpha = 0.5f;

    // Data
    [System.NonSerialized] public MenuState m_menuState = MenuState.COMMAND;
    [System.NonSerialized] public bool m_windowFocus = true;
    [System.NonSerialized] public bool m_noclip = true;
    [System.NonSerialized] public bool m_debugMenu = true;
    [System.NonSerialized] public bool m_wireframeMode = false;
    [System.NonSerialized] public EditorTool m_editorTool = new EditorTool();
    [System.NonSerialized] public bool m_showChunkLines = true;

    // Current Chunk the player resides in
    [System.NonSerialized] public Vector3Int m_chunkIndex = new Vector3Int(0, 0, 0);
    private void UpdateCurentInternalChunk()
    {
        Vector3 playerPositionGrounded = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        m_chunkIndex = Chunk.ConvertToChunkIndex(playerPositionGrounded);
    }

    // Utility
    public bool UsingMenu()
    {
        return m_menuState != MenuState.NONE;
    }
    public Camera GetCamera()
    {
        return m_playerCamera.GetCamera();
    }

    // Behaviour
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        UpdateCurentInternalChunk();

        m_playerMovement = GetComponent<PlayerMovement>();
        m_playerCamera = GetComponent<PlayerCamera>();
        m_controller = GetComponent<CharacterController>();
    }
    private void Start()
    {
        GameObject gameObject = (GameObject)Instantiate(GlobalData.prefab_blockOutline);
        m_blockOutline = gameObject.GetComponent<BlockOutline>();
    }

    private void Update()
    {
        // Update Chunk the player is in
        UpdateCurentInternalChunk();

        // Game Functionality [ No Menu ]
        if (m_menuState == MenuState.NONE)
        {
            // Destroy Block
            if (Input.GetMouseButtonDown(0) && m_blockOutline.HasHitBlock())
            {
                Block block = ChunkStorage.GetBlock(m_blockOutline.m_position);
                if (block != null)
                {
                    //  if (m_bigBreak)
                    //      WorldEdit.SetBlockRegion(
                    //          m_blockOutline.m_position + new Vector3(-1, -1, -1),
                    //          m_blockOutline.m_position + new Vector3(+1, +1, +1),
                    //          0
                    //          );
                    //  else
                        WorldEdit.SetBlock(m_blockOutline.m_position, new Block(0, block.m_localPosition));
                }
            }
            // Add Block
            if (Input.GetMouseButtonDown(1) && m_blockOutline.HasHitBlock())
            {
                // Update new block
                Block block = ChunkStorage.GetBlock(m_blockOutline.m_position + m_blockOutline.m_normal);
                if (block != null)
                {
                    WorldEdit.SetBlock(m_blockOutline.m_position + m_blockOutline.m_normal, new Block(2, block.m_localPosition));
                }
            }

            // Toggle Chunk Lines
            if (Input.GetKeyDown(KeyCode.F2))
                m_showChunkLines = !m_showChunkLines;

            // Toggle Debugmenu
            if (Input.GetKeyDown(KeyCode.Tab))
                m_debugMenu = !m_debugMenu;

            // Toggle Wireframe
            if (Input.GetKeyDown(KeyCode.F1))
                m_wireframeMode = !m_wireframeMode;

            // Toggle Noclip
            if (Input.GetKeyDown(KeyCode.N))
                m_noclip = !m_noclip;

            // Enter Commands
            if (Input.GetKeyDown(KeyCode.Return))
                m_menuState = MenuState.COMMAND;

            // Enter Pause
            if (Input.GetKeyDown(KeyCode.Escape))
                m_menuState = MenuState.PAUSED;

        }
        // Pause
        else if (m_menuState == MenuState.PAUSED)
        {
            // Exit
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_menuState = MenuState.NONE;
            }
        }
        else if (m_menuState == MenuState.COMMAND)
        {
            // Exit Commands
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_menuState = MenuState.NONE;
            }
        }
         

        // Lock Mouse
        if (m_windowFocus && !UsingMenu())
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        // Collision
        m_controller.enabled = !m_noclip;

    }

    void OnApplicationFocus(bool hasFocus)
    {
        m_windowFocus = hasFocus;
    }
}
