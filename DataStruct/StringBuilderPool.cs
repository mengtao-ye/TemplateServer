using System.Text;

namespace YSF
{
    /// <summary>
    /// StringBuilder对象池类，可以回收重复利用
    /// </summary>
    public class StringBuilderPool : IPool
    {
        private StringBuilder mStringBuilder;
        public int MaxCapacity { get { return mStringBuilder.MaxCapacity; } }
        public int Length { get { return mStringBuilder.Length; } set { mStringBuilder.Length = value; } }
        public int Capacity { get { return mStringBuilder.Capacity; } set { mStringBuilder.Capacity = value;  } }
        public bool isPop { get;  set; }
        public StringBuilderPool()
        {
            mStringBuilder = new StringBuilder();
        }

        public StringBuilder Append(string value) {
            return mStringBuilder.Append(value);
        }

        public StringBuilder Clear() {
            return mStringBuilder.Clear();
        }

        public override string ToString()
        {
            return mStringBuilder.ToString();
        }

        public void PopPool()
        {

        }
        public void PushPool()
        {
            mStringBuilder.Clear();
        }
        public void Recycle()
        {
            ClassPool<StringBuilderPool>.Push(this);
        }
    }
}
