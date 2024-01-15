namespace YSF
{
    /// <summary>
    /// 通用数据
    /// </summary>
    public class CommonConstData
    {
        public const short REQUESTCODE_SPAN = 256;//请求事件间的间隔
        public const int INVAILD_VALUE = -1;//无效的值
        public const int UDP_BUFFER_SIZE = 10240000;//接收UDP数据数组的长度
        public const ushort UDP_SPLIT_LENGTH = 60000;//UDP通信时一段数据的长度

    }
}
