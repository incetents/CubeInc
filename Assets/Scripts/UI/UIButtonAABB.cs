using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct UIButtonAABBChanges
{
    public Image image;
    public Color m_colorBase;
    public Color m_colorHovered;
    public Color m_colorPressed;
    public Color m_colorDisabled;
}

[RequireComponent(typeof(RectTransform))]
public class UIButtonAABB : MonoBehaviour
{
    // References
    private RectTransform m_rectTransform = null;
    private float worldSizeHalfX = 1;
    private float worldSizeHalfY = 1;

    // Data
    private bool m_hovered;

    // Settings
    public List<UIButtonAABBChanges> m_changes = new List<UIButtonAABBChanges>();
    public bool m_disabled = false;

    // Utility
    public bool IsHovered()
    {
        return m_hovered;
    }
    public bool IsPressed()
    {
        return m_hovered && !m_disabled && Input.GetMouseButton(0);
    }
    public bool IsPressedDown()
    {
        return m_hovered && !m_disabled && Input.GetMouseButtonDown(0);
    }

    // Start is called before the first frame update
    void Awake()
    {
        m_rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Precalculate half of width/height in world space
        if (m_rectTransform.hasChanged)
        {
            worldSizeHalfX = m_rectTransform.sizeDelta.x * 0.5f * m_rectTransform.lossyScale.x;
            worldSizeHalfY = m_rectTransform.sizeDelta.y * 0.5f * m_rectTransform.lossyScale.y;

            m_rectTransform.hasChanged = false;
        }

        // Check if mouse is hovering [AABB}
        m_hovered =
            Input.mousePosition.x >= transform.position.x - worldSizeHalfX &&
            Input.mousePosition.x <= transform.position.x + worldSizeHalfX &&
            Input.mousePosition.y >= transform.position.y - worldSizeHalfY &&
            Input.mousePosition.y <= transform.position.y + worldSizeHalfY;

        // Update Colored
        foreach(UIButtonAABBChanges change in m_changes)
        {
            Color c = change.image.color;

            if (m_disabled)
                c = change.m_colorDisabled;
            else if (IsPressed())
                c = change.m_colorPressed;
            else if (IsHovered())
                c = change.m_colorHovered;
            else
                c = change.m_colorBase;

            change.image.color = c;
        }
        
    }
}
