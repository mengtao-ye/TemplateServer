using System.Net;

namespace YSF
{
    public class SubUdpRequestHandleManager
    {
        private SubUdpServer mServer;
        private IMap<short, IUdpRequestHandle> mHandleMap;
        public SubUdpRequestHandleManager(SubUdpServer server, IMap<short, IUdpRequestHandle> map)
        {
            mHandleMap = map;
            mServer = server;
        }

        public byte[] Response(short udpCode, byte[] data,EndPoint point)
        {
            short requestCode = (short)((udpCode / CommonConstData.REQUESTCODE_SPAN) * CommonConstData.REQUESTCODE_SPAN);
            if (mHandleMap.Contains(requestCode))
            {
                return mHandleMap.Get(requestCode).Response(udpCode, data, point);
            }
            else
            {
                Debug.Log("ReqeustCode:" + requestCode + " not exist!");
                return null;
            }
        }
    }
}
