using System.Collections;
using System.Collections.Generic;

namespace Tetris
{
    public class TetrisStore
    {
        public TetrisStore()
        {
            // 初期化
            current = next = 0x00;
            BitBoard = new ushort[TetrisData.Rows];
        }

        public ushort[] BitBoard { get; private set; }

        private byte current;
        private byte next;


        public void SetTetrisData(TetrisData.TetrisSaveData data)
        {
            this.current = data.current;
            this.next = data.next;
            this.BitBoard = data.bitboard;
        }

        // TODO: LoadTetrisData
        public TetrisData.TetrisSaveData GetTetrisData()
        {
            return new TetrisData.TetrisSaveData(this.current, this.next, this.BitBoard);
        }
    }
}