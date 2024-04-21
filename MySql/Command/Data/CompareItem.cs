namespace YSF
{
    public class CompareItem : IPool
    {
        public string field;
        public CompareType compareType;
        public MySQLOperatorType operatorType;
        public string value;
        public bool isPop { get; set; }
        public void PopPool()
        {
            
        }

        public void PushPool()
        {
            
        }

        public void Recycle()
        {
            ClassPool<CompareItem>.Push(this);
        }
    }
}
