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
    private bool isWarped = false;


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
        // ワープ後の落下を防止する
        if (isWarped) return;

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
        // 下方向にあるオブジェクトを取得
        var hit = BottomRaycastHit();
        if (hit.distance == Mathf.Infinity) return;

        // 一気に下に下がる
        var down = (int)(hit.distance / Data.BlockInterval.y);
        Drop(down);

        isWarped = true;
    }

    // 底辺ブロックから下方向にRayを飛ばす．最も近くのRaycastHitを返す．
    public RaycastHit BottomRaycastHit()
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

        return nearest;
    }

    public abstract void Translate(Data.DirectionX direction);
    public abstract void Rotate(Data.DirectionX direction);
    protected abstract void Init();
}