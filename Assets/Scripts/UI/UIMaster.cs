using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMaster : MonoBehaviour
{
    private Player m_player;

    public GameObject object_Canvas;
    public GameObject object_BlackBG;
    public GameObject object_Crosshair;
    public GameObject object_Commands;
    public GameObject object_DebugText;
    public GameObject object_PauseMenu;
    public GameObject object_SelectedBlock;

    private BlockDisplayer blockDisplayer;

    private void UpdateStates()
    {
        object_BlackBG.SetActive(m_player.UsingMenu());
        object_DebugText.SetActive(m_player.m_debugMenu);
        object_PauseMenu.SetActive(m_player.m_menuState == MenuState.PAUSED);
        object_Commands.SetActive(m_player.m_menuState == MenuState.COMMAND);

        if(VoxelSniper.m_blockID > 0)
        {
            BlockInfo targetBlock = BlockDictionary.Get(VoxelSniper.m_blockID);
            Debug.Log(targetBlock.m_textureIDs.Count);

            blockDisplayer.id = (int)targetBlock.m_textureIDs[0];
        }
        blockDisplayer.m_visible = (VoxelSniper.m_blockID > 0);
    }

    // Behaviour
    void Start()
    {
        m_player = GlobalData.player;
        blockDisplayer = object_SelectedBlock.GetComponent<BlockDisplayer>();
        UpdateStates();
    }

    void Update()
    {
        UpdateStates();
    }
}
