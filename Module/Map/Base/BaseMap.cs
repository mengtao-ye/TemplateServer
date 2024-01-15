using System.Collections.Generic;

namespace YSF
{
    /// <summary>
    /// 映射表基础模块
    /// </summary>
    /// <typeparam name="Tkey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public abstract class BaseMap<Tkey, TValue> : IMap<Tkey, TValue>
    {
        private IDictionary<Tkey, TValue> mDataDict;
        public ICollection<Tkey> Keys => mDataDict.Keys;
        public int Count => mDataDict.Count;
        public BaseMap()
        {
            mDataDict = new Dictionary<Tkey, TValue>();
            Config();
        }
        protected abstract void Config();
        public void Add(Tkey key, TValue value)
        {
            if (!mDataDict.ContainsKey(key))
            {
                mDataDict.Add(key, value);
            }
            else
            {
                Debug.LogError("已包含Key:" + key.ToString());
            }
        }

        public TValue Get(Tkey key)
        {
            if (mDataDict.ContainsKey(key)) return mDataDict[key];
           Debug.LogError("未找到Key:" + key.ToString());
            return default(TValue);
        }

        public bool Contains(Tkey key)
        {
            return mDataDict.ContainsKey(key);
        }
    }
}
