using System.Drawing;
using wfaPaint.Managers;

namespace wfaPaint.Shape
{
    public class StarDrawer : ShapeDrawerBase
    {
        public StarDrawer(DrawingManager drawingManager) : base(drawingManager) { }
        
        protected override void Draw(Point start, Point end)
        {
            Rectangle rect = CalculateRectangle(start, end);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            int radius = Math.Min(rect.Width, rect.Height) / 2;
            int innerRadius = radius / 2;
            
            Point[] points = new Point[10];
            
            for (int i = 0; i < 10; i++)
            {
                int currentRadius = (i % 2 == 0) ? radius : innerRadius;
                double angle = Math.PI / 2 + i * Math.PI / 5;
                
                points[i] = new Point(
                    (int)(center.X + currentRadius * Math.Cos(angle)),
                    (int)(center.Y - currentRadius * Math.Sin(angle))
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