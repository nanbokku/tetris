using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = Tetris.TetrisData;

public class O_Tetrimino : Tetrimino
{
    private static readonly Vector3 standbyPosition = new Vector3(5f, 21f, 0);
    private static readonly Vector3 startPosition = new Vector3(5f, 19f, 0);


    protected override void Init()
    {
        foreach (var block in Blocks)
        {
            block.Init(Data.BlockType.O);
        }
    }

    public override void Standby()
    {
        transform.position = standbyPosition;

        foreach (var block in Blocks)
        {
            block.Collider.enabled = false;
        }
    }

    public override void Launch()
    {
        transform.position = startPosition;

        foreach (var block in Blocks)
        {
            block.Collider.enabled = true;
        }
    }

    public override void Translate(Data.DirectionX direction)
    {
        // 右端に達したとき左入力しか受け付けない
        if (transform.position.x >= 9f && direction > 0) return;
        // 左端に達したとき右入力しか受け付けない
        if (transform.position.x <= 1f && direction < 0) return;

        transform.Translate((int)direction * BlockInterval.x, 0f, 0f);

        foreach (var block in Blocks)
        {
            var pos = block.Position;
            block.Position = new Data.BlockPosition(pos.x + (int)direction, pos.y);
        }
    }

    public override void Rotate(Data.DirectionX direction)
    {
    }
}