using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = Tetris.TetrisData;

public class I_Tetrimino : Tetrimino
{
    protected override Data.BlockType BlockType { get { return Data.BlockType.I; } }
    protected override Vector3 StandbyPosition { get { return new Vector3(5f, 21f, 0f); } }
    protected override Vector3 StartPosition { get { return new Vector3(5f, 19f, 0f); } }

    private Data.BlockRotation rotation = Data.BlockRotation.Normal;
    private static readonly Vector3[,] rotPosition =
    {
        {new Vector3(-1.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(1.5f, 0.5f, -0.5f)},
        {new Vector3(0.5f, 1.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, -1.5f, -0.5f)}
    };


    public override void Translate(Data.DirectionX direction)
    {
        // 右端に達したとき左入力しか受け付けない

        // 左端に達したとき右入力しか受け付けない


    }

    public override void Rotate(Data.DirectionX direction)
    {
        rotation = rotation == Data.BlockRotation.Normal ? Data.BlockRotation.Reverse : Data.BlockRotation.Normal;
        var rot = rotation == Data.BlockRotation.Reverse ? 1 : 0;
        int idx = 0;

        foreach (var block in Blocks)
        {
            block.transform.localPosition = rotPosition[(int)rot, idx++];
        }
    }
}