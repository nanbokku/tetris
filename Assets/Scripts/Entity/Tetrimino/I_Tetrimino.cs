using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = Store.TetrisData;

public class I_Tetrimino : Tetrimino
{
    public override Data.BlockType BlockType { get { return Data.BlockType.I; } }

    protected override Vector3 StandbyPosition { get { return new Vector3(5f, 21f, 0f); } }
    protected override Vector3 StartPosition { get { return new Vector3(5f, 19f, 0f); } }
    protected override List<Block> BottomBlocks
    {
        get
        {
            if (Rotation == Data.BlockRotation.Normal)
            {
                return bottomBlocks[0];
            }
            else
            {
                return bottomBlocks[1];
            }
        }
    }

    private List<List<Block>> bottomBlocks;

    private static readonly Vector3[,] rotCoodinate =
    {
        {new Vector3(-1.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(1.5f, 0.5f, -0.5f)},
        {new Vector3(0.5f, 1.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, -1.5f, -0.5f)}
    };


    protected override void Start()
    {
        base.Start();

        // 底辺ブロックの初期化
        bottomBlocks = new List<List<Block>>();
        var blocks = new List<Block>();

        // 通常状態の底辺ブロック
        foreach (var block in Blocks)
        {
            blocks.Add(block);
        }

        bottomBlocks.Add(blocks);

        // 右回転したときの底辺ブロック
        blocks = new List<Block>();
        blocks.Add(Blocks[3]);
        bottomBlocks.Add(blocks);
    }

    public override void Translate(Data.DirectionX direction)
    {
        if (IsTouchingIn(direction)) return;

        transform.Translate((int)direction * Data.BlockInterval.x, 0f, 0f);

        foreach (var block in Blocks)
        {
            var pos = block.Position;
            block.Position = new Data.BlockPosition(pos.x + (int)direction, pos.y);
        }
    }

    public override void Rotate(Data.DirectionX direction)
    {
        var nextRot = Rotation;

        nextRot = Rotation == Data.BlockRotation.Normal ? Data.BlockRotation.Right : Data.BlockRotation.Normal;

        CheckAndRotate(nextRot, rotCoodinate);
    }

    protected override void Init()
    {
        var pos = 3;
        foreach (var block in Blocks)
        {
            block.Init(BlockType, new Data.BlockPosition(pos++, 19));
        }
    }
}