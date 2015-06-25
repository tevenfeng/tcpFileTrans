using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.Net.NetworkInformation;
using System.Net;
using System.Threading;

namespace tcpFileTrans
{
    class LanEnum
    {
        private List<ListViewItem> result = new List<ListViewItem>();
        private object obj = new object();

        public LanEnum()
        {

        }

        public List<ListViewItem> getResult()
        {
            ThreadStart threadStart;
            threadStart = new ThreadStart(searchMethod);
            Thread th1 = new Thread(threadStart);

            th1.Start();
            th1.Join();

            return result;
        }

        private void searchMethod()
        {
            lock (obj)
            {
                if (result != null)
                {
                    result.Clear();
                }
                ListViewItem tmp = new ListViewItem("Teven");
                tmp.ImageIndex = 0;
                tmp.SubItems.Add("127.0.0.1");
                result.Add(tmp);

                try
                {
                    for (int i = 1; i < 256; i++)
                    {
                        Ping myPing = new Ping();
                        myPing.PingCompleted += new PingCompletedEventHandler(_myPing_PingCompleted);

                        string pingIP = "192.168.1." + i.ToString();
                        myPing.SendAsync(pingIP, null);
                        Thread.Sleep(1);
                    }
                }
                catch (Exception exp)
                {
                    MessageBox.Show("错误！");
                }
            }
        }

        private void _myPing_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            if (e.Reply.Status == IPStatus.Success)
            {
                ListViewItem tmp = new ListViewItem(Dns.GetHostEntry(e.Reply.Address).HostName);
                tmp.SubItems.Add(e.Reply.Address.ToString());
                result.Add(tmp);
            }
        }
    }
}
