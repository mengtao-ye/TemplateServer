namespace YSF
{
    public class LIKE : BaseMySqlCommand
    {
        public override string mySqlStr { get ; set ; }
        public void SetData(string fieldName,  string value)
        {
            mySqlStr = fieldName + " LIKE '%" + value+"%' ";
        }
        public override void Recycle()
        {
            base.Recycle();
            ClassPool<LIKE>.Push(this);
        }
    }
}
