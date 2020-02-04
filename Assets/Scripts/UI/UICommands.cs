using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UICommands : MonoBehaviour
{
    // Objects
    private Player m_player;

    // Objects to control
    public TextMeshProUGUI m_text_history;
    public TextMeshProUGUI m_text_input;

    // Data
    private List<string> m_history = new List<string>();
    private const int m_historyMaximum = 100;
    private string m_input = "";
    private int m_cursorPosition = 0;

    private float m_holdLeftArrowKeyTime = 0.0f;
    private float m_holdRightArrowKeyTime = 0.0f;

    private void ParseKey(char key)
    {
        // Normal Text
        if (key >= ' ' && key <= '~')
        {
            // cursor position
            if(m_cursorPosition == m_input.Length)
                m_input += key;
            else
            {
                m_input =
                    m_input.Substring(0, m_cursorPosition) + // Before cursor
                    key + // new character
                    m_input.Substring(m_cursorPosition, m_input.Length - m_cursorPosition) // After cursor
                    ;
            }

            m_cursorPosition++;
        }
        // Delete (requires at least one character)
        if (m_input.Length > 0 && ((key == 127 || key == 8)))
        {
            m_input = m_input.Substring(0, m_input.Length - 1);
        }
    }

    // Behaviour
    void Start()
    {
        m_player = GlobalData.player;
    }

    void Update()
    {
        // Check input
        if (Input.inputString.Length > 0)
        {
            foreach (char c in Input.inputString)
            {
                ParseKey(c);
            }
        }

        // Hold down cursor key
        if (Input.GetKey(KeyCode.LeftArrow))
            m_holdLeftArrowKeyTime += Time.deltaTime;
        else
            m_holdLeftArrowKeyTime = 0;

        if (Input.GetKey(KeyCode.RightArrow))
            m_holdRightArrowKeyTime += Time.deltaTime;
        else
            m_holdRightArrowKeyTime = 0;

        // Move cursor position
        if (Input.GetKeyDown(KeyCode.LeftArrow) || m_holdLeftArrowKeyTime > 0.7f)
            m_cursorPosition--;

        else if (Input.GetKeyDown(KeyCode.RightArrow) || m_holdRightArrowKeyTime > 0.7f)
            m_cursorPosition++;

        m_cursorPosition = Mathf.Clamp(m_cursorPosition, 0, m_input.Length);

        // History
        string totalHistory = "";
        int indexStart = Mathf.Max(0, m_history.Count - 7);
        for(int i = indexStart; i < m_history.Count; i++)
              totalHistory += m_history[i] + '\n';
        m_text_history.text = totalHistory;

        // Flicker State
        bool flicker = ((Time.time % 1.0f) * 2.0f) < 1.0f;
        string flickerHexStr;
        if(flicker)
            flickerHexStr = "<color=#" + ColorUtility.ToHtmlStringRGB(Color.white) + ">|</color>";
        else
            flickerHexStr = "<color=#" + ColorUtility.ToHtmlStringRGB(Color.black) + ">|</color>";

        // Cursor Position
        if (m_cursorPosition == m_input.Length)
        {
            m_text_input.text = "> " + m_input + flickerHexStr;
        }
        else
        {
            string inputBeforeCursor = m_input.Substring(0, m_cursorPosition);
            string inputAfterCursor = m_input.Substring(m_cursorPosition, m_input.Length - m_cursorPosition);

            m_text_input.text = "> " + inputBeforeCursor + flickerHexStr + inputAfterCursor;
        }

        // Add to history
        if(Input.GetKeyDown(KeyCode.Return))
        {
            m_history.Add(m_input);
            if (m_history.Count > m_historyMaximum)
                m_history.RemoveAt(0);

            m_input = "";
        }
    }

}
