namespace Store
{
    public class ScoreStore : Subject
    {
        public ScoreStore()
        {
            Init();
        }

        private int level;
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

        private int score;
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

        private int line;
        public int Line
        {
            get
            {
                return line;
            }
            set
            {
                line = value;
                Notify(line);
            }
        }


        public void Init()
        {
            level = 0;
            score = 0;
            line = 0;
        }
    }
}