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
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.lblonlineSync = new System.Windows.Forms.Label();
            this.lblServicingOffice = new System.Windows.Forms.Label();
            this.lblMobileQ = new System.Windows.Forms.Label();
            this.btnShowQ = new System.Windows.Forms.Button();
            this.txtLocalStatus = new System.Windows.Forms.TextBox();
            this.txtOnlineStatus = new System.Windows.Forms.TextBox();
            this.lbllocalDBStatus = new System.Windows.Forms.Label();
            this.lblonlineDBStatus = new System.Windows.Forms.Label();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.comboBox2);
            this.panel1.Controls.Add(this.lblonlineSync);
            this.panel1.Controls.Add(this.lblServicingOffice);
            this.panel1.Controls.Add(this.lblMobileQ);
            this.panel1.Controls.Add(this.btnShowQ);
            this.panel1.Controls.Add(this.txtLocalStatus);
            this.panel1.Controls.Add(this.txtOnlineStatus);
            this.panel1.Controls.Add(this.lbllocalDBStatus);
            this.panel1.Controls.Add(this.lblonlineDBStatus);
            this.panel1.Location = new System.Drawing.Point(11, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(344, 286);
            this.panel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(101, 248);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 19);
            this.label1.TabIndex = 23;
            this.label1.Text = "Sync Every";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Segoe UI Semilight", 10F);
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Cashier",
            "Registrar",
            "UGTO"});
            this.comboBox1.Location = new System.Drawing.Point(134, 175);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(148, 25);
            this.comboBox1.TabIndex = 22;
            // 
            // checkBox2
            // 
            this.checkBox2.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox2.AutoSize = true;
            this.checkBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.checkBox2.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
            this.checkBox2.ForeColor = System.Drawing.Color.White;
            this.checkBox2.Location = new System.Drawing.Point(290, 173);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(38, 28);
            this.checkBox2.TabIndex = 21;
            this.checkBox2.Text = "ON";
            this.checkBox2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox2.UseVisualStyleBackColor = false;
            // 
            // checkBox1
            // 
            this.checkBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.checkBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.checkBox1.Font = new System.Drawing.Font("Calibri", 11F, System.Drawing.FontStyle.Bold);
            this.checkBox1.ForeColor = System.Drawing.Color.White;
            this.checkBox1.Location = new System.Drawing.Point(290, 139);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(38, 28);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "ON";
            this.checkBox1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBox1.UseVisualStyleBackColor = false;
            // 
            // comboBox2
            // 
            this.comboBox2.Font = new System.Drawing.Font("Segoe UI Semilight", 10F);
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "15 Seconds",
            "30 Seconds",
            "60 Seconds",
            "2 Minutes",
            "5 Minutes"});
            this.comboBox2.Location = new System.Drawing.Point(187, 246);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(141, 25);
            this.comboBox2.TabIndex = 19;
            // 
            // lblonlineSync
            // 
            this.lblonlineSync.AutoSize = true;
            this.lblonlineSync.Cursor = System.Windows.Forms.Cursors.Default;
            this.lblonlineSync.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblonlineSync.ForeColor = System.Drawing.Color.White;
            this.lblonlineSync.Location = new System.Drawing.Point(21, 220);
            this.lblonlineSync.Name = "lblonlineSync";
            this.lblonlineSync.Size = new System.Drawing.Size(163, 19);
            this.lblonlineSync.TabIndex = 17;
            this.lblonlineSync.Text = "Online Syncronization:";
            // 
            // lblServicingOffice
            // 
            this.lblServicingOffice.AutoSize = true;
            this.lblServicingOffice.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServicingOffice.ForeColor = System.Drawing.Color.White;
            this.lblServicingOffice.Location = new System.Drawing.Point(16, 177);
            this.lblServicingOffice.Name = "lblServicingOffice";
            this.lblServicingOffice.Size = new System.Drawing.Size(114, 19);
            this.lblServicingOffice.TabIndex = 16;
            this.lblServicingOffice.Text = "Servicing Office";
            // 
            // lblMobileQ
            // 
            this.lblMobileQ.AutoSize = true;
            this.lblMobileQ.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMobileQ.ForeColor = System.Drawing.Color.White;
            this.lblMobileQ.Location = new System.Drawing.Point(16, 141);
            this.lblMobileQ.Name = "lblMobileQ";
            this.lblMobileQ.Size = new System.Drawing.Size(201, 19);
            this.lblMobileQ.TabIndex = 15;
            this.lblMobileQ.Text = "Mobile Queuing Application";
            // 
            // btnShowQ
            // 
            this.btnShowQ.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(41)))), ((int)(((byte)(42)))));
            this.btnShowQ.FlatAppearance.BorderSize = 0;
            this.btnShowQ.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowQ.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnShowQ.ForeColor = System.Drawing.Color.White;
            this.btnShowQ.Location = new System.Drawing.Point(20, 87);
            this.btnShowQ.Name = "btnShowQ";
            this.btnShowQ.Size = new System.Drawing.Size(308, 32);
            this.btnShowQ.TabIndex = 14;
            this.btnShowQ.Text = "Show Queue Information";
            this.btnShowQ.UseVisualStyleBackColor = false;
            // 
            // txtLocalStatus
            // 
            this.txtLocalStatus.BackColor = System.Drawing.Color.White;
            this.txtLocalStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLocalStatus.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocalStatus.ForeColor = System.Drawing.Color.Black;
            this.txtLocalStatus.Location = new System.Drawing.Point(192, 50);
            this.txtLocalStatus.Name = "txtLocalStatus";
            this.txtLocalStatus.Size = new System.Drawing.Size(136, 22);
            this.txtLocalStatus.TabIndex = 13;
            this.txtLocalStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtOnlineStatus
            // 
            this.txtOnlineStatus.BackColor = System.Drawing.Color.White;
            this.txtOnlineStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtOnlineStatus.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOnlineStatus.ForeColor = System.Drawing.Color.Black;
            this.txtOnlineStatus.Location = new System.Drawing.Point(192, 11);
            this.txtOnlineStatus.Name = "txtOnlineStatus";
            this.txtOnlineStatus.Size = new System.Drawing.Size(136, 22);
            this.txtOnlineStatus.TabIndex = 12;
            this.txtOnlineStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lbllocalDBStatus
            // 
            this.lbllocalDBStatus.AutoSize = true;
            this.lbllocalDBStatus.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbllocalDBStatus.ForeColor = System.Drawing.Color.White;
            this.lbllocalDBStatus.Location = new System.Drawing.Point(16, 50);
            this.lbllocalDBStatus.Name = "lbllocalDBStatus";
            this.lbllocalDBStatus.Size = new System.Drawing.Size(160, 19);
            this.lbllocalDBStatus.TabIndex = 11;
            this.lbllocalDBStatus.Text = "Show Local DB Status:";
            // 
            // lblonlineDBStatus
            // 
            this.lblonlineDBStatus.AutoSize = true;
            this.lblonlineDBStatus.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblonlineDBStatus.ForeColor = System.Drawing.Color.White;
            this.lblonlineDBStatus.Location = new System.Drawing.Point(16, 14);
            this.lblonlineDBStatus.Name = "lblonlineDBStatus";
            this.lblonlineDBStatus.Size = new System.Drawing.Size(170, 19);
            this.lblonlineDBStatus.TabIndex = 10;
            this.lblonlineDBStatus.Text = "Show Online DB Status:";
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(41)))), ((int)(((byte)(42)))));
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(116, 9);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(113, 32);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Location = new System.Drawing.Point(12, 304);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(343, 50);
            this.panel2.TabIndex = 1;
            // 
            // main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(41)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(362, 363);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "main";
            this.Text = "main";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label lbllocalDBStatus;
        internal System.Windows.Forms.Label lblonlineDBStatus;
        internal System.Windows.Forms.TextBox txtLocalStatus;
        internal System.Windows.Forms.TextBox txtOnlineStatus;
        internal System.Windows.Forms.Label lblonlineSync;
        internal System.Windows.Forms.Label lblServicingOffice;
        internal System.Windows.Forms.Label lblMobileQ;
        internal System.Windows.Forms.Button btnShowQ;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Panel panel2;
    }
}