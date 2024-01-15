namespace YSF
{
    public static class StringExtend
    {
        /// <summary>
        /// 判断参数是否为空
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }
    }
}
