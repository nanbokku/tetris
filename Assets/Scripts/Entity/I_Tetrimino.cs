using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = Tetris.TetrisData;

public class I_Tetrimino : Tetrimino
{
    protected override Data.BlockType BlockType { get { return Data.BlockType.I; } }
    protected override Vector3 StandbyPosition { get { return new Vector3(5f, 21f, 0f); } }
    protected override Vector3 StartPosition { get { return new Vector3(5f, 19f, 0f); } }
    protected override List<Block> BottomBlocks
    {
        get
        {
            if (rotation == Data.BlockRotation.Normal)
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

    private static readonly Vector3[,] rotPosition =
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

        foreach (var block in Blocks)
        {
            blocks.Add(block);
        }

        bottomBlocks.Add(blocks);

        // 右回転したときの底辺ブロック
        var bottom = new List<Block>();
        bottom.Add(Blocks[3]);
        bottomBlocks.Add(bottom);
    }

    public override void Translate(Data.DirectionX direction)
    {
        // 右端に達したとき左入力しか受け付けない
        if (rotation == Data.BlockRotation.Normal && transform.position.x >= 8f && direction > 0
            || rotation == Data.BlockRotation.Right && transform.position.x >= 9f && direction > 0) return;

        // 左端に達したとき右入力しか受け付けない
        if (rotation == Data.BlockRotation.Normal && transform.position.x <= 2f && direction < 0
            || rotation == Data.BlockRotation.Right && transform.position.x <= 0f && direction < 0) return;

        transform.Translate((int)direction * BlockInterval.x, 0f, 0f);
    }

    public override void Rotate(Data.DirectionX direction)
    {
        // 回転制限
        if (rotation == Data.BlockRotation.Right
            && (transform.position.x < 2f || transform.position.x > 8f)) return;

        rotation = rotation == Data.BlockRotation.Normal ? Data.BlockRotation.Right : Data.BlockRotation.Normal;
        int idx = 0;
        var bottom = transform;

        foreach (var block in Blocks)
        {
            block.transform.localPosition = rotPosition[(int)rotation, idx++];
        }
    }
}