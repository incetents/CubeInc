using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // Components
    private Player m_player;

    // Settings
    public float X_Sensitivity = 1.0f;
    public float Y_Sensitivity = 1.0f;

    // Data
    private Vector2 m_mouseLook = new Vector2(0, 0);
    private Camera  m_mainCamera;

    void Awake()
    {
        m_player = GetComponent<Player>();
        m_mainCamera = m_player.CameraObject.GetComponent<Camera>();
    }

    void Update()
    {
        if (!m_player.isPaused())
        {
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
