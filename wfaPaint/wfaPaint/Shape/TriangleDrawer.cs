using System.Drawing;
using wfaPaint.Managers;

namespace wfaPaint.Shape
{
    public class TriangleDrawer : ShapeDrawerBase
    {
        public TriangleDrawer(DrawingManager drawingManager) : base(drawingManager) {}
        
        protected override void Draw(Point start, Point end)
        {
            Point thirdPoint = new Point(start.X - (end.X - start.X), end.Y);
            
            Point[] trianglePoints = new Point[]
            {
                start,
                end,
                thirdPoint
            };
            
            drawingManager.DrawPolygon(trianglePoints);
        }
    }
} 