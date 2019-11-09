using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Store;

public class ResultUIController : MonoBehaviour
{
    [SerializeField]
    private ResultUI resultUI = null;

    public UnityAction OnTitleBtnClicked { get; set; }

    void Start()
    {
        resultUI.OnTitleBtnClicked = () =>
        {
            OnTitleBtnClicked();
        };
    }

    public void Init(StoreManager store)
    {
        var score = store.ScoreStore.Score;
        resultUI.UpdateScoreTxt(score);
    }
}
