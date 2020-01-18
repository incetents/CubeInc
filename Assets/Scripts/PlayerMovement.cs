using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Components
    private Player m_player;

    // Options
    public float MinSpeed = 3.0f; // ?
    public float TopSpeed = 4.39f;
    public float SpeedMultOverTime = 5.5f;
    public float GroundDistance = 0.4f;
    public float Gravity = -0.5f;
    public float JumpHeight = 73.1f;
    public float TerminalVelocity = -11.63f;
    [Header("Floor Collision")]
    public LayerMask GroundMask;

    // Data
    private Camera              m_camera;
    private CharacterController m_controller;
    private Vector3             m_velocity = new Vector3();
    private bool                m_isGrounded = false;
    private Vector3             m_previousPosition = new Vector3();
    private float               m_currentSpeed = 0.0f;
    private float               m_timeMoving = 0.0f;


    void Awake()
    {
        m_player = GetComponent<Player>();
        m_controller = GetComponent<CharacterController>();
        m_camera = m_player.CameraObject.GetComponent<Camera>();
    }

    void Update()
    {
        if (!m_player.isPaused())
        {
            // Move
            Vector3 XZ_Forward = new Vector3(m_camera.transform.forward.x, 0, m_camera.transform.forward.z);
            Vector3 XZ_Right = new Vector3(m_camera.transform.right.x, 0, m_camera.transform.right.z);

            // Change in XZ Movement
            Vector3 XZ_Change = new Vector3(m_controller.transform.position.x - m_previousPosition.x, 0, m_controller.transform.position.z - m_previousPosition.z);
            if (XZ_Change.sqrMagnitude > 0.0f)
                m_timeMoving += Time.deltaTime;
            else
                m_timeMoving = 0.0f;

            float t = Mathf.Clamp01(m_timeMoving * SpeedMultOverTime);
            m_currentSpeed = Mathf.Lerp(MinSpeed, TopSpeed, t * t);

            Debug.Log(m_currentSpeed);

            //Debug.Log(m_timeMoving + " " + t + " " + m_currentSpeed);

            // Record prev position
            m_previousPosition = m_controller.transform.position;

            if (Input.GetKey(KeyCode.W))
            {
                m_controller.Move(XZ_Forward.normalized * m_currentSpeed * Time.deltaTime);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                m_controller.Move(-XZ_Forward.normalized * m_currentSpeed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                m_controller.Move(XZ_Right.normalized * m_currentSpeed * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.A))
            {
                m_controller.Move(-XZ_Right.normalized * m_currentSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.Space) && m_isGrounded)
            {
                m_velocity.y = Mathf.Sqrt(JumpHeight * -2.0f * Gravity);
            }

            // Falling
            if (m_velocity.y < 0.0f)
            {
                RaycastHit hit;

                Vector3 controllerBottom = transform.position + m_controller.center + Vector3.down * m_controller.height * 0.5f;
                Vector3 sphereCastPoint = controllerBottom + Vector3.up * (m_controller.radius + 0.05f);

                if (Physics.SphereCast(sphereCastPoint, m_controller.radius - 0.01f, Vector3.down, out hit, Mathf.Infinity, GroundMask.value))
                {
                    Debug.DrawLine(controllerBottom, hit.point, Color.red);
                    float distance = (controllerBottom.y - hit.point.y);
                    m_isGrounded = distance < 0.1f && distance > -0.05f;
                }
                else
                    m_isGrounded = false;
            }
            else
                m_isGrounded = false;

            // On Ground
            if (m_isGrounded && m_velocity.y < 0.1f)
            {
                m_velocity.y = -2.0f;
            }
            // In Air
            else
            {
                // Hit Ceiling
                if ((m_controller.collisionFlags & CollisionFlags.Above) != 0)
                {
                    m_velocity.y = Mathf.Min(m_velocity.y, 0);
                }

                // Gravity
                m_velocity.y += Gravity;
                m_velocity.y = Mathf.Max(m_velocity.y, TerminalVelocity);
                m_controller.Move(m_velocity * Time.deltaTime);
            }

          
        }
    }
}
