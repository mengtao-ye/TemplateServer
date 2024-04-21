using System.Collections.Generic;

namespace YSF
{
    public interface IListData<T> : IPool
    {
        List<T> list { get; set; }
        T this[int index] { get; set; }
        int Count { get; }
        void Add(T item);
        void Clear();
        bool Contains(T item);
        int IndexOf(T item);
        void Insert(int index, T item);
        bool Remove(T item);
        void RemoveAt(int index);
        T[] ToArray();
    }
}
