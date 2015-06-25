using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using MetroFramework;
using MetroFramework.Controls;
using System.IO;
using System.Threading;

namespace tcpFileTrans
{
    public partial class mainForm : MetroForm
    {
        private MetroButton button_refreshHostList;
        private MetroButton button_exploreFile;
        private MetroButton button_SendFile;
        private MetroTabControl tabCtr_sendRecv;
        private MetroButton button_refreshFileList;
        private string iptoSend;
        private Server myServer;

        private string[] filePaths = null, fileNames = null;

        public mainForm()
        {
            InitializeComponent();
            this.Text = "文件传输";

            hostList_Init();
            fileRecv_Init();
            fileSend_Init();
            Init_userControls();

            mainForm_addEventHandlers();
            this.Focus();

            //创建一个线程，开始监听
            Thread serverThread = new Thread(serverListen);
            serverThread.Start();
        }

        /// <summary>
        /// 线程调用函数，用于创建监听tcp连接的服务器
        /// </summary>
        private void serverListen()
        {
            myServer = new Server(9100, Environment.CurrentDirectory);
        }

        #region 自定义控件的初始化

        protected void Init_userControls()
        {
            //刷新按钮
            button_refreshHostList = new MetroButton();
            button_refreshHostList.Location = new Point(336, 60);
            button_refreshHostList.Text = "刷新";
            button_refreshHostList.Size = new System.Drawing.Size(61, 24);
            button_refreshHostList.KeyDown += new KeyEventHandler(this.mainForm_KeyDown);
            button_refreshHostList.Click += new EventHandler(button_refreshHostList_Clicked);
            this.Controls.Add(button_refreshHostList);

            //浏览按钮
            button_exploreFile = new MetroButton();
            button_exploreFile.Location = new Point(330-61, 390);
            button_exploreFile.Text = "浏览";
            button_exploreFile.Visible = false;
            button_exploreFile.Size = new System.Drawing.Size(61, 24);
            button_exploreFile.KeyDown += new KeyEventHandler(this.mainForm_KeyDown);
            button_exploreFile.Click += new EventHandler(this.button_exploreFile_Click);
            this.Controls.Add(button_exploreFile);

            //发送按钮
            button_SendFile = new MetroButton();
            button_SendFile.Location = new Point(355, 390);
            button_SendFile.Text = "发送";
            button_SendFile.Visible = false;
            button_SendFile.Size = new System.Drawing.Size(61, 24);
            button_SendFile.KeyDown += new KeyEventHandler(this.mainForm_KeyDown);
            button_SendFile.Click += new EventHandler(this.button_sendFile_Click);
            this.Controls.Add(button_SendFile);

            //刷新文件列表按钮
            button_refreshFileList = new MetroButton();
            button_refreshFileList.Location = new Point(342, 390);
            button_refreshFileList.Text = "刷新文件";
            button_refreshFileList.Height = 24;
            button_refreshFileList.Visible = true;
            button_refreshFileList.KeyDown += new KeyEventHandler(this.mainForm_KeyDown);
            button_refreshFileList.Click += new EventHandler(this.button_refreshFileList_Click);
            this.Controls.Add(button_refreshFileList);

            //TabControl
            tabCtr_sendRecv = new MetroTabControl();
            tabCtr_sendRecv.Size = new Size(397, 306);
            tabCtr_sendRecv.TabPages.Add("接收");
            tabCtr_sendRecv.TabPages.Add("发送");
            tabCtr_sendRecv.TabPages[0].Controls.Add(listView_fileRecv);
            tabCtr_sendRecv.TabPages[1].Controls.Add(listView_fileSend);
            tabCtr_sendRecv.Location = new Point(24, 390);
            tabCtr_sendRecv.SelectedIndexChanged += new EventHandler(tabCtr_sendRecv_SelectedIndexChanged);
            this.Controls.Add(tabCtr_sendRecv);

        }

        private void hostList_Init()
        {
            this.listView_hostList.Columns.Add("主机名");
            this.listView_hostList.Columns.Add("IP地址");
            this.listView_hostList.Columns[0].Width = 130;
            this.listView_hostList.Columns[1].Width = 130;
            this.listView_hostList.View = View.Details;
            this.listView_hostList.MultiSelect = false;
        }

        private void fileRecv_Init()
        {
            this.listView_fileRecv.Name = "listView_fileRecv";
            this.listView_fileRecv.Columns.Add("已接收文件名");
            this.listView_fileRecv.Columns.Add("文件路径");
            this.listView_fileRecv.Columns[0].Width = 120;
            this.listView_fileRecv.Columns[1].Width = 265;
            this.listView_fileRecv.View = View.Details;
        }

        private void fileSend_Init()
        {
            this.listView_fileSend.Columns.Add("待发送文件名");
            this.listView_fileSend.Columns.Add("文件路径");
            this.listView_fileSend.Columns[0].Width = 120;
            this.listView_fileSend.Columns[1].Width = 265;
            this.listView_fileSend.View = View.Details;
        }

        #endregion

        #region 各种事件处理函数，如Esc退出、鼠标拖动窗口

        /// <summary>
        /// 刷新hostList按钮点击事件的响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_refreshHostList_Clicked(object sender, EventArgs e)
        {
            if (listView_hostList.Items != null)
            {
                listView_hostList.Items.Clear();
            }

            LanEnum myLanEnum = new LanEnum();
            List<ListViewItem> result = myLanEnum.getResult();

            this.listView_hostList.BeginUpdate();

            if (result != null)
            {
                foreach (ListViewItem p in result)
                {
                    listView_hostList.Items.Add(p);
                }
            }

            this.listView_hostList.EndUpdate();
        }

        /// <summary>
        /// 刷新已接收文件列表按钮响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_refreshFileList_Click(object sender,EventArgs e)
        {
            //MetroMessageBox.Show(this,"刷新失败！","失败了呀");    
            var tmp = myServer.getDict();
            foreach (var p in tmp)
            {
                ListViewItem temp = new ListViewItem();
                temp.Text = p.Key;
                temp.SubItems.Add(p.Value);
                this.listView_fileRecv.Items.Add(temp);
            }
        }

        /// <summary>
        /// 发送文件按钮的响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_sendFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (filePaths != null)
                {
                    Thread myThread = new Thread(threadSend);
                    if (this.listView_hostList.SelectedItems.Count != 0)
                    {
                        this.iptoSend = this.listView_hostList.SelectedItems[0].SubItems[1].Text.ToString();
                        myThread.Start();
                    }
                    else
                    {
                        MetroMessageBox.Show(this, "请选择接收文件的主机地址名称！", "错误");
                    }
                }
                else
                {
                    MetroMessageBox.Show(this, "请选择要发送的文件！", "错误");
                }
            }
            catch (Exception ee)
            {
                MetroMessageBox.Show(this,"发送失败。错误信息："+ee.Message.ToString(),"失败了耶");
            }
        }

        private void threadSend()
        {
            for (int i = 0; i < filePaths.Length; i++)
            {
                Client myClient = new Client(this.iptoSend, 9100);
                var cnn = myClient.Connect();
                if (cnn == true)
                {
                    //连接成功
                    myClient.sendName(fileNames[i]);
                    myClient.sendFile(filePaths[i]);
                    MetroMessageBox.Show(this,"连接成功！","成功啦");
                }
                else
                {
                    //连接失败
                    MetroMessageBox.Show(this,"连接失败！请确认接收主机是否已开启！","失败了");
                }
            }
        }

        /// <summary>
        /// 浏览文件按钮响应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_exploreFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog(); 
            dialog.Filter = "所有文件(*.*)|*.*";
            dialog.Multiselect = true;
            DialogResult result = dialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string[] names = dialog.FileNames;
                string[] namesWithoutDirectory = dialog.SafeFileNames;
                //初始化一下两个字符串数组，之所以用names来初始化是为了保持大小一致

                filePaths = names;
                fileNames = namesWithoutDirectory;
                int j = 0;
                foreach (string name in names)
                {
                    FileInfo myFI = new FileInfo(name);
                    
                    //添加到列表中显示出来
                    ListViewItem tmp = new ListViewItem();
                    tmp.Text = myFI.Name;
                    tmp.SubItems.Add(myFI.DirectoryName);                 
                    this.listView_fileSend.Items.Add(tmp);                    
                }
            }
        }

        /// <summary>
        /// 按键事件的响应函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    this.quit();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 统一为三个list注册按键时间响应函数
        /// </summary>
        private void mainForm_addEventHandlers()
        {
            this.listView_hostList.KeyDown += new KeyEventHandler(this.mainForm_KeyDown);
            this.listView_fileRecv.KeyDown += new KeyEventHandler(this.mainForm_KeyDown);
            this.listView_fileSend.KeyDown += new KeyEventHandler(this.mainForm_KeyDown);
        }

        /// <summary>
        /// 控制发送按钮的显示，只有当tabcontrol切换成第二个标签页时才显示该按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tabCtr_sendRecv_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabCtr_sendRecv.SelectedIndex)
            {
                case 0:
                    button_SendFile.Visible = false;
                    button_refreshFileList.Visible = true;
                    button_exploreFile.Visible = false;
                    break;
                case 1:
                    button_SendFile.Visible = true;
                    button_refreshFileList.Visible = false;
                    button_exploreFile.Visible = true;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 统一的关闭函数
        /// </summary>
        private void quit()
        {
            this.Close();
        }

        #endregion

    }
}
