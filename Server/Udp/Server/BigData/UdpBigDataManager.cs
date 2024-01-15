using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace YSF
{
    public class UdpBigDataManager
    {
        private Dictionary<short, ushort> mMsgIDDict;
        private Socket mSocket;//Socket对象
        private IDictionaryData<int, IListData<UdpBigData>> mSendDict;//Key为用户ID，value为用户对应的数据
        public PlatformType platform;
        private int mMsgID;
        private int MAX = int.MaxValue - 1;
        public int msgID { get { return (mMsgID ++) % MAX; } }//防止越界
        public UdpBigDataManager(Socket socket, PlatformType type)
        {
            if (socket == null) return;
            platform = type;
            mSocket = socket;
            mMsgIDDict = new Dictionary<short, ushort>();
            mSendDict = ClassPool<DictionaryPoolData<int, IListData<UdpBigData>>>.Pop();
        }
        /// <summary>
        /// 重新设置Socket
        /// </summary>
        /// <param name="socket"></param>
        public void ResetSocket(Socket socket)
        {
            if (socket == null) return;
            mSocket = socket;
        }
        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="udpCode"></param>
        /// <param name="sendData"></param>
        /// <param name="sendTarget"></param>
        public void SendBigData(short udpCode, byte[] sendData, int userID, EndPoint sendTarget)
        {
            try
            {
                UdpBigData bigData = Split(sendData, SocketData.UDP_SPLIT_LENGTH, udpCode, userID, msgID);
                bigData.SendData(mSocket, sendTarget);
                if (!mSendDict.ContainsKey(userID))
                {
                    mSendDict.Add(userID, ClassPool<ListPoolData<UdpBigData>>.Pop());
                }
                mSendDict[userID].Add(bigData);
            }
            catch 
            {
            }
        }
        /// <summary>
        /// 实时刷新发送数据
        /// </summary>
        public void RefreshSendData()
        {
            try
            {
                int[] keys = mSendDict.data.Keys.ToArray();
                for (int i = 0; i < keys.Length; i++)
                {
                    IListData<UdpBigData>  bigData = null;
                    mSendDict.data.TryGetValue(keys[i], out bigData);
                    if (bigData != null)
                    {
                        for (int j = 0; j < bigData.Count; j++)
                        {
                            if (bigData.Count > i)
                            {
                                bigData[i].CheckDataIsFinish();
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }
        /// <summary>
        ///  接收到数据回调时的反应
        /// </summary>
        /// <param name="udpCode"></param>
        /// <param name="userID">收到了谁的回调消息</param>
        /// <param name="index"></param>
        /// <param name="msgCode"></param>
        public int ReceiveCallBack(int userID, short udpCode, ushort index)
        {
            UdpBigData bigData = FindBigData(userID, udpCode);
            if (bigData == null) return -1;
            return bigData.Receive(index);
        }
        /// <summary>
        /// 查找大数据块
        /// </summary>
        /// <param name="udpCode"></param>
        /// <param name="msgCode"></param>
        /// <returns></returns>
        private UdpBigData FindBigData(int userID, short udpCode)
        {
            if (!mSendDict.ContainsKey(userID)) return null;
            IListData<UdpBigData> mSendList = mSendDict[userID];
            for (int i = 0; i < mSendList.Count; i++)
            {
                if (mSendList[i].udpCode == udpCode)
                {
                    return mSendList[i];
                }
            }
            return null;
        }
        /// <summary>
        /// 切割数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="splitLen"></param>
        /// <param name="udpCode"></param>
        /// <param name="msgID"></param>
        /// <returns></returns>
        private UdpBigData Split(byte[] data, int splitLen, short udpCode, int userID, int msgID)
        {
            UdpBigData bigData = ClassPool<UdpBigData>.Pop();
            bigData.SetData(userID, udpCode, this);
            if (data == null || data.Length < splitLen)
            {
                UdpBigDataItem udpBigDataItem = ClassPool<UdpBigDataItem>.Pop();
                udpBigDataItem.SetData(0, 0, userID, data, msgID);
                bigData.Add(udpBigDataItem);
                return bigData;
            }
            int chunks = data.Length / splitLen;
            int remain = data.Length % splitLen;
            int lastIndex = remain == 0 ? chunks - 1 : chunks;
            for (int i = 0; i < chunks; i++)
            {
                byte[] tempData = ByteTools.SubBytes(data, i * splitLen, splitLen);
                UdpBigDataItem udpBigDataItem = ClassPool<UdpBigDataItem>.Pop();
                udpBigDataItem.SetData((ushort)i, (ushort)lastIndex, userID, tempData, msgID);
                bigData.Add(udpBigDataItem);
            }
            if (remain != 0)
            {
             
                byte[] tempData = ByteTools.SubBytes(data, chunks * splitLen, remain);
                UdpBigDataItem udpBigDataItem = ClassPool<UdpBigDataItem>.Pop();
                udpBigDataItem.SetData((ushort)chunks, (ushort)lastIndex, userID, tempData, msgID);
                bigData.Add(udpBigDataItem);
            }
            return bigData;
        }
        /// <summary>
        /// 移除已经完成的消息
        /// </summary>
        /// <param name="bigData"></param>
        public void Remove(int userID, short udpCode)
        {
            try
            {
                if (!mSendDict.ContainsKey(userID)) return;
                mSendDict[userID].list.CustomRemoveAllPool(item => item.udpCode == udpCode);
            }
            catch
            {

            }
        }
        /// <summary>
        /// 清空玩家的大数据片段
        /// </summary>
        /// <param name="userID"></param>
        public void ClearPlayerData(int userID)
        {
            if (mSendDict.ContainsKey(userID))
            {
                mSendDict[userID].Recycle();
                mSendDict.Remove(userID);
            }
        }
        /// <summary>
        /// 更新目标对象
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="target"></param>
        public void UpdateTargetPoint(int userID,EndPoint target)
        {
            try
            {
                if (mSendDict.ContainsKey(userID))
                {
                    IListData<UdpBigData> data = mSendDict[userID];
                    for (int i = 0; i < data.Count; i++)
                    {
                        data[i].SetTarget(target);
                    }
                }
            }
            catch 
            {
            }
        }
     
    }
}
