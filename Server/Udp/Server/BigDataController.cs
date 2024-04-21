using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace YSF
{
    public class BigDataController
    {
        private IDictionaryData<int, IDictionaryData<short, IListData<UdpBigDataItem>>> mBigData; //Key为Userid，Value为每个人对应的数据  key为EventID，value为该用户这个Udp的字段数据
        private Dictionary<int, int> mReceiveMsgDict; //当前已经收到的玩家的信息ID
        private IUdpServer mUdpServer;
        private byte[] mTempReturnData = null;
        public BigDataController(IUdpServer server)
        {
            mUdpServer= server;
            Init();
        }
        private void Init()
        {
            BoardCastModule.AddListener<int>((int)BoardCastID.PlayerLostLine, PlayerLostLine);
            //定义接收玩家大数据片段的UdpCode
            mBigData =ClassPool<DictionaryData<int, IDictionaryData<short, IListData<UdpBigDataItem>>>>.Pop();
            mReceiveMsgDict = new Dictionary<int, int>();
        }

        /// <summary>
        /// 接收到客户端发送过来的大数据信息
        /// </summary>
        /// <param name="udpCode"></param>
        /// <param name="data"></param>
        /// <param name="point"></param>
        public byte[] ResponseData(short udpCode,byte[] data, EndPoint point)
        {
            return AnalysiseReceiveData(udpCode, ReceiveBigDataCallBack, data, point);
        }
        /// <summary>
        /// 接收到大数据的回调
        /// </summary>
        /// <param name="data"></param>
        /// <param name="userID"></param>
        /// <param name="point"></param>
        /// <param name="udpCode"></param>
        private void ReceiveBigDataCallBack(byte[] data, int userID, EndPoint point,short udpCode)
        {
            mTempReturnData = mUdpServer.Response(data, point, udpCode);
            if (mTempReturnData != null) 
            {
                mUdpServer.UdpSend(point, udpCode,data );
            }
        }

        /// <summary>
        /// 解析收到的大数据
        /// </summary>
        /// <param name="udpCode">当前UDP Code</param>
        /// <param name="callBack">接收到数据后的回调</param>
        /// <param name="data">接收到的数据</param>
        /// <param name="point">玩家Point</param>
        /// <returns></returns>
        private byte[] AnalysiseReceiveData(short udpCode, Action<byte[], int, EndPoint,short> callBack, byte[] data, EndPoint point)
        {
            UdpBigDataItem bigData = ConverterDataTools.ToObjectPool<UdpBigDataItem>(data);
            if (bigData == null) return null;
            if (!mBigData.ContainsKey(bigData.userID)) mBigData.Add(bigData.userID,ClassPool<DictionaryPoolData<short, IListData<UdpBigDataItem>>>.Pop());
            IDictionaryData<short, IListData<UdpBigDataItem>>  bigDataDict = mBigData[bigData.userID];
            if (!bigDataDict.ContainsKey(udpCode))
            {
                bigDataDict.Add(udpCode,ClassPool<ListPoolData<UdpBigDataItem>>.Pop());
            }
            IListData<UdpBigDataItem> bigDataList = bigDataDict[udpCode];
            //请求超时，数据过期
            if (bigDataList.Count > 0 && bigDataList[0].msgID < bigData.msgID)
            {
                bigDataList.Clear();//清空之前的旧数据
            }
            bool isContainsData = false;
            for (int i = 0; i < bigDataList.Count; i++)
            {
                if (bigDataList[i].index == bigData.index)
                {
                    isContainsData = true;
                    break;
                }
            }
            bool isReceive = false;
            if (!isContainsData)
            {
                bigDataList.Add(bigData);
                isReceive = AnalysisBigData(bigDataList, point, callBack, bigData.userID,udpCode);
                if (isReceive) bigDataList.Clear();
            }
            return ByteTools.ConcatParam(BitConverter.GetBytes(bigData.index), BitConverter.GetBytes(isReceive),BitConverter.GetBytes(udpCode));
        }
        /// <summary>
        /// 解析大数据
        /// </summary>
        /// <param name="bigDatas"></param>
        /// <param name="point"></param>
        /// <param name="callBack"></param>
        /// <param name="sendUdpCode"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        private bool AnalysisBigData(IListData<UdpBigDataItem> bigDatas, EndPoint point, Action<byte[], int, EndPoint,short> callBack, int userID,short udpCode)
        {
            if (bigDatas.IsNullOrEmpty()) return false;
            if (bigDatas.Count == bigDatas[0].lastIndex + 1)//这里的加1是因为下标是从0开始的
            {
                if (!mReceiveMsgDict.ContainsKey(bigDatas[0].userID))
                {
                    mReceiveMsgDict.Add(bigDatas[0].userID, -1);
                }
                if (mReceiveMsgDict[bigDatas[0].userID] == bigDatas[0].msgID)
                {
                    return true;
                }
                mReceiveMsgDict[bigDatas[0].userID] = bigDatas[0].msgID;
                //接收到了所有的数据
                //开始解析数据
                int dataLength = 0;
                for (int i = 0; i < bigDatas.Count; i++)
                {
                    dataLength += bigDatas[i].Data == null ? 0 : bigDatas[i].Data.Length;
                }
                bigDatas.list = bigDatas.list.OrderBy(item => item.index).ToList();
                byte[] data = new byte[dataLength];
                int index = 0;
                for (int i = 0; i < bigDatas.Count; i++)
                {
                    if (bigDatas[i].Data != null)
                    {
                        for (int j = 0; j < bigDatas[i].Data.Length; j++)
                        {
                            data[index++] = bigDatas[i].Data[j];
                        }
                    }
                }
                if (callBack != null) callBack(data,userID, point,udpCode);
                return true;
            }
            return false;
        }
        /// <summary>
        /// 玩家掉线
        /// </summary>
        /// <param name="userID"></param>
        private void PlayerLostLine(int userID)
        {
            if (mBigData.ContainsKey(userID))
            {
                mBigData[userID].Recycle();
                mBigData.Remove(userID);
            }
            if (mReceiveMsgDict.ContainsKey(userID))
            {
                mReceiveMsgDict.Remove(userID);
            }
        }
    }
}
