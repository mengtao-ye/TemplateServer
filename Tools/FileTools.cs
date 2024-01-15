using System.IO;

namespace YSF
{
    /// <summary>
    /// 文件工具
    /// </summary>
    public static class FileTools
    {
        /// <summary>
        /// 清空Txt里面的内容
        /// </summary>
        /// <param name="path"></param>
        public static void ClearTxt(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            if (!File.Exists(path)) return;
            FileStream stream = File.Open(path, FileMode.OpenOrCreate, FileAccess.Write);
            stream.Seek(0, SeekOrigin.Begin);
            stream.SetLength(0);
            stream.Close();
        }

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="path"></param>
        public static void CreateFile(string path)
        {
            if (string.IsNullOrEmpty(path)) return;
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            if (!File.Exists(path))
            {
                File.Create(path).Close() ;
            }
        }
    }
}
