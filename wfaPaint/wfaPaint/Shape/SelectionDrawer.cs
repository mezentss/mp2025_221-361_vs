using System.Drawing;
using System.Drawing.Drawing2D;
using wfaPaint.Managers;

namespace wfaPaint.Shape
{
    public class SelectionDrawer : ShapeDrawerBase
    {
        private Rectangle selectionRect;
        private Rectangle selectionOldRect;
        private Bitmap selectedBitmap;
        private bool isMoving = false;
        private Point selectionOffset;
        private bool hasSelection = false;

        public Rectangle SelectionRect { get => selectionRect; set => selectionRect = value; }

        public SelectionDrawer(DrawingManager drawingManager) : base(drawingManager) { }
        
        public void CopySelection()
        {
            if (hasSelection && selectedBitmap != null)
            {
                drawingManager.CopyToClipboard(selectionRect);
            }
        }

        public void PasteFromClipboard()
        {
            if (hasSelection && selectedBitmap != null)
            {
                drawingManager.RestoreTemporaryState();
                DrawFillRect(selectionOldRect);
                
                drawingManager.PasteFromClipboard(selectionRect.Location);
                
                selectedBitmap = drawingManager.CaptureAreaBitmap(selectionRect);
                drawingManager.SaveTemporaryState();
            }
        }
        
        public void ClearSelection()
        {
            if (hasSelection && selectedBitmap != null)
            {
                drawingManager.RestoreTemporaryState();
                hasSelection = false;
                selectedBitmap = null;
            }
        }
       
        public override void OnPaint(Graphics g)
        {
            if (hasSelection)
            {
                if (isMoving && selectedBitmap != null)
                {
                    g.DrawImage(selectedBitmap, selectionRect);
                }

                DrawBorder(g);
            }
        }

        private void DrawBorder(Graphics g)
        {
            using (Pen selectionPen = new Pen(Color.Black, 1))
            {
                selectionPen.DashStyle = DashStyle.Dash;
                selectionPen.DashPattern = new float[] { 3, 3 };
                g.DrawRectangle(selectionPen, selectionRect);
            }
        }

        public override void OnMouseDown(Point location)
        {
            if (hasSelection && selectionRect.Contains(location) && selectedBitmap != null)
            {
                isMoving = true;
                selectionOffset = new Point(location.X - selectionRect.X, location.Y - selectionRect.Y);
                DrawFillRect(selectionOldRect);
            }
            else
            {
                if (hasSelection && selectedBitmap != null)
                {
                    ClearSelection();
                }
                isMoving = false;
                hasSelection = false;
            }
        }
        
        public override void OnMouseMove(Point location)
        {
            if (isMoving)
            {
                int newX = location.X - selectionOffset.X;
                int newY = location.Y - selectionOffset.Y;
                
                selectionRect = new Rectangle(newX, newY, selectionRect.Width, selectionRect.Height);
                drawingManager.Invalidate();
            }
            else
            {
                base.OnMouseMove(location);
            }
        }
        
        public override void OnMouseUp(Point location)
        {
            if (isMoving)
            {
                int newX = location.X - selectionOffset.X;
                int newY = location.Y - selectionOffset.Y;
                
                selectionRect = new Rectangle(newX, newY, selectionRect.Width, selectionRect.Height);
                
                drawingManager.RestoreTemporaryState();
                if (selectedBitmap != null)
                {
                    DrawFillRect(selectionOldRect);

                    drawingManager.DrawBitmap(selectedBitmap, selectionRect.Location);
                    drawingManager.SaveTemporaryState();
                }
                
                isMoving = false;
                hasSelection = false;
                selectedBitmap = null;
            }
            else
            {
                drawingManager.RestoreTemporaryState();
                
                selectionRect = CalculateRectangle(drawingManager.StartPoint, location);
                selectionOldRect = CalculateRectangle(drawingManager.StartPoint, location);
                
                if (selectionRect.Width > 0 && selectionRect.Height > 0)
                {
                    selectedBitmap = drawingManager.CaptureAreaBitmap(selectionRect);
                    hasSelection = true;
                    drawingManager.SaveTemporaryState();
                }
                else
                {
                    selectedBitmap = null;
                    hasSelection = false;
                }
            }
            
            drawingManager.Invalidate();
        }

        protected override void Draw(Point start, Point end)
        {
            Rectangle rect = CalculateRectangle(start, end);
            DrawBorder(rect);
            drawingManager.Invalidate();
        }
        private void DrawBorder(Rectangle rect)
        {
            using (Pen selectionPen = new Pen(Color.Black, 1))
            {
                selectionPen.DashStyle = DashStyle.Dash;
                selectionPen.DashPattern = new float[] { 3, 3 };
                drawingManager.DrawRectangleWithPen(rect, selectionPen);
            }
        }
        public void DrawFillRect(Rectangle rect)
        {
            using (Brush selectionBrush = new SolidBrush(Control.DefaultBackColor))
            {
                drawingManager.FillRectangle(rect, selectionBrush);
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