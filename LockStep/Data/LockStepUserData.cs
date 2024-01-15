using System;

namespace YSF
{
    /// <summary>
    /// 玩家帧同步数据
    /// </summary>
    public class LockStepUserData : IPool,IDataConverter
    {
        public int userID;//玩家ID
        public IListData<byte[]> data;//玩家帧数据
        public bool isPop { get; set; }
        public LockStepUserData()
        {

        }
        public void SetData(int userID, IListData<byte[]> data)
        {
            this.userID = userID;
            this.data = data;
        }

        public void Recycle()
        {
            ClassPool<LockStepUserData>.Push(this);
        }

        public byte[] ToBytes()
        {
            if (data == null || data.Count == 0)
            {
                return BitConverter.GetBytes(userID);
            }
            return ByteTools.Concat(BitConverter.GetBytes(userID), ListTools.GetBytes(data.list));
        }

        public void ToValue(byte[] data)
        {
            userID = BitConverter.ToInt32(data, 0);
            if (data.Length == 4) return;
            this.data = ListTools.ToList(data, 4);
        }

        public void PopPool()
        {
           
        }

        public void PushPool()
        {
           
        }
    }
}
