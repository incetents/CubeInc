using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class BlockDisplayer : MonoBehaviour
{
    // Reference
    private CanvasRenderer m_render = null;
    private RectTransform m_rectTransform = null;
    private Image m_image = null;

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
        if(internalSelectedBlock != null && !internalSelectedBlock.m_air)
            id = internalSelectedBlock.m_textureIDs[0];

        m_quadID[0] = new Vector2(id, 0);
        m_quadID[1] = new Vector2(id, 0);
        m_quadID[2] = new Vector2(id, 0);
        m_quadID[3] = new Vector2(id, 0);
    }

    // Data
    private int     m_setupChecks = 2; // Unity is finicky with its setup :thonk:
    private Vector2 m_size;

    // Settings
    [System.NonSerialized] public bool m_visible = true;

    // Id to display
    //public  int id;
    //private int internalID;\
    [System.NonSerialized] public BlockInfo selectedBlock = null;
    private BlockInfo internalSelectedBlock = null;

    // Text for name
    public GameObject textBG;
    public TextMeshProUGUI textMesh;

    // Start is called before the first frame update
    void Start()
    {
        m_render = GetComponent<CanvasRenderer>();
        m_rectTransform = GetComponent<RectTransform>();
        m_image = GetComponent<Image>();

        m_size = m_rectTransform.sizeDelta;

        m_quadVertices = new Vector3[4];
        CalculateNewVertices();

        m_quadID = new Vector2[4];
        CalculateNewIDs();
        //internalID = id;
        internalSelectedBlock = selectedBlock;

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
        if(
            !Mathf.Approximately(m_rectTransform.sizeDelta.x, m_size.x) ||
            !Mathf.Approximately(m_rectTransform.sizeDelta.y, m_size.y)
            )
        {
            m_size = m_rectTransform.sizeDelta;

            CalculateNewVertices();
            CalculateNewMesh();
           
            m_setupChecks = 2;
        }

        // Check if ID changes
        if(selectedBlock != null && internalSelectedBlock != selectedBlock)
        {
            internalSelectedBlock = selectedBlock;

            CalculateNewIDs();
            CalculateNewMesh();

            m_setupChecks = 2;
        }

        // Force invisible if no selectedBlock
        if (internalSelectedBlock == null)
            m_image.enabled = false;
        else
        {
            // Visible
            if (m_image.enabled != m_visible)
            {
                m_image.enabled = m_visible;
                m_setupChecks = 2;
            }
        }

        // Update new mesh
        if (m_setupChecks > 0 && m_image.enabled)
        {
            m_render.SetMesh(m_mesh);
            m_setupChecks--;
        }

        // Update Text
        textMesh.gameObject.SetActive(internalSelectedBlock != null);
        textBG.gameObject.SetActive(internalSelectedBlock != null);

        if(internalSelectedBlock != null)
        {
            textMesh.text = internalSelectedBlock.m_name;
        }
    }
}
