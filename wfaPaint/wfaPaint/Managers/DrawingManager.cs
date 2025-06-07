using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using wfaPaint.Shape;
using wfaPaint.Drawing;

namespace wfaPaint.Managers
{
    public class DrawingManager
    {
        private Bitmap mainBitmap;
        private Bitmap tempBitmap;
        private Graphics graphics;
        private Pen currentPen;
        private bool isDrawing;
        private PictureBox canvas;
        private HistoryManager historyManager;
        
        public DrawingTool CurrentTool { get; set; } = DrawingTool.Pencil;
        
        public Point StartPoint { get; private set; }
        public Point CurrentPoint { get; private set; }

        public DrawingManager(PictureBox canvas, ColorManager colorManager)
        {
            this.canvas = canvas;
            InitializeGraphics();
            historyManager = new HistoryManager(20);
            
            currentPen = new Pen(colorManager.CurrentColor, 5);
            currentPen.StartCap = currentPen.EndCap = LineCap.Round;
            
            colorManager.ColorChanged += (color) => currentPen.Color = color;

            canvas.MouseDown += Canvas_MouseDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseUp += Canvas_MouseUp;
            canvas.Paint += Canvas_Paint;
        }
        
        private void InitializeGraphics()
        {
            mainBitmap = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            graphics = Graphics.FromImage(mainBitmap);
            graphics.Clear(Control.DefaultBackColor);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }

        public void Undo()
        {
            if (historyManager.CanUndo)
            {
                mainBitmap = historyManager.Undo(mainBitmap);
                graphics.Dispose();
                graphics = Graphics.FromImage(mainBitmap);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                canvas.Invalidate();
            }
        }

        public void Redo()
        {
            if (historyManager.CanRedo)
            {
                mainBitmap = historyManager.Redo(mainBitmap);
                graphics.Dispose();
                graphics = Graphics.FromImage(mainBitmap);
                graphics.SmoothingMode = SmoothingMode.AntiAlias;
                canvas.Invalidate();
            }
        }
        
        public void SetPenWidth(int width)
        {
            currentPen.Width = width;
        }
        
        public float GetPenWidth()
        {
            return currentPen.Width;
        }
        
        public event Action<Point> MouseDown;
        public event Action<Point> MouseMove;
        public event Action<Point> MouseUp;
        public event Action<Graphics> MousePaint;
        
        private void Canvas_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDrawing = true;
                StartPoint = e.Location;
                CurrentPoint = e.Location;
                if (CurrentTool != DrawingTool.Selection)
                    SaveTemporaryState();
                
                MouseDown?.Invoke(e.Location);
                
                if (CurrentTool != DrawingTool.Selection)
                {
                    historyManager.SaveState(mainBitmap);
                }
            }
            canvas.Refresh();
        }
        
        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDrawing && e.Button == MouseButtons.Left)
            {
                MouseMove?.Invoke(e.Location);
                CurrentPoint = e.Location;
            }
            canvas.Refresh();
        }

        private void Canvas_MouseUp(object sender, MouseEventArgs e)
        {
            if (isDrawing && e.Button == MouseButtons.Left)
            {
                CurrentPoint = e.Location;
                MouseUp?.Invoke(e.Location);
                isDrawing = false;
                

            }
            canvas.Refresh();
        }

        private void Canvas_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawImage(mainBitmap, 0, 0);
            MousePaint?.Invoke(e.Graphics);
        }
        
        public void SaveTemporaryState()
        {
            tempBitmap = (Bitmap)mainBitmap.Clone();
        }
        
        public void RestoreTemporaryState()
        {
            graphics.Dispose();
            mainBitmap.Dispose();
            
            mainBitmap = (Bitmap)tempBitmap.Clone();
            graphics = Graphics.FromImage(mainBitmap);
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
        }
        
        public void DrawLine(Point start, Point end)
        {
            graphics.DrawLine(currentPen, start, end);
            canvas.Invalidate();
        }
        
        public void DrawLineWithCustomPen(Point start, Point end, Pen customPen)
        {
            graphics.DrawLine(customPen, start, end);
            canvas.Invalidate();
        }
        
        public void DrawEllipse(Rectangle rect)
        {
            graphics.DrawEllipse(currentPen, rect);
            canvas.Invalidate();
        }
        
        public void DrawRectangle(Rectangle rect, Graphics g = null)
        {
            (g?? graphics).DrawRectangle(currentPen, rect);
            canvas.Invalidate();
        }
        
        public void DrawPolygon(Point[] points)
        {
            graphics.DrawPolygon(currentPen, points);
            canvas.Invalidate();
        }
        
        public void DrawPath(GraphicsPath path)
        {
            graphics.DrawPath(currentPen, path);
            canvas.Invalidate();
        }
        
        public void DrawString(string text, Point location, Font font = null)
        {
            font ??= new Font("Arial", 12);
            
            using (SolidBrush brush = new SolidBrush(currentPen.Color))
            {
                graphics.DrawString(text, font, brush, location);
                canvas.Invalidate();
            }
        }
        
        public void Invalidate()
        {
            canvas.Invalidate();
        }
        
        public Color GetPixelColor(Point location)
        {
            if (location.X < 0 || location.X >= mainBitmap.Width || 
                location.Y < 0 || location.Y >= mainBitmap.Height)
            {
                return Color.Empty;
            }
            
            try
            {
                return mainBitmap.GetPixel(location.X, location.Y);
            }
            catch
            {
                return Color.Empty;
            }
        }

        public void FillRectangle(Rectangle rect, Brush brush)
        {
            graphics.FillRectangle(brush, rect);
            canvas.Invalidate();
        }

        public void DrawRectangleWithPen(Rectangle rect, Pen pen)
        {
            graphics.DrawRectangle(pen, rect);
            canvas.Invalidate();
        }
        
        public Bitmap CaptureAreaBitmap(Rectangle area)
        {
            Rectangle validArea = new Rectangle(
                Math.Max(0, area.X),
                Math.Max(0, area.Y),
                Math.Min(area.Width, mainBitmap.Width - Math.Max(0, area.X)),
                Math.Min(area.Height, mainBitmap.Height - Math.Max(0, area.Y))
            );
            
            if (validArea.Width <= 0 || validArea.Height <= 0)
            {
                return new Bitmap(1, 1);
            }
            
            Bitmap bitmap = new Bitmap(validArea.Width, validArea.Height);
            
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawImage(mainBitmap, 
                    new Rectangle(0, 0, validArea.Width, validArea.Height),
                    validArea,
                    GraphicsUnit.Pixel);
            }
            
            return bitmap;
        }
        
        public void DrawBitmapWithRect(Bitmap bitmap, Rectangle rect)
        {
            if (bitmap != null)
            {
                graphics.DrawImage(bitmap, rect);
                canvas.Invalidate();
            }
        }

        public void DrawBitmap(Bitmap bitmap, Point location)
        {
            if (bitmap != null)
            {
                graphics.DrawImage(bitmap, location);
                canvas.Invalidate();
            }
        }

        public void CopyToClipboard(Rectangle area)
        {
            Bitmap capturedBitmap = CaptureAreaBitmap(area);
            if (capturedBitmap != null)
            {
                Clipboard.SetImage(capturedBitmap);
                capturedBitmap.Dispose();
            }
        }

        public void PasteFromClipboard(Point location)
        {
            if (Clipboard.ContainsImage())
            {
                Image clipboardImage = Clipboard.GetImage();
                if (clipboardImage != null)
                {
                    DrawBitmap((Bitmap)clipboardImage, location);
                }
            }
        }
    }
} 