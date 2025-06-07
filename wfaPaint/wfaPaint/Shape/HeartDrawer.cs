using System.Drawing;
using wfaPaint.Managers;
using System.Drawing.Drawing2D;
namespace wfaPaint.Shape
{
    public class HeartDrawer : ShapeDrawerBase
    {
        public HeartDrawer(DrawingManager drawingManager) : base(drawingManager) { }
        
        protected override void Draw(Point start, Point end)
        {
            Rectangle rect = CalculateRectangle(start, end);
            
            int size = Math.Min(rect.Width, rect.Height);
            rect.Width = size;
            rect.Height = (int)(size * 0.9);
            
            using (GraphicsPath path = new GraphicsPath())
            {
                int halfWidth = rect.Width / 2;
                int halfHeight = rect.Height / 2;
                int quarterWidth = rect.Width / 4;
                
                path.AddBezier(
                    rect.X + halfWidth, rect.Y + rect.Height,  
                    rect.X, rect.Y + halfHeight,                  
                    rect.X, rect.Y,                               
                    rect.X + halfWidth, rect.Y + quarterWidth    
                );
                
                path.AddBezier(
                    rect.X + halfWidth, rect.Y + quarterWidth,   
                    rect.X + rect.Width, rect.Y,                  
                    rect.X + rect.Width, rect.Y + halfHeight,     
                    rect.X + halfWidth, rect.Y + rect.Height    
                );
                
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