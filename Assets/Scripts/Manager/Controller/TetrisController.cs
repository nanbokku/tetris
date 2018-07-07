using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tetris;
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

        // 1マスずつ下がる
        current.Drop(1);
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
        player.OnWarped = () =>
        {
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
        var bitboard = StoreManager.Instance.TetrisStore.BitBoard;
        var typeboard = StoreManager.Instance.TetrisStore.TypeBoard;

        // 置かれたBlockのビットを立てる
        foreach (var block in current.Blocks)
        {
            bitboard[block.Position.y] |= (ushort)(0x8000 >> block.Position.x);
            typeboard[block.Position.y * Data.Columns + block.Position.x] = (byte)(0x01 << (int)block.Type);

            // 置いたブロックを保存
            blocks[block.Position.y * Data.Columns + block.Position.x] = block;
        }

        // ブロックの状態を更新
        UpdateView(CheckLine(bitboard, typeboard));

        // ゲームが終了したか判定
        if (IsFinished(bitboard))
        {
            OnFinished();
            return;
        }

        // 現在，次のテトリミノの更新
        current = next;
        next = CreateTetrimino();

        // 現在の状態を保存
        SaveCurrentData(bitboard, typeboard);

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

    private void SaveCurrentData(ushort[] bitboard, byte[] typeboard)
    {
        byte crntMino = (byte)(0x01 << (int)this.current.BlockType);
        byte nextMino = (byte)(0x01 << (int)this.next.BlockType);
        var data = new TetrisData.TetrisSaveData(crntMino, nextMino, bitboard, typeboard);

        StoreManager.Instance.TetrisStore.SetTetrisData(data);
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

    // 横列をチェックし，消去する列番号を返す
    private List<int> CheckLine(ushort[] bitboard, byte[] typeboard)
    {
        var rmRow = new List<int>();
        ushort bits = 0xffc0;  // 11111111 11000000

        for (var rows = 0; rows < bitboard.Length; rows++)
        {
            // 横列が揃っているかチェック
            if ((bitboard[rows] & bits) == bits)
            {
                rmRow.Add(rows);
            }
        }

        if (rmRow.Count != 0)
        {
            DownBitLine(ref bitboard, ref typeboard);
        }

        return rmRow;
    }

    // 最下行から隙間の無いようににビットボードを詰める
    private void DownBitLine(ref ushort[] bitboard, ref byte[] typeboard)
    {
        ushort bits = 0xffc0;   // 11111111 11000000
        for (var rows = 0; rows < bitboard.Length; rows++)
        {
            // 横列が揃っていなければ次の列へ
            if ((bitboard[rows] & bits) != bits) continue;

            // 消せる列が何連続しているか探索する
            var continuation = 1;
            for (var uprow = rows + 1; uprow < bitboard.Length; uprow++)
            {
                if ((bitboard[uprow] & bits) != bits) break;

                continuation++;
            }

            // 下へ詰めていく
            for (var crntrow = rows; crntrow + continuation < bitboard.Length; crntrow++)
            {
                bitboard[crntrow] = bitboard[crntrow + continuation];
                for (var col = 0; col < Data.Columns; col++)
                {
                    typeboard[crntrow * Data.Columns + col] = typeboard[(crntrow + continuation) * Data.Columns + col];
                }
            }
        }
    }

    // 16bit用のビットカウント
    private int CountBits(ushort bits)
    {
        bits = (ushort)((bits & 0x5555) + ((bits & 0xaaaa) >> 1));
        bits = (ushort)((bits & 0x3333) + ((bits & 0xcccc) >> 2));
        bits = (ushort)((bits & 0x0f0f) + ((bits & 0xf0f0) >> 4));
        bits = (ushort)((bits & 0x00ff) + ((bits & 0xff00) >> 8));

        return (int)bits;
    }
}
