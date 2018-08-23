using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = Store.TetrisData;

public abstract class Tetrimino : MonoBehaviour
{
    public Block[] Blocks { get; private set; }
    public Action<Block[]> OnLand { get; set; }
    public Func<List<Data.BlockPosition>, bool> OnRotateChecked { get; set; }

    public abstract Data.BlockType BlockType { get; }

    protected abstract Vector3 StandbyPosition { get; }
    protected abstract Vector3 StartPosition { get; }
    protected abstract List<Block> BottomBlocks { get; }
    protected Data.BlockRotation Rotation = Data.BlockRotation.Normal;

    private bool isLanded = false;


    protected virtual void Awake()
    {
        Blocks = GetComponentsInChildren<Block>();

        foreach (var block in Blocks)
        {
            block.OnLand = () =>
            {
                // 既にイベントが呼ばれていた場合
                if (isLanded) return;

                isLanded = true;
                OnLand(Blocks);
            };
        }

        isLanded = false;
    }

    protected virtual void Start()
    {
        Init();
    }

    // スタンバイ状態
    public void Standby()
    {
        transform.position = StandbyPosition;

        foreach (var block in Blocks)
        {
            block.Collider.enabled = false;
        }
    }

    // 操作開始状態
    public void Launch()
    {
        transform.position = StartPosition;

        foreach (var block in Blocks)
        {
            block.Collider.enabled = true;
        }
    }

    public void Drop(int blockNum)
    {
        // 接地後の落下を防止
        if (isLanded) return;

        // 下方向に移動できないとき
        var distance = (int)(DistanceToBottomObject() / Data.BlockInterval.y);
        if (distance <= 0) return;

        // ブロックのめり込み防止
        blockNum = distance < blockNum ? distance : blockNum;

        transform.Translate(0f, -Data.BlockInterval.y * blockNum, 0f);
        foreach (var block in Blocks)
        {
            var pos = block.Position;
            block.Position = new Data.BlockPosition(pos.x, pos.y - blockNum);

            Debug.Log("(" + block.Position.x + ", " + block.Position.y + ")");
        }
        Debug.Log("");
    }

    public void Warp()
    {
        // 下方向にあるオブジェクトまでの距離を取得
        var distance = DistanceToBottomObject();
        if (distance == Mathf.Infinity) return;

        // 一気に下に下がる
        var down = (int)(distance / Data.BlockInterval.y);
        Drop(down);

        isLanded = true;
        OnLand(Blocks);
    }

    // 下方向にある最も近くのオブジェクトまでの距離
    private float DistanceToBottomObject()
    {
        RaycastHit nearest = new RaycastHit();
        nearest.distance = Mathf.Infinity;

        foreach (var block in BottomBlocks)
        {
            var ray = new Ray(block.transform.position, Vector3.down);
            RaycastHit hit;

            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                if (hit.distance < nearest.distance)
                {
                    nearest = hit;
                }
            }
        }

        return nearest.distance;
    }

    // direction方向に接触しているか
    protected bool IsTouchingIn(Data.DirectionX direction)
    {
        foreach (var block in Blocks)
        {
            if (block.IsTouchingInOtherBlock(direction)) return true;
        }

        return false;
    }

    // 回転できるかチェック．回転できるようなら回転を行う
    protected void CheckAndRotate(Data.BlockRotation nextRot, Vector3[,] rotCoodinate)
    {
        // ブロックの移動先を取得
        var idx = 0;
        var positions = new List<Data.BlockPosition>();

        foreach (var block in Blocks)
        {
            var diff = rotCoodinate[(int)nextRot, idx] - rotCoodinate[(int)Rotation, idx];
            var position = new Data.BlockPosition(block.Position.x + (int)diff.x, block.Position.y + (int)diff.y);

            positions.Add(position);

            idx++;
        }

        // 回転できるか
        var canRotate = OnRotateChecked(positions);

        if (!canRotate) return;

        // ブロックの位置を更新する
        idx = 0;
        foreach (var block in Blocks)
        {
            block.transform.localPosition = rotCoodinate[(int)nextRot, idx];
            block.Position = positions[idx];

            idx++;
        }

        Rotation = nextRot;
    }

    public abstract void Translate(Data.DirectionX direction);
    public abstract void Rotate(Data.DirectionX direction);
    protected abstract void Init();
}