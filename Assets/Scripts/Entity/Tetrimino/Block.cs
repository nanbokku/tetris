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

    private float landTime;

    private const float MovableTimeOnLand = 1.0f;
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

        landTime = -1;
    }

    // 指定した方向にブロックが接触しているか判定する
    // MEMO: 衝突判定イベントでブロックが接触しているか判定するのではどうしてもずれが生じたため，移動前にこのメソッドで接触していないか確認する
    public bool IsTouchingInOtherBlock(Data.DirectionX direction)
    {
        var distance = DistanceToOtherBlock(direction);

        // 接触していない
        if (distance < 0 || distance > Data.BlockInterval.x) return false;

        return true;
    }

    // 指定した方向にある，最も近い他ブロックとの距離を求める
    // 他ブロック以外のものが指定方向にある場合は負の値を返す．
    private float DistanceToOtherBlock(Data.DirectionX direction)
    {
        var ray = new Ray(this.transform.position, Vector3.right * (int)direction);
        RaycastHit hit;

        if (!Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity)) return -1;

        // 同じテトリミノを構成するブロックは無視
        if (hit.collider.transform.parent == this.transform.parent) return -1;

        return hit.distance;
    }

    void OnTriggerEnter(Collider col)
    {
        if (!IsCollisionWithBottomBlock(col)) return;

        landTime = Time.time;
    }

    void OnTriggerExit(Collider col)
    {
        if (!IsCollisionWithBottomBlock(col)) return;

        landTime = -1;
    }

    void OnTriggerStay(Collider col)
    {
        // 遊び時間中は何もしない
        if (landTime < 0 || Time.time - landTime < MovableTimeOnLand) return;

        if (!IsCollisionWithBottomBlock(col)) return;

        OnLand();
        landTime = -1;
    }

    private bool IsCollisionWithBottomBlock(Collider col)
    {
        // 同じテトリミノを構成するブロックは無視
        if (col.transform.parent == this.transform.parent) return false;

        // 他のBlockや地面にのみ衝突判定を行う
        if (col.gameObject.tag != BottomTag && col.gameObject.tag != BlockTag) return false;

        // 真下に衝突判定があるのか判定
        if (DirectionTo(col) != Vector3.down) return false;

        return true;
    }

    private Vector3 DirectionTo(Collider col)
    {
        return (col.ClosestPoint(this.transform.position) - this.transform.position).normalized;
    }
}