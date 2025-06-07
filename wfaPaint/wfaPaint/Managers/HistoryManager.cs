using System;
using System.Collections;
using System.Drawing;

namespace wfaPaint.Managers
{
    public class HistoryManager
    {
        private readonly Stack<Bitmap> undoStack;
        private readonly Stack<Bitmap> redoStack;
        private readonly int maxHistorySize;

        public HistoryManager(int maxHistorySize = 20)
        {
            this.maxHistorySize = maxHistorySize;
            undoStack = new Stack<Bitmap>();
            redoStack = new Stack<Bitmap>();
        }

        public void SaveState(Bitmap currentState)
        {
            redoStack.Clear();

            Bitmap stateCopy = new Bitmap(currentState);

            undoStack.Push(stateCopy);

            if (undoStack.Count > maxHistorySize)
            {
                undoStack.Pop().Dispose();
            }
        }

        public Bitmap Undo(Bitmap currentState)
        {
            if (undoStack.Count == 0)
                return currentState;

            Bitmap saveState = (Bitmap)currentState.Clone();
            redoStack.Push(saveState);

            Bitmap previousState = undoStack.Pop();

            if (redoStack.Count > maxHistorySize)
            {
                redoStack.Pop().Dispose();
            }

            return previousState;
        }

        public Bitmap Redo(Bitmap currentState)
        {
            if (redoStack.Count == 0)
                return currentState;
            Bitmap saveState = (Bitmap)currentState.Clone();

            undoStack.Push(new Bitmap(saveState));

            Bitmap nextState = redoStack.Pop();

            if (undoStack.Count > maxHistorySize)
            {
                undoStack.Pop().Dispose();
            }

            return nextState;
        }

        public bool CanUndo => undoStack.Count > 0;
        public bool CanRedo => redoStack.Count > 0;

        public void Clear()
        {
            while (undoStack.Count > 0)
                undoStack.Pop().Dispose();
            while (redoStack.Count > 0)
                redoStack.Pop().Dispose();
        }
    }
} 