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
        //string result = "";
        //foreach(string line in m_display)
        //{
        //    result += line;
        //}
        //m_textMesh.text = result;

        if (m_player.m_noclip)
            m_textMesh.text = "Debug:\nNoclip";
        else
            m_textMesh.text = "Debug:\n";
    }
}
