using System;
using UnityEngine;
using Store;

public class InGameUIController : MonoBehaviour
{
    public Action OnPaused { get; set; }
    public Action OnInGamed { get; set; }
    public Action OnTitled { get; set; }

    private InGameUI gameUI;


    void Awake()
    {
        gameUI = transform.Find("InGameUI").GetComponent<InGameUI>();
    }

    public void Init(StoreManager store)
    {
        gameUI.Store = store.ScoreStore;

        gameUI.OnPauseBtnClicked = () =>
        {
            gameUI.gameObject.SetActive(false);
            OnPaused();
        };
    }
}