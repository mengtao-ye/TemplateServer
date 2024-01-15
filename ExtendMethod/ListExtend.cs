using System.Collections.Generic;

namespace YSF
{
    public static class ListExtend
    {
        public static void Recycle<T>(this IList<T> @this) where T : IPool
        {
            if (@this.IsNullOrEmpty()) return;
            for (int i = 0; i < @this.Count; i++)
            {
                @this[i]?.Recycle();
            }
        }
    }
}
