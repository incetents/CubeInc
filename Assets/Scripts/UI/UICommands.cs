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
    public RectTransform m_scrollbar;

    // Data
    private List<string> m_history = new List<string>();
    private int m_historyLocalMax = 7;
    private int m_historyOffset = 0;
    private const int m_historyMaximum = 100;
    private string m_input = "";
    private int m_cursorPosition = 0;

    private float m_holdLeftArrowKeyTime = 0.0f;
    private float m_holdRightArrowKeyTime = 0.0f;

    private float m_scrollbarHeightMax;

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
    private string ParseCommand(string str)
    {
        string[] pieces = str.Split(' ');

        // Set Brush info
        if (pieces[0].Length == 1 && pieces[0][0] == 'b')
        {
            // Iterate through selections
            for (int i = 1; i < pieces.Length; i++)
            {
                // String
                switch (pieces[i])
                {
                    // -> Change BrushType
                    case "pencil":
                    case "p":
                        VoxelSniper.m_brushType = BrushType.PENCIL;
                        break;

                    case "ball":
                    case "b":
                        VoxelSniper.m_brushType = BrushType.BALL;
                        break;

                    case "voxel":
                    case "v":
                        VoxelSniper.m_brushType = BrushType.VOXEL;
                        break;

                    case "disc":
                    case "d":
                        VoxelSniper.m_brushType = BrushType.DISC;
                        break;

                    case "discwall":
                    case "dw":
                        VoxelSniper.m_brushType = BrushType.DISC_WALL;
                        break;

                    case "voxeldisc":
                    case "vd":
                        VoxelSniper.m_brushType = BrushType.VOXEL_DISC;
                        break;

                    case "voxeldiscwall":
                    case "vdw":
                        VoxelSniper.m_brushType = BrushType.VOXEL_DISC_WALL;
                        break;

                    // -> Change BrushPaintMode
                    case "m":
                        VoxelSniper.m_brushPaintType = BrushPaintType.MATERIAL;
                        break;
                    case "i":
                        VoxelSniper.m_brushPaintType = BrushPaintType.INK;
                        break;
                    case "c":
                        VoxelSniper.m_brushPaintType = BrushPaintType.COMBO;
                        break;
                }
                // Number
                // -> Change BrushSize
                uint newBrushSize = 0;
                bool hasNumber = uint.TryParse(pieces[i], out newBrushSize);
                if (hasNumber)
                {
                    newBrushSize = (newBrushSize > 100) ? 100 : newBrushSize;
                    VoxelSniper.m_brushSize = newBrushSize;
                }
            }

            return '[' +
                "Brush = " + VoxelSniper.m_brushType.ToString() +
                ", Size = " + VoxelSniper.m_brushSize.ToString() +
                ", PaintType = " + VoxelSniper.m_brushPaintType.ToString() +
                ']';
        }
        // Set Block ID/SubID
        else if (pieces[0].Length == 1 && pieces[0][0] == 'v')
        {
            // Iterate through selections
            for (int i = 1; i < pieces.Length; i++)
            {
                // Number
                // -> Change BlockID by ID
                uint newBlockID = 0;
                bool hasNumber = uint.TryParse(pieces[i], out newBlockID);
                if (hasNumber)
                {
                    if(BlockDictionary.Has(newBlockID))
                    {
                        VoxelSniper.m_blockID = newBlockID;
                        return "[Name = " + BlockDictionary.Get(VoxelSniper.m_blockID).m_name + ", BlockID = " + VoxelSniper.m_blockID.ToString() + ']';
                    }
                }
                // By Name
                // -> Change BlockID by string
                else
                {
                    if (BlockDictionary.Has(pieces[i]))
                    {
                        VoxelSniper.m_blockID = BlockDictionary.Get(pieces[i]).m_id;
                        return "[Name = " + BlockDictionary.Get(VoxelSniper.m_blockID).m_name + ", BlockID = " + VoxelSniper.m_blockID.ToString() + ']';
                    }
                }
            }
        }

        // Nothing Parsed
        return "";
    }

    // Behaviour
    void Start()
    {
        m_player = GlobalData.player;

        m_scrollbarHeightMax = m_scrollbar.sizeDelta.y;
    }

    void Update()
    {
        // Move cursor position
        if (Input.GetKeyDown(KeyCode.LeftArrow) || m_holdLeftArrowKeyTime > 0.7f)
            m_cursorPosition--;

        else if (Input.GetKeyDown(KeyCode.RightArrow) || m_holdRightArrowKeyTime > 0.7f)
            m_cursorPosition++;

        m_cursorPosition = Mathf.Clamp(m_cursorPosition, 0, m_input.Length);

        // Check input
        if (Input.inputString.Length > 0)
        {
            foreach (char c in Input.inputString)
            {
                ParseKey(c);
            }
        }

        m_cursorPosition = Mathf.Clamp(m_cursorPosition, 0, m_input.Length);

        // Hold down cursor key
        if (Input.GetKey(KeyCode.LeftArrow))
            m_holdLeftArrowKeyTime += Time.deltaTime;
        else
            m_holdLeftArrowKeyTime = 0;

        if (Input.GetKey(KeyCode.RightArrow))
            m_holdRightArrowKeyTime += Time.deltaTime;
        else
            m_holdRightArrowKeyTime = 0;

        // Scroll up/down
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        if (scrollDelta > 0f)
        {
            m_historyOffset++;
            m_historyOffset = Mathf.Clamp(m_historyOffset, 0, m_history.Count - m_historyLocalMax);
        }
        else if(scrollDelta < 0f)
        {
            m_historyOffset--;
            m_historyOffset = Mathf.Max(m_historyOffset, 0);
        }

        // History
        string totalHistory = "";
        int indexStart = Mathf.Max(0, m_history.Count - m_historyLocalMax - m_historyOffset);
        int indexEnd = Mathf.Max(0, m_history.Count - m_historyOffset);
        for (int i = indexStart; i < indexEnd; i++)
        {
            totalHistory += m_history[i] + '\n';
        }
        m_text_history.text = totalHistory;

        // Scrollbar
        m_scrollbar.gameObject.SetActive(m_history.Count > m_historyLocalMax);
        if (m_history.Count > m_historyLocalMax)
        {
            // -> Size
            Vector2 scrollbarSize = m_scrollbar.sizeDelta;
            scrollbarSize.y = m_scrollbarHeightMax * (float)m_historyLocalMax / (float)m_history.Count;
            m_scrollbar.sizeDelta = scrollbarSize;
            // -> Pos
            Vector3 scrollbarPos = m_scrollbar.localPosition;
            float scrollbarHalf = 0.5f * (float)m_historyLocalMax / (float)m_history.Count;
            float botY = -50.0f + 100.0f * (scrollbarHalf);
            float topY = +50.0f - 100.0f * (scrollbarHalf);
            float t = (float)m_historyOffset / (float)(m_history.Count - m_historyLocalMax);
            scrollbarPos.y = Mathf.Lerp(botY, topY, t);
            m_scrollbar.localPosition = scrollbarPos;
        }

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
            m_input = m_input.Trim();
            string message = ParseCommand(m_input.ToLower());
            if (m_input.Length > 0)
            {
                // Unknown Command
                if (message.Length == 0)
                {
                    m_history.Add(m_input);
                }
                // Command with message
                else
                {
                    m_history.Add(
                        m_input + ' ' +
                        Utility.HTMLColorStart(Color.yellow) +
                        message +
                        Utility.HTMLColorEnd()
                        );
                }

                // Remove excess history
                while (m_history.Count > m_historyMaximum)
                    m_history.RemoveAt(0);

                m_input = "";
            }
        }
    }

}
