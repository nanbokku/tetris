using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = Tetris.TetrisData;

public abstract class Tetrimino : MonoBehaviour
{
    public Block[] Blocks { get; private set; }
    public Action<Block[]> OnLand { get; set; }

    private bool isLanded = false;

    protected static readonly Vector3 BlockInterval = new Vector3(1.0f, 1.0f, 0f);


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

    void Start()
    {
        Init();
    }

    public void Drop()
    {
        transform.Translate(0f, -BlockInterval.y, 0f);
        foreach (var block in Blocks)
        {
            var pos = block.Position;
            block.Position = new Data.BlockPosition(pos.x, pos.y - 1);
        }
    }

    public abstract void Standby();
    public abstract void Launch();
    public abstract void Translate(Data.DirectionX direction);
    public abstract void Rotate(Data.DirectionX direction);

    protected abstract void Init();
}