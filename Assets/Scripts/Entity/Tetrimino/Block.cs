using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = Store.TetrisData;

public class Block : MonoBehaviour
{
    public Data.BlockType Type { get; private set; }
    public Data.BlockPosition Position { get; set; }
    public Collider Collider { get; set; }

    public Action OnLand { get; set; }

    // private float landTime;
    // private float landPosY;

    private const float MovableTimeOnLand = 0.5f;
    private const string BottomTag = "BottomFrame";
    private const string BlockTag = "Block";


    void Awake()
    {
        this.Collider = GetComponent<Collider>();
    }

    public void Init(Data.BlockType type, Data.BlockPosition position)
    {
        this.Type = type;
        this.Position = position;

        // landPosY = Mathf.Infinity;
        // landTime = 0;
    }

    void OnTriggerEnter(Collider col)
    {
        // TODO: 追加ルール：遊び時間の実装　テトリミノの移動，回転制限も考えなければならない

        // if (transform.position.y >= landPosY) return;

        // 他のBlockや地面にのみ衝突判定を行う
        if (col.gameObject.tag != BottomTag && col.gameObject.tag != BlockTag) return;
        if (col.transform.parent == this.transform.parent) return;

        // 真下に衝突判定があるのか判定
        var ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (!Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity)) return;
        if (hit.collider != col) return;

        // landTime = Time.time;
        // landPosY = transform.position.y;

        OnLand();
    }

    void OnTriggerStay(Collider col)
    {
        // // 遊び時間中は何もしない
        // if (landTime == 0 || Time.time - landTime < MovableTimeOnLand) return;

        // // 遊び時間を過ぎたら着地
        // OnLand();
    }
}