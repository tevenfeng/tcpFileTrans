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
    class Client
    {
        private string ip;
        private int port;
        private IPEndPoint iep;
        private FileStream fs;
        private TcpClient myTcpClient;
        private NetworkStream ns;

        public Client(string ipToConnect, int portToConnect)
        {
            this.ip = ipToConnect;
            this.port = portToConnect;
            this.iep = new IPEndPoint(IPAddress.Parse(ip), port);
        }

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
    }
}
