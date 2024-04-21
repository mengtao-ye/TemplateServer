using System.Collections.Generic;

namespace YSF
{
    public static class ConverterDataTools
    {
        public static byte[] ToByte<T>(T value) where T : IDataConverter, new()
        {
            return value.ToBytes();
        }
        public static byte[] ToByte<T>(IList<T> value) where T : IDataConverter, new()
        {
            if (value == null || value.Count == 0) return null;
            IListData<byte[]> byteList = ClassPool<ListData<byte[]>>.Pop();
            for (int i = 0; i < value.Count; i++)
            {
                byteList.Add(value[i].ToBytes());
            }
            byte[] data = byteList.list.ToBytes();
            byteList.Recycle();
            return data;
        }

        public static T ToObject<T>(byte[] data) where T : IDataConverter, new()
        {
            T value = new T();
            value.ToValue(data);
            return value;
        }

        public static TPool ToObjectPool<TPool>(byte[] data, int startIndex = 0) where TPool : class,IDataConverter,IPool, new()
        {
            TPool value = ClassPool<TPool>.Pop();
            value.ToValue(ByteTools.SubBytes(data, startIndex));
            return value;
        }
        public static IListData<T> ToListObjectPool<T>(byte[] data, int startIndex = 0) where T : class,IDataConverter, IPool,new()
        {
            if (data == null || data.Length == 0) return null;
            IListData<T> byteList = ClassPool<ListPoolData<T>>.Pop();
            IListData<byte[]> listBytes = ListTools.ToList(data, startIndex);
            for (int i = 0; i < listBytes.Count; i++)
            {
                byteList.Add(ToObjectPool<T>(listBytes[i]));
            }
            listBytes.Recycle();
            return byteList;
        }
        public static IListData<T> ToListObject<T>(byte[] data, int startIndex = 0) where T : IDataConverter, new()
        {
            if (data == null || data.Length == 0) return null;
            IListData<T> byteList = ClassPool<ListData<T>>.Pop();
            IListData<byte[]> listBytes = ListTools.ToList(data, startIndex);
            for (int i = 0; i < listBytes.Count; i++)
            {
                byteList.Add(ToObject<T>(listBytes[i]));
            }
            listBytes.Recycle();
            return byteList;
        }
    }
}
