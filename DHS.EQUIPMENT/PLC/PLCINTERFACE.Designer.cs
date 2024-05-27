
namespace DHS.EQUIPMENT
{
    partial class PLCINTERFACE
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
            this.radpnl_PlcInterfaceTitle = new Telerik.WinControls.UI.RadPanel();
            this.pnl_PlcInterface = new System.Windows.Forms.Panel();
            this.dgvPLC1 = new System.Windows.Forms.DataGridView();
            this.dgvPC1 = new System.Windows.Forms.DataGridView();
            this.radpnl_PLCTEST = new Telerik.WinControls.UI.RadPanel();
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
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_PlcInterfaceTitle)).BeginInit();
            this.pnl_PlcInterface.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPLC1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPC1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_PLCTEST)).BeginInit();
            this.radpnl_PLCTEST.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnWriteValue)).BeginInit();
            this.SuspendLayout();
            // 
            // radpnl_OperationMode
            // 
            this.radpnl_OperationMode.Controls.Add(this.radpnl_PlcInterfaceTitle);
            this.radpnl_OperationMode.Controls.Add(this.pnl_PlcInterface);
            this.radpnl_OperationMode.Location = new System.Drawing.Point(25, 25);
            this.radpnl_OperationMode.Name = "radpnl_OperationMode";
            this.radpnl_OperationMode.Padding = new System.Windows.Forms.Padding(5);
            this.radpnl_OperationMode.Size = new System.Drawing.Size(936, 734);
            this.radpnl_OperationMode.TabIndex = 11;
            this.radpnl_OperationMode.ThemeName = "ControlDefault";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_OperationMode.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_OperationMode.GetChildAt(0))).BorderHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(14)))), ((int)(((byte)(248)))));
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_OperationMode.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(5);
            // 
            // radpnl_PlcInterfaceTitle
            // 
            this.radpnl_PlcInterfaceTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.radpnl_PlcInterfaceTitle.Font = new System.Drawing.Font("Arial", 24F);
            this.radpnl_PlcInterfaceTitle.Location = new System.Drawing.Point(5, 5);
            this.radpnl_PlcInterfaceTitle.Name = "radpnl_PlcInterfaceTitle";
            this.radpnl_PlcInterfaceTitle.Padding = new System.Windows.Forms.Padding(5);
            this.radpnl_PlcInterfaceTitle.Size = new System.Drawing.Size(926, 89);
            this.radpnl_PlcInterfaceTitle.TabIndex = 13;
            this.radpnl_PlcInterfaceTitle.Text = "PLC INTERFACE";
            this.radpnl_PlcInterfaceTitle.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radpnl_PlcInterfaceTitle.ThemeName = "ControlDefault";
            this.radpnl_PlcInterfaceTitle.Click += new System.EventHandler(this.radpnl_PlcInterfaceTitle_Click);
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_PlcInterfaceTitle.GetChildAt(0))).Text = "PLC INTERFACE";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_PlcInterfaceTitle.GetChildAt(0))).BorderHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(14)))), ((int)(((byte)(248)))));
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_PlcInterfaceTitle.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(5);
            // 
            // pnl_PlcInterface
            // 
            this.pnl_PlcInterface.Controls.Add(this.dgvPLC1);
            this.pnl_PlcInterface.Controls.Add(this.dgvPC1);
            this.pnl_PlcInterface.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnl_PlcInterface.Location = new System.Drawing.Point(5, 94);
            this.pnl_PlcInterface.Name = "pnl_PlcInterface";
            this.pnl_PlcInterface.Padding = new System.Windows.Forms.Padding(10);
            this.pnl_PlcInterface.Size = new System.Drawing.Size(926, 635);
            this.pnl_PlcInterface.TabIndex = 51;
            // 
            // dgvPLC1
            // 
            this.dgvPLC1.AllowUserToResizeColumns = false;
            this.dgvPLC1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPLC1.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvPLC1.Location = new System.Drawing.Point(10, 10);
            this.dgvPLC1.Name = "dgvPLC1";
            this.dgvPLC1.RowHeadersVisible = false;
            this.dgvPLC1.RowHeadersWidth = 51;
            this.dgvPLC1.RowTemplate.Height = 23;
            this.dgvPLC1.Size = new System.Drawing.Size(450, 615);
            this.dgvPLC1.TabIndex = 0;
            // 
            // dgvPC1
            // 
            this.dgvPC1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPC1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPC1.Dock = System.Windows.Forms.DockStyle.Right;
            this.dgvPC1.Location = new System.Drawing.Point(466, 10);
            this.dgvPC1.Margin = new System.Windows.Forms.Padding(5);
            this.dgvPC1.Name = "dgvPC1";
            this.dgvPC1.RowHeadersVisible = false;
            this.dgvPC1.RowHeadersWidth = 51;
            this.dgvPC1.RowTemplate.Height = 23;
            this.dgvPC1.Size = new System.Drawing.Size(450, 615);
            this.dgvPC1.TabIndex = 1;
            // 
            // radpnl_PLCTEST
            // 
            this.radpnl_PLCTEST.Controls.Add(this.cbPCAddress);
            this.radpnl_PLCTEST.Controls.Add(this.label5);
            this.radpnl_PLCTEST.Controls.Add(this.label3);
            this.radpnl_PLCTEST.Controls.Add(this.tbTagValue);
            this.radpnl_PLCTEST.Controls.Add(this.label2);
            this.radpnl_PLCTEST.Controls.Add(this.tbTagName);
            this.radpnl_PLCTEST.Controls.Add(this.radBtnWriteValue);
            this.radpnl_PLCTEST.Controls.Add(this.label1);
            this.radpnl_PLCTEST.Location = new System.Drawing.Point(976, 25);
            this.radpnl_PLCTEST.Name = "radpnl_PLCTEST";
            this.radpnl_PLCTEST.Padding = new System.Windows.Forms.Padding(5);
            this.radpnl_PLCTEST.Size = new System.Drawing.Size(373, 534);
            this.radpnl_PLCTEST.TabIndex = 12;
            this.radpnl_PLCTEST.ThemeName = "ControlDefault";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_PLCTEST.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_PLCTEST.GetChildAt(0))).BorderHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(14)))), ((int)(((byte)(248)))));
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_PLCTEST.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(5);
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
            this.label5.Size = new System.Drawing.Size(87, 16);
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
            this.label3.Size = new System.Drawing.Size(73, 16);
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
            this.label2.Size = new System.Drawing.Size(74, 16);
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
            this.radBtnWriteValue.Click += new System.EventHandler(this.radBtnWriteValue_Click);
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
            this.label1.Size = new System.Drawing.Size(134, 16);
            this.label1.TabIndex = 50;
            this.label1.Text = "OPERATION MODE";
            // 
            // PLCINTERFACE
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1359, 765);
            this.Controls.Add(this.radpnl_PLCTEST);
            this.Controls.Add(this.radpnl_OperationMode);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PLCINTERFACE";
            this.Text = "PLCINTERFACE";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PLCINTERFACE_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_OperationMode)).EndInit();
            this.radpnl_OperationMode.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_PlcInterfaceTitle)).EndInit();
            this.pnl_PlcInterface.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPLC1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPC1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_PLCTEST)).EndInit();
            this.radpnl_PLCTEST.ResumeLayout(false);
            this.radpnl_PLCTEST.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radBtnWriteValue)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Telerik.WinControls.UI.RadPanel radpnl_OperationMode;
        private Telerik.WinControls.UI.RadPanel radpnl_PLCTEST;
        private Telerik.WinControls.UI.RadButton radBtnWriteValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnl_PlcInterface;
        private System.Windows.Forms.DataGridView dgvPLC1;
        private System.Windows.Forms.DataGridView dgvPC1;
        private Telerik.WinControls.UI.RadPanel radpnl_PlcInterfaceTitle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbTagValue;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbTagName;
        private System.Windows.Forms.ComboBox cbPCAddress;
        private System.Windows.Forms.Label label5;
    }
}