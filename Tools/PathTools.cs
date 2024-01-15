using System.IO;

namespace YSF
{
    public static class PathTools
    {
        /// <summary>
        /// 获取最新的版本号
        /// </summary>
        /// <returns>-1时代表数据错误</returns>
        public static int GetNewVersion() 
        {
            if (!Directory.Exists(Path.GetDirectoryName(PathData.VERTSION_PATH)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(PathData.VERTSION_PATH));
            }
            if (!File.Exists(PathData.VERTSION_PATH))
            {
                File.Create(PathData.VERTSION_PATH).Close();
            }
            string version = File.ReadAllText(PathData.VERTSION_PATH);
            int versionID = -1;
            if (int.TryParse(version,out versionID)) {
                return versionID;
            }
            return versionID;
        }

        /// <summary>
        /// 获取最新的下载地址
        /// </summary>
        /// <returns></returns>
        public static string GetNewVersionUrl()
        {
            if (!Directory.Exists(Path.GetDirectoryName(PathData.VERTSION_URL_PATH)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(PathData.VERTSION_URL_PATH));
            }
            if (!File.Exists(PathData.VERTSION_URL_PATH))
            {
                File.Create(PathData.VERTSION_URL_PATH).Close();
            }
            string version = File.ReadAllText(PathData.VERTSION_URL_PATH);
            if (string.IsNullOrEmpty(version))
            {
                return null;
            }
            return version;
        }
    }
}
