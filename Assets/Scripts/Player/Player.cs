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
        Vector3Int playerPositionGrounded = Utility.RoundWorldPosition(new Vector3(transform.position.x, transform.position.y, transform.position.z));
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

    // Left/Right Button
    //private bool PrevLeftButton = false;
    //private bool PrevRightButton = false;

    private float LeftButtonTime = 0;
    private float RightButtonTime = 0;

    private bool LeftButtonSlow = false;
    private bool RightButtonSlow = false;

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
            // Left/Right Button
            if (Input.GetMouseButton(0))
            {
                LeftButtonSlow = (Time.time > LeftButtonTime);
                if(LeftButtonSlow)
                    LeftButtonTime = Time.time + 0.1f;
            }
            else
                LeftButtonSlow = false;

            if (Input.GetMouseButton(1))
            {
                RightButtonSlow = (Time.time > RightButtonTime);
                if(RightButtonSlow)
                    RightButtonTime = Time.time + 0.1f;
            }
            else
                RightButtonSlow = false;

            // Edit Blocks
            if (m_blockOutline.HasHitBlock())
            {
                // Acquire Block player is looking at
                Vector3Int hitBlockPosition = Utility.RoundWorldPosition(m_blockOutline.m_position);
                Block hitBlock = ChunkStorage.GetBlock(hitBlockPosition);

                // Acquire nearby block of hitBlockPosition
                Vector3Int hitBlockPosition_Neighbor = Utility.RoundWorldPosition(m_blockOutline.m_position + m_blockOutline.m_normal);
                //Block hitBlock_Neighbor = ChunkStorage.GetBlock(hitBlockPosition_Neighbor);

                // Select Target Block [middle click]
                if (Input.GetMouseButtonDown(2) && hitBlock != null)
                {
                    // Set Sniper to block player is looking at
                    VoxelSniper.m_blockID = hitBlock.m_data.m_id;
                }

                // Break Block
                else if(VoxelSniper.m_sniperToolMode == SniperToolMode.BREAK)
                {
                    // Check Input
                    if (Input.GetMouseButtonDown(0))
                    {
                        WorldEdit.SetBlock(hitBlockPosition, 0, 0);

                        // Particle
                        //BlockInfo info = BlockDictionary.Get(VoxelSniper.m_blockID);
                        BlockInfo info = hitBlock.m_data;
                        if (info != null && info.m_textureIDs.Count > 0)
                        {
                            Vector3 blockCenter = new Vector3((float)hitBlockPosition.x + 0.5f, (float)hitBlockPosition.y + 0.5f, (float)hitBlockPosition.z + 0.5f);
                            GameObject p = (GameObject)Instantiate(GlobalData.prefab_blockBreakParticle, blockCenter, GlobalData.prefab_blockBreakParticle.transform.rotation);
                            p.GetComponent<BlockParticles>().SetMeshToBlock(info.m_textureIDs[0]);
                            p.GetComponent<ParticleSystem>().Play();
                        }

                    }
                    else if (Input.GetMouseButtonDown(1))
                        WorldEdit.SetBlock(hitBlockPosition_Neighbor, VoxelSniper.m_blockID, VoxelSniper.m_blockSubID);
                    
                }
                // Paint Block
                else if(VoxelSniper.m_sniperToolMode == SniperToolMode.PAINT)
                {
                    // Check Input
                    if(LeftButtonSlow)
                        VoxelSniper.Paint(hitBlockPosition, m_blockOutline.m_normalAxis);
                    else if(RightButtonSlow)
                        VoxelSniper.Paint(hitBlockPosition_Neighbor, m_blockOutline.m_normalAxis);
                }
            }


            // Toggle Sniper Tool
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (VoxelSniper.m_sniperToolMode == SniperToolMode.PAINT)
                    VoxelSniper.m_sniperToolMode = SniperToolMode.BREAK;
                else
                    VoxelSniper.m_sniperToolMode = SniperToolMode.PAINT;
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                if (VoxelSniper.m_sniperToolMode == SniperToolMode.SELECTION)
                    VoxelSniper.m_sniperToolMode = SniperToolMode.BREAK;
                else
                    VoxelSniper.m_sniperToolMode = SniperToolMode.SELECTION;
            }

            // Reset Sniper Tool
            else if(Input.GetKeyDown(KeyCode.K))
            {
                VoxelSniper.Reset();
            }

            // Toggle Chunk Lines
            else if (Input.GetKeyDown(KeyCode.F2))
                m_showChunkLines = !m_showChunkLines;

            // Toggle Wireframe
            else if (Input.GetKeyDown(KeyCode.F1))
                m_wireframeMode = !m_wireframeMode;

            // Toggle Noclip
            else if (Input.GetKeyDown(KeyCode.N))
                m_noclip = !m_noclip;

            // Enter Block Selector
            else if (Input.GetKeyDown(KeyCode.I))
                m_menuState = MenuState.BLOCK_SELECTION;

            // Enter Commands
            else if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Slash))
                m_menuState = MenuState.COMMAND;

            // Enter Pause
            else if (Input.GetKeyDown(KeyCode.Escape))
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
