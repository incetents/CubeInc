using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    // Options
    public float X_Sensitivity = 1.0f;
    public float Y_Sensitivity = 1.0f;
    public float Speed = 10.0f;
    public float GroundDistance = 0.4f;
    public float Gravity = -9.81f;
    public float JumpHeight = 10.0f;
    public float TerminalVelocity = -30.0f;
    public LayerMask GroundMask;

    // Objects to control
    public GameObject CameraObject;
    public GameObject GroundObject;

    // Data
    private Vector2 mouseLook = new Vector2(0, 0);
    private Camera m_mainCamera;
    private CharacterController m_controller;
    private Vector3 velocity = new Vector3();
    private bool isGrounded = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        m_mainCamera = CameraObject.GetComponent<Camera>();
        m_controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // FPS Controls
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // Get Mouse Values
            Vector2 mouse = new Vector2(Input.GetAxis("Mouse X") * X_Sensitivity, Input.GetAxis("Mouse Y") * Y_Sensitivity);
            mouseLook += mouse;
            mouseLook.y = Mathf.Clamp(mouseLook.y, -89.9f, 89.9f);

            // Rotate Camera Accordingly
            m_mainCamera.transform.localRotation = Quaternion.AngleAxis(-mouseLook.y, Vector3.right);
            Vector3 Euler = m_mainCamera.transform.eulerAngles;
            Euler.y = mouseLook.x;
            m_mainCamera.transform.eulerAngles = Euler;

            // Move
            Vector3 XZ_Forward = new Vector3(m_mainCamera.transform.forward.x, 0, m_mainCamera.transform.forward.z);
            Vector3 XZ_Right = new Vector3(m_mainCamera.transform.right.x, 0, m_mainCamera.transform.right.z);

            if (Input.GetKey(KeyCode.W))
            {
                m_controller.Move(XZ_Forward.normalized * Speed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                m_controller.Move(-XZ_Forward.normalized * Speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                m_controller.Move(XZ_Right.normalized * Speed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                m_controller.Move(-XZ_Right.normalized * Speed * Time.deltaTime);
            }

            isGrounded = Physics.CheckSphere(GroundObject.transform.position, GroundDistance, GroundMask);

            if (Input.GetKey(KeyCode.Space) && isGrounded)
            {
                velocity.y = Mathf.Sqrt(JumpHeight * -2.0f * Gravity);
                isGrounded = false;
            }

            if (isGrounded && velocity.y < 0)
                velocity.y = -2.0f;
            else
            {
                // Gravity
                velocity.y += Gravity;
                velocity.y = Mathf.Max(velocity.y, TerminalVelocity);
                m_controller.Move(velocity * Time.deltaTime);
            }

        }

        // Toggle Usability
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
            else
                Cursor.lockState = CursorLockMode.None;
        }
    }
}
