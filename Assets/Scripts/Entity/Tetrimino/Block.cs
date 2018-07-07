using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = Store.TetrisData;

public class Block : MonoBehaviour
{
    public Data.BlockType Type { get; private set; }
    public Data.BlockPosition Position { get; set; }
    public Collider Collider;

    public Action OnLand { get; set; }

    private const string bottomTag = "BottomFrame";
    private const string blockTag = "Block";


    void Awake()
    {
        this.Collider = GetComponent<Collider>();
    }

    public void Init(Data.BlockType type, Data.BlockPosition position)
    {
        this.Type = type;
        this.Position = position;
    }

    void OnTriggerEnter(Collider col)
    {
        // TODO: 追加ルール：遊び時間の実装
        if (col.gameObject.tag != bottomTag && col.gameObject.tag != blockTag) return;
        if (col.transform.parent == this.transform.parent) return;

        // 真下に衝突判定があるのか判定
        var ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (!Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity)) return;
        if (hit.collider != col) return;

        OnLand();

    }
}