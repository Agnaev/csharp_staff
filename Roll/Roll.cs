using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roll
{
    public class Roll<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IList, ICollection, IReadOnlyList<T>, IReadOnlyCollection<T>
    {
        private bool isReadonly = false;
        private static int count = 1;
        private T[] _ = new T[count];
        T IList<T>.this[int index] {
            get 
            {
                return _[index];
            }
            set
            {
                _[index] = value;
            }
        }

        int ICollection<T>.Count => count;

        bool ICollection<T>.IsReadOnly => isReadonly;

        bool IList.IsReadOnly => isReadonly;

        bool IList.IsFixedSize => throw new NotImplementedException();

        int ICollection.Count => count;

        object ICollection.SyncRoot => throw new NotImplementedException();

        bool ICollection.IsSynchronized => throw new NotImplementedException();

        int IReadOnlyCollection<T>.Count => count;

        T IReadOnlyList<T>.this[int index] => _[index];

        object IList.this[int index] 
        { 
            get => _[index];
            set
            {
                _[index] = (T)value;
            }
        }

        void ICollection<T>.Add(T item)
        {
            Array.Resize(ref _, ++count);
            _[count] = item;
        }

        void ICollection<T>.Clear()
        {
            Array.Resize(ref _, 0);
        }

        bool ICollection<T>.Contains(T item)
        {
            throw new NotImplementedException(); //make binnary search
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            if(arrayIndex > count)
            {
                throw new IndexOutOfRangeException();
            }
            for(int i = 0, j = arrayIndex; j < count; i++, j++)
            {
                array[i] = _[j];
            }
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            foreach(T item in _)
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        int IList<T>.IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        int IList.Add(object value)
        {
            throw new NotImplementedException();
        }

        bool IList.Contains(object value)
        {
            throw new NotImplementedException();
        }

        void IList.Clear()
        {
            throw new NotImplementedException();
        }

        int IList.IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new NotImplementedException();
        }

        void IList.Remove(object value)
        {
            throw new NotImplementedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }
    }
}
