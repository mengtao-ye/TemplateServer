namespace YSF
{
    /// <summary>
    /// 服务器中心类
    /// </summary>
    public class ServerCenter
    {
        private UdpServer mUdpServer;//Udp服务器对象
        public UdpServer udpServer { get { return mUdpServer; } }//Udp服务器对象
        private TCPServer mTcpServer;//Tcp服务器对象
        public TCPServer tcpServer { get { return mTcpServer; } }//Tcp服务器对象
        private MySQLManager mMySQLManager;//数据库管理类
        public MySQLManager mySQLManager { get { return mMySQLManager; } }//数据库管理类

        public ServerCenter()
        {
            mUdpServer = null;
            mTcpServer = null;
        }
        #region Helper
        /// <summary>
        /// 启动模块，前提是该配置的已经配置好了
        /// </summary>
        public void InitHelper(DataHelper dataHelper)
        {
            if (dataHelper == null)
            {
                throw new System.NullReferenceException("dataHelper is null");
            }
            DataHelper.Instance = dataHelper;
        } 
        #endregion
        #region TCP
        /// <summary>
        /// 启动TCP服务器
        /// </summary>
        public void LauncherTCPServer(string ipAddress, int port, IMap<short, ITCPRequestHandle> map)
        {
            if (ipAddress.IsNullOrEmpty())
            {
                throw new System.Exception("tcp ip address is null or empty");
            }
            if (map == null || map.Count == 0)
            {
                throw new System.Exception("TCPRequestHandle is null or empty");
            }
            if (ushort.MinValue >= port || port >= ushort.MaxValue)
            {
                throw new System.Exception("port out index ");
            }
            mTcpServer = new TCPServer(map);
            mTcpServer.Run(ipAddress, port);
        }

        #endregion
        #region UDP
        /// <summary>
        /// 启动UDPServer
        /// </summary>
        public void LauncherUDPServer(string ipAddress, int port, IMap<short, IUdpRequestHandle> map)
        {
            if (ipAddress.IsNullOrEmpty())
            {
                throw new System.Exception("udp ip address is null or empty");
            }
            if (ushort.MinValue >= port || port >= ushort.MaxValue)
            {
                throw new System.Exception("port out index ");
            }
            if (map == null )
            {
                throw new System.Exception("UDPRequestHandle is null or empty");
            }
            mUdpServer = new UdpServer();
            mUdpServer.Start(ipAddress, port, map);
        }
        #endregion
        #region MySQL

        public void LauncherMySQL(string ipAddress, string databaseName, string account, string password)
        {
            mMySQLManager = new MySQLManager();
            mMySQLManager.Launcher(ipAddress, databaseName, account, password);
        } 
        #endregion
    }
}
