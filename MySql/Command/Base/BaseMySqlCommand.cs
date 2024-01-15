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
        public IMySqlCommand And
        {
            get
            {
                IMySqlCommand command = MySQLCommand.And;
                Concat(command);
                return command;
            }
        }
        public IMySqlCommand Concat(IMySqlCommand command)
        {
            Next = command;
            command.Preview = this;
            mySqlStr +=" "+ command.mySqlStr;
            command.mySqlStr = mySqlStr;
            return command;
        }
        public IMySqlCommand End
        {
            get {
                IMySqlCommand command = MySQLCommand.End;
                Concat(command);
                return command;
            }
        }
        public IMySqlCommand Datebase(string db, string table)
        {
            IMySqlCommand command = MySQLCommand.Datebase(db, table);
            Concat(command);
            return command;
        }
        public IMySqlCommand Or
        {
            get
            {
                IMySqlCommand command = MySQLCommand.Or;
                Concat(command);
                return command;
            }
        }
        public IMySqlCommand Datebase(string table)
        {
            IMySqlCommand command = MySQLCommand.Datebase(MySQLData.DataBaseName, table);
            Concat(command);
            return command;
        }
        public IMySqlCommand Values(Dictionary<string,string> keyValues)
        {
            IMySqlCommand command = MySQLCommand.Values(keyValues);
            Concat(command);
            return command;
        }
        public IMySqlCommand Updates(string field, string value)
        {
            IMySqlCommand command = MySQLCommand.Updates(field,value);
            Concat(command);
            return command;
        }
        public IMySqlCommand Limit(int count)
        {
            IMySqlCommand command = MySQLCommand.Limit(count);
            Concat(command);
            return command;
        }
        public IMySqlCommand Compare(string field, CompareType compareType, string value)
        {
            IMySqlCommand command = MySQLCommand.Compare(field,compareType,value);
            Concat(command);
            return command;
        }

        public IMySqlCommand Order(string field, MySQLCoding coding, MySQLSort sort)
        {
            IMySqlCommand command = MySQLCommand.Order(field, coding, sort);
            Concat(command);
            return command;
        }

        public IMySqlCommand Where
        {
            get
            {
                IMySqlCommand command = MySQLCommand.Where;
                Concat(command);
                return command;
            }
        }
        public IMySqlCommand From
        {
            get
            {
                IMySqlCommand command = MySQLCommand.From;
                Concat(command);
                return command;
            }
        }
        public IMySqlCommand Insert
        {
            get
            {
                IMySqlCommand command = MySQLCommand.Insert;
                Concat(command);
                return command;
            }
        }
        public IMySqlCommand Set
        {
            get
            {
                IMySqlCommand command = MySQLCommand.Set;
                Concat(command);
                return command;
            }
        }

        public IMySqlCommand LeftParentheses
        {
            get
            {
                IMySqlCommand command = MySQLCommand.LeftParentheses;
                Concat(command);
                return command;
            }
        }

        public IMySqlCommand RightParentheses
        {
            get
            {
                IMySqlCommand command = MySQLCommand.RightParentheses;
                Concat(command);
                return command;
            }
        }
    }
}
