namespace YSF
{
    public class RequestHandleManager
    {
        private TcpServer mTcpServer;
        private Client mClient;
        public RequestHandleManager(TcpServer socketModule,Client client )
        {
            mClient = client;
            mTcpServer = socketModule;
        }
        public byte[] Response( short requestCode, short actionCode,  byte[] data,Client client)
        {
            if (mTcpServer.map.Contains(requestCode)) 
            {
                return mTcpServer.map.Get(requestCode).Response(actionCode,data, client);
            }
            return null;
        }
    }
}
