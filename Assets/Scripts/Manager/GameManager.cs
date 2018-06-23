﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager singleton;
    public static GameManager Instance
    {
        get
        {
            if (singleton == null)
            {
                singleton = FindObjectOfType<GameManager>();
                if (singleton == null)
                {
                    Debug.LogError(typeof(GameManager) + " is nothing.");
                }
            }
            return singleton;
        }
    }

    private IGameState currentState;


    void Start()
    {
        this.currentState = new GS_InGame();
        this.currentState.Enter();
    }

    public void ChangeState(IGameState state)
    {
        this.currentState.Exit();
        this.currentState = state;
        this.currentState.Enter();
    }
}
