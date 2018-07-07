using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Store;

public class InGameUIControlller : MonoBehaviour, IObserver
{
    public Action OnPauseBtnClicked { get; set; }

    [SerializeField]
    private Button pauseBtn;
    [SerializeField]
    private Text levelTxt;
    [SerializeField]
    private Text scoreTxt;

    private TetrisStore store;


    public void Init(StoreManager store)
    {
        this.store = store.TetrisStore;
        this.store.AddObserver(this);
    }


    public void Update(object value)
    {
        var level = "LEVEL: ";
        levelTxt.text = level + store.Level.ToString();

        var score = "SCORE: ";
        scoreTxt.text = score + store.Score.ToString();
    }
}