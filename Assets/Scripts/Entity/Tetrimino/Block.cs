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
    public Action<Data.DirectionX, bool> OnTranslatabilityChanged { get; set; }

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

    private bool IsTouchingIn(Vector3 direction, Collider col)
    {
        var ray = new Ray(transform.position, direction);
        RaycastHit hit;
        if (!Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity)) return false;
        if (hit.collider != col) return false;

        return true;
    }

    void OnTriggerEnter(Collider col)
    {
        // TODO: 追加ルール：遊び時間の実装　テトリミノの移動，回転制限も考えなければならない

        // if (transform.position.y >= landPosY) return;

        // landTime = Time.time;
        // landPosY = transform.position.y;

    }

    void OnTriggerExit(Collider col)
    {
        // 同じテトリミノを構成するブロックは無視
        if (col.transform.parent == this.transform.parent) return;

        // コライダ―との距離がブロックの間隔以下の場合無視
        if (Vector3.Distance(col.transform.position, transform.position) <= Data.BlockInterval.x) return;

        var direction = (col.transform.position - transform.position).normalized;
        if (Vector3.Angle(Vector3.right, direction) < 90f)
        {
            OnTranslatabilityChanged(Data.DirectionX.Right, true);
        }
        else
        {
            OnTranslatabilityChanged(Data.DirectionX.Left, true);
        }
    }

    void OnTriggerStay(Collider col)
    {
        // // 遊び時間中は何もしない
        // if (landTime == 0 || Time.time - landTime < MovableTimeOnLand) return;

        // // 遊び時間を過ぎたら着地
        // OnLand();


        // 同じテトリミノを構成するブロックは無視
        if (col.transform.parent == this.transform.parent) return;

        if (IsTouchingIn(Vector3.right, col))
        {
            OnTranslatabilityChanged(Data.DirectionX.Right, false);
        }
        else if (IsTouchingIn(Vector3.left, col))
        {
            OnTranslatabilityChanged(Data.DirectionX.Left, false);
        }

        // 他のBlockや地面にのみ衝突判定を行う
        if (col.gameObject.tag != BottomTag && col.gameObject.tag != BlockTag) return;

        // 真下に衝突判定があるのか判定
        if (!IsTouchingIn(Vector3.down, col)) return;

        OnLand();
    }
}