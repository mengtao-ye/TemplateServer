namespace YSF
{
    public class UPDATES : BaseMySqlCommand
    {
        public override string mySqlStr { get; set; }
        public  void SetData(string field,string value)
        {
            mySqlStr = field +"="+value;
        }
        public override void Recycle()
        {
            base.Recycle();
            ClassPool<UPDATES>.Push(this);
        }
    }
}
