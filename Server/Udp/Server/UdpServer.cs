using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace YSF
{
    public class UdpServer : IUdpServer
    {
        //Main
        private Socket mUdpSocket;//Udp对象
        public UdpRequestHandleManager udpHandle { get; private set; }
        private EndPoint mUdpReceivePoint;
        private byte[] mBuffer;
        private Dictionary<int, UdpOnLineData> mCurOnLinePlayer;//当前在线的玩家
        //Temp
        private List<int> mLostPlayerUserIDList;
        //大数据管理器
        public UdpBigDataManager bigDataManager { get; private set; }
        public BigDataController bigDataController { get; private set; }//大数据控制器
        private UdpMsg mTempUdpMsg;
        private UdpMsg mTempReceiveUdpMsg;
        public IMap<short, IUdpRequestHandle> map { get; private set; }
        public UdpServer()
        {

        }   
        public void Start(string ipAddress, int port,IMap<short, IUdpRequestHandle> map)
        {
            this.map = map;
            udpHandle = new UdpRequestHandleManager(this);
            InitUdpSocket(ipAddress, port);
        }

        private void InitUdpSocket(string ipAddress, int port)
        {
            mTempUdpMsg = new UdpMsg();
            mTempReceiveUdpMsg = new UdpMsg();
            mLostPlayerUserIDList = new List<int>();
            mCurOnLinePlayer = new Dictionary<int, UdpOnLineData>(500);
            mBuffer = new byte[SocketData.UDP_MAX_SIZE];
            mUdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            mUdpSocket.ReceiveBufferSize = SocketData.UDP_MAX_SIZE;
            mUdpSocket.SendBufferSize = SocketData.UDP_MAX_SIZE;
            mUdpSocket.Bind(new IPEndPoint(IPAddress.Parse(ipAddress), port));

            #region 设置网络断线重连的问题
            uint IOC_IN = 0x80000000;
            uint IOC_VENDOR = 0x18000000;
            uint SIO_UDP_CONNRESET = IOC_IN | IOC_VENDOR | 12;
            mUdpSocket.IOControl((int)SIO_UDP_CONNRESET, new byte[] { Convert.ToByte(false) }, null);
            #endregion
            
            mUdpReceivePoint = new IPEndPoint(IPAddress.Any, 0);
            bigDataManager = new UdpBigDataManager(mUdpSocket, PlatformType.Server);
            bigDataController = new BigDataController(this);

            new Thread(UdpReceive).Start();
            new Thread(IEClearLostPlayer).Start();
            new Thread(BigDataThread).Start();
            Debug.Log("UDP服务器启动成功");
        }

        private void BigDataThread()
        {
            while (true)
            {
                Thread.Sleep((int)(SocketData.BIG_DATA_REFRESH_TIME * 1000));
                bigDataManager.RefreshSendData();
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
            bigDataManager.SendBigData(udpCode, sendData, userID, target);
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
                bigDataManager.Remove(userID, udpCode);
            }
            else
            {
                bigDataManager.ReceiveCallBack(userID, udpCode, index);
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
                int userID = GetUserID(point);
                if (userID != CommonConstData.INVAILD_VALUE)
                {
                    SendBigData(udpCode, data, userID, point);
                }
            }
        }
        public void UdpSend(EndPoint point, short udpCode, string data)
        {
            UdpSend(point, udpCode, data.ToBytes());
        }
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
                            returnData = udpHandle.Response(mTempReceiveUdpMsg.udpCode, mTempReceiveUdpMsg.Data, mUdpReceivePoint);
                            UdpSend(mUdpReceivePoint, mTempReceiveUdpMsg.udpCode, returnData);
                            break;
                        case (byte)UdpMsgType.BigData:
                            returnData = bigDataController.ResponseData(mTempReceiveUdpMsg.udpCode, mTempReceiveUdpMsg.Data, mUdpReceivePoint);
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
        /// <summary>
        /// 处理在线请求
        /// </summary>
        /// <param name="userID"></param>
        public void OnLineEvent(int userID, EndPoint point)
        {
            if (!mCurOnLinePlayer.ContainsKey(userID))
            {
                mCurOnLinePlayer.Add(userID, ClassPool<UdpOnLineData>.Pop());
            }
            mCurOnLinePlayer[userID].LastTime = DateTime.Now;
            mCurOnLinePlayer[userID].Point = point;
            bigDataManager.UpdateTargetPoint(userID, point);
        }
        /// <summary>
        ///判断玩家是否在线
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public bool IsOnLine(int userID)
        {
            return mCurOnLinePlayer.ContainsKey(userID);
        }
        /// <summary>
        /// 定时检查掉线的玩家
        /// </summary>
        private void IEClearLostPlayer()
        {
            while (true)
            {
                Thread.Sleep(1000);//一秒检测一下
                try
                {
                    mLostPlayerUserIDList.Clear();
                    foreach (var item in mCurOnLinePlayer)
                    {
                        if ((DateTime.Now - item.Value.LastTime).TotalSeconds > GameConstData.CHECK_LOST_PLAYER_TIME)//超过3秒没同步数据的话说明掉线了
                        {
                            mLostPlayerUserIDList.Add(item.Key);
                        }
                    }
                    for (int i = 0; i < mLostPlayerUserIDList.Count; i++)
                    {
                        RemoveLostPlayer(mLostPlayerUserIDList[i]);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message);
                }
            }
        }
        /// <summary>
        /// 移除掉线的玩家
        /// </summary>
        /// <param name="userID"></param>
        public void RemoveLostPlayer(int userID)
        {
            if (mCurOnLinePlayer.ContainsKey(userID))
            {
                mCurOnLinePlayer[userID].Recycle();
                mCurOnLinePlayer.Remove(userID);
                bigDataManager.ClearPlayerData(userID);
                BoardCastModule.Broadcast<int>((int)BoardCastID.PlayerLostLine, userID);
            }
        }
        /// <summary>
        /// 发送数据到客户端
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="udpCode"></param>
        /// <param name="data"></param>
        public void SendDataToEndPoint(int userID, short udpCode, byte[] data)
        {
            if (!mCurOnLinePlayer.ContainsKey(userID)) return;
            UdpSend(mCurOnLinePlayer[userID].Point, udpCode, data);
        }
        /// <summary>
        /// 发送大数据到客户端那里
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="udpCode"></param>
        /// <param name="data"></param>
        /// <param name="point"></param>
        public void SendBigDataToEndPoint(int userID, short udpCode, byte[] data)
        {
            if (!mCurOnLinePlayer.ContainsKey(userID)) return;
            SendBigData(udpCode, data, userID, mCurOnLinePlayer[userID].Point);
        }
        /// <summary>
        /// 根据UserID查找玩家的EndPoint
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public EndPoint FindPlayerEndPoint(int userID)
        {
            if (!mCurOnLinePlayer.ContainsKey(userID)) return null;
            return mCurOnLinePlayer[userID].Point;
        }
        /// <summary>
        /// 当前在线人数
        /// </summary>
        public int OnLineCount
        {
            get
            {
                return mCurOnLinePlayer.Count;
            }
        }
        /// <summary>
        /// 根据EndPoint查找用户ID
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public int GetUserID(EndPoint point)
        {
            if (point == null) return CommonConstData.INVAILD_VALUE;
            foreach (var item in mCurOnLinePlayer)
            {
                if (item.Value.Point.ToString() == point.ToString())
                {
                    return item.Key;
                }
            }
            return CommonConstData.INVAILD_VALUE;
        }

        public byte[] Response(byte[] data, EndPoint point, short udpCode)
        {
            return udpHandle.Response(udpCode,data,point);
        }
    }
}