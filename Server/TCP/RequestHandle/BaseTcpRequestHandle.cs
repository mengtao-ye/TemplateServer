using SubServer;
using System;
using System.Collections.Generic;
using System.Net;

namespace YSF
{
    public abstract class BaseTcpRequestHandle : ITCPRequestHandle
    {
        protected abstract short mRequestCode { get; }
        public short requestCode { get { return mRequestCode; } }
        protected Dictionary<short, Func<byte[],Client,byte[]>> mActionDict;
        private ITcpServer mServer;
        public BaseTcpRequestHandle( ITcpServer server )
        {
            mServer = server;
            mActionDict = new Dictionary<short, Func<byte[], Client, byte[]>>();
            ComfigActionCode();
        }
        public byte[] Response(short actionCode,byte[] data,Client client)
        {
            if (mActionDict.ContainsKey(actionCode))
            {
               return  mActionDict[actionCode].Invoke(data, client);
            }
            return null;
        }
        public void Add(short actionCode, Func<byte[],Client, byte[]> callBack)
        {
            if (mActionDict.ContainsKey(actionCode))
            {
                Debug.LogError("已包含ActioCode:" + actionCode.ToString());
                return;
            }
            if (callBack == null)
            {
                Debug.LogError("callBack is null");
                return;
            }
            mActionDict.Add(actionCode, callBack);
        }
        protected abstract void ComfigActionCode();
    }
}
