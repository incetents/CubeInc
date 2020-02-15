using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMaster : MonoBehaviour
{
    private Player m_player;

    public GameObject object_Canvas;
    public GameObject object_BlackBG;
    public GameObject object_BlockDisplayer;
    public GameObject object_Crosshair;
    public GameObject object_Commands;
    public GameObject object_DebugText;
    public GameObject object_PauseMenu;
    public GameObject object_SelectedBlock;
    public GameObject object_BlockSelector;

    private UIBlockDisplayer blockDisplayer;

    private void UpdateStates()
    {
        object_BlackBG.SetActive(m_player.UsingMenu());
        object_DebugText.SetActive(m_player.m_debugMenu);
        object_BlockDisplayer.SetActive(m_player.m_menuState == MenuState.NONE || m_player.m_menuState == MenuState.BLOCK_SELECTION);
        object_PauseMenu.SetActive(m_player.m_menuState == MenuState.PAUSED);
        object_Commands.SetActive(m_player.m_menuState == MenuState.COMMAND);
        object_BlockSelector.SetActive(m_player.m_menuState == MenuState.BLOCK_SELECTION);
        object_Crosshair.SetActive(m_player.m_menuState == MenuState.NONE);

        //if (VoxelSniper.m_blockID > 0)
        {
            BlockInfo targetBlock = BlockDictionary.Get(VoxelSniper.m_blockID);
            blockDisplayer.SetBlock(targetBlock);
        }
        blockDisplayer.m_visible = (VoxelSniper.m_blockID > 0);
    }

    // Behaviour
    void Start()
    {
        m_player = GlobalData.player;
        blockDisplayer = object_SelectedBlock.GetComponent<UIBlockDisplayer>();
        UpdateStates();
    }

    void Update()
    {
        UpdateStates();
    }
}
