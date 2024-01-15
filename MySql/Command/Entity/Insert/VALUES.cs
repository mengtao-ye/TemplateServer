using System;
using System.Collections.Generic;

namespace YSF
{
    public class VALUES : BaseMySqlCommand
    {
        public override string mySqlStr { get; set; }
        public  void SetData(Dictionary<string,string> values)
        {
            if (values.IsNullOrEmpty()) throw new Exception("values cant null!");
            string key = "";
            string value = "";
            int count = 0;
            foreach (var item in values)
            {
                count++;

                if (count == values.Count)
                {
                    key += item.Key;
                    value += "'" + item.Value + "'";
                }
                else
                {
                    key += item.Key + ",";
                    value = "'" + item.Value + "',";
                }
            }
           
            mySqlStr = "( "+key+" ) VALUES ("+value+")";
        }
        public override void Recycle()
        {
            base.Recycle();
            ClassPool<VALUES>.Push(this);
        }
    }
}
