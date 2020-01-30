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
    //public List<string> m_display;

    // Behaviour
    private void Start()
    {
        m_player = GlobalData.player;
        m_chunkManager = FindObjectOfType<ChunkManager>();
    }
    private void Update()
    {
        // Fix Text Rect Transform to always adjust to screen
        RectTransform textTransform = m_textMesh.GetComponent<RectTransform>();
        textTransform.sizeDelta = new Vector2(Screen.width - 12, Screen.height);

        // Text
        string result = "Debug Info:\n";

        float fps = (1.0f / Time.smoothDeltaTime);
        result += "FPS: " + fps.ToString() + '\n';

        result += "X: " + m_player.transform.position.x.ToString() + '\n';
        result += "Y: " + m_player.transform.position.y.ToString() + '\n';
        result += "Z: " + m_player.transform.position.z.ToString() + '\n';

        result += "[G] reGenerate Chunks\n";

        if (m_chunkManager.m_generateChunks)
            result += "[L] Load Chunks: ON\n";
        else
            result += "[L] Load Chunks: OFF\n";

        if (m_player.m_noclip)
            result += "[N] Noclip: ON\n";
        else
            result += "[N] Noclip: OFF\n";

        if (m_player.m_wireframeMode)
            result += "[F1] Wireframe: ON\n";
        else
            result += "[F1] Wireframe: OFF\n";

        if (m_player.m_bigBreak)
            result += "[B] BigBreak: ON\n";
        else
            result += "[B] BigBreak: OFF\n";

        // Result
        m_textMesh.text = result;
    }
}
