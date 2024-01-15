using System.Net;

namespace YSF
{
    /// <summary>
    /// udp请求接收数据接口
    /// </summary>
    public interface IUdpRequestHandle
    {
         short requestCode { get; }
        byte[] Response(short udpCode, byte[] data, EndPoint point);
    }
}
