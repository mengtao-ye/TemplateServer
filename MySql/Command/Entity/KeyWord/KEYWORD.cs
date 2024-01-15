namespace YSF
{
    public class KEYWORD  : BaseMySqlCommand
    {
        public override string mySqlStr { get; set; }
        public  void SetData(string keyword)
        {
            mySqlStr = keyword;
        }
        public override void Recycle()
        {
            base.Recycle();
            ClassPool<KEYWORD>.Push(this);
        }
    }
}
