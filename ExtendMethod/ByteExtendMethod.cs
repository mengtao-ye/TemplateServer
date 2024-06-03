using System;

namespace YSF
{
    public static class ByteExtendMethod
    {



        /// <summary>
        ///  获取截取的字节数据
        /// </summary>
        /// <param name="bs"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <param name="verse">反向获取当前数组里面的值</param>
        /// <returns></returns>
        public static byte[] GetBytes(this byte[] bs, int startIndex, int length, bool verse = false)
        {
            if (bs.Length == 0)
            {
                throw new Exception("byte长度不能为0！");
            }
            if (bs.Length < length)
            {
                throw new Exception("byte数据长度不足！");
            }
            if (startIndex < 0 || startIndex > bs.Length - 1 || length - startIndex > bs.Length)
            {
                throw new Exception("开始下标错误！");
            }
            byte[] tempData = new byte[length];
            int index = 0;
            if (!verse)
            {
                for (int i = startIndex; i < startIndex + length; i++)
                {
                    tempData[index++] = bs[i];
                }
            }
            else
            {
                for (int i = startIndex + length - 1; i >= startIndex; i--)
                {
                    tempData[index++] = bs[i];
                }
            }
            return tempData;
        }
    }
}
