
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;


namespace YSF
{
    public class TCPServer
    {
        private Socket mTcpSocket;//Tcp对象
        private List<Client> mClinetDict;//当前所有客户端集合
        public IMap<short, ITCPRequestHandle> map { get; private set; }
        public TCPServer(IMap<short, ITCPRequestHandle> map)
        {
            this.map = map;
            Init();
        }
        private void Init()
        {
            mClinetDict = new List<Client>();
        }
        public void Run(string ipAddress, int port)
        {
            mTcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            mTcpSocket.Bind(new IPEndPoint(IPAddress.Parse(ipAddress), port));
            mTcpSocket.Listen(500);
            mTcpSocket.BeginAccept(new AsyncCallback(CallBack), null);
            Debug.Log("TCP服务器启动成功");
        }
        private void Receive()
        {
            try
            {
                mTcpSocket.BeginAccept(new AsyncCallback(CallBack), null);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
        }
        private void CallBack(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = mTcpSocket.EndAccept(ar);
                Client client = new Client(clientSocket, this);
                mClinetDict.Add( client);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            Receive();
        }
        public void RemoveClient(Client client)
        {
            if (mClinetDict.Contains(client))
            {
                mClinetDict.Remove(client);
            }
        }
    }
}
