namespace YSF
{
    public class RequestHandleManager
    {
        private TCPServer mTcpServer;
        private Client mClient;
        public RequestHandleManager(TCPServer socketModule,Client client )
        {
            mClient = client;
            mTcpServer = socketModule;
        }
        public byte[] Response( short requestCode, short actionCode,  byte[] data)
        {
            if (mTcpServer.map.Contains(requestCode)) {
                return mTcpServer.map.Get(requestCode).Response(actionCode,data);
            }
            return null;
        }
    }
}
