using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Store;
using Data = Store.TetrisData;

public class TetrisController : MonoBehaviour
{
    public Action OnFinished { get; set; }

    private Block[] blocks;
    private float time;
    private Tetrimino current, next;
    private Player player;

    private float interval = InitDropInterval;
    private float oldTime = 0;

    private const float InitDropInterval = 1.5f;
    private const float MinDropInterval = 0.01f;
    private const float SpeedUpTimeInterval = 20.0f;


    void Update()
    {
        if (current == null) return;

        time += Time.deltaTime;
        if (Time.time - oldTime < interval) return;

        oldTime = Time.time;

        // 1マスずつ下がる
        current.Drop(1);

        // テトリミノの落ちるスピードを更新する
        UpdateLevelAndDropSpeed();
    }

    public void Init()
    {
        blocks = new Block[Data.Columns * Data.Rows];
        time = 0.0f;
        oldTime = Time.time;
        player = MonoBehaviour.FindObjectOfType<Player>();

        // Tetriminoの生成
        current = CreateTetrimino();
        next = CreateTetrimino();

        // 各Tetriminoの初期化
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

        // FIXME: ワープとUpdate.Drop()の呼ばれるタイミングによって，ブロックの着地地点がおかしくなる
        player.OnWarped = () =>
        {
            if (current == null) return;

            // 下方向にあるオブジェクトを取得
            var hit = current.BottomRaycastHit();
            if (hit.distance == Mathf.Infinity) return;

            // 一気に下に下がる
            var down = (int)(hit.distance / Data.BlockInterval.y);
            current.Drop(down);
        };
    }

    private void Next()
    {
        var bitboard = StoreManager.Instance.BoardStore.BitBoard;
        var typeboard = StoreManager.Instance.BoardStore.TypeBoard;

        // 置かれたBlockのビットを立てる
        foreach (var block in current.Blocks)
        {
            TetrisBitBoard.SetPositionFlag(ref bitboard, block);
            TetrisBitBoard.SetTypeFlag(ref typeboard, block);

            // 置いたブロックを保存
            blocks[block.Position.y * Data.Columns + block.Position.x] = block;
        }

        var removeRow = TetrisBitBoard.CheckLine(bitboard, typeboard);

        // Lineの更新
        UpdateScoreAndLine(removeRow.Count);

        // ブロックの状態を更新
        UpdateView(removeRow);

        // ゲームが終了したか判定
        if (IsFinished(bitboard))
        {
            current = next = null;

            OnFinished();
            return;
        }

        // 現在，次のテトリミノの更新
        current = next;
        next = CreateTetrimino();

        // 現在の状態を保存
        TetrisBitBoard.SaveCurrentData(current, next, bitboard, typeboard);

        // 次の操作へ
        current.Launch();
        next.Standby();
    }

    private bool IsFinished(ushort[] bitboard)
    {
        var bits = 0x0c00;

        // 最上段の真ん中2マスが埋まっていたら終了
        if ((bitboard[Data.Rows - 1] & bits) == bits)
        {
            return true;
        }

        return false;
    }

    private Tetrimino CreateTetrimino()
    {
        var idx = UnityEngine.Random.Range(0, Data.TetriminoPrefab.Length);
        var tetrimino = GameObject.Instantiate(Data.TetriminoPrefab[idx]).GetComponent<Tetrimino>();

        // Tetriminoのイベント登録
        tetrimino.OnLand = (blocks) =>
        {
            // 親子関係の解消
            foreach (var block in blocks)
            {
                block.transform.parent = null;
            }

            // テトリミノの削除
            Destroy(tetrimino.gameObject);

            // 次のテトリミノの操作へ移る
            Next();
        };

        return tetrimino;
    }

    // ブロックの状態を更新
    private void UpdateView(List<int> rmRow)
    {
        // 消す列がなければ終了
        if (rmRow.Count == 0) return;

        foreach (var row in rmRow)
        {
            // 揃った列のブロックを削除
            for (var col = 0; col < Data.Columns; col++)
            {
                Destroy(blocks[row * Data.Columns + col].gameObject);
            }
        }

        // 消した列を下に詰める
        for (var row = rmRow.Count - 1; row >= 0; row--)    // 上から下へ順に
        {
            // 空行が下にどれだけ連続しているか探索
            var continuation = 1;
            for (var diff = 1; row - diff >= 0; diff++)
            {
                if (rmRow[row] - rmRow[row - diff] != 1) break;

                continuation++;
            }

            row = row - continuation + 1;

            // 下に詰めていく
            // FIXME: 何かの拍子にずれる？そろってるはずだけど消えないときがあるかもしれない？要確認．
            for (var crntrow = rmRow[row]; crntrow + continuation < Data.Rows; crntrow++)
            {
                for (var col = 0; col < Data.Columns; col++)
                {
                    var translated = blocks[(crntrow + continuation) * Data.Columns + col];

                    blocks[crntrow * Data.Columns + col] = translated;

                    // 詰めるブロックがなければ次へ
                    if (translated == null) continue;

                    // positionの更新
                    var pos = translated.transform.position;
                    pos.y -= Data.BlockInterval.y * continuation;
                    translated.transform.position = pos;

                    translated.Position = new Data.BlockPosition(col, crntrow);
                }
            }
        }
    }

    private void UpdateLevelAndDropSpeed()
    {
        var level = (int)(time / SpeedUpTimeInterval);

        // Levelの更新
        if (StoreManager.Instance.ScoreStore.Level < level)
        {
            StoreManager.Instance.ScoreStore.Level = level;
        }

        // 落下スピードの更新
        interval = InitDropInterval - StoreManager.Instance.ScoreStore.Level * 0.05f;
        interval = interval > MinDropInterval ? interval : MinDropInterval;
    }

    private void UpdateScoreAndLine(int removelines)
    {
        if (removelines == 0) return;

        var level = StoreManager.Instance.ScoreStore.Level;
        StoreManager.Instance.ScoreStore.Score += Data.ScoreByBlockLines[removelines - 1] * (level + 1);
        StoreManager.Instance.ScoreStore.Line += removelines;
    }
}
