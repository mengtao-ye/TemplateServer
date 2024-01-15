﻿namespace Server
{
    public static class ConverterExtend
    {
        public static bool ToBool(this int i)
        {
            return i == 1;    
        }
        public static byte ToByte(this int i)
        {
            return i.CastTo_Struct<byte>();
        }
        public static short ToShort(this int i)
        {
            return i.CastTo_Struct<short>();
        }
        public static int ToInt(this object obj)
        {
            return int.Parse( obj.ToString());    
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
        public static bool ToBool(this object obj)
        {
            return obj.ToString().Equals("1") ;
        }
    }
}
