namespace YSF
{
    public class DATABASE : BaseMySqlCommand
    {
        public override string mySqlStr { get; set; }
        public  void SetData(  string db,string table)
        {
            mySqlStr = db + "." + table;
        }
        public override void Recycle()
        {
            base.Recycle();
            ClassPool<DATABASE>.Push(this);
        }
    }
}
