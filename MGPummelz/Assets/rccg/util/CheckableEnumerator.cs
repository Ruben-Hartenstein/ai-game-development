using System;
using System.Collections;
using System.Collections.Generic;


namespace RelegatiaCCG.rccg.util
{
    public class CheckableEnumerator<T> : IEnumerator<T>
    {
        private IEnumerator<T> internalEnumerator;
        private bool reachedEnd;

        public T Current
        {
            get
            {
                return internalEnumerator.Current;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return internalEnumerator.Current;
            }
        }

        public CheckableEnumerator(IEnumerable<T> enumerable)
        {
            this.internalEnumerator = enumerable.GetEnumerator();
            this.reachedEnd = false;
        }

        public CheckableEnumerator(IEnumerator<T> enumerator) {
            this.internalEnumerator = enumerator;
            this.reachedEnd = false;
            }

        public bool MoveNext()
        {
            bool result = internalEnumerator.MoveNext();
            this.reachedEnd = !result;
            return result;
        }

        public void Reset()
        {
            internalEnumerator.Reset();
            this.reachedEnd = false;
        }

        public void Dispose()
        {
            internalEnumerator.Dispose();
        }

        public bool hasNext()
        {
            return !reachedEnd;
        }

        public bool endReached()
        {
            return reachedEnd;
        }
    }
}
