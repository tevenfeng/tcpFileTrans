using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using MetroFramework;
using System.Windows.Forms;

namespace tcpFileTrans
{
    /// <summary>
    /// 服务端代码，监听并接收数据
    /// </summary>
    class Server
    {
        #region properties

        /// <summary>
        /// 监听器
        /// </summary>
        private TcpListener myListener;
        /// <summary>
        /// 监听的端口
        /// </summary>
        private int port;
        /// <summary>
        /// 收到的文件的存储路径
        /// </summary>
        private string savePath;
        /// <summary>
        /// 二进制流，用于发送文件名
        /// </summary>
        private BinaryReader br;
        /// <summary>
        /// 用于存储收到的文件名和路径，方便更新已接收文件列表
        /// </summary>
        private Dictionary<string, string> dict = new Dictionary<string, string>();

        #endregion

        #region 构造函数

        /// <summary>
        /// 指定端口和接受文件存储路径，构造一个服务器tcp监听器
        /// </summary>
        /// <param name="port">监听的端口</param>
        /// <param name="savePath">接收文件的存储路径</param>
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

        #endregion

        #region methods

        /// <summary>
        /// 创建监听线程调用的线程方法，监听并接受文件名和文件
        /// </summary>
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
                        br = new BinaryReader(stream);
                        string name = br.ReadString();

                        string fileSavePath = this.savePath + "\\" + name;//获得用户保存文件的路径
                        FileStream fs = new FileStream(fileSavePath, FileMode.Create, FileAccess.Write);

                        byte[] buffer = new byte[512];
                        while ((size = stream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fs.Write(buffer, 0, size);
                            len += size;
                        }

                        dict.Add(name, fileSavePath);

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

        /// <summary>
        /// 获取已接收文件的文件名和存储路径，用于更新已接收文件列表
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> getDict()
        {
            return dict;
        }

        #endregion
    }
}
