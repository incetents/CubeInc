using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MenuState
{
    NONE,
    PAUSED,
    COMMAND,
    BLOCK_SELECTION
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
    [System.NonSerialized] public MenuState m_menuState = MenuState.NONE;
    [System.NonSerialized] public bool m_windowFocus = true;
    [System.NonSerialized] public bool m_noclip = true;
    [System.NonSerialized] public bool m_debugMenu = true;
    [System.NonSerialized] public bool m_wireframeMode = false;
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

        // Toggle Debugmenu
        if (Input.GetKeyDown(KeyCode.Tab))
            m_debugMenu = !m_debugMenu;

        // Game Functionality [ No Menu ]
        if (m_menuState == MenuState.NONE)
        {
            // Select Target Block [middle click]
            if (Input.GetMouseButtonDown(2) && m_blockOutline.HasHitBlock())
            {
                Vector3Int hitBlockPosition = Chunk.RoundWorldPosition(m_blockOutline.m_position);
                Block hitBlock = ChunkStorage.GetBlock(hitBlockPosition);
                // Set Sniper to block player is looking at
                VoxelSniper.m_blockID = hitBlock.m_data.m_id;
            }

            // Paint/Add Block
            else if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && m_blockOutline.HasHitBlock())
            {
                Vector3Int hitBlockPosition = Vector3Int.zero;

                // Paint
                if (Input.GetMouseButtonDown(0))
                    hitBlockPosition = Chunk.RoundWorldPosition(m_blockOutline.m_position);

                // Add
                else if (Input.GetMouseButtonDown(1))
                    hitBlockPosition = Chunk.RoundWorldPosition(m_blockOutline.m_position + m_blockOutline.m_normal);

                Block hitBlock = ChunkStorage.GetBlock(hitBlockPosition);
                if (hitBlock != null)
                {
                    if (VoxelSniper.m_sniperToolMode == SniperToolMode.PAINT)
                    {
                        switch (VoxelSniper.m_brushType)
                        {
                            case BrushType.PENCIL:
                                WorldEdit.SetBlock(hitBlockPosition, VoxelSniper.m_blockID, VoxelSniper.m_blockSubID);
                                break;

                            case BrushType.VOXEL:
                                WorldEdit.SetBlockRegion(
                                    hitBlockPosition + new Vector3((int)-VoxelSniper.m_brushSize, (int)-VoxelSniper.m_brushSize, (int)-VoxelSniper.m_brushSize),
                                    hitBlockPosition + new Vector3((int)+VoxelSniper.m_brushSize, (int)+VoxelSniper.m_brushSize, (int)+VoxelSniper.m_brushSize),
                                    VoxelSniper.m_blockID, VoxelSniper.m_blockSubID
                                    );
                                break;

                            case BrushType.BALL:
                                WorldEdit.SetBlockSphere(
                                    hitBlockPosition, new Vector3(VoxelSniper.m_brushSize, VoxelSniper.m_brushSize, VoxelSniper.m_brushSize),
                                     VoxelSniper.m_blockID, VoxelSniper.m_blockSubID
                                    );
                                break;

                            case BrushType.DISC:
                                WorldEdit.SetBlockSphere(
                                    hitBlockPosition, new Vector3(VoxelSniper.m_brushSize, 0.5f, VoxelSniper.m_brushSize),
                                     VoxelSniper.m_blockID, VoxelSniper.m_blockSubID
                                    );
                                break;

                            case BrushType.VOXEL_DISC:
                                WorldEdit.SetBlockRegion(
                                    hitBlockPosition + new Vector3((int)-VoxelSniper.m_brushSize, 0.5f, (int)-VoxelSniper.m_brushSize),
                                    hitBlockPosition + new Vector3((int)+VoxelSniper.m_brushSize, 0.5f, (int)+VoxelSniper.m_brushSize),
                                    VoxelSniper.m_blockID, VoxelSniper.m_blockSubID
                                    );
                                break;
                        }
                    }
                }
            }

            // Toggle Sniper Tool
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (VoxelSniper.m_sniperToolMode == SniperToolMode.PAINT)
                    VoxelSniper.m_sniperToolMode = SniperToolMode.SELECTION;
                else
                    VoxelSniper.m_sniperToolMode = SniperToolMode.PAINT;
            }

            // Reset Sniper Tool
            if(Input.GetKeyDown(KeyCode.K))
            {
                VoxelSniper.Reset();
            }

            // Toggle Chunk Lines
            if (Input.GetKeyDown(KeyCode.F2))
                m_showChunkLines = !m_showChunkLines;

            // Toggle Wireframe
            if (Input.GetKeyDown(KeyCode.F1))
                m_wireframeMode = !m_wireframeMode;

            // Toggle Noclip
            if (Input.GetKeyDown(KeyCode.N))
                m_noclip = !m_noclip;

            // Enter Block Selector
            if (Input.GetKeyDown(KeyCode.I))
                m_menuState = MenuState.BLOCK_SELECTION;

            // Enter Commands
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Slash))
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
        // Commands
        else if (m_menuState == MenuState.COMMAND)
        {
            // Exit
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                m_menuState = MenuState.NONE;
            }
        }
        // Block Selector
        else if(m_menuState == MenuState.BLOCK_SELECTION)
        {
            // Exit
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.I))
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
