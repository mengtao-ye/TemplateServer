using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace YSF
{
    /// <summary>
    /// 分布式服务器
    /// </summary>
    public  class SubUdpServer : IUdpServer
    {
        protected Socket mUdpSocket;//Udp对象
        private EndPoint mPoint;
        public EndPoint serverPoint { get { return mPoint; } }//服务器对象
        private SubUdpRequestHandleManager mUdpHandle;
        private EndPoint mUdpReceivePoint;
        private byte[] mBuffer;
        private string mIPAddress = null;
        private int mPort = 0;
        private UdpMsg mTempReceiveUdpMsg;
        private UdpMsg mTempUdpMsg;
        private UdpBigDataManager mBigDataManager;
        private BigDataController mBigDataController;//大数据控制器
        private Thread mReceiveThread;
        public SubUdpServer(IMap<short, IUdpRequestHandle> map)
        {
            mUdpHandle = new SubUdpRequestHandleManager(this, map);
            mBigDataManager = new UdpBigDataManager(mUdpSocket, PlatformType.Server);
            mBigDataController = new BigDataController(this);
        }
        /// <summary>
        /// 打开UDP连接
        /// </summary>
        /// <param name="ipAddress"></param>
        /// <param name="port"></param>
        public void Open(string ipAddress, int port)
        {
            InitUdpSocket(ipAddress, port);
        }

        private void InitUdpSocket(string ipAddress, int port)
        {
            try
            {
                mTempReceiveUdpMsg = new UdpMsg();
                mTempUdpMsg = new UdpMsg();
                mIPAddress = ipAddress;
                mPort = port;
                mBuffer = new byte[CommonConstData.UDP_BUFFER_SIZE];
                ReConnectServer();
            }
            catch
            {
            }
        }
        /// <summary>
        /// 重新连接服务器
        /// </summary>
        public void ReConnectServer()
        {
            try
            {
                if (mUdpSocket != null && mUdpSocket.Connected)
                {
                    mUdpSocket.Shutdown(SocketShutdown.Both);
                    mUdpSocket.Close();
                }
                mUdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                mPoint = new IPEndPoint(IPAddress.Parse(mIPAddress), mPort);
                mUdpSocket.Connect(mPoint);
                mUdpSocket.ReceiveBufferSize = CommonConstData.UDP_BUFFER_SIZE;
                mUdpSocket.SendBufferSize = CommonConstData.UDP_BUFFER_SIZE;
                mUdpReceivePoint = new IPEndPoint(IPAddress.Any, 0);
                if (mReceiveThread != null)
                {
                    mReceiveThread.Abort();
                    mReceiveThread = null;
                }
                mReceiveThread = new Thread(UdpReceive);
                mReceiveThread.Start();
                Debug.Log("UDP服务器启动成功");
            }
            catch
            {
                Debug.Log("UDP服务器启动失败");
            }
        }
        /// <summary>
        /// UDP发送消息
        /// </summary>
        /// <param name="udpCode"></param>
        /// <param name="type">数据类型 0 为小数据，1为大数据</param>
        /// <param name="data"></param>
        private void UdpSend(short udpCode, byte type, byte[] data)
        {
            try
            {
                if (data != null && data.Length > CommonConstData.UDP_SPLIT_LENGTH)
                {
                    Debug.LogError("UdpCode:" + udpCode + "发送的数据过长");
                    return;
                }
                if (mUdpSocket != null && mPoint != null)
                {
                    UdpMsg msg = new UdpMsg();
                    msg.SetUdpMsg(udpCode, type, data);
                    mUdpSocket.SendTo(msg.ToBytes(), mPoint);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// udp发送数据
        /// </summary>
        /// <param name="udpCode"></param>
        /// <param name="data"></param>
        public void UdpSend(short udpCode, byte[] data)
        {
            if (data == null) return;
            if (data.Length < SocketData.UDP_SPLIT_LENGTH) //发送的是小数据
            {
               UdpSend(udpCode, (byte)UdpMsgType.SmallData, data);
            }
            else
            {
                mBigDataManager.SendBigData(udpCode, data, int.MaxValue, mPoint);
            }
        }

        /// <summary>
        /// 发送数据到客户端
        /// </summary>
        /// <param name="point"></param>
        /// <param name="udpCode"></param>
        /// <param name="data"></param>
        public void UdpSend(EndPoint point, short udpCode, byte[] data)
        {
            if (data == null) return;
            if (data.Length < SocketData.UDP_SPLIT_LENGTH) //发送的是小数据
            {
                mTempUdpMsg.SetUdpMsg(udpCode, (byte)UdpMsgType.SmallData, data);
                mUdpSocket.SendTo(mTempUdpMsg.ToBytes(), point);
            }
            else //发送的是大数据
            {
                 SendBigData(udpCode, data,int.MaxValue, point);
            }
        }
        /// <summary>
        /// 发送大数据
        /// </summary>
        /// <param name="udpCode">大数据UdpCode</param>
        /// <param name="sendData">发送的数据</param>
        /// <param name="userID">改数据的UserID</param>
        /// <param name="target">发送的对象</param>
        public void SendBigData(short udpCode, byte[] sendData, int userID, EndPoint target)
        {
            mBigDataManager.SendBigData(udpCode, sendData, userID, target);
        }
        /// <summary>
        /// UDP数据接收
        /// </summary>
        private void UdpReceive()
        {
            while (true)
            {
                try
                {
                    int length = mUdpSocket.ReceiveFrom(mBuffer, ref mUdpReceivePoint);
                    if (length < 3) continue; //如果没有接收到任何数据时 
                    byte[] returnData = null;
                    mTempReceiveUdpMsg.ToValue(ByteTools.SubBytes(mBuffer, 0, length));
                    switch (mTempReceiveUdpMsg.type)
                    {
                        case (byte)UdpMsgType.SmallData:
                            returnData = mUdpHandle.Response(mTempReceiveUdpMsg.udpCode, mTempReceiveUdpMsg.Data, mUdpReceivePoint);
                            UdpSend(mUdpReceivePoint, mTempReceiveUdpMsg.udpCode, returnData);
                            break;
                        case (byte)UdpMsgType.BigData:
                            returnData = mBigDataController.ResponseData(mTempReceiveUdpMsg.udpCode, mTempReceiveUdpMsg.Data, mUdpReceivePoint);
                            UdpSend(mUdpReceivePoint, (short)UdpCode.ServerBigDataResponse, returnData);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }
        public byte[] Response(byte[] data, EndPoint point, short udpCode)
        {
            return mUdpHandle.Response(udpCode,data,point);
        }
        /// <summary>
        /// 收到玩家的信息回调
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="index"></param>
        /// <param name="udpCode"></param>
        public void ReceiveBigDataIndex(int userID, ushort index, short udpCode, bool isReceive)
        {
            if (isReceive)
            {
                mBigDataManager.Remove(userID, udpCode);
            }
            else
            {
                mBigDataManager.ReceiveCallBack(userID, udpCode, index);
            }
        }
    }
}
