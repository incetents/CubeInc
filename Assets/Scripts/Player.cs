using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Components
    private PlayerMovement m_playerMovement;
    private PlayerCamera m_playerCamera;

    // Objects to control
    public GameObject CameraObject;

    // Data
    private bool m_windowFocus = true;
    private bool m_paused = false;

    // Utility
    public bool isPaused()
    {
        return m_paused;
    }

    // Behaviour
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        m_playerMovement = GetComponent<PlayerMovement>();
        m_playerCamera = GetComponent<PlayerCamera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            m_paused = !m_paused;

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
