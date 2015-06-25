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
    /// <summary>
    /// 局域网主机发现类，使用ping的方式发现主机并获取其主机名和ip地址
    /// </summary>
    class LanEnum
    {
        #region properties
        /// <summary>
        /// List<ListViewItem>，里面存放的是当前局域网内能ping通的主机
        /// </summary>
        private List<ListViewItem> result = new List<ListViewItem>();
        /// <summary>
        /// 线程互斥锁
        /// </summary>
        private object obj = new object();

        #endregion

        #region 构造函数
        public LanEnum()
        {

        }
        #endregion

        #region methods

        /// <summary>
        /// 返回一个List<ListViewItem>，里面存放的是当前局域网内能ping通的主机
        /// </summary>
        /// <returns></returns>
        public List<ListViewItem> getResult()
        {
            ThreadStart threadStart;
            threadStart = new ThreadStart(searchMethod);
            Thread th1 = new Thread(threadStart);

            th1.Start();
            th1.Join();

            return result;
        }

        /// <summary>
        /// 线程创建时调用此方法来搜索局域网主机
        /// </summary>
        private void searchMethod()
        {
            lock (obj)
            {
                if (result != null)
                {
                    result.Clear();
                }

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

        /// <summary>
        /// ping完成回掉函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _myPing_PingCompleted(object sender, PingCompletedEventArgs e)
        {
            if (e.Reply.Status == IPStatus.Success)
            {
                ListViewItem tmp = new ListViewItem(Dns.GetHostEntry(e.Reply.Address).HostName);
                tmp.SubItems.Add(e.Reply.Address.ToString());
                result.Add(tmp);
            }
        }

        #endregion
    }
}
