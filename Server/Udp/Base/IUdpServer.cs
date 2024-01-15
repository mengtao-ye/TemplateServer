using System.Net;

namespace YSF
{
   public  interface IUdpServer
    {
        byte[] Response(byte[] data,  EndPoint point, short udpCode);
        void UdpSend(EndPoint point, short udpCode, byte[] data);
        void ReceiveBigDataIndex(int userID, ushort index, short udpCode, bool isReceive);
    }
}
