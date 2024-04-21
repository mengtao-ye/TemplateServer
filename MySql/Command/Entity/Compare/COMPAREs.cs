namespace YSF
{
    public class COMPAREs : BaseMySqlCommand
    {
        public override string mySqlStr { get; set; }
        public void SetData(CompareItem[] compareItems)
        {
            for (int i = 0; i < compareItems.Length; i++)
            {
                string compare = "";
                switch (compareItems[i].compareType)
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
                mySqlStr += compareItems[i].field + compare + compareItems[i].value;
                switch (compareItems[i].operatorType)
                {
                    case MySQLOperatorType.And:
                        mySqlStr += " AND ";
                        break;
                    case MySQLOperatorType.Or:
                        mySqlStr += " OR ";
                        break;
                }
            }
        }
        public override void Recycle()
        {
            base.Recycle();
            ClassPool<COMPAREs>.Push(this);
        }
    }
}
