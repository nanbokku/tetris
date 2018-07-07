using System;
using UnityEngine;
using Data = Store.TetrisData;

public class Player : MonoBehaviour
{
    public Action<Data.DirectionX> OnTranslated { get; set; }
    public Action<Data.DirectionX> OnRotated { get; set; }
    public Action OnWarped { get; set; }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            OnTranslated(Data.DirectionX.Right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            OnTranslated(Data.DirectionX.Left);
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
}