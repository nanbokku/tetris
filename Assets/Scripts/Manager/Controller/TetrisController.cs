using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = Tetris.TetrisData;

public class TetrisController : MonoBehaviour
{
    public Action OnFinished { get; set; }

    private Block[] blocks;
    private float time;
    private Tetrimino current, next;
    private Player player;

    private float interval = 1f; //仮
    private float oldTime = 0;


    void Update()
    {
        if (current == null) return;

        time += Time.deltaTime;
        if (Time.time - oldTime < interval) return;

        oldTime = Time.time;
        current.Drop();
    }

    public void Init()
    {
        current = CreateTetrimino();
        next = CreateTetrimino();
        time = 0.0f;
        oldTime = Time.time;
        player = MonoBehaviour.FindObjectOfType<Player>();

        current.Launch();
        next.Standby();

        // Tetriminoのイベント登録
        current.OnLand = (blocks) =>
        {
            current = null;
            Next();
        };

        player.OnRotated = (dir) =>
        {
            if (current == null) return;

            current.Rotate(dir);
        };
        player.OnTranslated = (dir) =>
        {
            if (current == null) return;

            current.Translate(dir);
        };
    }

    private void Next()
    {

    }

    private Tetrimino CreateTetrimino()
    {
        var idx = UnityEngine.Random.Range(0, Data.TetriminoPrefab.Length);
        var tetrimino = GameObject.Instantiate(Data.TetriminoPrefab[idx]).GetComponent<Tetrimino>();

        return tetrimino;
    }
}
