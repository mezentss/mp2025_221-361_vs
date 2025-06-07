using System.Drawing;
using wfaPaint.Managers;

namespace wfaPaint.Shape
{
    public class RectangleDrawer : ShapeDrawerBase
    {
        public RectangleDrawer(DrawingManager drawingManager) : base(drawingManager) { }
        
        protected override void Draw(Point start, Point end)
        {
            Rectangle rect = CalculateRectangle(start, end);
            drawingManager.DrawRectangle(rect);
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