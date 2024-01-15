namespace YSF
{
    /// <summary>
    /// 自定义的随机数
    /// </summary>
    public static class Random_My
    {
        private static System.Random mRandom = new System.Random();
        public static int Range(int min,int max)
        {
            return mRandom.Next(min,max);
        }
        public static short Range(short min, short max)
        {
            return (short)mRandom.Next(min, max);
        }
        public static byte Range(byte min, byte max)
        {
            return (byte)mRandom.Next(min, max);
        }
        public static float Range(float min, float max)
        {
            return mRandom.Next((int)(min *1000), (int)(max * 1000)) / 10000;
        }
        public static double Range(double min, double max)
        {
            return mRandom.Next((int)(min * 1000), (int)(max * 1000)) / 10000;
        }
    }
}
