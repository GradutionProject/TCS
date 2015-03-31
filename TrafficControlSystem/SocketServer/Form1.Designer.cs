namespace SocketServer
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tbLog = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPort = new System.Windows.Forms.TextBox();
            this.pbTemp = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTemp = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(6, 19);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(414, 154);
            this.tbLog.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbLog);
            this.groupBox1.Location = new System.Drawing.Point(12, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(426, 179);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Log";
            // 
            // btStart
            // 
            this.btStart.Location = new System.Drawing.Point(328, 12);
            this.btStart.Name = "btStart";
            this.btStart.Size = new System.Drawing.Size(110, 23);
            this.btStart.TabIndex = 2;
            this.btStart.Text = "Start listening";
            this.btStart.UseVisualStyleBackColor = true;
            this.btStart.Click += new System.EventHandler(this.btStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(232, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Port";
            // 
            // tbPort
            // 
            this.tbPort.Location = new System.Drawing.Point(264, 14);
            this.tbPort.Name = "tbPort";
            this.tbPort.Size = new System.Drawing.Size(52, 20);
            this.tbPort.TabIndex = 4;
            this.tbPort.Text = "1000";
            // 
            // pbTemp
            // 
            this.pbTemp.Location = new System.Drawing.Point(173, 49);
            this.pbTemp.Maximum = 300;
            this.pbTemp.Name = "pbTemp";
            this.pbTemp.Size = new System.Drawing.Size(265, 23);
            this.pbTemp.Step = 1;
            this.pbTemp.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbTemp.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Room temperature";
            // 
            // tbTemp
            // 
            this.tbTemp.Location = new System.Drawing.Point(115, 52);
            this.tbTemp.Name = "tbTemp";
            this.tbTemp.ReadOnly = true;
            this.tbTemp.Size = new System.Drawing.Size(52, 20);
            this.tbTemp.TabIndex = 7;
            this.tbTemp.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 262);
            this.Controls.Add(this.tbTemp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pbTemp);
            this.Controls.Add(this.tbPort);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btStart);
            this.Controls.Add(this.groupBox1);
            this.Name = "Form1";
            this.Text = "SocketServer";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPort;
        private System.Windows.Forms.ProgressBar pbTemp;
        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.TextBox tbTemp;
    }
}

