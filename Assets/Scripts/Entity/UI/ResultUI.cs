using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ResultUI : MonoBehaviour
{
    [SerializeField]
    private Button titleBtn = null;
    [SerializeField]
    private Text scoreTxt = null;

    public UnityAction OnTitleBtnClicked { get; set; }

    void Start()
    {
        titleBtn.onClick.AddListener(() =>
        {
            OnTitleBtnClicked();
        });
    }

    public void UpdateScoreTxt(int score)
    {
        scoreTxt.text = "SCORE : " + score.ToString();
    }
}
