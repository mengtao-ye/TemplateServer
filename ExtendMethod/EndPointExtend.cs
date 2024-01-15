using System.Net;

namespace YSF
{
    public static class EndPointExtend
    {
        public static byte[] ToBytes(this EndPoint endPoint)
        {
            if (endPoint == null) return null;
            string[] strs = endPoint.ToString().Split(':');
            byte[] bytes = new byte[8];
            string[] ips = strs[0].Split('.');
            for (int i = 0; i < 4; i++)
            {
                bytes[i] = ips[i].ToByte();
            }
            byte[] pointBytes = strs[1].ToInt().ToBytes();
            for (int i =4; i < 8; i++)
            {
                bytes[i] = pointBytes[i-4];
            }
            return bytes;
        }
    }
}
