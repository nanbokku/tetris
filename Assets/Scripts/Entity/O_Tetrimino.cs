using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = Tetris.TetrisData;

public class O_Tetrimino : Tetrimino
{
    protected override Data.BlockType BlockType { get { return Data.BlockType.O; } }
    protected override Vector3 StandbyPosition { get { return new Vector3(5f, 21f, 0); } }
    protected override Vector3 StartPosition { get { return new Vector3(5f, 19f, 0); } }
    protected override List<Block> BottomBlocks { get { return bottomBlocks; } }

    private List<Block> bottomBlocks;

    protected override void Start()
    {
        base.Start();

        // 底辺ブロックの初期化
        bottomBlocks = new List<Block>();
        foreach (var block in Blocks)
        {
            if (block.transform.position.y < transform.position.y)
            {
                bottomBlocks.Add(block);
            }
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