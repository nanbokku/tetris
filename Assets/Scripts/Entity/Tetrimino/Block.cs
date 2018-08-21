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

    // 指定した方向にブロックが接触しているか判定する
    // MEMO: 衝突判定イベントでブロックが接触しているか判定するのではどうしてもずれが生じたため，移動前にこのメソッドで接触していないか確認する
    public bool IsTouchingIn(Data.DirectionX direction)
    {
        var ray = new Ray(transform.position, Vector3.right * (int)direction);
        RaycastHit hit;

        if (!Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity)) return false;

        // 同じテトリミノを構成するブロックは無視
        if (hit.collider.transform.parent == transform.parent) return false;

        // 接触していない
        if (hit.distance > Data.BlockInterval.x) return false;

        return true;
    }

    void OnTriggerEnter(Collider col)
    {
        // TODO: 追加ルール：遊び時間の実装　テトリミノの移動，回転制限も考えなければならない

        // if (transform.position.y >= landPosY) return;

        // landTime = Time.time;
        // landPosY = transform.position.y;

    }

    void OnTriggerStay(Collider col)
    {
        // // 遊び時間中は何もしない
        // if (landTime == 0 || Time.time - landTime < MovableTimeOnLand) return;

        // // 遊び時間を過ぎたら着地
        // OnLand();


        // 同じテトリミノを構成するブロックは無視
        if (col.transform.parent == this.transform.parent) return;

        // 他のBlockや地面にのみ衝突判定を行う
        if (col.gameObject.tag != BottomTag && col.gameObject.tag != BlockTag) return;

        // 真下に衝突判定があるのか判定
        var direction = (col.ClosestPoint(this.transform.position) - this.transform.position).normalized;
        if (direction != Vector3.down) return;

        OnLand();
    }
}