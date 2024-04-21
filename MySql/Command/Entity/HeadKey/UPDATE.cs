namespace YSF
{
    public class UPDATE : BaseMySqlCommand
    {
        public override string mySqlStr { get; set; }
        public  void SetData(string content)
        {
            mySqlStr = "UPDATE " + content;
        }
        public override void Recycle()
        {
            base.Recycle();
            ClassPool<UPDATE>.Push(this);
        }
    }
}
