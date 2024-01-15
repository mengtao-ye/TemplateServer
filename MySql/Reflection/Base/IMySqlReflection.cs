
using MySql.Data.MySqlClient;

namespace YSF
{
    public interface IMySqlReflection : IPool, IDataConverter
    {
        /// <summary>
        /// 映射MySQL数据
        /// </summary>
        /// <param name="reader"></param>
        void ReflectionMySQLData(MySqlDataReader reader);
    }
}
