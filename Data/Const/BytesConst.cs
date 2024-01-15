using System;

namespace YSF
{
    /// <summary>
    /// 字节常量数据
    /// </summary>
    public static class BytesConst
    {
        /// <summary>
        /// 空字节
        /// </summary>
        public static byte[] Empty = new byte[0];
        /// <summary>
        /// true的常量字节数据
        /// </summary>
        public static byte[] TRUE_BYTES = BitConverter.GetBytes(true);
        /// <summary>
        /// false 的常量字节数据
        /// </summary>
        public static byte[] FALSE_BYTES = BitConverter.GetBytes(false);
    }
}
