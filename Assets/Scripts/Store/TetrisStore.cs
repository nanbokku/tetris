using System.Collections;
using System.Collections.Generic;

namespace Store
{
    public class TetrisStore : Subject
    {
        public TetrisStore()
        {
            Init();
        }

        public int Level
        {
            get
            {
                return level;
            }
            set
            {
                level = value;
                Notify(level);
            }
        }

        public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                Notify(score);
            }
        }

        public ushort[] BitBoard { get; private set; }
        public byte[] TypeBoard { get; private set; }

        private int level;
        private int score;

        // BlockTypeの情報のみ
        private byte current;
        private byte next;


        public void Init()
        {
            // 初期化
            level = 0;
            score = 0;

            current = next = 0x00;
            BitBoard = new ushort[TetrisData.Rows];
            TypeBoard = new byte[TetrisData.Rows * TetrisData.Columns];
        }

        public void SetTetrisData(TetrisData.TetrisSaveData data)
        {
            this.current = data.current;
            this.next = data.next;
            this.BitBoard = data.bitboard;
            this.TypeBoard = data.typeboard;
        }

        // TODO: LoadTetrisData
        public TetrisData.TetrisSaveData GetTetrisData()
        {
            return new TetrisData.TetrisSaveData(this.current, this.next, this.BitBoard, this.TypeBoard);
        }
    }
}