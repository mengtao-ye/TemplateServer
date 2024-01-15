namespace YSF
{
    public abstract class DataHelper
    {
        public static DataHelper Instance;
       public abstract string AppName { get; }//英文名称
       public abstract string AppCNName { get; }//中文名称
    }
}
