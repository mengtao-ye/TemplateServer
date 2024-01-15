namespace YSF
{
    public static class PathData
    {
        public static string LOG_PATH = @"C:\"+DataHelper.Instance.AppName+@"\Log.txt";
        public static string VERTSION_PATH = @"C:\"+ DataHelper.Instance.AppName+@"\Version.txt";//版本号
        public static string VERTSION_URL_PATH = @"C:\"+ DataHelper.Instance.AppName+@"\VersionUrl.txt";//版本地址
        public static string ERROR_TXT = @"C:\"+ DataHelper.Instance.AppName+@"\Error.txt";//用户错误信息
    }
}
