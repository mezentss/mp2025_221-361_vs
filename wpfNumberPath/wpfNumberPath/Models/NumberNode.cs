using System.Windows;
using System.Windows.Media;

namespace wpfNumberPath.Models
{
    public class NumberNode
    {
        public int Value { get; set; }
        public Point Position { get; set; }
        public bool IsSelected { get; set; }
        public bool IsPartOfSolution { get; set; }
        public Brush Background { get; set; } = Brushes.White;
    }
} 