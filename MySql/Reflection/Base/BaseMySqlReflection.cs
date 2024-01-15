using MySql.Data.MySqlClient;

namespace YSF
{
    public abstract class BaseMySqlReflection : IMySqlReflection
    {
        public bool isPop { get;  set; }
        public virtual void PopPool()
        {
            isPop = true;    
        }
        public virtual void PushPool() {
            isPop = false;
        }
        public abstract void Recycle();
        public abstract void ReflectionMySQLData(MySqlDataReader reader);
        public abstract byte[] ToBytes();
        public abstract void ToValue(byte[] data);
    }
}
