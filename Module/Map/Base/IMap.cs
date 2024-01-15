using System.Collections.Generic;

namespace YSF
{
    public interface IMap<TKey, TValue>
    {
        ICollection<TKey> Keys { get; }
        void Add(TKey key, TValue value);
        TValue Get(TKey key);
        bool Contains(TKey key);
        int Count { get; }
    }
}
