using System.Collections.Generic;
using System.Xml.Serialization;

namespace YSF
{
    /// <summary>
    /// 服务器信息
    /// </summary>
    [System.Serializable]
    public class ServerInfo
    {
        [XmlElement]
        public VersionInfo[] VersionInfos;
    }
    /// <summary>
    /// 版本信息
    /// </summary>
    [System.Serializable]
    public class VersionInfo
    {
        [XmlAttribute]
        public string Version;
        [XmlElement]
        public Patches[] Patches;
    }
    /// <summary>
    /// 所有补丁包信息
    /// </summary>
    [System.Serializable]
    public class Patches
    {
        [XmlAttribute]
        public float Version;
        [XmlAttribute]
        public string Describute;
        [XmlElement]
        public List<Patch> Files;
    }
    /// <summary>
    /// 补丁包
    /// </summary>
    [System.Serializable]
    public class Patch
    {
        [XmlAttribute]
        public string Name;
        [XmlAttribute]
        public string Url;
        [XmlAttribute]
        public string Platform;
        [XmlAttribute]
        public string MD5;
        [XmlAttribute]
        public float Size;
    }
}
