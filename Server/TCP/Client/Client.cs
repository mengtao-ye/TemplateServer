using System;
using System.Net.Sockets;


namespace YSF
{
    public class Client
    {
        private Socket mClient;
        private TcpServer mServer;
        public TcpServer server { get { return mServer; } }
        private Message mMsg;
        public int userID = CommonConstData.INVAILD_VALUE;
        private RequestHandleManager mRequestHandleManager;
        public long Token;//用户Token
        public Client(Socket client, TcpServer server)
        {
            mClient = client;
            mServer = server;
            mMsg = new Message();
            mRequestHandleManager = new RequestHandleManager(mServer,this);
            Debug.Log("用户连接:" + client.RemoteEndPoint.ToString());
            Receive();
        }
        private void Receive()
        {
            mClient.BeginReceive(mMsg.data, mMsg.index, mMsg.resize, SocketFlags.None, new AsyncCallback(CallBack), null);
        }
        private void CallBack(IAsyncResult ar)
        {
            int size;
            try
            {
                size = mClient.EndReceive(ar);
                if (size <= 0)
                {
                    //该客户端断开连接
                    Close();
                    return;
                }
            }
            catch (Exception)
            {
                Close();
                return;
            }
            mMsg.Read(size, ResponseData);
            Receive();
        }

        private void ResponseData(short requestCode, short actionCode,byte[] data)
        {
            byte[] returnCode = mRequestHandleManager.Response(requestCode, actionCode, data, this);
            if (returnCode != null)
            {
                Send(actionCode, returnCode);
            }
        }
        /// <summary>
        /// 发送信息到客户端
        /// </summary>
        /// <param name="actionCode"></param>
        /// <param name="data"></param>
        public void Send(short actionCode,byte[] data)
        {
            if (data == null) return;
            int len = 4 + 2 + data.Length;
            mClient.Send(ByteTools.ConcatParam(  len.ToBytes(), actionCode.ToBytes(), data)  , SocketFlags.None);
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public virtual void Close()
        {
            if (mClient == null) return;
            mClient.Shutdown(SocketShutdown.Both);
            mClient.Close();
            mClient = null;
            mMsg = null;
            if (mServer != null)
            {
                mServer.RemoveClient(this);
                mServer = null;
            }
            
        }
    }
}
