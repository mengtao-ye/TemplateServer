using System;
using System.Net;

namespace YSF
{
    /// <summary>
    /// udp网络请求模式下的在线数据
    /// </summary>
    public class UdpOnLineData : IPool
    {
        public DateTime LastTime;//最后一次同步数据的时间
        public EndPoint Point;//玩家地址信息
        public bool isPop { get; set; }
        public void PopPool()
        {
        }
        public void PushPool()
        {
        }

        public void Recycle()
        {
            ClassPool<UdpOnLineData>.Push(this);
        }
    }
}
