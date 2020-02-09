using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDebugMenu : MonoBehaviour
{
    // Objects
    private Player m_player;
    private ChunkManager m_chunkManager;

    // Objects to control
    public TextMeshProUGUI m_textMesh;

    // Data
    private TextProUtility m_display = new TextProUtility();

    // FPS
    private float[]     m_rollingFPS = new float[10];
    private int         m_rollgingFPS_index = 0;
    private float       m_averageFPS = 0;

    // Behaviour
    private void Start()
    {
        m_player = GlobalData.player;
        m_chunkManager = FindObjectOfType<ChunkManager>();
    }
    private void Update()
    {
        // Fix Text Rect Transform to always adjust to screen
        //RectTransform textTransform = m_textMesh.GetComponent<RectTransform>();
        //textTransform.sizeDelta = new Vector2(1920 - 12, 1080 - 12);

        // Calc FPS
        float fps = (1.0f / Time.smoothDeltaTime);
        m_rollingFPS[m_rollgingFPS_index] = fps;
        m_rollgingFPS_index = (++m_rollgingFPS_index % m_rollingFPS.Length);
        m_averageFPS = 0.0f;
        for (int i = 0; i < m_rollingFPS.Length; i++)
            m_averageFPS += m_rollingFPS[i];
        m_averageFPS /= (float)m_rollingFPS.Length;

        // Text
        m_display.Clear();
        m_display.Write("Debug Info:");
        m_display.Write("FPS", m_averageFPS, Color.yellow);
        m_display.Write("Position", m_player.transform.position, Color.yellow);
        m_display.Write("ChunkID", m_player.m_chunkIndex, Color.yellow);

        m_display.NewLine();

        m_display.Write("[L] Load Chunks", m_chunkManager.m_generateChunks, Color.green, Color.red);
        m_display.Write("[N] Noclip", m_player.m_noclip, Color.green, Color.red);
        m_display.Write("[F1] Wireframe", m_player.m_wireframeMode, Color.green, Color.red);
        m_display.Write("[F2] Chunk Outlines", m_player.m_showChunkLines, Color.green, Color.red);

        m_display.NewLine();

        m_display.Write("Tool Info:");

        m_display.Write("Type", System.Enum.GetName(typeof(ToolType), m_player.m_editorTool.m_toolType), Color.yellow);
        m_display.Write("Size", m_player.m_editorTool.m_size, Color.yellow);
        m_display.Write("BlockID", m_player.m_editorTool.m_blockID, Color.yellow);
        m_display.Write("BlockSubID", m_player.m_editorTool.m_blockSubID, Color.yellow);

        // Result
        m_textMesh.text = m_display.text;
    }
}
