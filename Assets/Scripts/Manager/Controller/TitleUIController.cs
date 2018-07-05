using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIController : MonoBehaviour
{
    public System.Action OnStarted { get; set; }

    private TitleUI titleUI;


    void Awake()
    {
        titleUI = GetComponentInChildren<TitleUI>();
    }

    void Start()
    {
        titleUI.OnStartKeyPressed = () =>
        {
            OnStarted();
        };
    }
}
