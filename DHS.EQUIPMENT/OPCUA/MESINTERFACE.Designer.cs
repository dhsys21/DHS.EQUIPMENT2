
namespace DHS.EQUIPMENT
{
    partial class MESINTERFACE
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
            this.radpnl_OperationMode = new Telerik.WinControls.UI.RadPanel();
            this.radpnl_MesInterfaceTitle = new Telerik.WinControls.UI.RadPanel();
            this.pnl_PlcInterface = new System.Windows.Forms.Panel();
            this.dgvMES = new System.Windows.Forms.DataGridView();
            this.dgvPC = new System.Windows.Forms.DataGridView();
            this.radpnl_MESTEST = new Telerik.WinControls.UI.RadPanel();
            this.cbPCAddress = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tbTagValue = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbTagName = new System.Windows.Forms.TextBox();
            this.radBtnWriteValue = new Telerik.WinControls.UI.RadButton();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_OperationMode)).BeginInit();
            this.radpnl_OperationMode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_MesInterfaceTitle)).BeginInit();
            this.pnl_PlcInterface.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMES)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPC)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_MESTEST)).BeginInit();
            this.radpnl_MESTEST.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnWriteValue)).BeginInit();
            this.SuspendLayout();
            // 
            // radpnl_OperationMode
            // 
            this.radpnl_OperationMode.Controls.Add(this.radpnl_MesInterfaceTitle);
            this.radpnl_OperationMode.Controls.Add(this.pnl_PlcInterface);
            this.radpnl_OperationMode.Location = new System.Drawing.Point(25, 25);
            this.radpnl_OperationMode.Name = "radpnl_OperationMode";
            this.radpnl_OperationMode.Padding = new System.Windows.Forms.Padding(5);
            this.radpnl_OperationMode.Size = new System.Drawing.Size(936, 534);
            this.radpnl_OperationMode.TabIndex = 12;
            this.radpnl_OperationMode.ThemeName = "ControlDefault";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_OperationMode.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_OperationMode.GetChildAt(0))).BorderHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(14)))), ((int)(((byte)(248)))));
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_OperationMode.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(5);
            // 
            // radpnl_MesInterfaceTitle
            // 
            this.radpnl_MesInterfaceTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.radpnl_MesInterfaceTitle.Font = new System.Drawing.Font("Arial", 24F);
            this.radpnl_MesInterfaceTitle.Location = new System.Drawing.Point(5, 5);
            this.radpnl_MesInterfaceTitle.Name = "radpnl_MesInterfaceTitle";
            this.radpnl_MesInterfaceTitle.Padding = new System.Windows.Forms.Padding(5);
            this.radpnl_MesInterfaceTitle.Size = new System.Drawing.Size(926, 89);
            this.radpnl_MesInterfaceTitle.TabIndex = 13;
            this.radpnl_MesInterfaceTitle.Text = "MES INTERFACE";
            this.radpnl_MesInterfaceTitle.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radpnl_MesInterfaceTitle.ThemeName = "ControlDefault";
            this.radpnl_MesInterfaceTitle.Click += new System.EventHandler(this.radpnl_MesInterfaceTitle_Click);
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_MesInterfaceTitle.GetChildAt(0))).Text = "MES INTERFACE";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_MesInterfaceTitle.GetChildAt(0))).BorderHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(14)))), ((int)(((byte)(248)))));
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_MesInterfaceTitle.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(5);
            // 
            // pnl_PlcInterface
            // 
            this.pnl_PlcInterface.Controls.Add(this.dgvMES);
            this.pnl_PlcInterface.Controls.Add(this.dgvPC);
            this.pnl_PlcInterface.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl_PlcInterface.Location = new System.Drawing.Point(5, 94);
            this.pnl_PlcInterface.Name = "pnl_PlcInterface";
            this.pnl_PlcInterface.Padding = new System.Windows.Forms.Padding(10);
            this.pnl_PlcInterface.Size = new System.Drawing.Size(926, 435);
            this.pnl_PlcInterface.TabIndex = 51;
            // 
            // dgvMES
            // 
            this.dgvMES.AllowUserToResizeColumns = false;
            this.dgvMES.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMES.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvMES.Location = new System.Drawing.Point(10, 10);
            this.dgvMES.Name = "dgvMES";
            this.dgvMES.RowHeadersVisible = false;
            this.dgvMES.RowHeadersWidth = 51;
            this.dgvMES.RowTemplate.Height = 23;
            this.dgvMES.Size = new System.Drawing.Size(450, 415);
            this.dgvMES.TabIndex = 0;
            // 
            // dgvPC
            // 
            this.dgvPC.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPC.Dock = System.Windows.Forms.DockStyle.Right;
            this.dgvPC.Location = new System.Drawing.Point(466, 10);
            this.dgvPC.Margin = new System.Windows.Forms.Padding(5);
            this.dgvPC.Name = "dgvPC";
            this.dgvPC.RowHeadersVisible = false;
            this.dgvPC.RowHeadersWidth = 51;
            this.dgvPC.RowTemplate.Height = 23;
            this.dgvPC.Size = new System.Drawing.Size(450, 415);
            this.dgvPC.TabIndex = 1;
            // 
            // radpnl_MESTEST
            // 
            this.radpnl_MESTEST.Controls.Add(this.cbPCAddress);
            this.radpnl_MESTEST.Controls.Add(this.label5);
            this.radpnl_MESTEST.Controls.Add(this.label3);
            this.radpnl_MESTEST.Controls.Add(this.tbTagValue);
            this.radpnl_MESTEST.Controls.Add(this.label2);
            this.radpnl_MESTEST.Controls.Add(this.tbTagName);
            this.radpnl_MESTEST.Controls.Add(this.radBtnWriteValue);
            this.radpnl_MESTEST.Controls.Add(this.label1);
            this.radpnl_MESTEST.Location = new System.Drawing.Point(976, 25);
            this.radpnl_MESTEST.Name = "radpnl_MESTEST";
            this.radpnl_MESTEST.Padding = new System.Windows.Forms.Padding(5);
            this.radpnl_MESTEST.Size = new System.Drawing.Size(373, 534);
            this.radpnl_MESTEST.TabIndex = 13;
            this.radpnl_MESTEST.ThemeName = "ControlDefault";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_MESTEST.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_MESTEST.GetChildAt(0))).BorderHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(14)))), ((int)(((byte)(248)))));
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_MESTEST.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(5);
            // 
            // cbPCAddress
            // 
            this.cbPCAddress.FormattingEnabled = true;
            this.cbPCAddress.Items.AddRange(new object[] {
            "PC Heart Beat",
            "PC Auto Manual",
            "PC Error",
            "Tray Out",
            "Tray Down",
            "Tray Up",
            "Measurement Wait",
            "Measurement Run",
            "Measurement Complete"});
            this.cbPCAddress.Location = new System.Drawing.Point(195, 122);
            this.cbPCAddress.Name = "cbPCAddress";
            this.cbPCAddress.Size = new System.Drawing.Size(153, 20);
            this.cbPCAddress.TabIndex = 61;
            this.cbPCAddress.Text = "Select Address";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.SteelBlue;
            this.label5.Location = new System.Drawing.Point(43, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 16);
            this.label5.TabIndex = 60;
            this.label5.Text = "PC Address";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.SteelBlue;
            this.label3.Location = new System.Drawing.Point(43, 161);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(74, 16);
            this.label3.TabIndex = 57;
            this.label3.Text = "Tag Value";
            // 
            // tbTagValue
            // 
            this.tbTagValue.Location = new System.Drawing.Point(195, 156);
            this.tbTagValue.Name = "tbTagValue";
            this.tbTagValue.Size = new System.Drawing.Size(100, 21);
            this.tbTagValue.TabIndex = 56;
            this.tbTagValue.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.SteelBlue;
            this.label2.Location = new System.Drawing.Point(43, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 16);
            this.label2.TabIndex = 55;
            this.label2.Text = "Tag Name";
            this.label2.Visible = false;
            // 
            // tbTagName
            // 
            this.tbTagName.Location = new System.Drawing.Point(195, 49);
            this.tbTagName.Name = "tbTagName";
            this.tbTagName.Size = new System.Drawing.Size(100, 21);
            this.tbTagName.TabIndex = 54;
            this.tbTagName.Text = "DB85.DBB0";
            this.tbTagName.Visible = false;
            // 
            // radBtnWriteValue
            // 
            this.radBtnWriteValue.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.radBtnWriteValue.ForeColor = System.Drawing.Color.White;
            this.radBtnWriteValue.Location = new System.Drawing.Point(140, 201);
            this.radBtnWriteValue.Margin = new System.Windows.Forms.Padding(4);
            this.radBtnWriteValue.Name = "radBtnWriteValue";
            this.radBtnWriteValue.Size = new System.Drawing.Size(155, 60);
            this.radBtnWriteValue.TabIndex = 52;
            this.radBtnWriteValue.Text = "Write Value";
            this.radBtnWriteValue.ThemeName = "ControlDefault";
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnWriteValue.GetChildAt(0))).Text = "Write Value";
            ((Telerik.WinControls.UI.RadButtonElement)(this.radBtnWriteValue.GetChildAt(0))).Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnWriteValue.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnWriteValue.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnWriteValue.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radBtnWriteValue.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SteelBlue;
            this.label1.Location = new System.Drawing.Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 16);
            this.label1.TabIndex = 50;
            this.label1.Text = "OPERATION MODE";
            // 
            // MESINTERFACE
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1359, 565);
            this.Controls.Add(this.radpnl_MESTEST);
            this.Controls.Add(this.radpnl_OperationMode);
            this.Name = "MESINTERFACE";
            this.Text = "MESINTERFACE";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MESINTERFACE_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_OperationMode)).EndInit();
            this.radpnl_OperationMode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_MesInterfaceTitle)).EndInit();
            this.pnl_PlcInterface.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMES)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPC)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_MESTEST)).EndInit();
            this.radpnl_MESTEST.ResumeLayout(false);
            this.radpnl_MESTEST.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnWriteValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadPanel radpnl_OperationMode;
        private Telerik.WinControls.UI.RadPanel radpnl_MesInterfaceTitle;
        private System.Windows.Forms.Panel pnl_PlcInterface;
        private System.Windows.Forms.DataGridView dgvMES;
        private System.Windows.Forms.DataGridView dgvPC;
        private Telerik.WinControls.UI.RadPanel radpnl_MESTEST;
        private System.Windows.Forms.ComboBox cbPCAddress;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbTagValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbTagName;
        private Telerik.WinControls.UI.RadButton radBtnWriteValue;
        private System.Windows.Forms.Label label1;
    }
}