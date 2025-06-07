using System;
using System.Collections.Generic;
using System.IO;

namespace cnsGenRoad
{
    public class RoadMap
    {
        private readonly int _width;
        private readonly int _height;
        private char[,] _grid;

        private static readonly Dictionary<(char, char), char> ConnectionRules = new Dictionary<(char, char), char>
        {
            // Horizontal and vertical connections
            { ('─', '│'), '┼' }, { ('│', '─'), '┼' },
            
            // Corners with horizontal lines
            { ('─', '┌'), '┬' }, { ('┌', '─'), '┬' },
            { ('─', '┐'), '┬' }, { ('┐', '─'), '┬' },
            { ('─', '└'), '┴' }, { ('└', '─'), '┴' },
            { ('─', '┘'), '┴' }, { ('┘', '─'), '┴' },
            
            // Corners with vertical lines
            { ('│', '┌'), '├' }, { ('┌', '│'), '├' },
            { ('│', '┐'), '┤' }, { ('┐', '│'), '┤' },
            { ('│', '└'), '├' }, { ('└', '│'), '├' },
            { ('│', '┘'), '┤' }, { ('┘', '│'), '┤' },
            
            // Complex connections
            { ('┌', '┘'), '┼' }, { ('┘', '┌'), '┼' },
            { ('└', '┐'), '┼' }, { ('┐', '└'), '┼' }
        };

        public RoadMap(int width, int height)
        {
            _width = width;
            _height = height;
            ClearGrid();
        }

        public void CreateHorizontalLine(int startX, int endX, int y)
        {
            if (y < 0 || y >= _height || startX < 0 || endX >= _width || startX > endX)
                throw new ArgumentException("Invalid coordinates for horizontal line");

            for (int x = startX; x <= endX; x++)
            {
                ConnectSymbol(x, y, '─');
            }
        }

        public void CreateVerticalLine(int startY, int endY, int x)
        {
            if (x < 0 || x >= _width || startY < 0 || endY >= _height || startY > endY)
                throw new ArgumentException("Invalid coordinates for vertical line");

            for (int y = startY; y <= endY; y++)
            {
                ConnectSymbol(x, y, '│');
            }
        }

        public void CreateRectangle(int x1, int y1, int x2, int y2)
        {
            if (x1 < 0 || y1 < 0 || x2 >= _width || y2 >= _height || x1 >= x2 || y1 >= y2)
                throw new ArgumentException("Invalid coordinates for rectangle");

            // Corners
            ConnectSymbol(x1, y1, '┌');
            ConnectSymbol(x2, y1, '┐');
            ConnectSymbol(x1, y2, '└');
            ConnectSymbol(x2, y2, '┘');

            // Edges
            CreateHorizontalLine(x1 + 1, x2 - 1, y1);
            CreateHorizontalLine(x1 + 1, x2 - 1, y2);
            CreateVerticalLine(y1 + 1, y2 - 1, x1);
            CreateVerticalLine(y1 + 1, y2 - 1, x2);
        }

        public void ExportToFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Filename cannot be empty");

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fullPath = Path.Combine(documentsPath, $"{fileName}.txt");

            try
            {
                using (var writer = new StreamWriter(fullPath))
                {
                    for (int y = 0; y < _height; y++)
                    {
                        for (int x = 0; x < _width; x++)
                        {
                            writer.Write(_grid[y, x]);
                        }
                        writer.WriteLine();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to save map to file: {ex.Message}", ex);
            }
        }

        public void ImportFromFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Filename cannot be empty");

            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string fullPath = Path.Combine(documentsPath, $"{fileName}.txt");

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Map file not found: {fileName}");

            ClearGrid();

            try
            {
                using (var reader = new StreamReader(fullPath))
                {
                    for (int y = 0; y < _height && !reader.EndOfStream; y++)
                    {
                        string line = reader.ReadLine();
                        if (line == null) break;

                        for (int x = 0; x < Math.Min(_width, line.Length); x++)
                        {
                            _grid[y, x] = line[x];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new IOException($"Failed to load map from file: {ex.Message}", ex);
            }
        }

        public void Display()
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    Console.Write(_grid[y, x]);
                }
                Console.WriteLine();
            }
        }

        private void ClearGrid()
        {
            _grid = new char[_height, _width];
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    _grid[y, x] = ' ';
                }
            }
        }

        private void ConnectSymbol(int x, int y, char newSymbol)
        {
            if (x < 0 || x >= _width || y < 0 || y >= _height)
                return;

            char existing = _grid[y, x];
            _grid[y, x] = CombineSymbols(existing, newSymbol);
        }

        private char CombineSymbols(char existing, char added)
        {
            if (existing == ' ') return added;
            if (added == ' ') return existing;
            if (existing == added) return existing;

            if (ConnectionRules.TryGetValue((existing, added), out char combined))
                return combined;

            return existing;
        }
    }
}