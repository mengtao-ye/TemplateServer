namespace YSF
{
    public class LIMIT : BaseMySqlCommand
    {
        public override string mySqlStr { get; set; }
        public void SetData(int count) 
        {
            mySqlStr = "LIMIT " + count;
        }
        public override void Recycle()
        {
            base.Recycle();
            ClassPool<LIMIT>.Push(this);
        }
    }
}
