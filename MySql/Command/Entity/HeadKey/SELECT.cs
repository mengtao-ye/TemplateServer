namespace YSF
{
    public class SELECT : BaseMySqlCommand
    {
        public override string mySqlStr { get; set; }
        public  void SetData(string content)
        {
            mySqlStr = "SELECT " + content;
        }
        public override void Recycle()
        {
            base.Recycle();
            ClassPool<SELECT>.Push(this);
        }
    }
}
