using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class BlockParticles : MonoBehaviour
{
    [System.NonSerialized] public ParticleSystem m_particleSystem = null;
    [System.NonSerialized] public ParticleSystemRenderer m_particleSystemRenderer = null;
    private Mesh m_mesh = null;
    private Player m_player;

    private void Awake()
    {
        m_player = FindObjectOfType<Player>();
        m_particleSystem = GetComponent<ParticleSystem>();
        m_particleSystemRenderer = m_particleSystem.GetComponent<ParticleSystemRenderer>();
    }

    // Start is called before the first frame update
    public void SetMeshToBlock(uint textureID)
    {
        if (textureID >= (uint)BlockManager.blockTextureArray.depth)
        {
            Debug.LogError("Invalid Texture ID for Particle");
            return;
        }

        // Create duplicate mesh from existing billboard quad one
        m_mesh = new Mesh();
        Vector3[] vertices =
        {
            new Vector3(-0.5f, -0.5f, 0),
            new Vector3(+0.5f, -0.5f, 0),
            new Vector3(+0.5f, +0.5f, 0),
            new Vector3(-0.5f, +0.5f, 0)
        };
        Vector2[] uvs =
        {
            new Vector2(-0, -0),
            new Vector2(+1, -0),
            new Vector2(+1, +1),
            new Vector2(-0, +1)
        };
        //  Vector3[] normals =
        //  {
        //      new Vector3(0, 0, 1),
        //      new Vector3(0, 0, 1),
        //      new Vector3(0, 0, 1),
        //      new Vector3(0, 0, 1)
        //  };
        //  Color[] colors =
        //  {
        //      new Color(1, 1, 1, 1),
        //      new Color(1, 1, 1, 1),
        //      new Color(1, 1, 1, 1),
        //      new Color(1, 1, 1, 1)
        //  };
        Vector2[] id =
        {
            new Vector2(textureID, 0),
            new Vector2(textureID, 0),
            new Vector2(textureID, 0),
            new Vector2(textureID, 0),
        };
        int[] triangles =
        {
            0, 2, 1,
            0, 3, 2
        };

        m_mesh.vertices = vertices;
        m_mesh.uv = uvs;
        //m_mesh.colors = colors;
        m_mesh.triangles = triangles;
        m_mesh.uv2 = id;

        //m_mesh.hideFlags = HideFlags.HideAndDontSave;
        var VBC = m_mesh.vertexBufferCount; // Unity is broken, use this to initialize data correctly

        //m_particleSystemRenderer.renderMode = ParticleSystemRenderMode.Mesh;
        m_particleSystemRenderer.mesh = m_mesh;
    }

    // Behaviour
    private void Update()
    {
        // Pause Self if needed
        if (m_player.UsingMenu())
            m_particleSystem.Pause();
        else if (m_particleSystem.isPaused)
            m_particleSystem.Play();

        // Destroy self if needed
        if (!m_particleSystem.IsAlive())
            Destroy(this.gameObject);
    }
}
