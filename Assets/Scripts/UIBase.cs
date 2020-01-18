using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public GameObject m_master;
    private Player m_player;

    private void Awake()
    {
        m_player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        m_master.SetActive(m_player.isPaused());
    }
}
