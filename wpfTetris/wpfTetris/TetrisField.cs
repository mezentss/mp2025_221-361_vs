using System;
using System.Windows.Media;

namespace wpfTetris
{
    public class TetrisField
    {
        public readonly int Width = 10;
        public readonly int Height = 18;
        public Color?[,] Field { get; }

        public TetrisField()
        {
            Field = new Color?[Width, Height];
        }

        public bool IsCellFree(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height && Field[x, y] == null;
        }

        public bool CanPlace(Tetromino tetromino, int offsetX, int offsetY)
        {
            var shape = tetromino.Shape;
            for (int y = 0; y < shape.GetLength(0); y++)
            {
                for (int x = 0; x < shape.GetLength(1); x++)
                {
                    if (shape[y, x] == 0) continue;
                    int fx = offsetX + x;
                    int fy = offsetY + y;
                    if (fx < 0 || fx >= Width || fy < 0 || fy >= Height) return false;
                    if (Field[fx, fy] != null) return false;
                }
            }
            return true;
        }

        public void PlaceTetromino(Tetromino tetromino)
        {
            var shape = tetromino.Shape;
            for (int y = 0; y < shape.GetLength(0); y++)
            {
                for (int x = 0; x < shape.GetLength(1); x++)
                {
                    if (shape[y, x] == 0) continue;
                    int fx = tetromino.X + x;
                    int fy = tetromino.Y + y;
                    if (fx >= 0 && fx < Width && fy >= 0 && fy < Height)
                        Field[fx, fy] = tetromino.Color;
                }
            }
        }

        public int ClearLines()
        {
            int linesCleared = 0;
            for (int y = Height - 1; y >= 0; y--)
            {
                bool full = true;
                for (int x = 0; x < Width; x++)
                {
                    if (Field[x, y] == null)
                    {
                        full = false;
                        break;
                    }
                }
                if (full)
                {
                    linesCleared++;
                    for (int yy = y; yy > 0; yy--)
                        for (int x = 0; x < Width; x++)
                            Field[x, yy] = Field[x, yy - 1];
                    for (int x = 0; x < Width; x++)
                        Field[x, 0] = null;
                    y++; // пересмотреть ту же строку
                }
            }
            return linesCleared;
        }

        public void Clear()
        {
            for (int y = 0; y < Height; y++)
                for (int x = 0; x < Width; x++)
                    Field[x, y] = null;
        }
    }
} 