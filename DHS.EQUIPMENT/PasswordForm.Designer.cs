namespace DHS.EQUIPMENT
{
    partial class PasswordForm
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
            this.radpnl_processinfo = new Telerik.WinControls.UI.RadPanel();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.radBtnCancel = new Telerik.WinControls.UI.RadButton();
            this.radBtnOk = new Telerik.WinControls.UI.RadButton();
            this.lblMsg = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_processinfo)).BeginInit();
            this.radpnl_processinfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnOk)).BeginInit();
            this.SuspendLayout();
            // 
            // radpnl_processinfo
            // 
            this.radpnl_processinfo.Controls.Add(this.lblMsg);
            this.radpnl_processinfo.Controls.Add(this.radBtnCancel);
            this.radpnl_processinfo.Controls.Add(this.tbPassword);
            this.radpnl_processinfo.Controls.Add(this.radBtnOk);
            this.radpnl_processinfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radpnl_processinfo.Location = new System.Drawing.Point(3, 3);
            this.radpnl_processinfo.Name = "radpnl_processinfo";
            this.radpnl_processinfo.Padding = new System.Windows.Forms.Padding(5);
            this.radpnl_processinfo.Size = new System.Drawing.Size(378, 139);
            this.radpnl_processinfo.TabIndex = 11;
            this.radpnl_processinfo.ThemeName = "ControlDefault";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_processinfo.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_processinfo.GetChildAt(0))).BorderHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(14)))), ((int)(((byte)(248)))));
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_processinfo.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(5);
            // 
            // tbPassword
            // 
            this.tbPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbPassword.Font = new System.Drawing.Font("Times New Roman", 16F);
            this.tbPassword.Location = new System.Drawing.Point(21, 47);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(340, 32);
            this.tbPassword.TabIndex = 70;
            this.tbPassword.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tbPassword.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPassword_KeyPress);
            // 
            // radBtnCancel
            // 
            this.radBtnCancel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.radBtnCancel.ForeColor = System.Drawing.Color.Black;
            this.radBtnCancel.Location = new System.Drawing.Point(211, 95);
            this.radBtnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.radBtnCancel.Name = "radBtnCancel";
            this.radBtnCancel.Size = new System.Drawing.Size(100, 32);
            this.radBtnCancel.TabIndex = 74;
            this.radBtnCancel.Text = "CANCEL";
            this.radBtnCancel.ThemeName = "ControlDefault";
            this.radBtnCancel.Click += new System.EventHandler(this.radBtnCancel_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnCancel.GetChildAt(0))).Text = "CANCEL";
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnCancel.GetChildAt(0))).Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnCancel.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(241)))), ((int)(((byte)(252)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnCancel.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(226)))), ((int)(((byte)(244)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnCancel.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(226)))), ((int)(((byte)(244)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnCancel.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.White;
            // 
            // radBtnOk
            // 
            this.radBtnOk.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.radBtnOk.ForeColor = System.Drawing.Color.Black;
            this.radBtnOk.Location = new System.Drawing.Point(71, 95);
            this.radBtnOk.Margin = new System.Windows.Forms.Padding(4);
            this.radBtnOk.Name = "radBtnOk";
            this.radBtnOk.Size = new System.Drawing.Size(100, 32);
            this.radBtnOk.TabIndex = 73;
            this.radBtnOk.Text = "OK";
            this.radBtnOk.ThemeName = "ControlDefault";
            this.radBtnOk.Click += new System.EventHandler(this.radBtnOk_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnOk.GetChildAt(0))).Text = "OK";
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnOk.GetChildAt(0))).Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnOk.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(241)))), ((int)(((byte)(252)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnOk.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(226)))), ((int)(((byte)(244)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnOk.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(226)))), ((int)(((byte)(244)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnOk.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.White;
            // 
            // lblMsg
            // 
            this.lblMsg.AutoSize = true;
            this.lblMsg.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMsg.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMsg.Location = new System.Drawing.Point(18, 15);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(175, 16);
            this.lblMsg.TabIndex = 75;
            this.lblMsg.Text = "Please insert password ";
            // 
            // PasswordForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(384, 145);
            this.Controls.Add(this.radpnl_processinfo);
            this.Name = "PasswordForm";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "Password";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PasswordForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_processinfo)).EndInit();
            this.radpnl_processinfo.ResumeLayout(false);
            this.radpnl_processinfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnOk)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radpnl_processinfo;
        private System.Windows.Forms.TextBox tbPassword;
        private Telerik.WinControls.UI.RadButton radBtnCancel;
        private Telerik.WinControls.UI.RadButton radBtnOk;
        private System.Windows.Forms.Label lblMsg;
    }
}