using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUI : MonoBehaviour
{
    public System.Action OnStartKeyPressed { get; set; }

    [SerializeField]
    private Text guideTxt;

    private float oldTime;
    private bool on;

    private const float intervalTime = 0.5f;


    void OnEnable()
    {
        oldTime = Time.time;
        on = true;
    }

    void Update()
    {
        // 点滅するテキストの更新
        if (Time.time - oldTime > intervalTime)
        {
            oldTime = Time.time;

            on = !on;
            guideTxt.gameObject.SetActive(on);
        }

        // 入力の確認
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnStartKeyPressed();
        }
    }
}
