using System.Collections.Generic;
using Store;

/// <summary>
/// Tetrisのビットボードに関する操作を行うクラス
/// </summary>
public static class TetrisBitBoard
{
    // 置かれたBlockのビットを立てる
    public static void SetPositionFlag(ref ushort[] bitboard, Block block)
    {
        bitboard[block.Position.y] |= (ushort)(0x8000 >> block.Position.x);
    }

    public static void SetTypeFlag(ref byte[] typeboard, Block block)
    {
        typeboard[block.Position.y * TetrisData.Columns + block.Position.x] = (byte)(0x01 << (int)block.Type);
    }

    // 横列をチェックし，消去する列番号を返す
    public static List<int> CheckLine(ushort[] bitboard, byte[] typeboard)
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

    public static void SaveCurrentData(Tetrimino current, Tetrimino next, ushort[] bitboard, byte[] typeboard)
    {
        byte crntMino = (byte)(0x01 << (int)current.BlockType);
        byte nextMino = (byte)(0x01 << (int)next.BlockType);
        var data = new TetrisData.TetrisSaveData(crntMino, nextMino, bitboard, typeboard);

        StoreManager.Instance.BoardStore.SetTetrisData(data);
    }

    // 最下行から隙間の無いようににビットボードを詰める
    private static void DownBitLine(ref ushort[] bitboard, ref byte[] typeboard)
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
                for (var col = 0; col < TetrisData.Columns; col++)
                {
                    typeboard[crntrow * TetrisData.Columns + col] = typeboard[(crntrow + continuation) * TetrisData.Columns + col];
                }
            }
        }
    }

    // 16bit用のビットカウント
    public static int CountBits(ushort bits)
    {
        bits = (ushort)((bits & 0x5555) + ((bits & 0xaaaa) >> 1));
        bits = (ushort)((bits & 0x3333) + ((bits & 0xcccc) >> 2));
        bits = (ushort)((bits & 0x0f0f) + ((bits & 0xf0f0) >> 4));
        bits = (ushort)((bits & 0x00ff) + ((bits & 0xff00) >> 8));

        return (int)bits;
    }
}