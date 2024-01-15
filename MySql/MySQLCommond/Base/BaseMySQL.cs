using System.Collections.Generic;

namespace YSF
{
    public abstract class BaseMySQL : IMySQL
    {
        public IDictionaryData<string, string> parameters { get; protected set; }
        public string tableName { get; protected set; }
        public bool isPop { get;  set; }
        public virtual void PopPool()
        {
            isPop = true;
        }
        public virtual void PushPool()
        {
            isPop = false;
        }
        public virtual void Recycle()
        {
            if (parameters != null)
            {
                parameters.Recycle();
                parameters = null;
            }
        }
    }
}
