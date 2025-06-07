using System;
using System.Drawing;
using System.Windows.Forms;

namespace wfaPaint.Managers
{
    public class ColorManager
    {
        private Panel[] colorPanels;
        
        public Color CurrentColor { get; private set; }
        
        public event Action<Color> ColorChanged;
        
        public ColorManager(params Panel[] panels)
        {
            colorPanels = panels;
            CurrentColor = panels[0].BackColor; 
            
            foreach (var panel in panels)
            {
                panel.Click += (s, e) =>
                {
                    CurrentColor = panel.BackColor;
                    ColorChanged?.Invoke(CurrentColor);
                };
            }
        }
        
        public void SetCustomColor(Color color)
        {
            CurrentColor = color;
            ColorChanged?.Invoke(CurrentColor);
        }
    }
} 