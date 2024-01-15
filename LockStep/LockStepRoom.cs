
using System;
using System.Collections.Generic;

namespace YSF
{
    /// <summary>
    /// 帧同步房间
    /// </summary>
    public class LockStepRoom : IPool
    {
        private LockStepManager mLockStepManager;//帧同步管理类
        private IDictionaryData<int, IListData<byte[]>> mPlayerLockStepDataDict;//玩家帧同步数据，Key为用户ID，value为用户帧数据
        private IListData<LockStepData> mLockStepDataList;
        private float mTimer = 0;
        public int roomID { get; private set; }//房间ID
        public bool isPop { get;  set; }
        private List<int> mRoomPlayer;//房间里玩家的ID信息
        private int mRoomMaxCount = 4;//房间最大人数
        private byte[] mTempLockStepData;
        public LockStepRoom()
        {
            Init();
        }

     
        /// <summary>
        /// 初始化方法
        /// </summary>
        private void Init()
        {
            mPlayerLockStepDataDict =ClassPool<DictionaryPoolData<int, IListData<byte[]>>>.Pop();
            mLockStepDataList = ClassPool<ListPoolData<LockStepData>>.Pop();
            mRoomPlayer = new List<int>();
            AddPlayer(0);//临时添加测试数据
        }

        /// <summary>
        /// 设置帧同步管理器
        /// </summary>
        /// <param name="manager"></param>
        public void SetLockStepManager(LockStepManager manager)
        {
            mLockStepManager = manager;
        }
        /// <summary>
        /// 添加帧同步数据
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <param name="data">用户的数据</param>
        public void AddLockStepData(int userID, byte[] data)
        {
            if (!mPlayerLockStepDataDict.ContainsKey(userID))
            {
                mPlayerLockStepDataDict.Add(userID, ClassPool<ListData<byte[]>>.Pop());
            }
            mPlayerLockStepDataDict[userID].Add(data);
        }
        /// <summary>
        /// 帧函数
        /// </summary>
        public void Update()
        {
            mTimer += Time.DeltaTime;
            if (mTimer > CommonLockStepData.SERVER_REFRESH_TIME)
            {
                mTimer = 0;
                CollectionFrameData();
                SendFrameDataToClient();
            }
        }
        /// <summary>
        /// 收集这一帧的数据
        /// </summary>
        private void CollectionFrameData()
        {
            //当前帧加一
            LockStepMsg.GameFrame++;
            //记录这一帧的数据
            LockStepData data = ClassPool<LockStepData>.Pop() ;
            data.frameIndex = LockStepMsg.GameFrame;
            data.frameData = ClassPool<ListPoolData<LockStepUserData>>.Pop();
            foreach (var item in mPlayerLockStepDataDict.data)
            {
                LockStepUserData lockStepUserData = ClassPool<LockStepUserData>.Pop();
                lockStepUserData.SetData(item.Key, item.Value);
                data.frameData.Add(lockStepUserData);
            }
            //清空缓存数据
            mPlayerLockStepDataDict.Clear();
            //记录这一帧的数据
            mLockStepDataList.Add(data);
        }
        /// <summary>
        /// 发送最后一帧数据到客户端
        /// </summary>
        private void SendFrameDataToClient()
        {
            mTempLockStepData =  mLockStepDataList[mLockStepDataList.Count - 1].ToBytes();
            for (int i = 0; i < mRoomPlayer.Count; i++)
            {
                mLockStepManager.udpServer.SendDataToEndPoint(mRoomPlayer[i],(short)UdpCode.LockStep_ServerData, mTempLockStepData);
            }
        }

        /// <summary>
        /// 设置房间ID
        /// </summary>
        /// <param name="roomID"></param>
        public void SetRoomID(int roomID)
        {
            this.roomID = roomID;
        }
        /// <summary>
        /// 添加玩家信息
        /// </summary>
        /// <param name="userID"></param>
        public void AddPlayer(int userID) 
        {
            if (mRoomPlayer.Contains(userID)) return;
            if (mRoomPlayer.Count >= mRoomMaxCount) return;
            mRoomPlayer.Add(userID);
        }

        public  void Recycle()
        {
            ClassPool<LockStepRoom>.Push(this);
        }
        public void PopPool()
        {
        }
        public void PushPool()
        {
        }
    }
}
