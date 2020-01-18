using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Objects to control
    public GameObject CameraObject;

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

    // Data
    private bool m_windowFocus = true;
    [HideInInspector] public bool m_paused = false;
    [HideInInspector] public bool m_noclip = false;
    [HideInInspector] public bool m_debugMenu = true;

    // Behaviour
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        m_playerMovement = GetComponent<PlayerMovement>();
        m_playerCamera = GetComponent<PlayerCamera>();
        m_controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Toggle Debugmenu
        if (Input.GetKeyDown(KeyCode.Tab))
            m_debugMenu = !m_debugMenu;

        // Toggle Noclip
        if (Input.GetKeyDown(KeyCode.N))
            m_noclip = !m_noclip;

        // Toggle Pause
        if (Input.GetKeyDown(KeyCode.Escape))
            m_paused = !m_paused;

        // Collision
        m_controller.enabled = !m_noclip;

        // Cursor
        if (m_windowFocus && !m_paused)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    void OnApplicationFocus(bool hasFocus)
    {
        m_windowFocus = hasFocus;
    }
}
