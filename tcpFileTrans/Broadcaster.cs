using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;
using System.Threading;

namespace tcpFileTrans
{
    class Broadcaster
    {
        // 存储本机ip地址
        private string ip = null;
        // 存储局域网的子网掩码
        private string subnetMask = null;
        // 存储局域网的广播地址
        private string broadCastIP = null;
        // 广播socket句柄
        private Socket broadcastSocket = null;
        // 广播的端口
        private int port = 9101;

        /// <summary>
        /// 构造函数
        /// </summary>
        public Broadcaster()
        {
            getLocalIp();
            getSubnetMask();
            getBroadCastIP();

            int a = 1;
        }

        /// <summary>
        /// 获取本地电脑在局域网内的ip地址
        /// </summary>
        private void getLocalIp()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry ip = Dns.GetHostEntry(hostName);

            foreach (IPAddress ipAddress in ip.AddressList)
            {
                if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    this.ip = ipAddress.ToString();
                }
            }
        }

        /// <summary>
        /// 获取局域网的子网掩码
        /// </summary>
        private void getSubnetMask()
        {
            NetworkInterface[] Interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface Interface in Interfaces)
            {
                if (Interface.NetworkInterfaceType == NetworkInterfaceType.Loopback) continue;
                UnicastIPAddressInformationCollection UnicastIPInfoCol = Interface.GetIPProperties().UnicastAddresses;
                foreach (UnicastIPAddressInformation UnicatIPInfo in UnicastIPInfoCol)
                {
                    if (this.ip == UnicatIPInfo.Address.ToString())
                    {
                        SubnetMask = UnicatIPInfo.IPv4Mask.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 根据子网掩码和ip计算局域网的广播地址
        /// </summary>
        private void getBroadCastIP()
        {
            byte[] ip = IPAddress.Parse(this.ip).GetAddressBytes();
            byte[] sub = IPAddress.Parse(this.subnetMask).GetAddressBytes();

            // 广播地址=子网按位求反 再 或IP地址 
            for (int i = 0; i < ip.Length; i++)
            {
                ip[i] = (byte)((~sub[i]) | ip[i]);
            }
            this.broadCastIP = new IPAddress(ip).ToString();
        }

        /// <summary>
        /// 上线，广播通知其余所有在线的客户端，并侦听回复
        /// </summary>
        public void enterBroadCast()
        {
            int enterDataGram = 1;

            // 开始发送广播
            this.broadcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, this.port);
            byte[] data = Encoding.ASCII.GetBytes(enterDataGram.ToString());
            this.broadcastSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            while (true)
            {
                this.broadcastSocket.SendTo(data, iep);
                Thread.Sleep(5000);                          
            }
        }

        /// <summary>
        /// 下线，广播通知其他所有在线的客户端
        /// </summary>
        public void exitBroadCast()
        {
            int exitDataGram = 0;

            // 开始发送广播
            this.broadcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, this.port);
            byte[] data = Encoding.ASCII.GetBytes(exitDataGram.ToString());
            this.broadcastSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            this.broadcastSocket.SendTo(data, iep);
        }

        public void replyUser()
        {

        }

        public string Ip
        {
            get
            {
                return ip;
            }

            set
            {
                ip = value;
            }
        }

        public string SubnetMask
        {
            get
            {
                return subnetMask;
            }

            set
            {
                subnetMask = value;
            }
        }

        public string BroadCastIP
        {
            get
            {
                return broadCastIP;
            }

            set
            {
                broadCastIP = value;
            }
        }
    }
}
