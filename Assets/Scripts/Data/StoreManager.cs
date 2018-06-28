using System.Collections;
using System.Collections.Generic;

namespace Tetris
{
    public class StoreManager
    {
        private static StoreManager singleton = new StoreManager();
        private StoreManager()
        {
            this.TetrisStore = new TetrisStore();
        }
        public static StoreManager Instance { get { return singleton; } }

        public TetrisStore TetrisStore { get; private set; }
    }
}