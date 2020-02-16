using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIBlockSelectorScreen : MonoBehaviour
{
    // Main Block Displayer
    public UIBlockDisplayer mainBlockDisplayer;
    public UIButtonAABB buttonPrev;
    public UIButtonAABB buttonNext;
    public TextMeshProUGUI pageNumber;

    // Data
    private uint m_pageNumber = 0;

    // All Block Displayers
    private List<UIBlockDisplayer> displayers = new List<UIBlockDisplayer>();
    private void AcquireDisplayer(Transform reference)
    {
        UIBlockDisplayer result = reference.gameObject.GetComponent<UIBlockDisplayer>();
        if (result != null)
            displayers.Add(result);

        foreach (Transform child in reference)
            AcquireDisplayer(child);
    }
    private void PopulateBlockDisplayers()
    {
        uint counter = m_pageNumber * (uint)displayers.Count;
        foreach (UIBlockDisplayer bd in displayers)
        {
            bd.SetBlock(BlockDictionary.Get(counter++));
        }
    }

    // Selection Objects
    void Awake()
    {
        // Acquire all BlockDisplayers in tree
        AcquireDisplayer(transform);
    }

    private void Start()
    {
        PopulateBlockDisplayers();
    }

    void Update()
    {
        // Select Block
        if(Input.GetMouseButtonDown(0))
        {
            foreach (UIBlockDisplayer bd in displayers)
            {
                if (!bd.isActiveAndEnabled)
                    continue;

                if (bd.IsPressed() && bd.GetBlock() != null)
                {
                    VoxelSniper.m_blockID = bd.GetBlock().m_id;
                    break;
                }
            }
        }

        // Page Buttons
        buttonPrev.m_disabled = (m_pageNumber == 0);

        if (buttonPrev.IsPressedDown() && m_pageNumber > 0)
        {
            m_pageNumber--;
            PopulateBlockDisplayers();
        }
        else if(buttonNext.IsPressedDown())
        {
            m_pageNumber++;
            PopulateBlockDisplayers();
        }
        // Page Text
        pageNumber.text = "[ " + m_pageNumber.ToString() + " ]";
    }
}
