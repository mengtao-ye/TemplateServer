using System.Collections.Generic;

namespace YSF
{
    public static class LinqExtend
    {
        /// <summary>
        /// 获取到对应的下标数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static T GetIndex<T>(this ICollection<T> collection,int index)
        {
            if (collection.IsNullOrEmpty()) return default(T);
            if(index <0 || index>=collection.Count) return default(T);
            int i = 0;
            IEnumerator<T> enumerator = collection.GetEnumerator();
            while (enumerator.MoveNext()) 
            {
                if (i == index) return enumerator.Current;
                i++;
            }
            return default(T);
        }
        /// <summary>
        /// 移除对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool RemoveTarget<T>(this ICollection<T> collection ,T target ) 
        {
            if (collection.IsNull()) return false;
            if (target.IsNull()) return false;
            if (collection.Contains(target))
            {
                return collection.Remove(target);
            }
            return false;
        } 
        /// <summary>
        /// 移除所有对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        public static void CustomRemoveAll<T>(this ICollection<T> collection,System.Predicate<T> predicate)
        {
            if (collection.IsNull()) return;
            int count = collection.Count;
            int index = 0;
            T value = default(T);
            for ( ; index < count; index++)
            {
                value = collection.GetIndex(index);
                if (value == null) continue;
                if (predicate(collection.GetIndex(index)))
                {
                    collection.RemoveTarget(value);
                    index = -1;//这里等于-1是因为下一轮会加一
                    count--;
                }
            }
        }
        /// <summary>
        /// 移除所有对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="predicate"></param>
        public static void CustomRemoveAllPool<T>(this ICollection<T> collection, System.Predicate<T> predicate) where T : IPool
        {
            if (collection.IsNull()) return;
            int count = collection.Count;
            int index = 0;
            T value = default(T);
            for (; index < count; index++)
            {
                value = collection.GetIndex(index);
                if (value == null) continue;
                if (predicate(collection.GetIndex(index)))
                {
                    collection.RemoveTarget(value);
                    index = -1;//这里等于-1是因为下一轮会加一
                    count--;
                    value.Recycle();
                }
            }
        }
    }
}
