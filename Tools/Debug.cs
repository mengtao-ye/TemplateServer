namespace YSF
{
    using System;
    using System.IO;

    public static class Debug
    {
        private static int Count;
        public static void Log<T>(T t)
        {
            Count++;
            if (Count > 1000)
            {
                Console.Clear();
                Count = 0;
            }
            if (t == null)
                Console.WriteLine("NULL");
            else
                Console.WriteLine(t.ToString());
        }
        public static void LogError<T>(T t)
        {
            if (t == null) return;
            Log("Error:" + t.ToString());
            RecordLog(t.ToString()); ;
        }
        private static void RecordLog(string msg)
        {
            if (!Directory.Exists(Path.GetDirectoryName(PathData.LOG_PATH))) {
                Directory.CreateDirectory(Path.GetDirectoryName(PathData.LOG_PATH));
            }
            if (!File.Exists(PathData.LOG_PATH))
            {
                File.Create(PathData.LOG_PATH).Close();
            }
            File.WriteAllText(PathData.LOG_PATH, msg);
        }
    }
}