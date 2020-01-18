using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDebugMenu : MonoBehaviour
{
    // Objects
    private Player m_player;

    // Objects to control
    public TextMeshProUGUI m_textMesh;

    // Data
    //public List<string> m_display;

    // Behaviour
    private void Awake()
    {
        m_player = FindObjectOfType<Player>();
    }
    private void Update()
    {
        string result = "Debug Info:\n";

        if (m_player.m_noclip)
            result += "[N] Noclip: ON\n";
        else
            result += "[N] Noclip: OFF\n";

        if (m_player.m_wireframeMode)
            result += "[F1] Wireframe: ON";
        else
            result += "[F1] Wireframe: OFF";

        // Result
        m_textMesh.text = result;
    }
}
