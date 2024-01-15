namespace YSF
{
    public class ORDER : BaseMySqlCommand
    {
        public override string mySqlStr { get; set; }
        public void SetData(string field, MySQLCoding coding,MySQLSort sort) 
        {
            mySqlStr = "ORDER BY CONVERT("+ field + " USING "+coding.ToString()+") "+sort.ToString();
        }
        public override void Recycle()
        {
            base.Recycle();
            ClassPool<ORDER>.Push(this);
        }
    }
}
