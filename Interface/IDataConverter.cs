namespace YSF
{
    /// <summary>
    /// 数据转换接口
    /// </summary>
    public interface IDataConverter
    {
        /// <summary>
        /// 转换为字节
        /// </summary>
        /// <returns></returns>
        byte[] ToBytes();
        /// <summary>
        /// 将字节转换成数据
        /// </summary>
        /// <param name="data"></param>
        void ToValue(byte[] data);
    }
}
