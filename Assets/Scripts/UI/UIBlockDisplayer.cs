using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image), typeof(UIButtonAABB))]
public class UIBlockDisplayer : MonoBehaviour
{
    // Reference
    private CanvasRenderer m_render = null;
    private RectTransform m_rectTransform = null;
    private Image m_image = null;
    private Image m_BGimage = null;
    private Image m_TextBGImage = null;
    private UIButtonAABB m_button = null;

    // Reference Objects
    public GameObject textBG;
    public TextMeshProUGUI textMesh;
    public GameObject BG;

    // Customized Mesh Data
    private Mesh m_mesh = null;
    private void CalculateNewMesh()
    {
        m_mesh = new Mesh();
        m_mesh.vertices = m_quadVertices;
        m_mesh.uv = m_quadUvs;
        m_mesh.uv2 = m_quadID;
        m_mesh.triangles = m_quadIndices;
    }
    private Vector3[] m_quadVertices;
    private void CalculateNewVertices()
    {
        float halfW = m_size.x / 2;
        float halfH = m_size.y / 2;
        m_quadVertices[0] = new Vector3(-halfW, -halfH, 0);
        m_quadVertices[1] = new Vector3(+halfW, -halfH, 0);
        m_quadVertices[2] = new Vector3(+halfW, +halfH, 0);
        m_quadVertices[3] = new Vector3(-halfW, +halfH, 0);
    }
    private Vector2[] m_quadUvs =
    {
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(0, 1)
    };
    private int[] m_quadIndices = { 0, 2, 1, 0, 3, 2 };
    private Vector2[] m_quadID;
    private void CalculateNewIDs()
    {
        uint id = 0;
        if(m_block != null && !m_block.m_air)
            id = m_block.m_textureIDs[0];

        m_quadID[0] = new Vector2(id, 0);
        m_quadID[1] = new Vector2(id, 0);
        m_quadID[2] = new Vector2(id, 0);
        m_quadID[3] = new Vector2(id, 0);
    }

    // Data
    private Vector2     m_size;
    private BlockInfo   m_block = null;
    private bool        m_blockDirty = false;

    // Settings
    [System.NonSerialized] public bool m_visible = true;

    // Utility
    public void SetBlock(BlockInfo data)
    {
        if (m_block == data)
            return;

        m_block = data;

        if(m_block != null)
        {
            m_blockDirty = true;
        }
    }
    public BlockInfo GetBlock()
    {
        return m_block;
    }
    public bool IsPressed()
    {
        return m_button.IsPressed();
    }

    // Start is called before the first frame update
    void Start()
    {
        m_button = GetComponent<UIButtonAABB>();
        m_render = GetComponent<CanvasRenderer>();
        m_rectTransform = GetComponent<RectTransform>();
        m_image = GetComponent<Image>();
        m_BGimage = BG.GetComponent<Image>();
        m_TextBGImage = textBG.GetComponent<Image>();

        m_size = m_rectTransform.sizeDelta;

        m_quadVertices = new Vector3[4];
        CalculateNewVertices();

        m_quadID = new Vector2[4];
        CalculateNewIDs();

        m_mesh = new Mesh();
        m_mesh.vertices = m_quadVertices;
        m_mesh.uv = m_quadUvs;
        m_mesh.uv2 = m_quadID;
        m_mesh.triangles = m_quadIndices;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if size changes
        if (
            !Mathf.Approximately(m_rectTransform.sizeDelta.x, m_size.x) ||
            !Mathf.Approximately(m_rectTransform.sizeDelta.y, m_size.y)
            )
        {
            m_size = m_rectTransform.sizeDelta;

            CalculateNewVertices();
            CalculateNewMesh();
        }

        // Check if ID changes
        if(m_block != null && m_blockDirty)
        {
            m_blockDirty = false;

            CalculateNewIDs();
            CalculateNewMesh();
        }

        // Force invisible if no selectedBlock
        if (m_block == null || m_block.m_air)
            m_image.enabled = false;
        else
        {
            // Visible
            m_image.enabled = m_visible;
        }

        // Update new mesh
        if (m_image.enabled)
        {
            m_render.SetMesh(m_mesh);
        }

        // Update Text
        textMesh.gameObject.SetActive(m_block != null);
        textBG.gameObject.SetActive(m_block != null);

        if(m_block != null)
        {
            textMesh.text = m_block.m_name;
        }
    }
}
