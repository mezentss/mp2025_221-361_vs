using System.Drawing;
using wfaPaint.Managers;

namespace wfaPaint.Shape
{
    public class EraserDrawer : ShapeDrawerBase
    {
        private Pen eraserPen;
        
        public EraserDrawer(DrawingManager drawingManager) : base(drawingManager)
        {
            eraserPen = new Pen(Control.DefaultBackColor, drawingManager.GetPenWidth());
            eraserPen.StartCap = eraserPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
        }
        
        public void SetWidth(float width)
        {
            eraserPen.Width = width;
        }
        
        public override void OnMouseMove(Point location)
        {
            drawingManager.DrawLineWithCustomPen(drawingManager.CurrentPoint, location, eraserPen);
        }

        protected override void Draw(Point start, Point end)
        {
            drawingManager.DrawLineWithCustomPen(drawingManager.CurrentPoint, end, eraserPen);
        }
    }
} 