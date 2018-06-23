using System;
using UnityEngine;
using Data = Tetris.TetrisData;

public class Player : MonoBehaviour
{
    public Action<Data.DirectionX> OnTranslated { get; set; }
    public Action<Data.DirectionX> OnRotated { get; set; }


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
    }
}