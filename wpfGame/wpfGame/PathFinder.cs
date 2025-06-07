using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfGame
{
    public static class PathFinder
    {
        public static List<(int, int)> FindPath(int[,] grid, int targetSum)
        {
            // Алгоритм поиска пути (например, backtracking)
            return new List<(int, int)>();
        }

        public static bool ValidatePath(List<(int, int)> path, int[,] grid, int targetSum)
        {
            int sum = 0;
            foreach (var (i, j) in path)
            {
                sum += grid[i, j];
            }
            return sum == targetSum;
        }
    }
}
