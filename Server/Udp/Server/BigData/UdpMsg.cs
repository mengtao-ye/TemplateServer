﻿
using System;
using System.Net;

namespace YSF
{
    public class UdpMsg : IPool, IDataConverter
    {
        public short udpCode;
        public byte type;//0为SmallData 1 为BigData
        public byte[] Data = null;
        public bool isPop { get; set; }
        public UdpMsg()
        {

        }
        public void SetUdpMsg(short udpCode, byte type, byte[] data)
        {
            this.udpCode = udpCode;
            Data = data;
            this.type = type;
        }
     
        public byte[] ToBytes()
        {
            if (Data.IsNullOrEmpty())
            {
                return ByteTools.Concat( BitConverter.GetBytes(udpCode),(byte)UdpMsgType.SmallData);
            }
            else
            {
                int len = 3 + Data.Length;
                byte[] tempData = new byte[len];
                int index = 0;
                tempData[index++] = BitConverter.GetBytes(udpCode)[0];
                tempData[index++] = BitConverter.GetBytes(udpCode)[1];
                tempData[index++] = type;
                for (int i = 0; i < Data.Length; i++)
                {
                    tempData[index++] = Data[i];
                }
                return tempData;
            }
        }
        public void ToValue(byte[] data)
        {
            if (data.Length < 3) return;
            this.udpCode = BitConverter.ToInt16(data, 0);
            this.type = data[2];
            this.Data = ByteTools.SubBytes(data, 3);
        }
        public void Recycle()
        {
            ClassPool<UdpMsg>.Push(this);
        }
        public void PopPool()
        {
        }
        public void PushPool()
        {
        }
    }
}
