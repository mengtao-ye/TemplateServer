namespace YSF
{
    public class WhereMySQL : BaseMySQL
    {
        public IDictionaryData<string, string> where { get; private set; }
        public void SetData(string tableName, IDictionaryData<string, string> parameters, IDictionaryData<string, string> where)
        {
            this.tableName = tableName;
            base.parameters = parameters;
            this.where = where;
        }

        public void SetData(string tableName, string key, string value, string whereKey, string whereValue)
        {
            IDictionaryData<string, string> param = ClassPool<DictionaryData<string, string>>.Pop();
            param.Add(key, value);
            IDictionaryData<string, string> where = ClassPool<DictionaryData<string, string>>.Pop();
            where.Add(whereKey, whereValue);
            SetData(tableName, param, where);
        }
        public override void Recycle()
        {
            base.Recycle();
            if (where != null)
            {
                where.Recycle();
                where = null;
            }
            ClassPool<WhereMySQL>.Push(this);
        }
    }
}
