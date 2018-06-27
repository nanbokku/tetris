using UnityEngine;

namespace Tetris
{
    public class TetrisData
    {
        public enum BlockType
        {
            I, O, L, J, S, Z, T,
        }

        public struct BlockPosition
        {
            public int x;
            public int y;

            public BlockPosition(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public enum DirectionX
        {
            Right = 1,
            Left = -1
        }

        public enum BlockRotation
        {
            Normal, Right, Reverse, Left
        }

        public static readonly GameObject[] TetriminoPrefab =
        {
            Resources.Load<GameObject>("Block/IBlock"),
            Resources.Load<GameObject>("Block/OBlock"),
            Resources.Load<GameObject>("Block/LBlock"),
            Resources.Load<GameObject>("Block/JBlock"),
            //Resources.Load<GameObject>("Block/SBlock"),
            //Resources.Load<GameObject>("Block/ZBlock"),
            //Resources.Load<GameObject>("Block/TBlock")
        };

        public struct TetrisSaveData
        {
            public char current;
            public char next;
            public ulong[] bitboard;

            public TetrisSaveData(char current, char next, ulong[] bitboard)
            {
                this.current = current;
                this.next = next;
                this.bitboard = bitboard;
            }
        }
    }
}