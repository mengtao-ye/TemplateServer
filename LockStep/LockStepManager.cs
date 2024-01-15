using System.Collections.Generic;

namespace YSF
{
    /// <summary>
    /// 帧同步核心管理类
    /// </summary>
    public class LockStepManager
    {
        public UdpServer udpServer { get; private set; }//Udp通行模块对象
        private Dictionary<int, LockStepRoom> mLockStepRoomDict;//帧同步房间对象
        private Dictionary<int, LockStepRoom> .Enumerator mForeachTemp;
        public LockStepManager(UdpServer server)
        {
            udpServer = server;
            Init();
        }
        /// <summary>
        /// 初始化方法
        /// </summary>
        private void Init()
        {
            mLockStepRoomDict = new Dictionary<int, LockStepRoom>();
            AddLockStepRoom(111111,new LockStepRoom());//临时数据
        }

        /// <summary>
        /// 帧更新方法
        /// </summary>
        public void Update()
        {
            if(mLockStepRoomDict.Count != 0)
            {
                mForeachTemp = mLockStepRoomDict.GetEnumerator();
                mForeachTemp.MoveNext();
                for (int i = 0; i < mLockStepRoomDict.Count; mForeachTemp.MoveNext(), i++)
                {
                    mForeachTemp.Current.Value.Update();
                }
            }
        }
        /// <summary>
        /// 添加帧同步房间
        /// </summary>
        /// <param name="roomID"></param>
        /// <param name="room"></param>
        public void AddLockStepRoom(int roomID,LockStepRoom room)
        {
            if (mLockStepRoomDict.ContainsKey(roomID)) return;
            room.SetLockStepManager(this);
            room.SetRoomID(roomID);
            mLockStepRoomDict.Add(roomID,room);
        }

        /// <summary>
        /// 移除帧同步房间
        /// </summary>
        /// <param name="roomID"></param>
        /// <param name="room"></param>
        public void RemoveLockStepRoom(int roomID)
        {
            if (!mLockStepRoomDict.ContainsKey(roomID)) return;
            mLockStepRoomDict.Remove(roomID);
        }

        /// <summary>
        /// 查找帧同步房间
        /// </summary>
        /// <param name="roomID"></param>
        /// <param name="room"></param>
        public LockStepRoom FindLockStepRoom(int roomID)
        {
            if (!mLockStepRoomDict.ContainsKey(roomID)) return null;
            return mLockStepRoomDict[roomID];
        }
    }
}
