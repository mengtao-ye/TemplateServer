using System.Net;

namespace YSF
{
    public static class EndPointTools
    {
        /// <summary>
        /// 获取Point
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        public static IPEndPoint GetPoint(byte[] datas,int startIndex)
        {
            if (datas.IsNull() || datas.Length < 8)
            {
                return null;
            }
            return new IPEndPoint(new IPAddress(ByteTools.SubBytes(datas, startIndex, 4)), datas.ToInt(startIndex + 4));
        }
        /// <summary>
        /// IP地址转byte数组
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static byte[] IPAddresToBytes(string ip)
        {
            string[] strs = ip.Split(':');
            byte[] bytes = new byte[8];
            string[] ips = strs[0].Split('.');
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = ips[i].ToByte();
            }
            return bytes;
        }
    }
}
