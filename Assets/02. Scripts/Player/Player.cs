using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public PlayerView playerView;
    public Equipment equipment;


    private void Awake()
    {
        PlayerManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        playerView = GetComponent<PlayerView>();
        equipment = GetComponent<Equipment>();
    }

}
