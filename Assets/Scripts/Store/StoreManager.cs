using System.Collections;
using System.Collections.Generic;

namespace Store
{
    public class StoreManager
    {
        private static StoreManager singleton = new StoreManager();
        private StoreManager()
        {
            this.BoardStore = new BoardStore();
            this.ScoreStore = new ScoreStore();
        }
        public static StoreManager Instance { get { return singleton; } }

        public BoardStore BoardStore { get; private set; }
        public ScoreStore ScoreStore { get; private set; }
    }
}