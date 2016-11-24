namespace tcpFileTrans
{
    partial class mainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.listView_hostList = new System.Windows.Forms.ListView();
            this.listView_fileRecv = new System.Windows.Forms.ListView();
            this.listView_fileSend = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // listView_hostList
            // 
            this.listView_hostList.Location = new System.Drawing.Point(24, 60);
            this.listView_hostList.Margin = new System.Windows.Forms.Padding(4);
            this.listView_hostList.Name = "listView_hostList";
            this.listView_hostList.Size = new System.Drawing.Size(264, 306);
            this.listView_hostList.TabIndex = 0;
            this.listView_hostList.UseCompatibleStateImageBehavior = false;
            // 
            // listView_fileRecv
            // 
            this.listView_fileRecv.Location = new System.Drawing.Point(0, 0);
            this.listView_fileRecv.Name = "listView_fileRecv";
            this.listView_fileRecv.Size = new System.Drawing.Size(389, 264);
            this.listView_fileRecv.TabIndex = 1;
            this.listView_fileRecv.UseCompatibleStateImageBehavior = false;
            // 
            // listView_fileSend
            // 
            this.listView_fileSend.Location = new System.Drawing.Point(0, 0);
            this.listView_fileSend.Name = "listView_fileSend";
            this.listView_fileSend.Size = new System.Drawing.Size(389, 264);
            this.listView_fileSend.TabIndex = 1;
            this.listView_fileSend.UseCompatibleStateImageBehavior = false;
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(445, 720);
            this.Controls.Add(this.listView_hostList);
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "mainForm";
            this.Padding = new System.Windows.Forms.Padding(23, 85, 23, 28);
            this.Resizable = false;
            this.Text = "文件传输";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.mainForm_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.mainForm_KeyDown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listView_hostList;
        private System.Windows.Forms.ListView listView_fileRecv;
        private System.Windows.Forms.ListView listView_fileSend;


    }
}

