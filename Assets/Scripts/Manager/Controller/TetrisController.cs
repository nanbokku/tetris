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
        time = 0.0f;
        oldTime = Time.time;
        player = MonoBehaviour.FindObjectOfType<Player>();

        current = CreateTetrimino();
        next = CreateTetrimino();

        current.Launch();
        next.Standby();

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
        player.OnLanded = () =>
        {
            var hit = current.BottomRaycastHit();
            if (hit.distance == Mathf.Infinity) return;

            var pos = current.transform.position;
            pos.y = current.transform.position.y - hit.distance;

            var minoH = current.Blocks[0].transform.localScale.y / 2;
            current.transform.position = pos + new Vector3(0, minoH, 0);
        };
    }

    private void Next()
    {
        current = next;
        next = CreateTetrimino();

        current.Launch();
        next.Standby();
    }

    private Tetrimino CreateTetrimino()
    {
        var idx = UnityEngine.Random.Range(0, Data.TetriminoPrefab.Length);
        var tetrimino = GameObject.Instantiate(Data.TetriminoPrefab[idx]).GetComponent<Tetrimino>();

        // Tetriminoのイベント登録
        tetrimino.OnLand = (blocks) =>
        {
            current = null;
            Next();
        };

        return tetrimino;
    }
}
