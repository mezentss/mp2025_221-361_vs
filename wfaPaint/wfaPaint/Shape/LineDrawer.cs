using System.Drawing;
using wfaPaint.Managers;

namespace wfaPaint.Shape
{
    public class LineDrawer : ShapeDrawerBase
    {
        public LineDrawer(DrawingManager drawingManager) : base(drawingManager) { }
        
        protected override void Draw(Point start, Point end)
        {
            drawingManager.DrawLine(start, end);
        }
    }
} 