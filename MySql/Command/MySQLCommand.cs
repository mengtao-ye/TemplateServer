namespace YSF
{
    public partial class MySQLCommand
    {
        public static IMySqlCommand Select(string content = "*")
        {
            SELECT select = ClassPool<SELECT>.Pop();
            select.SetData(content);
            return select;
        }
        public static IMySqlCommand Update(string talbe)
        {
            UPDATE UPDATE = ClassPool<UPDATE>.Pop();
            UPDATE.SetData(talbe);
            return UPDATE;
        }
        public static IMySqlCommand Delete
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("DELETE");
                return keyword;
            }
        }
    }
}
