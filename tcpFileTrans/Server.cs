using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace tcpFileTrans
{
    class Server
    {
        private TcpListener myListener;
        private int port;
        private string savePath;

        public Server(int port ,string savePath)
        {
            this.port = port;
            this.savePath = savePath;
            this.myListener = new TcpListener(IPAddress.Any, this.port);
            this.myListener.Start();

            Thread recvThread = new Thread(ReceiveMsg);
            recvThread.Start();
            recvThread.IsBackground = true;
        }

        public void ReceiveMsg()
        {
            while (true)
            {
                try
                {
                    int size = 0;
                    int len = 0;
                    TcpClient client = myListener.AcceptTcpClient();

                    NetworkStream stream = client.GetStream();
                    if (stream != null)
                    {
                        string fileSavePath = this.savePath+"\\";//获得用户保存文件的路径
                        FileStream fs = new FileStream(fileSavePath, FileMode.Create, FileAccess.Write);

                        byte[] buffer = new byte[512];
                        while ((size = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fs.Write(buffer, 0, size);
                            len += size;
                        }
                        fs.Flush();
                        stream.Flush();
                        stream.Close();
                        client.Close();                        
                    }
                }
                catch (Exception ex)
                {
                    
                }
            }
        }

    }
}
