using System.IO;

namespace YSF
{
    /// <summary>
    /// 热更管理器
    /// </summary>
    public static class HotFixManager
    {
        private static ServerInfo mServerInfo;
        private static void Init()
        {
            if (string.IsNullOrEmpty(HotFixData.ServerInfoPath))
            {
                Debug.Log("Server Info 配置文件地址为空！");
                return;
            }
            if (!File.Exists(HotFixData.ServerInfoPath))
            {
                Debug.Log("无法找到本地Server Info 配置文件，地址：" + HotFixData.ServerInfoPath);
                return;
            }
            string serverInfo = File.ReadAllText(HotFixData.ServerInfoPath);
            mServerInfo = XmlMapper.ToObject<ServerInfo>(serverInfo);
        }
        public static Patches GetPatchs(string version)
        {
            if (mServerInfo == null) Init();
            if (mServerInfo == null)
            {
                return null;
            }
            for (int i = 0; i < mServerInfo.VersionInfos.Length; i++)
            {
                if (mServerInfo.VersionInfos[i].Version == version)
                {
                    return mServerInfo.VersionInfos[i].Patches[mServerInfo.VersionInfos[i].Patches.Length - 1];
                }
            }
            return null;
        }
    }

}