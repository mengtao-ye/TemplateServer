using System;
using MySql.Data.MySqlClient;
using System.Data;
using System.Text;
using System.Collections.Generic;

namespace YSF
{
    public class MySQLManager
    {
        private MySqlConnection mConnect;
        public MySqlCommand command { get; private set; }
        public MySqlConnection connect { get { return mConnect; } }
        public ConnectionState connectionState { get { return connect.State; } }
        private MySqlDataReader mReader;
        public bool IsConnect { get; private set; }

        public MySQLManager()
        {
        }
        ~MySQLManager()
        {
            mConnect.Dispose();
            mConnect.Close();
        }

        public void Launcher(string ipAddress, string databaseName, string account, string password)
        {
            if (ConnectToDatabase(ipAddress, databaseName, account, password))
            {
                IsConnect = true;
            }
        }

        private bool ConnectToDatabase(string ipAddress, string databaseName, string account, string password)
        {
            MySQLData.DataBaseName = databaseName;
            string connectStr = "server=" + ipAddress + ";database=" + databaseName + ";uid=" + account + ";pwd=" + password;
            try
            {
                mConnect = new MySqlConnection(connectStr);
                connect.Open();
            }
            catch (Exception)
            {
                Debug.Log("数据库连接失败");
            }
            if (connect.State == ConnectionState.Open)
            {
                Debug.Log("数据库连接成功");
                command = new MySqlCommand();
                command.Connection = mConnect;
                return true;
            }
            else
            {
                Debug.Log("数据库连接失败");
                return false;
            }
        }
        /// <summary>
        /// 检查连接
        /// </summary>
        public void CheckCommand()
        {
            if (command == null)
            {
                command = new MySqlCommand();
                command.Connection = mConnect;
            }
            if (command.Connection.State == ConnectionState.Closed)
            {
                command.Connection.Open();
            }
            command.Parameters.Clear();
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        public void CloseCommand()
        {
            if (command != null && command.Connection.State == ConnectionState.Open)
            {
                command.Connection.Close();
                command.Dispose();
                command = null;
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="mySQL"></param>
        /// <param name="lastID"></param>
        /// <returns></returns>
        public bool Add(MySQL mySQL, out long lastID)
        {
            lastID = -1;
            if (mySQL == null || mySQL.parameters == null) return false;
            CheckCommand();
            StringBuilderPool key = ClassPool<StringBuilderPool>.Pop();
            StringBuilderPool value = ClassPool<StringBuilderPool>.Pop();
            int count = 0;
            foreach (var item in mySQL.parameters.data)
            {
                key.Append("`" + item.Key + "`");
                value.Append("@" + item.Key);
                if (count != mySQL.parameters.Count - 1)
                {
                    key.Append(",");
                    value.Append(",");
                }
                count++;
            }
            command.CommandText = "INSERT INTO `" + MySQLData.DataBaseName + "`.`" + mySQL.tableName + "` (" + key.ToString() + ") VALUES (" + value.ToString() + ");";
            foreach (var item in mySQL.parameters.data)
            {
                command.Parameters.AddWithValue("@" + item.Key, item.Value);
            }
            int rawEffect = 0;
            try
            {
                rawEffect = command.ExecuteNonQuery();
                command.CommandText = "SELECT LAST_INSERT_ID();";
                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    lastID = reader.GetInt64(0);
                }
                if (reader != null)
                {
                    reader.Dispose();
                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                CloseCommand();
                key.Recycle();
                value.Recycle();
                return false;
            }
            CloseCommand();
            key.Recycle();
            value.Recycle();
            if (rawEffect == 0) return false;
            else return true;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="mySQL"></param>
        /// <returns></returns>
        public bool Add(string mySQL)
        {
            if (mySQL == null) return false;
            CheckCommand();
            command.CommandText = mySQL;
            int rawEffect = 0;
            try
            {
                rawEffect = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                CloseCommand();
                return false;
            }
            CloseCommand();
            if (rawEffect == 0) return false;
            else return true;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="mySQL"></param>
        /// <returns></returns>
        public bool Add(MySQL mySQL)
        {
            if (mySQL == null || mySQL.parameters == null) return false;
            CheckCommand();
            StringBuilderPool key = ClassPool<StringBuilderPool>.Pop();
            StringBuilderPool value = ClassPool<StringBuilderPool>.Pop();
            int count = 0;
            foreach (var item in mySQL.parameters.data)
            {
                key.Append("`" + item.Key + "`");
                value.Append("@" + item.Key);
                if (count != mySQL.parameters.Count - 1)
                {
                    key.Append(",");
                    value.Append(",");
                }
                count++;
            }
            command.CommandText = "INSERT INTO `" + MySQLData.DataBaseName + "`.`" + mySQL.tableName + "` (" + key.ToString() + ") VALUES (" + value.ToString() + ");";

            foreach (var item in mySQL.parameters.data)
            {
                command.Parameters.AddWithValue("@" + item.Key, item.Value);
            }
            int rawEffect = 0;
            try
            {
                rawEffect = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                CloseCommand();
                key.Recycle();
                value.Recycle();
                return false;
            }
            CloseCommand();
            key.Recycle();
            value.Recycle();
            if (rawEffect == 0) return false;
            else return true;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="mySQL"></param>
        /// <returns></returns>
        public bool Delete(string mySQL)
        {
            if (mySQL.IsNullOrEmpty()) return false;
            CheckCommand();
            command.CommandText = mySQL;
            int rawEffect = 0;
            try
            {
                rawEffect = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                CloseCommand();
                return false;
            }
            CloseCommand();
            if (rawEffect == 0) return false;
            else return true;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="mySQL"></param>
        /// <returns></returns>
        public bool Delete(MySQL mySQL)
        {
            if (mySQL == null || mySQL.parameters == null) return false;
            CheckCommand();
            StringBuilderPool key = ClassPool<StringBuilderPool>.Pop();
            int count = 0;
            foreach (var item in mySQL.parameters.data)
            {
                key.Append("`" + item.Key + "`");
                key.Append("=");
                key.Append("@" + item.Key);
                if (count != mySQL.parameters.Count - 1)
                {
                    key.Append(" and ");
                }
                count++;
            }
            command.CommandText = "DELETE FROM `" + MySQLData.DataBaseName + "`.`" + mySQL.tableName + "` WHERE " + key.ToString() + ";";
            foreach (var item in mySQL.parameters.data)
            {
                command.Parameters.AddWithValue("@" + item.Key, item.Value);
            }
            int rawEffect = 0;
            try
            {
                rawEffect = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                CloseCommand();
                key.Recycle();
                return false;
            }
            CloseCommand();
            key.Recycle();
            if (rawEffect == 0) return false;
            else return true;
        }

        /// <summary>
        /// 执行数据
        /// </summary>
        /// <param name="mySQL"></param>
        /// <returns></returns>
        public bool Exe(string mysql)
        {
            if (mysql .IsNullOrEmpty()) return false;
            CheckCommand();
            command.CommandText = mysql;
            int rawEffect = 0;
            try
            {
                rawEffect = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                CloseCommand();
                return false;
            }
            CloseCommand();
            if (rawEffect == 0) return false;
            else return true;
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="mySQL"></param>
        /// <returns></returns>
        public bool Update(WhereMySQL mySQL)
        {
            if (mySQL == null || mySQL.parameters == null) return false;
            CheckCommand();
            StringBuilderPool parameter = ClassPool<StringBuilderPool>.Pop();
            int count = 0;
            foreach (var item in mySQL.parameters.data)
            {
                parameter.Append("`" + item.Key + "`");
                parameter.Append("=");
                parameter.Append("@" + item.Key);
                if (count != mySQL.parameters.Count - 1)
                {
                    parameter.Append(",");
                }
                count++;
            }
            StringBuilderPool where = ClassPool<StringBuilderPool>.Pop();
            count = 0;
            foreach (var item in mySQL.where.data)
            {
                where.Append("`" + item.Key + "`");
                where.Append("=");
                where.Append("@" + item.Key);
                if (count != mySQL.where.Count - 1)
                {
                    where.Append(" and ");
                }
                count++;
            }
            command.CommandText = "UPDATE `" + MySQLData.DataBaseName + "`.`" + mySQL.tableName + "` SET  " + parameter.ToString() + "  WHERE " + where.ToString() + ";";
            foreach (var item in mySQL.parameters.data)
            {
                command.Parameters.AddWithValue("@" + item.Key, item.Value);

            }
            foreach (var item in mySQL.where.data)
            {
                command.Parameters.AddWithValue("@" + item.Key, item.Value);
            }
            int rawEffect = 0;
            try
            {
                rawEffect = command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                CloseCommand();
                parameter.Recycle();
                where.Recycle();
                return false;
            }
            CloseCommand();
            parameter.Recycle();
            where.Recycle();
            if (rawEffect == 0) return false;
            else return true;
        }
        /// <summary>
        /// 执行MySQL语句
        /// </summary>
        /// <param name="mySql"></param>
        /// <returns></returns>
        public bool IsExist(string mySql)
        {
            if (string.IsNullOrEmpty(mySql)) return false;
            CheckCommand();
            command.CommandText = mySql;
            mReader = command.ExecuteReader();
            if (mReader == null)
            {
                CloseCommand();
                return false;
            }
            bool hasValue = mReader.HasRows;
            mReader.Dispose();
            mReader.Close();
            CloseCommand();
            return hasValue;
        }
        /// <summary>
        /// 判断是否存在数据
        /// </summary>
        /// <param name="mySQL"></param>
        /// <returns></returns>
        public bool IsExist(MySQL mySQL)
        {
            if (mySQL == null || mySQL.parameters == null) return false;
            CheckCommand();
            StringBuilderPool where = ClassPool<StringBuilderPool>.Pop();
            int count = 0;
            foreach (var item in mySQL.parameters.data)
            {
                where.Append("`" + item.Key + "`");
                where.Append("=");
                where.Append("@" + item.Key);
                if (count != mySQL.parameters.Count - 1)
                {
                    where.Append(" and ");
                }
                count++;
            }
            command.CommandText = "SELECT * FROM `" + MySQLData.DataBaseName + "`.`" + mySQL.tableName + "` where " + where.ToString() + ";";
            foreach (var item in mySQL.parameters.data)
            {
                command.Parameters.AddWithValue("@" + item.Key, item.Value);
            }
            mReader = command.ExecuteReader();
            if (mReader == null)
            {
                CloseCommand();
                where.Recycle();
                return false;
            }

            bool hasValue = mReader.HasRows;
            mReader.Dispose();
            mReader.Close();
            CloseCommand();
            where.Recycle();
            return hasValue;
        }
        /// <summary>
        /// 查找对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="mysql"></param>
        /// <returns></returns>
        public T Find<T>(T target, string mysql) where T : IMySqlReflection
        {
            CheckCommand();
            command.CommandText = mysql;
            mReader = command.ExecuteReader();
            if (mReader == null)
            {
                CloseCommand();
                return default(T);
            }
            bool hasValue = false;
            if (mReader.Read())
            {
                hasValue = true;
                target.ReflectionMySQLData(mReader);
            }
            mReader.Dispose();
            mReader.Close();
            CloseCommand();
            if (hasValue)
            {
                return target;
            }
            else
            {
                return default(T);
            }
        }
        /// <summary>
        /// 查找对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="mysql"></param>
        /// <returns></returns>
        public T Find<T>(string mysql) where T : class, IMySqlReflection, new()
        {
            CheckCommand();
            command.CommandText = mysql;
            mReader = command.ExecuteReader();
            if (mReader == null)
            {
                CloseCommand();
                return default(T);
            }
            T t = null;
            if (mReader.Read())
            {
                t = new T();
                t.ReflectionMySQLData(mReader);
            }
            mReader.Dispose();
            mReader.Close();
            CloseCommand();
            return t;
        }
        /// <summary>
        /// 查找所有对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="mysql"></param>
        /// <returns></returns>
        public List<T> FindAll<T>(string mysql) where T : class, IMySqlReflection, new()
        {
            CheckCommand();
            command.CommandText = mysql;
            mReader = command.ExecuteReader();
            if (mReader == null)
            {
                CloseCommand();
                return null;
            }
            bool hasValue = false;
            List<T> lists = new List<T>();
            T classTarget = ClassPool<T>.Pop();
            while (mReader.Read())
            {
                hasValue = true;
                classTarget.ReflectionMySQLData(mReader);
                lists.Add(classTarget);
            }
            mReader.Dispose();
            mReader.Close();
            CloseCommand();
            if (hasValue)
            {
                return lists;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 查找所有对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="mysql"></param>
        /// <returns></returns>
        public IListData<long> FindAll(string mysql,string key) 
        {
            CheckCommand();
            command.CommandText = mysql;
            mReader = command.ExecuteReader();
            if (mReader == null)
            {
                CloseCommand();
                return null;
            }
            bool hasValue = false;
            IListData<long> list = null;
            while (mReader.Read())
            {
                hasValue = true;
                long value = mReader.GetInt64(key);
                if (list == null) 
                {
                    list = ClassPool<ListData<long>>.Pop();
                }
                list.Add(value);
            }
            mReader.Dispose();
            mReader.Close();
            CloseCommand();
            if (hasValue)
            {
                return list;
            }
            else
            {
                return null;
            }
        }

        public IListData<T> FindAllByListPoolData<T>(MySQL mySQL) where T : class, IMySqlReflection, IPool, new()
        {
            if (mySQL == null || mySQL.parameters == null) return null;
            CheckCommand();
            StringBuilderPool where = ClassPool<StringBuilderPool>.Pop();
            int count = 0;
            foreach (var item in mySQL.parameters.data)
            {
                where.Append("`" + item.Key + "`");
                where.Append("=");
                where.Append("@" + item.Key);
                if (count != mySQL.parameters.Count - 1)
                {
                    where.Append(" and ");
                }
                count++;
            }
            command.CommandText = "SELECT * FROM `" + MySQLData.DataBaseName + "`.`" + mySQL.tableName + "` where " + where.ToString() + ";";
            where.Recycle();
            mReader = command.ExecuteReader();
            if (mReader == null)
            {
                CloseCommand();
                return null;
            }
            bool hasValue = false;
            IListData<T> lists = ClassPool<ListPoolData<T>>.Pop();
            while (mReader.Read())
            {
                T target = ClassPool<T>.Pop();
                hasValue = true;
                target.ReflectionMySQLData(mReader);
                lists.Add(target);
            }
            mReader.Dispose();
            mReader.Close();
            CloseCommand();
            if (hasValue)
            {
                return lists;
            }
            else
            {
                return null;
            }
        }

        public IListData<T> FindAllByListPoolData<T>(string mysql) where T : class, IMySqlReflection, IPool, new()
        {
            CheckCommand();
            command.CommandText = mysql;
            mReader = command.ExecuteReader();
            if (mReader == null)
            {
                CloseCommand();
                return null;
            }
            bool hasValue = false;
            IListData<T> lists = null ;
            while (mReader.Read())
            {
                if (lists == null)
                {
                    lists = ClassPool<ListPoolData<T>>.Pop();
                }
                T target = ClassPool<T>.Pop();
                hasValue = true;
                target.ReflectionMySQLData(mReader);
                lists.Add(target);
            }
            mReader.Dispose();
            mReader.Close();
            CloseCommand();
            if (hasValue)
            {
                return lists;
            }
            else
            {
                return null;
            }
        }
        public IListData<T> FindAllByListData<T>(string mysql) where T : class, IMySqlReflection, IPool, new()
        {
            CheckCommand();
            command.CommandText = mysql;
            mReader = command.ExecuteReader();
            if (mReader == null)
            {
                CloseCommand();
                return null;
            }
            bool hasValue = false;
            IListData<T> lists = ClassPool<ListData<T>>.Pop();
            while (mReader.Read())
            {
                T target = ClassPool<T>.Pop();
                hasValue = true;
                target.ReflectionMySQLData(mReader);
                lists.Add(target);
            }
            mReader.Dispose();
            mReader.Close();
            CloseCommand();
            if (hasValue)
            {
                return lists;
            }
            else
            {
                return null;
            }
        }
        public T Find<T>(MySQL mySQL) where T : class, IMySqlReflection, new()
        {
            CheckCommand();
            StringBuilderPool where = ClassPool<StringBuilderPool>.Pop();
            int count = 0;
            foreach (var item in mySQL.parameters.data)
            {
                where.Append("`" + item.Key + "`");
                where.Append("=");
                where.Append("@" + item.Key);
                if (count != mySQL.parameters.Count - 1)
                {
                    where.Append(" and ");
                }
                count++;
            }
            command.CommandText = "SELECT * FROM `" + MySQLData.DataBaseName + "`.`" + mySQL.tableName + "` where " + where.ToString() + ";";
            foreach (var item in mySQL.parameters.data)
            {
                command.Parameters.AddWithValue("@" + item.Key, item.Value);
            }
            mReader = command.ExecuteReader();
            if (mReader == null)
            {
                CloseCommand();
                where.Recycle();
                return default(T);
            }
            T target = null;
            if (mReader.Read())
            {
                target = new T();
                target.ReflectionMySQLData(mReader);
            }
            else
            {
                mReader.Dispose();
                mReader.Close();
                CloseCommand();
                where.Recycle();
                return default(T);
            }
            mReader.Dispose();
            mReader.Close();
            CloseCommand();
            where.Recycle();
            return target;
        }

        public T FindPool<T>(string mySQL) where T : class, IMySqlReflection, IPool, new()
        {
            CheckCommand();
            command.CommandText = mySQL;
            mReader = command.ExecuteReader();
            if (mReader == null)
            {
                CloseCommand();
            
                return default(T);
            }
            T target = null;
            if (mReader.Read())
            {
                target = ClassPool<T>.Pop();
                target.ReflectionMySQLData(mReader);
            }
            else
            {
                mReader.Dispose();
                mReader.Close();
                CloseCommand();
             
                return default(T);
            }
            mReader.Dispose();
            mReader.Close();
            CloseCommand();
            return target;
        }

        public T FindPool<T>(MySQL mySQL) where T : class, IMySqlReflection, IPool, new()
        {
            CheckCommand();
            StringBuilderPool where = ClassPool<StringBuilderPool>.Pop();
            int count = 0;
            foreach (var item in mySQL.parameters.data)
            {
                where.Append("`" + item.Key + "`");
                where.Append("=");
                where.Append("@" + item.Key);
                if (count != mySQL.parameters.Count - 1)
                {
                    where.Append(" and ");
                }
                count++;
            }
            command.CommandText = "SELECT * FROM `" + MySQLData.DataBaseName + "`.`" + mySQL.tableName + "` where " + where.ToString() + ";";
            foreach (var item in mySQL.parameters.data)
            {
                command.Parameters.AddWithValue("@" + item.Key, item.Value);
            }
            mReader = command.ExecuteReader();
            if (mReader == null)
            {
                CloseCommand();
                where?.Recycle();
                return default(T);
            }
            T target = null;
            if (mReader.Read())
            {
                target = ClassPool<T>.Pop();
                target.ReflectionMySQLData(mReader);
            }
            else
            {
                mReader.Dispose();
                mReader.Close();
                CloseCommand();
                where?.Recycle();
                return default(T);
            }
            mReader.Dispose();
            mReader.Close();
            CloseCommand();
            where?.Recycle();
            return target;
        }
        /// <summary>
        /// 结尾的MySQL语句
        /// </summary>
        /// <param name="mySQL"></param>
        /// <param name="endSql"></param>
        /// <returns></returns>
        public int Count(MySQL mySQL)
        {
            CheckCommand();
            StringBuilderPool where = ClassPool<StringBuilderPool>.Pop();
            int count = 0;
            foreach (var item in mySQL.parameters.data)
            {
                where.Append("`" + item.Key + "`");
                where.Append("=");
                where.Append("@" + item.Key);
                if (count != mySQL.parameters.Count - 1)
                {
                    where.Append(" and ");
                }
                count++;
            }
            command.CommandText = "SELECT * FROM `" + MySQLData.DataBaseName + "`.`" + mySQL.tableName + "` where " + where.ToString() + ";";
            foreach (var item in mySQL.parameters.data)
            {
                command.Parameters.AddWithValue("@" + item.Key, item.Value);
            }
            mReader = command.ExecuteReader();
            if (mReader == null)
            {
                CloseCommand();
                where.Recycle();
                return 0;
            }
            int account = 0;
            while (mReader.Read())
            {
                account++;
            }
            mReader.Dispose();
            mReader.Close();
            CloseCommand();
            where.Recycle();
            return account;
        }
    }

}