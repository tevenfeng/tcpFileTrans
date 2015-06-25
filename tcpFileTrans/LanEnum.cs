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
                //因为是刷新列表，所以要先清空
                if (result != null)
                {
                    result.Clear();
                }

                try
                {
                    //扫描整个网段
                    for (int i = 1; i < 256; i++)
                    {
                        //ping主机从而获取其IP地址和主机名
                        Ping myPing = new Ping();
                        myPing.PingCompleted += new PingCompletedEventHandler(_myPing_PingCompleted);

                        //如果路由器不是这么设置的这个地方就要改~~~偷个懒~^_^
                        string pingIP = "192.168.1." + i.ToString();

                        //异步ping，防止主界面过长时间不响应
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
            //ping通说明该主机可连接
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
