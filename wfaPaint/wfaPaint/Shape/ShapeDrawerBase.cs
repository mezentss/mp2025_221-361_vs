using System.Drawing;
using wfaPaint.Managers;

namespace wfaPaint.Shape
{
    public abstract class ShapeDrawerBase
    {
        protected DrawingManager drawingManager;
        
        public ShapeDrawerBase(DrawingManager drawingManager)
        {
            this.drawingManager = drawingManager;
        }
        
        public virtual void OnMouseDown(Point location) { }
        
        public virtual void OnMouseMove(Point location)
        {
            drawingManager.RestoreTemporaryState();
            Draw(drawingManager.StartPoint, location);
        }
        
        public virtual void OnMouseUp(Point location)
        {
            Draw(drawingManager.StartPoint, location);
        }


        protected abstract void Draw(Point start, Point end);
        
        public EraserDrawer ToEraser(){
            if (this is EraserDrawer){
                return (EraserDrawer)this;
            }
            else{
                throw new Exception("This is not an EraserDrawer");
            }
        }
        public virtual void OnPaint(Graphics g) { }
        public SelectionDrawer ToSelection()
        {
            if (this is SelectionDrawer)
            {
                return (SelectionDrawer)this;
            }
            else
            {
                throw new Exception("This is not an SelectionDrawer");
            }
        }

    }
} 