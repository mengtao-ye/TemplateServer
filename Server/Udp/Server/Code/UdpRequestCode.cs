namespace YSF
{
    public enum UdpRequestCode : short
    {
        Common = CommonConstData.REQUESTCODE_SPAN * 0,//共同数据相关请求
        LockStep = CommonConstData.REQUESTCODE_SPAN * 1,//帧同步相关请求
    }
}
