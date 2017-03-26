namespace HID
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.imageListBigIcon = new System.Windows.Forms.ImageList(this.components);
            this.imageListSmallIcon = new System.Windows.Forms.ImageList(this.components);
            this.Open = new System.Windows.Forms.Button();
            this.SetTrue = new System.Windows.Forms.Button();
            this.SetFalse = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.hidTreeView = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(353, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 12);
            this.label3.TabIndex = 10;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(341, 41);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 11;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // imageListBigIcon
            // 
            this.imageListBigIcon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListBigIcon.ImageStream")));
            this.imageListBigIcon.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListBigIcon.Images.SetKeyName(0, "flag-at.ico");
            this.imageListBigIcon.Images.SetKeyName(1, "flag-ad.ico");
            this.imageListBigIcon.Images.SetKeyName(2, "flag-ae.ico");
            this.imageListBigIcon.Images.SetKeyName(3, "flag-af.ico");
            this.imageListBigIcon.Images.SetKeyName(4, "flag-ag.ico");
            this.imageListBigIcon.Images.SetKeyName(5, "flag-ai.ico");
            this.imageListBigIcon.Images.SetKeyName(6, "flag-al.ico");
            this.imageListBigIcon.Images.SetKeyName(7, "flag-alderney.ico");
            this.imageListBigIcon.Images.SetKeyName(8, "flag-am.ico");
            this.imageListBigIcon.Images.SetKeyName(9, "flag-an.ico");
            this.imageListBigIcon.Images.SetKeyName(10, "flag-ao.ico");
            this.imageListBigIcon.Images.SetKeyName(11, "flag-aq.ico");
            this.imageListBigIcon.Images.SetKeyName(12, "flag-ar.ico");
            this.imageListBigIcon.Images.SetKeyName(13, "flag-as.ico");
            // 
            // imageListSmallIcon
            // 
            this.imageListSmallIcon.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListSmallIcon.ImageStream")));
            this.imageListSmallIcon.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListSmallIcon.Images.SetKeyName(0, "flag-az.ico");
            this.imageListSmallIcon.Images.SetKeyName(1, "flag-ad.ico");
            this.imageListSmallIcon.Images.SetKeyName(2, "flag-ae.ico");
            this.imageListSmallIcon.Images.SetKeyName(3, "flag-af.ico");
            this.imageListSmallIcon.Images.SetKeyName(4, "flag-ag.ico");
            this.imageListSmallIcon.Images.SetKeyName(5, "flag-ai.ico");
            this.imageListSmallIcon.Images.SetKeyName(6, "flag-al.ico");
            this.imageListSmallIcon.Images.SetKeyName(7, "flag-am.ico");
            this.imageListSmallIcon.Images.SetKeyName(8, "flag-an.ico");
            this.imageListSmallIcon.Images.SetKeyName(9, "flag-ao.ico");
            this.imageListSmallIcon.Images.SetKeyName(10, "flag-ar.ico");
            this.imageListSmallIcon.Images.SetKeyName(11, "flag-as.ico");
            this.imageListSmallIcon.Images.SetKeyName(12, "flag-at.ico");
            this.imageListSmallIcon.Images.SetKeyName(13, "flag-au.ico");
            // 
            // Open
            // 
            this.Open.Location = new System.Drawing.Point(341, 12);
            this.Open.Name = "Open";
            this.Open.Size = new System.Drawing.Size(75, 23);
            this.Open.TabIndex = 13;
            this.Open.Text = "&Open";
            this.Open.UseVisualStyleBackColor = true;
            this.Open.Click += new System.EventHandler(this.Open_Click);
            // 
            // SetTrue
            // 
            this.SetTrue.Location = new System.Drawing.Point(341, 123);
            this.SetTrue.Name = "SetTrue";
            this.SetTrue.Size = new System.Drawing.Size(75, 23);
            this.SetTrue.TabIndex = 14;
            this.SetTrue.Text = "Set True";
            this.SetTrue.UseVisualStyleBackColor = true;
            this.SetTrue.Click += new System.EventHandler(this.SetTrue_Click);
            // 
            // SetFalse
            // 
            this.SetFalse.Location = new System.Drawing.Point(341, 94);
            this.SetFalse.Name = "SetFalse";
            this.SetFalse.Size = new System.Drawing.Size(75, 23);
            this.SetFalse.TabIndex = 15;
            this.SetFalse.Text = "Set False";
            this.SetFalse.UseVisualStyleBackColor = true;
            this.SetFalse.Click += new System.EventHandler(this.SetFalse_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6,
            this.Column7,
            this.Column8});
            this.dataGridView1.Location = new System.Drawing.Point(206, 269);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 27;
            this.dataGridView1.Size = new System.Drawing.Size(73, 75);
            this.dataGridView1.TabIndex = 16;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "BO";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 42;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "B1";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 42;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "B2";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 42;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "B3";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 42;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "B4";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 42;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "B5";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 42;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "B6";
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 42;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "B7";
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 42;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial Unicode MS", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBox1.Location = new System.Drawing.Point(451, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(302, 444);
            this.textBox1.TabIndex = 17;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(374, 379);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(64, 20);
            this.button2.TabIndex = 18;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // hidTreeView
            // 
            this.hidTreeView.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.hidTreeView.Location = new System.Drawing.Point(0, 0);
            this.hidTreeView.Name = "hidTreeView";
            this.hidTreeView.Size = new System.Drawing.Size(175, 444);
            this.hidTreeView.TabIndex = 19;
            this.hidTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.hidTreeView_AfterSelect);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(751, 445);
            this.Controls.Add(this.hidTreeView);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.SetFalse);
            this.Controls.Add(this.SetTrue);
            this.Controls.Add(this.Open);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ImageList imageListBigIcon;
        private System.Windows.Forms.ImageList imageListSmallIcon;
        private System.Windows.Forms.Button Open;
        private System.Windows.Forms.Button SetTrue;
        private System.Windows.Forms.Button SetFalse;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TreeView hidTreeView;
    }
}

