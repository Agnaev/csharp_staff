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
        private static int count = 0;
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

        T IReadOnlyList<T>.this[int index] {
            get
            {
                return _[index];
            }
        }

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
            foreach(var elem in _)
            {
                if (Equals(elem, item))
                {
                    return true;
                }
            }
            return false;
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
            Array.Resize(ref _, --count);
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
            foreach (T item in _)
            {
                yield return item;
            }
        }

        int IList<T>.IndexOf(T item)
        {
            int pos = -1;
            foreach(var elem in _)
            {
                pos++;
                if(Equals(elem, item))
                {
                    return pos;
                }
            }
            return -1;
        }

        void IList<T>.Insert(int index, T item)
        {
            if (index != count - 1)
            {
                for (int i = index; i < count - 1; i++)
                {
                    _[i] = _[i + 1];
                }
            }
            Array.Resize(ref _, --count);
        }

        bool ICollection<T>.Remove(T item)
        {
            int index = -1;
            foreach(var elem in _)
            {
                index++;
                if(Equals(item, elem))
                {
                    if (index != count - 1) // если не последний элемент
                    {
                        for (int i = index; i < count - 1; i++) // то сдвигать
                        {
                            _[i] = _[i + 1];
                        }
                        Array.Resize(ref _, --count);
                    }
                    return true;
                }
            }
            return false;
        }

        void IList<T>.RemoveAt(int index)
        {
            if (index != count - 1) // если не последний элемент
            {
                for (int i = index; i < count - 1; i++) // то сдвигать
                {
                    _[i] = _[i + 1];
                }
            }
            Array.Resize(ref _, --count);
        }

        int IList.Add(object value)
        {
            Array.Resize(ref _, ++count);
            _[count] = (T)value;
            return count;
            //throw new NotImplementedException();
        }

        public int Add(object value)
        {
            Array.Resize(ref _, ++count);
            _[count - 1] = (T)value;
            return count;
        }

        bool IList.Contains(object value)
        {
            foreach (var elem in _)
            {
                if (Equals(elem, (T)value))
                {
                    return true;
                }
            }
            return false;
        }

        void IList.Clear()
        {
            Array.Resize(ref _, 0);
        }

        public void ForEach(Action<T> act)
        {
            foreach(T t in _)
            {
                act(t);
            }
        }

        public bool All(Func<T, bool> act)
        {
            foreach(T t in _)
            {
                if (!act(t))
                {
                    return false;
                }
            }
            return true;
        }

        public bool Any(Func<T, bool> action)
        {
            foreach(T t in _)
            {
                if (action(t))
                {
                    return true;
                }
            }
            return false;
        }

        int IList.IndexOf(object value)
        {
            for(int index = 0; index < count; index++)
            {
                if(Equals((T)value, _[index]))
                {
                    return index;
                }
            }
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            if(index == count - 1)
            {
                Array.Resize(ref _, count - 1);
            }
            for(int i = index; i < count - 1; i++)
            {
                _[i] = _[i + 1];
            }
            Array.Resize(ref _, --count);
        }

        void IList.Remove(object value)
        {
            int index = -1;
            foreach (var elem in _)
            {
                index++;
                if (Equals((T)value, elem))
                {
                    if (index != count - 1) // если не последний элемент
                    {
                        for (int i = index; i < count - 1; i++) // то сдвигать
                        {
                            _[i] = _[i + 1];
                        }
                        Array.Resize(ref _, --count);
                    }
                }
            }
        }

        void IList.RemoveAt(int index)
        {
            if (index != count - 1) // если не последний элемент
            {
                for (int i = index; i < count - 1; i++) // то сдвигать
                {
                    _[i] = _[i + 1];
                }
            }
            Array.Resize(ref _, --count);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            //for(int i = index, j = 0; i < count; i++, j++)
            //{
            //    array[j] = _[i];
            //}

        }

        private bool Equals(T a, T b)
        {
            return Comparer<T>.Default.Compare(a, b) == 0;
        }
    }
}
