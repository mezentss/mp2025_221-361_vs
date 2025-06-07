using System.Drawing;
using wfaPaint.Managers;

namespace wfaPaint.Shape
{
    public class TextDrawer : ShapeDrawerBase
    {
        private readonly ShapeManager shapeManager;
        private Font textFont;
        public string TextToDraw { get; set; } = "";
        
        public TextDrawer(DrawingManager drawingManager, ShapeManager shapeManager) : base(drawingManager)
        {
            this.shapeManager = shapeManager;
            this.textFont = new Font("Arial", 16);
        }

        public void SetFont(Font font)
        {
            if (font != null)
            {
                textFont?.Dispose();
                textFont = new Font(font.FontFamily, font.Size, font.Style);
            }
        }

        protected override void Draw(Point start, Point end)
        {
            if (!string.IsNullOrEmpty(TextToDraw))
            {
                DrawText(end);
            }
        }

        public override void OnMouseUp(Point location)
        {
            DrawText(location);
            TextToDraw = "";
        }
        
        private void DrawText(Point location)
        {
            drawingManager.DrawString(TextToDraw, location, textFont);
        }
    }
} 