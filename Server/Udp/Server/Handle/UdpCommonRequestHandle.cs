using System.Net;

namespace YSF
{
    public class UdpCommonRequestHandle : BaseUdpRequestHandle
    {
        public override short requestCode => (short)UdpRequestCode.Common;
        public UdpCommonRequestHandle( UdpServer center) : base(center)
        {

        }
        protected override void ComfigActionCode()
        {
            Add((short)UdpCode.ClientBigDataResponse, ClientBigDataResponse);
        }
        private byte[] ClientBigDataResponse(byte[] data,EndPoint point)
        {
            if (data.IsNullOrEmpty()) return null;
            IDictionaryData<byte, byte[]> dict = data.ToBytesDictionary();
            int userID = dict[0].ToInt();
            ushort index =dict[1].ToUShort();
            bool isReceive = dict[2].ToBool() ;
            short udpCode = dict[3].ToShort();
            mServer.ReceiveBigDataIndex(userID, index, udpCode, isReceive);
            dict.Recycle();
            return null;//这里只是接收数据，不需要再返回数据到客户端
        }
    }

}