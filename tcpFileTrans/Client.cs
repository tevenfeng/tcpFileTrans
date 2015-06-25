using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace tcpFileTrans
{
    /// <summary>
    /// “客户端”代码，主要用于发送消息和文件
    /// </summary>
    class Client
    {
        #region properties

        /// <summary>
        /// 存储远程端点的ip
        /// </summary>
        private string ip;
        /// <summary>
        /// 存储软件所用到的端口
        /// </summary>
        private int port;
        /// <summary>
        /// 远程端点
        /// </summary>
        private IPEndPoint iep;
        /// <summary>
        /// 文件流，用于读取文件
        /// </summary>
        private FileStream fs;
        /// <summary>
        /// tcp连接
        /// </summary>
        private TcpClient myTcpClient;
        /// <summary>
        /// 从tcp连接建立以后获取的网络流
        /// </summary>
        private NetworkStream ns;
        /// <summary>
        /// 用于发送文件名
        /// </summary>
        private BinaryWriter bw;

        #endregion

        #region 构造函数

        /// <summary>
        /// 根据指定ip和端口号建立一个tcp连接
        /// </summary>
        /// <param name="ipToConnect">远程端点的ip</param>
        /// <param name="portToConnect">tcp连接使用的端口号</param>
        public Client(string ipToConnect, int portToConnect)
        {
            this.ip = ipToConnect;
            this.port = portToConnect;
            this.iep = new IPEndPoint(IPAddress.Parse(ip), port);
        }

        #endregion

        #region methods

        /// <summary>
        /// 建立连接，成功返回true，否则返回false
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            bool result = false;

            try
            {
                this.myTcpClient = new TcpClient();

                this.myTcpClient.Connect(iep);
                if (myTcpClient != null)
                {
                    this.ns = myTcpClient.GetStream();
                }

                result = true;
            }
            catch (Exception exp)
            {
                result = false;
            }

            return result;
        }

        /// <summary>
        /// 发送指定路径的文件
        /// </summary>
        /// <param name="filePath">要发送文件的路径</param>
        public void sendFile(string filePath)
        {
            string path = filePath;
            this.fs = new FileStream(path, FileMode.Open);

            int size = 0;
            int len = 0;
            while (len < fs.Length)
            {
                byte[] buffer = new byte[512];
                size = fs.Read(buffer, 0, buffer.Length);
                ns.Write(buffer, 0, size);
                len += size;  
            }
            fs.Flush();
            ns.Flush();
            fs.Close();
            ns.Close();
        }

        /// <summary>
        /// 发送指定文件的文件名
        /// </summary>
        /// <param name="fileName">要发送的文件的文件名</param>
        public void sendName(string fileName)
        {
            try
            {
                string nameToSend = fileName;
                this.bw = new BinaryWriter(ns);

                bw.Write(fileName);
                bw.Flush();
                ns.Flush();
            }
            catch (Exception exp)
            {

            }
        }

        #endregion
    }
}
