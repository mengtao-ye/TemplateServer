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
        public void SetData(string tableName, string key1, string value1, string key2 = null, string value2=null, string key3 = null, string value3=null, string key4 = null, string value4 = null, string key5 = null, string value5 = null)
        {
            IDictionaryData<string, string> dict = ClassPool<DictionaryData<string, string>>.Pop();
            dict.Add(key1, value1);
            if(key2!=null && value2 !=null) dict.Add(key2, value2);
            if(key3 != null && value3 != null) dict.Add(key3, value3);
            if(key4 != null && value4 != null) dict.Add(key4, value4);
            if(key5 != null && value5 != null) dict.Add(key5, value5);
            SetData(tableName, dict);
        }
        public override void Recycle()
        {
            base.Recycle();
            ClassPool<MySQL>.Push(this);
        }
    }
}
