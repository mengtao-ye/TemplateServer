using System.Collections.Generic;
namespace YSF
{
    public interface IMySqlCommand : IPool
    {
        string mySqlStr { get; set; }
        IMySqlCommand Next { get; }
        IMySqlCommand Preview { get; set; }
        IMySqlCommand Concat(IMySqlCommand command);
        IMySqlCommand Compare(string field,CompareType compareType, string value);
        IMySqlCommand Compares(CompareItem[] compareItems);
        IMySqlCommand Datebase(string db, string table);
        IMySqlCommand Updates(string field, string value);
        IMySqlCommand Datebase(string table);
        IMySqlCommand Limit(int count);
        IMySqlCommand Values(Dictionary<string,string> keyValues);
        IMySqlCommand Order(string field, MySQLCoding coding, MySQLSort sort);
        IMySqlCommand Order(string field,  MySQLSort sort);
        IMySqlCommand Like(string field, string value);
        IMySqlCommand And { get; }
        IMySqlCommand Or { get; }
        IMySqlCommand End { get; }
        IMySqlCommand Where { get; }
        IMySqlCommand From { get; }
        IMySqlCommand Insert { get; }
        IMySqlCommand Set { get; }
        IMySqlCommand Update { get; }
        IMySqlCommand LeftParentheses { get; }
        IMySqlCommand RightParentheses { get; }
    }
}
