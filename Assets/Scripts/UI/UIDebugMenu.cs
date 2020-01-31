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
    private string m_display;

    private float[]     m_rollingFPS = new float[5];
    private int         m_rollgingFPS_index = 0;
    private float       m_averageFPS = 0;

    private void StartColor(Color c)
    {
        m_display += "<color=#" + ColorUtility.ToHtmlStringRGB(c) + '>';
    }
    private void EndColor()
    {
        m_display += "</color>";
    }

    private void Write(string text)
    {
        m_display += text + '\n';
    }

    private void Write(string text, bool value)
    {
        m_display += text + ": " + (value ? "[ON]" : "[OFF]") + '\n';
    }
    private void Write(string text, bool value, Color colorOn, Color colorOff)
    {
        m_display += text + ": ";
        StartColor(value ? colorOn : colorOff);
        m_display += (value ? "[ON]" : "[OFF]");
        EndColor();
        m_display += '\n';
    }

    private void Write(string text, float value)
    {
        m_display += text + ": " + value.ToString("F2") + '\n';
    }
    private void Write(string text, float value, Color color)
    {
        m_display += text + ": ";
        StartColor(color);
        m_display += value.ToString("F2");
        EndColor();
        m_display += '\n';
    }

    private void Write(string text, Vector3 vec)
    {
        m_display += text + ": [X: " + vec.x.ToString("F2") + ",Y: " + vec.y.ToString("F2") + ",Z: " + vec.z.ToString("F2") + "]\n";
    }
    private void Write(string text, Vector3 vec, Color color)
    {
        m_display += text + ": ";
        StartColor(color);
        m_display += "[X: " + vec.x.ToString("F2") + ", Y: " + vec.y.ToString("F2") + ", Z: " + vec.z.ToString("F2") + ']';
        EndColor();
        m_display += '\n';
    }


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
        m_display = "Debug Info:\n";

        // Calc FPS
        float fps = (1.0f / Time.smoothDeltaTime);
        m_rollingFPS[m_rollgingFPS_index] = fps;
        m_rollgingFPS_index = (++m_rollgingFPS_index % 5);
        m_averageFPS = 0.0f;
        for (int i = 0; i < 5; i++)
            m_averageFPS += m_rollingFPS[i];
        m_averageFPS /= 5.0f;

        Write("FPS", m_averageFPS, Color.yellow);

        Write("Position", m_player.transform.position, Color.yellow);

        m_display += '\n';
        Write("[L] Load Chunks", m_chunkManager.m_generateChunks, Color.green, Color.red);
        Write("[N] Noclip", m_player.m_noclip, Color.green, Color.red);
        Write("[F1] Wireframe", m_player.m_wireframeMode, Color.green, Color.red);
        Write("[B] BigBreak", m_player.m_bigBreak, Color.green, Color.red);

        // Text
        //  string result = "Debug Info:\n";
        //  
        //  
        //  result += "FPS: " + fps.ToString() + '\n';
        //  
        //  result += "X: " + m_player.transform.position.x.ToString() + '\n';
        //  result += "Y: " + m_player.transform.position.y.ToString() + '\n';
        //  result += "Z: " + m_player.transform.position.z.ToString() + '\n';
        //  
        //  result += "<color=#FF0000> blablabla </color>";
        //  result += "[G] reGenerate Chunks\n";
        //  
        //  if (m_chunkManager.m_generateChunks)
        //      result += "[L] Load Chunks: ON\n";
        //  else
        //      result += "[L] Load Chunks: OFF\n";
        //  
        //  if (m_player.m_noclip)
        //      result += "[N] Noclip: ON\n";
        //  else
        //      result += "[N] Noclip: OFF\n";
        //  
        //  if (m_player.m_wireframeMode)
        //      result += "[F1] Wireframe: ON\n";
        //  else
        //      result += "[F1] Wireframe: OFF\n";
        //  
        //  if (m_player.m_bigBreak)
        //      result += "[B] BigBreak: ON\n";
        //  else
        //      result += "[B] BigBreak: OFF\n";

        // Result
        //m_textMesh.text = result;
        m_textMesh.text = m_display;
    }
}
