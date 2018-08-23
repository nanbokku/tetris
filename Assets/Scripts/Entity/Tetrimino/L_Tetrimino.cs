using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = Store.TetrisData;

public class L_Tetrimino : Tetrimino
{
    public override Data.BlockType BlockType { get { return Data.BlockType.L; } }

    protected override Vector3 StandbyPosition { get { return new Vector3(4.5f, 21.5f, 0f); } }
    protected override Vector3 StartPosition { get { return new Vector3(4.5f, 19.5f, 0f); } }
    protected override List<Block> BottomBlocks
    {
        get
        {
            if (Rotation == Data.BlockRotation.Normal || Rotation == Data.BlockRotation.Reverse)
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

    private Vector3[,] rotPosition =
    {
        {new Vector3(-1f, -1f, -0.5f), new Vector3(0f,0f,-0.5f), new Vector3(1f, 0f, -0.5f), new Vector3(-1f,0f, -0.5f)},
        {new Vector3(-1f, 1f, -0.5f), new Vector3(0f,-1f,-0.5f), new Vector3(0f, 1f, -0.5f), new Vector3(0f,0f, -0.5f)},
        {new Vector3(-1f, -1f, -0.5f), new Vector3(0f,-1f,-0.5f), new Vector3(1f, -1f, -0.5f), new Vector3(1f,0f, -0.5f)},
        {new Vector3(0f, -1f, -0.5f), new Vector3(1f,-1f,-0.5f), new Vector3(0f, 1f, -0.5f), new Vector3(0f,0f, -0.5f)},
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

        // rotation: Right or Leftの底辺ブロック
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
        var nextRot = Rotation;

        // 左向きの状態で右回転したとき，Normalに戻す
        if (direction == Data.DirectionX.Right && Rotation == Data.BlockRotation.Left)
        {
            nextRot = Data.BlockRotation.Normal;
        }
        // 通常状態で左回転したとき，Left状態にする
        else if (direction == Data.DirectionX.Left && Rotation == Data.BlockRotation.Normal)
        {
            nextRot = Data.BlockRotation.Left;
        }
        else
        {
            nextRot += (int)direction;
        }

        CheckAndRotate(nextRot, rotPosition);
    }

    protected override void Init()
    {
        var iniPos = new Vector2Int[] { new Vector2Int(3, 18), new Vector2Int(4, 19), new Vector2Int(5, 19), new Vector2Int(3, 19) };
        for (var i = 0; i < Blocks.Length; i++)
        {
            Blocks[i].Init(BlockType, new Data.BlockPosition(iniPos[i].x, iniPos[i].y));
        }
    }
}