using System.Windows.Media;

namespace wpfTetris
{
    public enum TetrominoType { I, O, T, S, Z, J, L }

    public class Tetromino
    {
        public TetrominoType Type { get; }
        public int[,] Shape { get; set; }
        public Color Color { get; }
        public int X { get; set; }
        public int Y { get; set; }

        public Tetromino(TetrominoType type)
        {
            Type = type;
            Shape = GetShape(type);
            Color = GetColor(type);
            X = 3; // стартовая позиция по X
            Y = 0; // стартовая позиция по Y
        }

        private int[,] GetShape(TetrominoType type)
        {
            switch (type)
            {
                case TetrominoType.I:
                    return new int[,] { { 1, 1, 1, 1 } };
                case TetrominoType.O:
                    return new int[,] { { 1, 1 }, { 1, 1 } };
                case TetrominoType.T:
                    return new int[,] { { 0, 1, 0 }, { 1, 1, 1 } };
                case TetrominoType.S:
                    return new int[,] { { 0, 1, 1 }, { 1, 1, 0 } };
                case TetrominoType.Z:
                    return new int[,] { { 1, 1, 0 }, { 0, 1, 1 } };
                case TetrominoType.J:
                    return new int[,] { { 1, 0, 0 }, { 1, 1, 1 } };
                case TetrominoType.L:
                    return new int[,] { { 0, 0, 1 }, { 1, 1, 1 } };
                default:
                    return new int[0, 0];
            }
        }

        private Color GetColor(TetrominoType type)
        {
            switch (type)
            {
                case TetrominoType.I: return Colors.Cyan;
                case TetrominoType.O: return Colors.Yellow;
                case TetrominoType.T: return Colors.Purple;
                case TetrominoType.S: return Colors.LimeGreen;
                case TetrominoType.Z: return Colors.Red;
                case TetrominoType.J: return Colors.Blue;
                case TetrominoType.L: return Colors.Orange;
                default: return Colors.White;
            }
        }

        public void Rotate()
        {
            int row = Shape.GetLength(0);
            int col = Shape.GetLength(1);
            int[,] rotated = new int[col, row];
            for (int y = 0; y < row; y++)
                for (int x = 0; x < col; x++)
                    rotated[x, row - y - 1] = Shape[y, x];
            Shape = rotated;
        }
    }
} 