using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = Store.TetrisData;

public class J_Tetrimino : Tetrimino
{
    public override Data.BlockType BlockType { get { return Data.BlockType.J; } }

    protected override Vector3 StandbyPosition { get { return new Vector3(4.5f, 21.5f, 0f); } }
    protected override Vector3 StartPosition { get { return new Vector3(4.5f, 19.5f, 0f); } }
    protected override List<Block> BottomBlocks
    {
        get
        {
            if (rotation == Data.BlockRotation.Normal || rotation == Data.BlockRotation.Reverse)
            {
                return bottomBlocks[0];
            }
            else
            {
                return bottomBlocks[1];
            }
        }
    }

    private Data.BlockRotation rotation = Data.BlockRotation.Normal;
    private List<List<Block>> bottomBlocks;

    private static readonly Vector3[,] rotCoodinate =
    {
        {new Vector3(-1f, 0f, -0.5f), new Vector3(0f, 0f, -0.5f), new Vector3(1f, -1f,-0.5f), new Vector3(1f, 0f, -0.5f)},//012
        {new Vector3(0f, -1f, -0.5f), new Vector3(-1f, -1f,-0.5f),new Vector3(0f, 1f, -0.5f), new Vector3(0f, 0f, -0.5f)},//01
        {new Vector3(-1f, -1f, -0.5f), new Vector3(0f, -1f, -0.5f), new Vector3(1f, -1f,-0.5f),new Vector3(-1f, 0f, -0.5f)},//012
        {new Vector3(0f, -1f,-0.5f),  new Vector3(1f, 1f, -0.5f), new Vector3(0f, 1f, -0.5f), new Vector3(0f, 0f, -0.5f)},//01
    };


    protected override void Start()
    {
        base.Start();

        // 底辺ブロックの初期化
        bottomBlocks = new List<List<Block>>();

        // rotation: Normal or Reverse状態の底辺ブロック
        var blocks = new List<Block>();
        blocks.Add(Blocks[0]);
        blocks.Add(Blocks[1]);
        blocks.Add(Blocks[2]);

        bottomBlocks.Add(blocks);

        // rotation: Right or Left状態の底辺ブロック
        blocks = new List<Block>();
        blocks.Add(Blocks[0]);
        blocks.Add(Blocks[1]);

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
        // 回転制限
        if (rotation == Data.BlockRotation.Right && IsTouchingIn(Data.DirectionX.Right)) return;
        if (rotation == Data.BlockRotation.Left && IsTouchingIn(Data.DirectionX.Left)) return;

        var crntRot = rotation;

        // 左向きの状態で右回転をしたとき，Normal状態に戻す
        if (direction == Data.DirectionX.Right && rotation == Data.BlockRotation.Left)
        {
            rotation = Data.BlockRotation.Normal;
        }
        // 通常状態で左回転をしたとき，Left状態にする
        else if (direction == Data.DirectionX.Left && rotation == Data.BlockRotation.Normal)
        {
            rotation = Data.BlockRotation.Left;
        }
        else
        {
            rotation += (int)direction;
        }

        var idx = 0;
        foreach (var block in Blocks)
        {
            block.transform.localPosition = rotCoodinate[(int)rotation, idx];

            // Positionの更新
            var diff = rotCoodinate[(int)rotation, idx] - rotCoodinate[(int)crntRot, idx];
            block.Position = new Data.BlockPosition(block.Position.x + (int)diff.x, block.Position.y + (int)diff.y);

            idx++;
        }
    }

    protected override void Init()
    {
        var iniPos = new Vector2Int[] { new Vector2Int(3, 19), new Vector2Int(4, 19), new Vector2Int(5, 18), new Vector2Int(5, 19) };
        for (var i = 0; i < Blocks.Length; i++)
        {
            Blocks[i].Init(BlockType, new Data.BlockPosition(iniPos[i].x, iniPos[i].y));
        }
    }
}