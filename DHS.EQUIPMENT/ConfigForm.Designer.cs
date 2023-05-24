
namespace DHS.EQUIPMENT
{
    partial class ConfigForm
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
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.tbPlcDbNumber = new System.Windows.Forms.TextBox();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.tbPlcIpaddress = new System.Windows.Forms.TextBox();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.radpnl_processinfo = new Telerik.WinControls.UI.RadPanel();
            this.tbMesPort = new System.Windows.Forms.TextBox();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.tbMesIpaddress = new System.Windows.Forms.TextBox();
            this.radlblOffsetChannel = new Telerik.WinControls.UI.RadLabel();
            this.label12 = new System.Windows.Forms.Label();
            this.radBtnCancel = new Telerik.WinControls.UI.RadButton();
            this.radBtnSave = new Telerik.WinControls.UI.RadButton();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_processinfo)).BeginInit();
            this.radpnl_processinfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radlblOffsetChannel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnSave)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.tbPlcDbNumber);
            this.radPanel1.Controls.Add(this.radLabel2);
            this.radPanel1.Controls.Add(this.tbPlcIpaddress);
            this.radPanel1.Controls.Add(this.radLabel1);
            this.radPanel1.Controls.Add(this.label1);
            this.radPanel1.Location = new System.Drawing.Point(12, 158);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.radPanel1.Size = new System.Drawing.Size(368, 124);
            this.radPanel1.TabIndex = 13;
            this.radPanel1.ThemeName = "ControlDefault";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanel1.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanel1.GetChildAt(0))).BorderHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(14)))), ((int)(((byte)(248)))));
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanel1.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(5);
            // 
            // tbPlcDbNumber
            // 
            this.tbPlcDbNumber.Font = new System.Drawing.Font("Times New Roman", 16F);
            this.tbPlcDbNumber.Location = new System.Drawing.Point(200, 77);
            this.tbPlcDbNumber.Name = "tbPlcDbNumber";
            this.tbPlcDbNumber.Size = new System.Drawing.Size(157, 32);
            this.tbPlcDbNumber.TabIndex = 72;
            this.tbPlcDbNumber.Text = "85";
            this.tbPlcDbNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // radLabel2
            // 
            this.radLabel2.AutoSize = false;
            this.radLabel2.BackColor = System.Drawing.Color.LightGray;
            this.radLabel2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.radLabel2.Location = new System.Drawing.Point(66, 77);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(128, 32);
            this.radLabel2.TabIndex = 71;
            this.radLabel2.Text = "DB No.";
            this.radLabel2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbPlcIpaddress
            // 
            this.tbPlcIpaddress.Font = new System.Drawing.Font("Times New Roman", 16F);
            this.tbPlcIpaddress.Location = new System.Drawing.Point(200, 39);
            this.tbPlcIpaddress.Name = "tbPlcIpaddress";
            this.tbPlcIpaddress.Size = new System.Drawing.Size(157, 32);
            this.tbPlcIpaddress.TabIndex = 70;
            this.tbPlcIpaddress.Text = "192.168.10.1";
            this.tbPlcIpaddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // radLabel1
            // 
            this.radLabel1.AutoSize = false;
            this.radLabel1.BackColor = System.Drawing.Color.LightGray;
            this.radLabel1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.radLabel1.Location = new System.Drawing.Point(66, 39);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(128, 32);
            this.radLabel1.TabIndex = 69;
            this.radLabel1.Text = "IP ADDRESS";
            this.radLabel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 16);
            this.label1.TabIndex = 50;
            this.label1.Text = "PLC SETTING";
            // 
            // radpnl_processinfo
            // 
            this.radpnl_processinfo.Controls.Add(this.tbMesPort);
            this.radpnl_processinfo.Controls.Add(this.radLabel3);
            this.radpnl_processinfo.Controls.Add(this.tbMesIpaddress);
            this.radpnl_processinfo.Controls.Add(this.radlblOffsetChannel);
            this.radpnl_processinfo.Controls.Add(this.label12);
            this.radpnl_processinfo.Location = new System.Drawing.Point(12, 19);
            this.radpnl_processinfo.Name = "radpnl_processinfo";
            this.radpnl_processinfo.Padding = new System.Windows.Forms.Padding(5);
            this.radpnl_processinfo.Size = new System.Drawing.Size(368, 124);
            this.radpnl_processinfo.TabIndex = 12;
            this.radpnl_processinfo.ThemeName = "ControlDefault";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_processinfo.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_processinfo.GetChildAt(0))).BorderHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(14)))), ((int)(((byte)(248)))));
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_processinfo.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(5);
            // 
            // tbMesPort
            // 
            this.tbMesPort.Font = new System.Drawing.Font("Times New Roman", 16F);
            this.tbMesPort.Location = new System.Drawing.Point(200, 77);
            this.tbMesPort.Name = "tbMesPort";
            this.tbMesPort.Size = new System.Drawing.Size(157, 32);
            this.tbMesPort.TabIndex = 74;
            this.tbMesPort.Text = "1000";
            this.tbMesPort.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // radLabel3
            // 
            this.radLabel3.AutoSize = false;
            this.radLabel3.BackColor = System.Drawing.Color.LightGray;
            this.radLabel3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.radLabel3.Location = new System.Drawing.Point(66, 77);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(128, 32);
            this.radLabel3.TabIndex = 73;
            this.radLabel3.Text = "Port";
            this.radLabel3.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tbMesIpaddress
            // 
            this.tbMesIpaddress.Font = new System.Drawing.Font("Times New Roman", 16F);
            this.tbMesIpaddress.Location = new System.Drawing.Point(200, 39);
            this.tbMesIpaddress.Name = "tbMesIpaddress";
            this.tbMesIpaddress.Size = new System.Drawing.Size(157, 32);
            this.tbMesIpaddress.TabIndex = 70;
            this.tbMesIpaddress.Text = "192.168.0.1";
            this.tbMesIpaddress.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // radlblOffsetChannel
            // 
            this.radlblOffsetChannel.AutoSize = false;
            this.radlblOffsetChannel.BackColor = System.Drawing.Color.LightGray;
            this.radlblOffsetChannel.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.radlblOffsetChannel.Location = new System.Drawing.Point(66, 39);
            this.radlblOffsetChannel.Name = "radlblOffsetChannel";
            this.radlblOffsetChannel.Size = new System.Drawing.Size(128, 32);
            this.radlblOffsetChannel.TabIndex = 69;
            this.radlblOffsetChannel.Text = "IP ADDRESS";
            this.radlblOffsetChannel.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.SteelBlue;
            this.label12.Location = new System.Drawing.Point(5, 5);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(102, 16);
            this.label12.TabIndex = 50;
            this.label12.Text = "MES SETTING";
            // 
            // radBtnCancel
            // 
            this.radBtnCancel.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.radBtnCancel.ForeColor = System.Drawing.Color.Black;
            this.radBtnCancel.Location = new System.Drawing.Point(230, 299);
            this.radBtnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.radBtnCancel.Name = "radBtnCancel";
            this.radBtnCancel.Size = new System.Drawing.Size(90, 32);
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
            // radBtnSave
            // 
            this.radBtnSave.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.radBtnSave.ForeColor = System.Drawing.Color.Black;
            this.radBtnSave.Location = new System.Drawing.Point(78, 299);
            this.radBtnSave.Margin = new System.Windows.Forms.Padding(4);
            this.radBtnSave.Name = "radBtnSave";
            this.radBtnSave.Size = new System.Drawing.Size(90, 32);
            this.radBtnSave.TabIndex = 73;
            this.radBtnSave.Text = "SAVE";
            this.radBtnSave.ThemeName = "ControlDefault";
            this.radBtnSave.Click += new System.EventHandler(this.radBtnSave_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnSave.GetChildAt(0))).Text = "SAVE";
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnSave.GetChildAt(0))).Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnSave.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(241)))), ((int)(((byte)(252)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnSave.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(226)))), ((int)(((byte)(244)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnSave.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(226)))), ((int)(((byte)(244)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnSave.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.White;
            // 
            // ConfigForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(390, 344);
            this.Controls.Add(this.radBtnCancel);
            this.Controls.Add(this.radBtnSave);
            this.Controls.Add(this.radPanel1);
            this.Controls.Add(this.radpnl_processinfo);
            this.Name = "ConfigForm";
            this.Text = "ConfigForm";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_processinfo)).EndInit();
            this.radpnl_processinfo.ResumeLayout(false);
            this.radpnl_processinfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radlblOffsetChannel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnSave)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radPanel1;
        private System.Windows.Forms.TextBox tbPlcIpaddress;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private System.Windows.Forms.Label label1;
        private Telerik.WinControls.UI.RadPanel radpnl_processinfo;
        private System.Windows.Forms.TextBox tbMesIpaddress;
        private Telerik.WinControls.UI.RadLabel radlblOffsetChannel;
        private System.Windows.Forms.Label label12;
        private Telerik.WinControls.UI.RadButton radBtnCancel;
        private Telerik.WinControls.UI.RadButton radBtnSave;
        private System.Windows.Forms.TextBox tbPlcDbNumber;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private System.Windows.Forms.TextBox tbMesPort;
        private Telerik.WinControls.UI.RadLabel radLabel3;
    }
}