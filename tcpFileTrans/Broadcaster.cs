using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.NetworkInformation;

namespace tcpFileTrans
{
    class Broadcaster
    {
        private string myIP = null;
        private string mySubnetMask = null;

        public Broadcaster()
        {
            
        }

        public void getLocalIp()
        {
            string hostName = Dns.GetHostName();
            IPHostEntry ip = Dns.GetHostEntry(hostName);

            foreach (IPAddress ipAddress in ip.AddressList)
            {
                if (ipAddress.AddressFamily.ToString() == "InterNetwork")
                {
                    this.myIP = ipAddress.ToString();                    
                }
            }            
        }

        public void getSubnetMask()
        {
            
        }
    }
}
