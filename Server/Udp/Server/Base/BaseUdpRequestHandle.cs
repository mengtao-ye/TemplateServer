using System;
using System.Collections.Generic;
using System.Net;

namespace YSF
{
    /// <summary>
    ///udp请求处理数据基类
    /// </summary>
    public abstract class BaseUdpRequestHandle : IUdpRequestHandle
    {
        /// <summary>
        /// 请求类型编码
        /// </summary>
        public abstract short requestCode {  get;  }
        public ServerCenter center { get; set ; }
        protected IUdpServer mServer;
        private Dictionary<short, Func<byte[], EndPoint, byte[]>> mActionDict;
        public BaseUdpRequestHandle(IUdpServer server)
        {
            mServer = server;
            mActionDict = new Dictionary<short, Func<byte[], EndPoint, byte[]>>();
            ComfigActionCode();
        }
        public byte[] Response(short udpCode, byte[] data, EndPoint point)
        {
            if (mActionDict.ContainsKey(udpCode))
            {
                return mActionDict[udpCode].Invoke(data, point);
            }
            else
            {
                Debug.Log("UdpCode:" + udpCode + " not exist!");
                return null;
            }
        }
        protected abstract void ComfigActionCode();
        public void Add(short code, Func<byte[], EndPoint, byte[]> action)
        {
            if (mActionDict.ContainsKey(code))
            {
                Debug.LogError("Code:" + code + "已注册");
                return;
            }
            mActionDict.Add(code, action);
        }
    }

}