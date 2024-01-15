using System.Collections.Generic;
namespace YSF
{
    public partial class MySQLCommand
    {

        public static IMySqlCommand Limit(int count)
        {
            LIMIT select = ClassPool<LIMIT>.Pop();
            select.SetData(count);
            return select;
        }

        public static IMySqlCommand Values(Dictionary<string,string> pairs)
        {
            VALUES select = ClassPool<VALUES>.Pop();
            select.SetData(pairs);
            return select;
        }
        public static IMySqlCommand Select(string content = "*")
        {
            SELECT select = ClassPool<SELECT>.Pop();
            select.SetData(content);
            return select;
        }
        public static IMySqlCommand Datebase(string db ,string table)
        {
            DATABASE From = ClassPool<DATABASE>.Pop();
            From.SetData(db, table);
            return From;
        }
        public static IMySqlCommand Where
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("WHERE");
                return keyword;
            }
        }
        public static IMySqlCommand Compare(string fieldName,CompareType compareType,string value)
        {
            COMPARE COMPARE = ClassPool<COMPARE>.Pop();
            COMPARE.SetData(fieldName,compareType, value);
            return COMPARE;
        }
        public static IMySqlCommand Updates(string fieldName, string value)
        {
            UPDATES UPDATES = ClassPool<UPDATES>.Pop();
            UPDATES.SetData(fieldName, value);
            return UPDATES;
        }
        public static IMySqlCommand Order(string field, MySQLCoding coding, MySQLSort sort)
        {
            ORDER order = ClassPool<ORDER>.Pop();
            order.SetData(field,coding,sort);
            return order;
        }

        public static IMySqlCommand End
        {
            get {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData(";");
                return keyword;
            }
        }
        public static IMySqlCommand Or
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("OR");
                return keyword;
            }
        }
        public static IMySqlCommand And
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("AND");
                return keyword;
            }
        }
        public static IMySqlCommand From
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("FROM");
                return keyword;
            }
        }

        public static IMySqlCommand Insert
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("INSERT INTO");
                return keyword;
            }
        }

        public static IMySqlCommand Delete
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("DELETE");
                return keyword;
            }
        }
        public static IMySqlCommand Update
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("UPDATE");
                return keyword;
            }
        }
        public static IMySqlCommand Set
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("SET");
                return keyword;
            }
        }
        /// <summary>
        /// 左边小括号
        /// </summary>
        public static IMySqlCommand LeftParentheses
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData("(");
                return keyword;
            }
        }
        /// <summary>
        /// 左边小括号
        /// </summary>
        public static IMySqlCommand RightParentheses
        {
            get
            {
                KEYWORD keyword = ClassPool<KEYWORD>.Pop();
                keyword.SetData(")");
                return keyword;
            }
        }
      
    }
}
