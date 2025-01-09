using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesignObject
{
    public class UndoRedoHistory<T>
    {
        const int DefaultUndoCount = 10;
        LimitedStack<T> undoStack;
        LimitedStack<T> redoStack;

        public bool IsCanUndo
        {
            get
            {
                return this.undoStack.Count > 1;
            }
        }
        public bool IsCanRedo

        {
            get { return this.redoStack.Count > 0; }
        }



        public UndoRedoHistory()
            : this(DefaultUndoCount)
        {
        }
        public UndoRedoHistory(int defaultUndoCount)
        {
            undoStack = new LimitedStack<T>(defaultUndoCount);
            redoStack = new LimitedStack<T>(defaultUndoCount);
        }
        public T Undo(T current)
        {
            if (this.undoStack.Count == 0)
                return current;

            T state = this.undoStack.Pop();
            this.redoStack.Push(current);
            return state;
        }
        public T Redo(T current)
        {
            if (this.redoStack.Count == 0)
                return current;
            T state = this.redoStack.Pop();
            this.undoStack.Push(current);
            return state;
        }
        public void AddState(T state)
        {
            this.undoStack.Push(state);
            this.redoStack.Clear();
        }
        internal void Clear()
        {
            this.undoStack.Clear();
            this.redoStack.Clear();
        }
    }
    internal class LimitedStack<T>
    {
        List<T> list = new List<T>();
        readonly int capacity;
        public int Count
        {
            get { return this.list.Count; }
        }
        public LimitedStack(int capacity)
        {
            this.capacity = capacity;
        }
        internal T Pop()
        {
            T t = this.list[0];
            this.list.RemoveAt(0);
            return t;
        }
        internal void Push(T state)
        {
            this.list.Insert(0, state);
            if (this.list.Count > capacity)
            {
                this.list.RemoveAt(this.list.Count - 1);
            }
        }
        internal T Peek()
        {
            return this.list[0];
        }
        internal void Clear()
        {
            this.list.Clear();
        }
    }
}
