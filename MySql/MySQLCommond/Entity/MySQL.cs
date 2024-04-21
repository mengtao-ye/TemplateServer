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
        public void SetData(string tableName, string key1, string value1, string key2, string value2)
        {
            IDictionaryData<string, string> dict = ClassPool<DictionaryData<string, string>>.Pop();
            dict.Add(key1, value1);
            dict.Add(key2, value2);
            SetData(tableName, dict);
        }
        public void SetData(string tableName, string key1, string value1, string key2, string value2, string key3, string value3)
        {
            IDictionaryData<string, string> dict = ClassPool<DictionaryData<string, string>>.Pop();
            dict.Add(key1, value1);
            dict.Add(key2, value2);
            dict.Add(key3, value3);
            SetData(tableName, dict);
        }
        public void SetData(string tableName, string key1, string value1, string key2, string value2, string key3, string value3, string key4, string value4)
        {
            IDictionaryData<string, string> dict = ClassPool<DictionaryData<string, string>>.Pop();
            dict.Add(key1, value1);
            dict.Add(key2, value2);
            dict.Add(key3, value3);
            dict.Add(key4, value4);
            SetData(tableName, dict);
        }
        public void SetData(string tableName, string key1, string value1, string key2, string value2, string key3, string value3, string key4, string value4, string key5, string value5)
        {
            IDictionaryData<string, string> dict = ClassPool<DictionaryData<string, string>>.Pop();
            dict.Add(key1, value1);
            dict.Add(key2, value2);
            dict.Add(key3, value3);
            dict.Add(key4, value4);
            dict.Add(key5, value5);
            SetData(tableName, dict);
        }
        public override void Recycle()
        {
            base.Recycle();
            ClassPool<MySQL>.Push(this);
        }
    }
}
