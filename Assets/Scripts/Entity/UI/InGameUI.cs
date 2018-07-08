using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Store;

public class InGameUI : MonoBehaviour, IObserver
{
    public Action OnPauseBtnClicked { get; set; }

    private ScoreStore store;
    public ScoreStore Store
    {
        get
        {
            return store;
        }
        set
        {
            store = value;

            store.AddObserver(this);
            ValueChanged(0);
        }
    }

    [SerializeField]
    private Button pauseBtn;
    [SerializeField]
    private Text levelTxt;
    [SerializeField]
    private Text scoreTxt;
    [SerializeField]
    private Text lineTxt;


    void Start()
    {
        pauseBtn.onClick.AddListener(() => { OnPauseBtnClicked(); });
    }

    public void ValueChanged(object value)
    {
        var level = "LEVEL\n";
        levelTxt.text = level + store.Level.ToString();

        var score = "SCORE\n";
        scoreTxt.text = score + store.Score.ToString();

        var line = "LINE\n";
        lineTxt.text = line + store.Line.ToString();
    }
}