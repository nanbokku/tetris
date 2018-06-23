using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = Tetris.TetrisData;

public class Block : MonoBehaviour
{
    public Data.BlockType Type { get; private set; }
    public Data.BlockPosition Position { get; set; }
    public Collider Collider;

    public Action OnLand { get; set; }

    private static readonly Vector3 origin = new Vector3(0.5f, 0.5f, -0.5f);
    private const string bottomTag = "BottomFrame";
    private const string blockTag = "Block";


    void Awake()
    {
        this.Collider = GetComponent<Collider>();
    }

    public void Init(Data.BlockType type)
    {
        this.Type = type;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag != bottomTag && col.gameObject.tag != blockTag) return;
        if (col.transform.parent == this.transform.parent) return;

        OnLand();
    }
}