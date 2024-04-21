using System;

namespace YSF
{
    /// <summary>
    /// 帧同步数据
    /// </summary>
    public class LockStepData : IPool, IDataConverter
    {
        public int frameIndex;//帧下标
        public IListData<LockStepUserData> frameData;//帧数据
        public bool isPop { get; set; }
        public LockStepData()
        {
        }
        public byte[] ToBytes()
        {
            if (frameData == null || frameData.Count == 0)
            {
                return BitConverter.GetBytes(frameIndex);
            }
            return ByteTools.Concat(BitConverter.GetBytes(frameIndex), frameData.list.ToBytes());
        }
        public void ToValue(byte[] data)
        {
            frameIndex = BitConverter.ToInt32(data, 0);
            if (data.Length == 4) return;
            this.frameData = ConverterDataTools.ToListObjectPool<LockStepUserData>(data, 4);
        }
        public void Recycle()
        {
            if (!frameData.IsNullOrEmpty())
            {
                frameData?.Recycle();
            }
            ClassPool<LockStepData>.Push(this);
        }
        public void PopPool()
        {
        }
        public void PushPool()
        {
        }
    }
}
