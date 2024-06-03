namespace YSF
{

    public enum CompareType 
    { 
        Equal,
        NotEqual,
        Big,
        Small
    }
    public class COMPARE : BaseMySqlCommand
    {
        public override string mySqlStr { get; set; }
        public void SetData(string fieldName,CompareType compareType ,string value)
        {
            string compare = string.Empty;
            switch (compareType)
            {
                case CompareType.Equal:
                    compare = "=";
                    break;
                case CompareType.NotEqual:
                    compare = "!=";
                    break;
                case CompareType.Big:
                    compare = ">";
                    break;
                case CompareType.Small:
                    compare = "<";
                    break;
            }
            mySqlStr = fieldName + compare + value;
        }
        public override void Recycle()
        {
            base.Recycle();
            ClassPool<COMPARE>.Push(this);
        }
    }
}
