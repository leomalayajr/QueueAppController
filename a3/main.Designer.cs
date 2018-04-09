namespace a3
{
    partial class main
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.toggleSync = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lblServicingOffice = new System.Windows.Forms.Label();
            this.lblMobileQ = new System.Windows.Forms.Label();
            this.btnShowQ = new System.Windows.Forms.Button();
            this.txtLocalStatus = new System.Windows.Forms.TextBox();
            this.txtOnlineStatus = new System.Windows.Forms.TextBox();
            this.lbllocalDBStatus = new System.Windows.Forms.Label();
            this.lblonlineDBStatus = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.button6 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.panel1.Controls.Add(this.toggleSync);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.lblServicingOffice);
            this.panel1.Controls.Add(this.lblMobileQ);
            this.panel1.Controls.Add(this.btnShowQ);
            this.panel1.Controls.Add(this.txtLocalStatus);
            this.panel1.Controls.Add(this.txtOnlineStatus);
            this.panel1.Controls.Add(this.lbllocalDBStatus);
            this.panel1.Controls.Add(this.lblonlineDBStatus);
            this.panel1.Location = new System.Drawing.Point(15, 15);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(513, 402);
            this.panel1.TabIndex = 0;
            // 
            // toggleSync
            // 
            this.toggleSync.BackColor = System.Drawing.Color.Gray;
            this.toggleSync.FlatAppearance.BorderSize = 0;
            this.toggleSync.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.toggleSync.Font = new System.Drawing.Font("Segoe UI Semilight", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toggleSync.ForeColor = System.Drawing.Color.White;
            this.toggleSync.Location = new System.Drawing.Point(335, 172);
            this.toggleSync.Margin = new System.Windows.Forms.Padding(4);
            this.toggleSync.Name = "toggleSync";
            this.toggleSync.Size = new System.Drawing.Size(151, 39);
            this.toggleSync.TabIndex = 25;
            this.toggleSync.Text = "OFF";
            this.toggleSync.UseVisualStyleBackColor = false;
            this.toggleSync.Click += new System.EventHandler(this.toggleSync_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(41)))), ((int)(((byte)(42)))));
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Segoe UI Semilight", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.ForeColor = System.Drawing.Color.White;
            this.button2.Location = new System.Drawing.Point(335, 232);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(151, 39);
            this.button2.TabIndex = 24;
            this.button2.Text = "Show Info";
            this.button2.UseVisualStyleBackColor = false;
            // 
            // lblServicingOffice
            // 
            this.lblServicingOffice.AutoSize = true;
            this.lblServicingOffice.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServicingOffice.ForeColor = System.Drawing.Color.White;
            this.lblServicingOffice.Location = new System.Drawing.Point(23, 232);
            this.lblServicingOffice.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblServicingOffice.Name = "lblServicingOffice";
            this.lblServicingOffice.Size = new System.Drawing.Size(145, 24);
            this.lblServicingOffice.TabIndex = 16;
            this.lblServicingOffice.Text = "Servicing Offices";
            // 
            // lblMobileQ
            // 
            this.lblMobileQ.AutoSize = true;
            this.lblMobileQ.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMobileQ.ForeColor = System.Drawing.Color.White;
            this.lblMobileQ.Location = new System.Drawing.Point(23, 179);
            this.lblMobileQ.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMobileQ.Name = "lblMobileQ";
            this.lblMobileQ.Size = new System.Drawing.Size(150, 24);
            this.lblMobileQ.TabIndex = 15;
            this.lblMobileQ.Text = "Mobile App Sync";
            // 
            // btnShowQ
            // 
            this.btnShowQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(41)))), ((int)(((byte)(42)))));
            this.btnShowQ.FlatAppearance.BorderSize = 0;
            this.btnShowQ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowQ.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowQ.ForeColor = System.Drawing.Color.White;
            this.btnShowQ.Location = new System.Drawing.Point(27, 107);
            this.btnShowQ.Margin = new System.Windows.Forms.Padding(4);
            this.btnShowQ.Name = "btnShowQ";
            this.btnShowQ.Size = new System.Drawing.Size(459, 39);
            this.btnShowQ.TabIndex = 14;
            this.btnShowQ.Text = "System Settings";
            this.btnShowQ.UseVisualStyleBackColor = false;
            // 
            // txtLocalStatus
            // 
            this.txtLocalStatus.BackColor = System.Drawing.Color.White;
            this.txtLocalStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLocalStatus.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocalStatus.ForeColor = System.Drawing.Color.Black;
            this.txtLocalStatus.Location = new System.Drawing.Point(305, 62);
            this.txtLocalStatus.Margin = new System.Windows.Forms.Padding(4);
            this.txtLocalStatus.Name = "txtLocalStatus";
            this.txtLocalStatus.Size = new System.Drawing.Size(181, 27);
            this.txtLocalStatus.TabIndex = 13;
            this.txtLocalStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtOnlineStatus
            // 
            this.txtOnlineStatus.BackColor = System.Drawing.Color.White;
            this.txtOnlineStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOnlineStatus.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOnlineStatus.ForeColor = System.Drawing.Color.Black;
            this.txtOnlineStatus.Location = new System.Drawing.Point(305, 13);
            this.txtOnlineStatus.Margin = new System.Windows.Forms.Padding(4);
            this.txtOnlineStatus.Name = "txtOnlineStatus";
            this.txtOnlineStatus.Size = new System.Drawing.Size(181, 27);
            this.txtOnlineStatus.TabIndex = 12;
            this.txtOnlineStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbllocalDBStatus
            // 
            this.lbllocalDBStatus.AutoSize = true;
            this.lbllocalDBStatus.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbllocalDBStatus.ForeColor = System.Drawing.Color.White;
            this.lbllocalDBStatus.Location = new System.Drawing.Point(21, 62);
            this.lbllocalDBStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbllocalDBStatus.Name = "lbllocalDBStatus";
            this.lbllocalDBStatus.Size = new System.Drawing.Size(144, 24);
            this.lbllocalDBStatus.TabIndex = 11;
            this.lbllocalDBStatus.Text = "Local DB Status:";
            // 
            // lblonlineDBStatus
            // 
            this.lblonlineDBStatus.AutoSize = true;
            this.lblonlineDBStatus.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblonlineDBStatus.ForeColor = System.Drawing.Color.White;
            this.lblonlineDBStatus.Location = new System.Drawing.Point(21, 17);
            this.lblonlineDBStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblonlineDBStatus.Name = "lblonlineDBStatus";
            this.lblonlineDBStatus.Size = new System.Drawing.Size(158, 24);
            this.lblonlineDBStatus.TabIndex = 10;
            this.lblonlineDBStatus.Text = "Online DB Status:";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(41)))), ((int)(((byte)(42)))));
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(172, 15);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(151, 39);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Location = new System.Drawing.Point(17, 425);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(511, 72);
            this.panel2.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.panel4.Controls.Add(this.txtLog);
            this.panel4.Controls.Add(this.button6);
            this.panel4.Location = new System.Drawing.Point(551, 15);
            this.panel4.Margin = new System.Windows.Forms.Padding(4);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(521, 482);
            this.panel4.TabIndex = 6;
            // 
            // txtLog
            // 
            this.txtLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLog.Location = new System.Drawing.Point(22, 65);
            this.txtLog.Margin = new System.Windows.Forms.Padding(4);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ReadOnly = true;
            this.txtLog.Size = new System.Drawing.Size(476, 399);
            this.txtLog.TabIndex = 15;
            this.txtLog.WordWrap = false;
            // 
            // button6
            // 
            this.button6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(41)))), ((int)(((byte)(42)))));
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button6.ForeColor = System.Drawing.Color.White;
            this.button6.Location = new System.Drawing.Point(22, 9);
            this.button6.Margin = new System.Windows.Forms.Padding(4);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(476, 39);
            this.button6.TabIndex = 14;
            this.button6.Text = "System Logs";
            this.button6.UseVisualStyleBackColor = false;
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(41)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(1085, 510);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "main";
            this.Text = "main";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label lbllocalDBStatus;
        internal System.Windows.Forms.Label lblonlineDBStatus;
        internal System.Windows.Forms.TextBox txtLocalStatus;
        internal System.Windows.Forms.TextBox txtOnlineStatus;
        internal System.Windows.Forms.Label lblServicingOffice;
        internal System.Windows.Forms.Label lblMobileQ;
        internal System.Windows.Forms.Button btnShowQ;
        internal System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel4;
        internal System.Windows.Forms.Button button6;
        internal System.Windows.Forms.Button toggleSync;
        private System.Windows.Forms.TextBox txtLog;
    }
}