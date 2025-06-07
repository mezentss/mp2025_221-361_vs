using System.Drawing;
using System.Drawing.Drawing2D;
using wfaPaint.Managers;

namespace wfaPaint.Shape
{
    public class SpeechBubbleDrawer : ShapeDrawerBase
    {
        public SpeechBubbleDrawer(DrawingManager drawingManager) : base(drawingManager) { }
        
        protected override void Draw(Point start, Point end)
        {
            Rectangle rect = CalculateRectangle(start, end);
            
            if (rect.Width < 10 || rect.Height < 10)
            {
                drawingManager.DrawRectangle(rect);
                return;
            }
            
            using (GraphicsPath path = new GraphicsPath())
            {
                int cornerRadius = Math.Max(rect.Height / 5, 1);
                
                int diameter = cornerRadius * 2;
                
                diameter = Math.Min(diameter, Math.Min(rect.Width / 2, rect.Height / 2));
                
                if (diameter < 1)
                {
                    drawingManager.DrawRectangle(rect);
                    return;
                }
                
                Rectangle arcRect = new Rectangle(rect.X, rect.Y, diameter, diameter);
                
                path.AddArc(arcRect, 180, 90);
                
                arcRect.X = rect.Right - diameter;
                path.AddArc(arcRect, 270, 90);
                
                arcRect.Y = rect.Bottom - diameter;
                path.AddArc(arcRect, 0, 90);
                
                int tailSize = Math.Max(rect.Width / 5, 5); 
                path.AddLine(
                    rect.X + rect.Width / 2 + tailSize, rect.Bottom,
                    rect.X + rect.Width / 2, rect.Bottom + tailSize
                );
                path.AddLine(
                    rect.X + rect.Width / 2, rect.Bottom + tailSize,
                    rect.X + rect.Width / 2 - tailSize, rect.Bottom
                );
                
                arcRect.X = rect.Left;
                path.AddArc(arcRect, 90, 90);
                
                path.CloseFigure();
                
                drawingManager.DrawPath(path);
            }
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