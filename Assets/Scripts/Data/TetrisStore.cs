using System.Collections;
using System.Collections.Generic;

namespace Tetris
{
    public class TetrisStore
    {
        public TetrisStore()
        {
            // 初期化
        }

        public char Current { get; private set; }
        public char Next { get; private set; }
        public ulong[] BitBoard { get; private set; }


        public void SetTetrisData(TetrisData.TetrisSaveData data)
        {

        }

        public TetrisData.TetrisSaveData GetTetrisData()
        {
            return new TetrisData.TetrisSaveData();
        }

        public void AddBlocks(List<Block> blocks)
        {

        }
    }
}