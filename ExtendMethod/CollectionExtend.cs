using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace YSF
{
    public static class CollectionExtend
    {
        /// <summary>
        /// 遍历字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="dict"></param>
        /// <param name="action"></param>
        /// <param name="value"></param>
        public static void Foreach<TKey, TValue, T1,T2>(this IDictionary<TKey, TValue> dict, Func<TKey, TValue, T1,T2,bool> action, T1 value,T2 value2)
        {
            if (dict.IsNullOrEmpty()) return;
            IEnumerator<KeyValuePair<TKey, TValue>> enumerator = dict.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (action != null)
                {
                    bool res = action(enumerator.Current.Key, enumerator.Current.Value, value, value2);
                    if (res) break;
                }
            }
        }
        /// <summary>
        ///遍历字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="action"></param>
        public static void Foreach<TKey, TValue,T>(this IDictionary<TKey, TValue> dict,Action<TKey, TValue,T> action,T value)
        {
            if (dict.IsNullOrEmpty()) return;
            IEnumerator<KeyValuePair<TKey, TValue>> enumerator = dict.GetEnumerator();
            while (enumerator.MoveNext())
            {
                if (action != null) action(enumerator.Current.Key, enumerator.Current.Value,value);
            }
        }
        /// <summary>
        ///遍历字典
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="action"></param>
        public static void Foreach<TKey, TValue>(this IDictionary<TKey, TValue> dict, Action<TKey, TValue> action)
        {
            if (dict.IsNullOrEmpty()) return;
            IEnumerator<KeyValuePair<TKey, TValue>> enumerator = dict.GetEnumerator();
            try
            {
                while (enumerator.MoveNext())
                {
                    if (action != null) action(enumerator.Current.Key, enumerator.Current.Value);
                }
            }
            catch {}
        }
        public static bool IsNullOrEmpty<T>(this ICollection<T> collection)
        {
            if (collection == null || collection.Count == 0) return true;
            return false;
        }
        public static bool IsNullOrEmpty<T>(this IListData<T> collection)
        {
            if (collection == null || collection.list.IsNullOrEmpty()) return true;
            return false;
        }
        public static TValue TryGet<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            if (dict == null || dict.Count == 0)
            {
                return default(TValue);
            }
            TValue value;
            if (dict.TryGetValue(key, out value))
            {
                return value;
            }
            return default(TValue);
        }

        public static void Disrupted<T>(this List<T> collection)
        {
            int randomData1, randomData2;
            T tempData;
            for (int i = 0; i < collection.Count; i++)
            {
                randomData1 = Random_My.Range(0, collection.Count);
                randomData2 = Random_My.Range(0, collection.Count);
                if (randomData1 != randomData2)
                {
                    tempData = collection[randomData1];
                    collection[randomData1] = collection[randomData2];
                    collection[randomData2] = tempData;
                }
            }
        }
    }

}