using System.Collections.Generic;

namespace YSF
{
    public abstract class BaseMySqlCommand : IMySqlCommand
    {
        public IMySqlCommand Next { get; private set; }
        public IMySqlCommand Preview { get; set; }
        public abstract string mySqlStr { get; set; }
        public bool isPop { get;  set; }
        public virtual void PopPool() 
        {
            isPop = true;
            Next = null;
            Preview = null;
            mySqlStr = null;
        }
        public virtual void PushPool()
        {
            isPop = false;
        }
        public virtual void Recycle()
        {
            if (Preview != null) Preview.Recycle();
        }
        public IMySqlCommand Concat(IMySqlCommand command)
        {
            Next = command;
            command.Preview = this;
            mySqlStr +=" "+ command.mySqlStr;
            command.mySqlStr = mySqlStr;
            return command;
        }
        public IMySqlCommand And
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("AND");
                Concat(keyword);
                return keyword;
            }
        }
        public IMySqlCommand End
        {
            get 
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData(";");
                Concat(keyword);
                return keyword;
            }
        }
       
        public IMySqlCommand Or
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("OR");
                Concat(keyword);
                return keyword;
            }
        }
      
        public IMySqlCommand Values(Dictionary<string,string> keyValues)
        {
            VALUES select = ClassPool<VALUES>.Pop();
            select.SetData(keyValues);
            Concat(select);
            return select;
        }
        public IMySqlCommand Updates(string field, string value)
        {
            UPDATES UPDATES = ClassPool<UPDATES>.Pop();
            UPDATES.SetData(field, value);
            Concat(UPDATES);
            return UPDATES;
        }
        public IMySqlCommand Limit(int count)
        {
            LIMIT select = ClassPool<LIMIT>.Pop();
            select.SetData(count);
            Concat(select);
            return select;
        }
        public IMySqlCommand Compare(string field, CompareType compareType, string value)
        {
            COMPARE COMPARE = ClassPool<COMPARE>.Pop();
            COMPARE.SetData(field, compareType, value);
            Concat(COMPARE);
            return COMPARE;
        }

        public IMySqlCommand Compares(CompareItem[] compareItems)
        {
            if (compareItems.IsNullOrEmpty()) return Preview;
            COMPAREs COMPARE = ClassPool<COMPAREs>.Pop();
            COMPARE.SetData(compareItems);
            Concat(COMPARE);
            return COMPARE;
        }
        public IMySqlCommand Order(string field,  MySQLSort sort)
        {
            ORDER order = ClassPool<ORDER>.Pop();
            order.SetData(field, sort);
            Concat(order);
            return order;
        }
        public IMySqlCommand Order(string field, MySQLCoding coding, MySQLSort sort)
        {
            ORDER order = ClassPool<ORDER>.Pop();
            order.SetData(field, coding, sort);
            Concat(order);
            return order;
        }

        public IMySqlCommand Like(string field, string value)
        {
            LIKE select = ClassPool<LIKE>.Pop();
            select.SetData(field, value);
            Concat(select);
            return select;
        }
        public IMySqlCommand Datebase(string table)
        {
            return Datebase(MySQLData.DataBaseName, table);
        }
        public IMySqlCommand Datebase(string db, string table)
        {
            DATABASE From = ClassPool<DATABASE>.Pop();
            From.SetData(db, table);
            Concat(From);
            return From;
        }

      

        public IMySqlCommand Where
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("WHERE");
                Concat(keyword);
                return keyword;
            }
        }
        public IMySqlCommand From
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("FROM");
                Concat(keyword);
                return keyword;
            }
        }
        public IMySqlCommand Insert
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("INSERT INTO");
                Concat(keyword);
                return keyword;
            }
        }
        public IMySqlCommand Set
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("SET");
                Concat(keyword);
                return keyword;
            }
        }

        public IMySqlCommand LeftParentheses
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("(");
                Concat(keyword);
                return keyword;
            }
        }

        public IMySqlCommand RightParentheses
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData(")");
                Concat(keyword);
                return keyword;
            }
        }
       

        public IMySqlCommand Update
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("UPDATE");
                Concat(keyword);
                return keyword;
            }
        }
    }
}
