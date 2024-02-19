
namespace DHS.EQUIPMENT
{
    partial class ErrorForm
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblCheckMsg = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblErrMsg = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(684, 80);
            this.panel1.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblTitle.ForeColor = System.Drawing.Color.Yellow;
            this.lblTitle.Location = new System.Drawing.Point(10, 10);
            this.lblTitle.Margin = new System.Windows.Forms.Padding(5);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(5);
            this.lblTitle.Size = new System.Drawing.Size(664, 60);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Title";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lblCheckMsg);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.lblErrMsg);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 80);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10, 60, 10, 10);
            this.panel2.Size = new System.Drawing.Size(684, 281);
            this.panel2.TabIndex = 3;
            // 
            // lblCheckMsg
            // 
            this.lblCheckMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblCheckMsg.ForeColor = System.Drawing.Color.Red;
            this.lblCheckMsg.Location = new System.Drawing.Point(10, 120);
            this.lblCheckMsg.Margin = new System.Windows.Forms.Padding(10);
            this.lblCheckMsg.Name = "lblCheckMsg";
            this.lblCheckMsg.Padding = new System.Windows.Forms.Padding(5);
            this.lblCheckMsg.Size = new System.Drawing.Size(663, 40);
            this.lblCheckMsg.TabIndex = 4;
            this.lblCheckMsg.Text = " Please check the error.";
            this.lblCheckMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnOK);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(10, 191);
            this.panel3.Name = "panel3";
            this.panel3.Padding = new System.Windows.Forms.Padding(233, 15, 233, 15);
            this.panel3.Size = new System.Drawing.Size(664, 80);
            this.panel3.TabIndex = 3;
            // 
            // btnOK
            // 
            this.btnOK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.btnOK.Location = new System.Drawing.Point(233, 15);
            this.btnOK.Margin = new System.Windows.Forms.Padding(5);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(198, 50);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblErrMsg
            // 
            this.lblErrMsg.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.lblErrMsg.ForeColor = System.Drawing.Color.Red;
            this.lblErrMsg.Location = new System.Drawing.Point(10, 50);
            this.lblErrMsg.Margin = new System.Windows.Forms.Padding(10);
            this.lblErrMsg.Name = "lblErrMsg";
            this.lblErrMsg.Padding = new System.Windows.Forms.Padding(5);
            this.lblErrMsg.Size = new System.Drawing.Size(663, 40);
            this.lblErrMsg.TabIndex = 0;
            this.lblErrMsg.Text = "Error Message";
            this.lblErrMsg.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ErrorForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(684, 361);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ErrorForm";
            this.Text = "ErrorForm";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblCheckMsg;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblErrMsg;
    }
}