using System;
using UnityEngine;
using Data = Store.TetrisData;

public class Player : MonoBehaviour
{
    public Action<Data.DirectionX> OnTranslated { get; set; }
    public Action<Data.DirectionX> OnRotated { get; set; }
    public Action OnWarped { get; set; }

    private Data.DirectionX prevdir;
    private float prevtime;

    private const float MinTranslationInterval = 0.06f;


    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Translate(Data.DirectionX.Right);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Translate(Data.DirectionX.Left);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            OnRotated(Data.DirectionX.Right);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            OnRotated(Data.DirectionX.Left);
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            OnWarped();
        }
    }

    private void Translate(Data.DirectionX dir)
    {
        // 前回と移動方向が異なる場合
        if (prevdir != dir)
        {
            prevdir = dir;
            prevtime = Time.time;

            OnTranslated(dir);
            return;
        }

        // 一定の時間間隔で移動
        if (Time.time - prevtime < MinTranslationInterval) return;

        prevtime = Time.time;

        OnTranslated(dir);
    }
}