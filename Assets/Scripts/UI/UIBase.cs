﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    private Player m_player;

    public GameObject m_master;
    public GameObject m_pauseMenu;
    public GameObject m_debugInfo;
    public GameObject m_userInfo;

    private void Start()
    {
        m_player = GlobalData.player;
    }

    // Update is called once per frame
    void Update()
    {
        //  bool allInactive =
        //      !m_player.m_debugMenu &&
        //      !m_player.m_paused
        //      ;
        //  
        //  m_master.SetActive(!allInactive);
        //  
        //  if(!allInactive)
        {
            m_debugInfo.SetActive(m_player.m_debugMenu);
            m_pauseMenu.SetActive(m_player.m_paused);
        }
    }
}