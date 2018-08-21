using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = Store.TetrisData;

public abstract class Tetrimino : MonoBehaviour
{
    public Block[] Blocks { get; private set; }
    public Action<Block[]> OnLand { get; set; }

    public abstract Data.BlockType BlockType { get; }

    protected abstract Vector3 StandbyPosition { get; }
    protected abstract Vector3 StartPosition { get; }
    protected abstract List<Block> BottomBlocks { get; }

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
            if (block.IsTouchingIn(direction)) return true;
        }

        return false;
    }

    public abstract void Translate(Data.DirectionX direction);
    public abstract void Rotate(Data.DirectionX direction);
    protected abstract void Init();
}