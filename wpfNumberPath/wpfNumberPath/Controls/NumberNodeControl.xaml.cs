using System.Windows;
using System.Windows.Controls;
using wpfNumberPath.Models;
using System.Windows.Media;

namespace wpfNumberPath.Controls
{
    public partial class NumberNodeControl : UserControl
    {
        public NumberNode Node { get; set; }
        public event RoutedEventHandler NumberSelected;

        public NumberNodeControl()
        {
            InitializeComponent();
        }

        public void SetNode(NumberNode node)
        {
            Node = node;
            DataContext = node;
            UpdateVisual();
        }

        private void MainButton_Click(object sender, RoutedEventArgs e)
        {
            NumberSelected?.Invoke(this, new RoutedEventArgs());
        }

        public void UpdateVisual()
        {
            if (Node == null) return;
            if (Node.IsSelected)
            {
                Circle.Fill = new SolidColorBrush(Color.FromRgb(33, 150, 243)); // синий
                NumberText.Foreground = Brushes.White;
            }
            else if (Node.IsPartOfSolution)
            {
                Circle.Fill = Brushes.Gold;
                NumberText.Foreground = Brushes.Black;
            }
            else
            {
                Circle.Fill = Brushes.White;
                NumberText.Foreground = new SolidColorBrush(Color.FromRgb(68, 68, 68)); // тёмно-серый
            }
        }
    }
} 