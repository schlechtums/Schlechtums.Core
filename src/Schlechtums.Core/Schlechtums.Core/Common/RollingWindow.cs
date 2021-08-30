using System.Collections;
using System.Collections.Generic;

namespace Schlechtums.Core.Common
{
    public class RollingWindow<T> : IEnumerable<T>
    {
        public RollingWindow(int windowSize)
        {
            this._WindowSize = windowSize;
            this._Queue = new Queue<T>(windowSize);
        }

        private int _WindowSize;
        private Queue<T> _Queue;

        public void Add(T item)
        {
            if (this._Queue.Count == this._WindowSize)
                this._Queue.Dequeue();

            this._Queue.Enqueue(item);
        }

        public void Add(params T[] items)
        {
            foreach (var i in items)
            {
                this.Add(i);
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)_Queue).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_Queue).GetEnumerator();
        }
    }
}