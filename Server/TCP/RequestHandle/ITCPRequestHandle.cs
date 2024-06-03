namespace YSF
{
    public interface ITCPRequestHandle
    {
        short requestCode { get; }
        byte[] Response(short actionCode, byte[] data,Client client);
    }
}
