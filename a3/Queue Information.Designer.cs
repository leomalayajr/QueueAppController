namespace a3
{
    partial class Queue_Information
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
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            this.ListView1 = new System.Windows.Forms.ListView();
            this.ColumnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ColumnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSyncStatus = new System.Windows.Forms.TextBox();
            this.txtLastSync = new System.Windows.Forms.TextBox();
            this.lblLastSync = new System.Windows.Forms.Label();
            this.lblSyncStatus = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnExit = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // ListView1
            // 
            this.ListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ColumnHeader1,
            this.ColumnHeader2,
            this.ColumnHeader3,
            this.columnHeader5});
            this.ListView1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListView1.FullRowSelect = true;
            listViewGroup2.Header = "ListViewGroup";
            listViewGroup2.Name = "listViewGroup1";
            listViewGroup2.Tag = "sdfdsf";
            this.ListView1.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup2});
            this.ListView1.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.ListView1.Location = new System.Drawing.Point(6, 4);
            this.ListView1.Margin = new System.Windows.Forms.Padding(4);
            this.ListView1.Name = "ListView1";
            this.ListView1.ShowGroups = false;
            this.ListView1.Size = new System.Drawing.Size(683, 522);
            this.ListView1.TabIndex = 18;
            this.ListView1.UseCompatibleStateImageBehavior = false;
            this.ListView1.View = System.Windows.Forms.View.Details;
            // 
            // ColumnHeader1
            // 
            this.ColumnHeader1.Text = "Servicing Office";
            this.ColumnHeader1.Width = 200;
            // 
            // ColumnHeader2
            // 
            this.ColumnHeader2.Text = "Window #";
            this.ColumnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader2.Width = 160;
            // 
            // ColumnHeader3
            // 
            this.ColumnHeader3.Text = "Now Serving";
            this.ColumnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ColumnHeader3.Width = 131;
            // 
            // columnHeader5
            // 
            this.columnHeader5.Text = "Avg. Serving Time";
            this.columnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.columnHeader5.Width = 180;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.panel1.Controls.Add(this.txtSyncStatus);
            this.panel1.Controls.Add(this.txtLastSync);
            this.panel1.Controls.Add(this.lblLastSync);
            this.panel1.Controls.Add(this.lblSyncStatus);
            this.panel1.Controls.Add(this.ListView1);
            this.panel1.Location = new System.Drawing.Point(7, 11);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(693, 642);
            this.panel1.TabIndex = 19;
            // 
            // txtSyncStatus
            // 
            this.txtSyncStatus.BackColor = System.Drawing.Color.White;
            this.txtSyncStatus.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSyncStatus.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSyncStatus.ForeColor = System.Drawing.Color.Black;
            this.txtSyncStatus.Location = new System.Drawing.Point(196, 555);
            this.txtSyncStatus.Margin = new System.Windows.Forms.Padding(4);
            this.txtSyncStatus.Name = "txtSyncStatus";
            this.txtSyncStatus.Size = new System.Drawing.Size(293, 27);
            this.txtSyncStatus.TabIndex = 22;
            this.txtSyncStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtLastSync
            // 
            this.txtLastSync.BackColor = System.Drawing.Color.White;
            this.txtLastSync.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtLastSync.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLastSync.ForeColor = System.Drawing.Color.Black;
            this.txtLastSync.Location = new System.Drawing.Point(196, 594);
            this.txtLastSync.Margin = new System.Windows.Forms.Padding(4);
            this.txtLastSync.Name = "txtLastSync";
            this.txtLastSync.Size = new System.Drawing.Size(293, 27);
            this.txtLastSync.TabIndex = 21;
            this.txtLastSync.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lblLastSync
            // 
            this.lblLastSync.AutoSize = true;
            this.lblLastSync.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastSync.ForeColor = System.Drawing.Color.White;
            this.lblLastSync.Location = new System.Drawing.Point(9, 597);
            this.lblLastSync.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLastSync.Name = "lblLastSync";
            this.lblLastSync.Size = new System.Drawing.Size(91, 24);
            this.lblLastSync.TabIndex = 20;
            this.lblLastSync.Text = "Last Sync:";
            // 
            // lblSyncStatus
            // 
            this.lblSyncStatus.AutoSize = true;
            this.lblSyncStatus.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSyncStatus.ForeColor = System.Drawing.Color.White;
            this.lblSyncStatus.Location = new System.Drawing.Point(9, 558);
            this.lblSyncStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSyncStatus.Name = "lblSyncStatus";
            this.lblSyncStatus.Size = new System.Drawing.Size(161, 24);
            this.lblSyncStatus.TabIndex = 19;
            this.lblSyncStatus.Text = "Show Sync Status:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(41)))), ((int)(((byte)(50)))));
            this.panel2.Controls.Add(this.btnExit);
            this.panel2.Location = new System.Drawing.Point(7, 661);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(816, 62);
            this.panel2.TabIndex = 20;
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(41)))), ((int)(((byte)(42)))));
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.White;
            this.btnExit.Location = new System.Drawing.Point(332, 11);
            this.btnExit.Margin = new System.Windows.Forms.Padding(4);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(151, 39);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = false;
            // 
            // Queue_Information
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(181)))), ((int)(((byte)(41)))), ((int)(((byte)(42)))));
            this.ClientSize = new System.Drawing.Size(713, 730);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Queue_Information";
            this.Text = "Queue_Information";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.ListView ListView1;
        internal System.Windows.Forms.ColumnHeader ColumnHeader1;
        internal System.Windows.Forms.ColumnHeader ColumnHeader2;
        internal System.Windows.Forms.ColumnHeader ColumnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Label lblLastSync;
        internal System.Windows.Forms.Label lblSyncStatus;
        internal System.Windows.Forms.TextBox txtSyncStatus;
        internal System.Windows.Forms.TextBox txtLastSync;
        private System.Windows.Forms.Panel panel2;
        internal System.Windows.Forms.Button btnExit;
    }
}