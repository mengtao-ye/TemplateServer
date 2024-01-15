using System;
using System.Collections.Generic;

namespace YSF
{
    public abstract class BaseTcpRequestHandle : ITCPRequestHandle
    {
        protected abstract short mRequestCode { get; }
        public short requestCode { get { return mRequestCode; } }
        protected Dictionary<short, Func<byte[],byte[]>> mActionDict;
        private TCPServer mServer;
        public BaseTcpRequestHandle( TCPServer server )
        {
            mServer = server;
            mActionDict = new Dictionary<short, Func<byte[], byte[]>>();
            ComfigActionCode();
        }
        public byte[] Response(short actionCode,byte[] data)
        {
            if (mActionDict.ContainsKey(actionCode))
            {
               return  mActionDict[actionCode].Invoke(data);
            }
            return null;
        }
        public void Add(short actionCode, Func<byte[], byte[]> callBack)
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
