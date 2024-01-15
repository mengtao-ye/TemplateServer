namespace YSF
{
    /// <summary>
    /// 帧同步信息
    /// </summary>
    public static class LockStepMsg
    {
        /// <summary>
        /// 当前游戏帧
        /// </summary>
        public static int GameFrame = 0;
        /// <summary>
        /// 清空帧同步数据
        /// </summary>
        public static void Clear()
        {
            GameFrame = 0;
        }
    }
}
