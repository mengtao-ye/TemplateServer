using System.Net;

namespace YSF
{
    public class UdpRequestHandleManager
    {
        private UdpServer mServer;
        public UdpRequestHandleManager(UdpServer server)
        {
            mServer = server;
        }
        public byte[] Response(short udpCode, byte[] data, EndPoint point)
        {
            short requestCode =(short)( (short)(udpCode / CommonConstData.REQUESTCODE_SPAN)* CommonConstData.REQUESTCODE_SPAN);
            if (mServer.map.Contains(requestCode))
            {
                return mServer.map.Get(requestCode).Response(udpCode, data, point);
            }
            else
            {
                Debug.Log("ReqeustCode:" + requestCode + " not exist!");
                return null;
            }
        }
    }
}