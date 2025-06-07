using System.Drawing;
using wfaPaint.Managers;

namespace wfaPaint.Shape
{
    public class PencilDrawer : ShapeDrawerBase
    {
        private DrawingManager drawingManager;
        
        public PencilDrawer(DrawingManager drawingManager): base(drawingManager)
        {
            this.drawingManager = drawingManager;
        }
        
        public override void OnMouseMove(Point location)
        {
            drawingManager.DrawLine(drawingManager.CurrentPoint, location);
        }

        protected override void Draw(Point start, Point end)
        {
            drawingManager.DrawLine(drawingManager.CurrentPoint, end);
        }
    }
} 