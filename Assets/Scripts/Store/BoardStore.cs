using System.Collections;
using System.Collections.Generic;

namespace Store
{
    public class BoardStore
    {
        public BoardStore()
        {
            Init();
        }

        public ushort[] BitBoard { get; private set; }
        public byte[] TypeBoard { get; private set; }

        // BlockTypeの情報のみ
        private byte current;
        private byte next;


        public void Init()
        {
            // 初期化
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