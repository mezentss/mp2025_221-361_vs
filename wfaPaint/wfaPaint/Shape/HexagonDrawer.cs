using System.Drawing;
using wfaPaint.Managers;

namespace wfaPaint.Shape
{
    public class HexagonDrawer : ShapeDrawerBase
    {
        public HexagonDrawer(DrawingManager drawingManager) : base(drawingManager) { }
        
        protected override void Draw(Point start, Point end)
        {
            Rectangle rect = CalculateRectangle(start, end);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            int radius = Math.Min(rect.Width, rect.Height) / 2;
            
            Point[] points = new Point[6];
            
            for (int i = 0; i < 6; i++)
            {
                double angle = Math.PI / 6 + i * Math.PI / 3; 
                
                points[i] = new Point(
                    (int)(center.X + radius * Math.Cos(angle)),
                    (int)(center.Y + radius * Math.Sin(angle))
                );
            }
            
            drawingManager.DrawPolygon(points);
        }
        
        private Rectangle CalculateRectangle(Point start, Point end)
        {
            int x = Math.Min(start.X, end.X);
            int y = Math.Min(start.Y, end.Y);
            int width = Math.Abs(end.X - start.X);
            int height = Math.Abs(end.Y - start.Y);

            return new Rectangle(x, y, width, height);
        }
    }
} 