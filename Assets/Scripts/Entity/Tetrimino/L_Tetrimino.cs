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
        if (!CanTranslate[direction]) return;

        // 左に達したとき右入力しか受け付けない
        if (direction == Data.DirectionX.Left)
        {
            if (rotation != Data.BlockRotation.Left && transform.position.x <= 1.5f) return;
            if (rotation == Data.BlockRotation.Left && transform.position.x <= 0.5f) return;
        }

        // 右に達したとき左入力しか受け付けない
        if (direction == Data.DirectionX.Right)
        {
            if (rotation != Data.BlockRotation.Right && transform.position.x >= 8.5f) return;
            if (rotation == Data.BlockRotation.Right && transform.position.x >= 9.5f) return;
        }

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
        if (transform.position.x < 1.0f || transform.position.x > 9.0f) return;

        var crntRot = rotation;

        // 左向きの状態で右回転したとき，Normalに戻す
        if (direction == Data.DirectionX.Right && rotation == Data.BlockRotation.Left)
        {
            rotation = Data.BlockRotation.Normal;
        }
        // 通常状態で左回転したとき，Left状態にする
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
            block.transform.localPosition = rotPosition[(int)rotation, idx];

            // Positionの更新
            var diff = rotPosition[(int)rotation, idx] - rotPosition[(int)crntRot, idx];
            block.Position = new Data.BlockPosition(block.Position.x + (int)diff.x, block.Position.y + (int)diff.y);

            idx++;
        }
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