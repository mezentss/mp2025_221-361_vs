using System.Drawing;
using wfaPaint.Managers;

namespace wfaPaint.Shape
{
    public class ArrowDrawer : ShapeDrawerBase
    {
        public ArrowDrawer(DrawingManager drawingManager) : base(drawingManager) { }
        
        protected override void Draw(Point start, Point end)
        {
            int arrowSize = 15;
            float angle = (float)Math.Atan2(end.Y - start.Y, end.X - start.X);
            float arrowAngle = (float)(Math.PI / 6);
            
            drawingManager.DrawLine(start, end);
            
            Point[] arrowHead = new Point[3];
            arrowHead[0] = end;
            arrowHead[1] = new Point(
                (int)(end.X - arrowSize * Math.Cos(angle - arrowAngle)),
                (int)(end.Y - arrowSize * Math.Sin(angle - arrowAngle))
            );
            arrowHead[2] = new Point(
                (int)(end.X - arrowSize * Math.Cos(angle + arrowAngle)),
                (int)(end.Y - arrowSize * Math.Sin(angle + arrowAngle))
            );
            
            drawingManager.DrawPolygon(arrowHead);
        }
    }
} 