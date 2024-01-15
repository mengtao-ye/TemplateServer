namespace YSF
{
    public static class ObjectConverterExtend
    {
        public static int ToInt(this object obj)
        {
            return int.Parse(obj.ToString());
        }
        public static long ToLong(this object obj)
        {
            return long.Parse(obj.ToString());
        }
        public static double ToDouble(this object obj)
        {
            return double.Parse(obj.ToString());
        }
        public static short ToShort(this object obj)
        {
            return short.Parse(obj.ToString());
        }
        public static byte ToByte(this object obj)
        {
            return byte.Parse(obj.ToString());
        }
        public static bool ToBoolIsOne(this object obj)
        {
            return obj.ToString().Equals("1");
        }
        public static bool ToBoolIsTrue(this object obj)
        {
            return obj.ToString().Equals("true") || obj.ToString().Equals("True")|| obj.ToString().Equals("TRUE");
        }
    }
}
