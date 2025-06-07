using System;
using System.Collections.Generic;
using System.Drawing;
using wfaPaint.Drawing;
using wfaPaint.Shape;

namespace wfaPaint.Managers
{
    public class ShapeManager
    {
        private DrawingManager drawingManager;
        private Dictionary<DrawingTool, ShapeDrawerBase> shapeDrawers;
        private ShapeDrawerBase currentDrawer;
        
        private DrawingTool currentTool;
        public DrawingTool CurrentTool
        {
            get => currentTool;
            set
            {

                if (currentTool == DrawingTool.Selection && value != DrawingTool.Selection)
                {

                    currentDrawer.ToSelection().ClearSelection();
                }
                
                currentTool = value;
                currentDrawer = shapeDrawers[value];
            }
        }

        public ShapeDrawerBase GetCurrentDrawer()
        {
            return currentDrawer;
        }

        public void setTextToDraw(String text)
        {
            if (currentTool == DrawingTool.Text)
            {
                ((TextDrawer)currentDrawer).TextToDraw = text;
            }
        }
        
        public void setTextFont(Font font)
        {
            if (currentTool == DrawingTool.Text && font != null)
            {
                ((TextDrawer)currentDrawer).SetFont(font);
            }
        }
        
        public ShapeManager(DrawingManager drawingManager)
        {
            this.drawingManager = drawingManager;
            currentDrawer = new PencilDrawer(drawingManager);
            shapeDrawers = new Dictionary<DrawingTool, ShapeDrawerBase>
            {
                { DrawingTool.Pencil, new PencilDrawer(drawingManager) },
                { DrawingTool.Line, new LineDrawer(drawingManager) },
                { DrawingTool.Ellipse, new EllipseDrawer(drawingManager) },
                { DrawingTool.Rectangle, new RectangleDrawer(drawingManager) },
                { DrawingTool.Triangle, new TriangleDrawer(drawingManager) },
                { DrawingTool.Arrow, new ArrowDrawer(drawingManager) },
                { DrawingTool.Star, new StarDrawer(drawingManager) },
                { DrawingTool.Hexagon, new HexagonDrawer(drawingManager) },
                { DrawingTool.Diamond, new DiamondDrawer(drawingManager) },
                { DrawingTool.Pentagon, new PentagonDrawer(drawingManager) },
                { DrawingTool.Heart, new HeartDrawer(drawingManager) },
                { DrawingTool.SpeechBubble, new SpeechBubbleDrawer(drawingManager) },
                { DrawingTool.Text, new TextDrawer(drawingManager, this) },
                { DrawingTool.Selection, new SelectionDrawer(drawingManager) },
                { DrawingTool.Eraser, new EraserDrawer(drawingManager) }
            };
            
            currentDrawer = shapeDrawers[DrawingTool.Pencil];
            
            drawingManager.MouseDown += OnMouseDown;
            drawingManager.MouseMove += OnMouseMove;
            drawingManager.MouseUp += OnMouseUp;
            drawingManager.MousePaint += OnPaint;
        }
        
        public void UpdateEraserWidth(float width)
        {
            currentDrawer.ToEraser().SetWidth(width);
        }
        
        private void OnMouseDown(Point location)
        {
            currentDrawer.OnMouseDown(location);
        }
        
        private void OnMouseMove(Point location)
        {
            currentDrawer.OnMouseMove(location);
        }

        private void OnMouseUp(Point location)
        {
            currentDrawer.OnMouseUp(location);
        }

        private void OnPaint(Graphics g)
        {
            currentDrawer.OnPaint(g);
        }
    }
} 