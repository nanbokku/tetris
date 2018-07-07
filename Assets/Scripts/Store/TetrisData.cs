using UnityEngine;

namespace Store
{
    public class TetrisData
    {
        public const int Rows = 20;
        public const int Columns = 10;

        public static readonly Vector3 BlockInterval = new Vector3(1.0f, 1.0f, 0f);

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
            public byte current;
            public byte next;
            public ushort[] bitboard;
            public byte[] typeboard;

            public TetrisSaveData(byte current, byte next, ushort[] bitboard, byte[] typeboard)
            {
                this.current = current;
                this.next = next;
                this.bitboard = bitboard;
                this.typeboard = typeboard;
            }
        }
    }
}