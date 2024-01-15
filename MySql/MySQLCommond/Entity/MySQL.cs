namespace YSF
{
    public class MySQL : BaseMySQL
    {
        public void SetData(string tableName, IDictionaryData<string,string> parameters)
        {
            this.tableName = tableName;
            if (!base.parameters .IsNull()) 
            {
                //覆盖之前的数据
                base.parameters.Recycle();
            }
            base.parameters = parameters;
        }

        public void SetData(string tableName, string key, string value)
        {
            IDictionaryData<string, string> dict = ClassPool<DictionaryData<string, string>>.Pop();
            dict.Add(key,value);
            SetData(tableName, dict);
        }
        public override void Recycle()
        {
            base.Recycle();
            ClassPool<MySQL>.Push(this);
        }
    }
}
