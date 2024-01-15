namespace YSF
{
    public enum UdpCode : short
    {
        //Common
        ServerBigDataResponse = UdpRequestCode.Common + 1,//服务器端大数据下标反馈
        ClientBigDataResponse = UdpRequestCode.Common + 2,//客户端大数据下标反馈
        //帧同步数据
        LockStep_ClientData = UdpRequestCode.LockStep + 1,//客户端帧同步数据
        LockStep_ServerData = UdpRequestCode.LockStep + 2,//服务器端帧同步数据
    }
}
