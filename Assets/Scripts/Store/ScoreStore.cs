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


        public void Init()
        {
            level = 0;
            score = 0;
        }
    }
}