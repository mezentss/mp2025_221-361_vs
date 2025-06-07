using System.Drawing;
using wfaPaint.Managers;

namespace wfaPaint.Shape
{
    public class DiamondDrawer : ShapeDrawerBase
    {
        public DiamondDrawer(DrawingManager drawingManager) : base(drawingManager) { }
        
        protected override void Draw(Point start, Point end)
        {
            Rectangle rect = CalculateRectangle(start, end);
            Point center = new Point(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);
            
            Point[] points = new Point[4];
            points[0] = new Point(center.X, rect.Y);
            points[1] = new Point(rect.X + rect.Width, center.Y);
            points[2] = new Point(center.X, rect.Y + rect.Height);
            points[3] = new Point(rect.X, center.Y);
            
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