namespace YSF
{
    /// <summary>
    /// 对象池接口
    /// </summary>
    public interface IPool : IRecycle
    {
        /// <summary>
        /// 是否已经弹出池子了
        /// </summary>
        bool isPop { get; set; }
        /// <summary>
        /// 从对象池出来时 
        /// </summary>
        void PopPool();
        /// <summary>
        ///从对象池进来
        /// </summary>
        void PushPool();
    }
}
