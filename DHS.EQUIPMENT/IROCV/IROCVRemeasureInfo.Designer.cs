
namespace DHS.EQUIPMENT
{
    partial class IROCVRemeasureInfo
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvPLC1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.radbtn_TrayOut = new Telerik.WinControls.UI.RadButton();
            this.radbtn_Remeasure = new Telerik.WinControls.UI.RadButton();
            this.radbtn_AllRemeasure = new Telerik.WinControls.UI.RadButton();
            this.radpnl_RemeausreTitle = new Telerik.WinControls.UI.RadPanel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPLC1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radbtn_TrayOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radbtn_Remeasure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radbtn_AllRemeasure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_RemeausreTitle)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Orange;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.dgvPLC1);
            this.panel1.Controls.Add(this.radPanel1);
            this.panel1.Controls.Add(this.radpnl_RemeausreTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(652, 529);
            this.panel1.TabIndex = 0;
            // 
            // dgvPLC1
            // 
            this.dgvPLC1.AllowUserToResizeColumns = false;
            this.dgvPLC1.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPLC1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvPLC1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPLC1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            this.dgvPLC1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPLC1.GridColor = System.Drawing.Color.White;
            this.dgvPLC1.Location = new System.Drawing.Point(0, 60);
            this.dgvPLC1.Name = "dgvPLC1";
            this.dgvPLC1.RowHeadersVisible = false;
            this.dgvPLC1.RowHeadersWidth = 51;
            this.dgvPLC1.RowTemplate.Height = 23;
            this.dgvPLC1.Size = new System.Drawing.Size(650, 377);
            this.dgvPLC1.TabIndex = 52;
            // 
            // Column1
            // 
            dataGridViewCellStyle14.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle14;
            this.Column1.HeaderText = "Channel";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            dataGridViewCellStyle15.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold);
            this.Column2.DefaultCellStyle = dataGridViewCellStyle15;
            this.Column2.HeaderText = "NG Code";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            dataGridViewCellStyle16.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold);
            this.Column3.DefaultCellStyle = dataGridViewCellStyle16;
            this.Column3.HeaderText = "NG Name";
            this.Column3.Name = "Column3";
            this.Column3.Width = 240;
            // 
            // Column4
            // 
            dataGridViewCellStyle17.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold);
            this.Column4.DefaultCellStyle = dataGridViewCellStyle17;
            this.Column4.HeaderText = "IR";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            dataGridViewCellStyle18.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold);
            this.Column5.DefaultCellStyle = dataGridViewCellStyle18;
            this.Column5.HeaderText = "OCV";
            this.Column5.Name = "Column5";
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.radbtn_TrayOut);
            this.radPanel1.Controls.Add(this.radbtn_Remeasure);
            this.radPanel1.Controls.Add(this.radbtn_AllRemeasure);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radPanel1.Font = new System.Drawing.Font("Arial", 24F);
            this.radPanel1.ForeColor = System.Drawing.Color.White;
            this.radPanel1.Location = new System.Drawing.Point(0, 437);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.radPanel1.Size = new System.Drawing.Size(650, 90);
            this.radPanel1.TabIndex = 58;
            this.radPanel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radPanel1.ThemeName = "ControlDefault";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanel1.GetChildAt(0))).Text = "";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanel1.GetChildAt(0))).BorderHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(14)))), ((int)(((byte)(248)))));
            ((Telerik.WinControls.UI.RadPanelElement)(this.radPanel1.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(5);
            // 
            // radbtn_TrayOut
            // 
            this.radbtn_TrayOut.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.radbtn_TrayOut.ForeColor = System.Drawing.Color.Black;
            this.radbtn_TrayOut.Location = new System.Drawing.Point(432, 15);
            this.radbtn_TrayOut.Margin = new System.Windows.Forms.Padding(4);
            this.radbtn_TrayOut.Name = "radbtn_TrayOut";
            this.radbtn_TrayOut.Size = new System.Drawing.Size(200, 60);
            this.radbtn_TrayOut.TabIndex = 54;
            this.radbtn_TrayOut.Text = "TRAY OUT";
            this.radbtn_TrayOut.TextWrap = true;
            this.radbtn_TrayOut.ThemeName = "ControlDefault";
            this.radbtn_TrayOut.Click += new System.EventHandler(this.radbtn_TrayOut_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radbtn_TrayOut.GetChildAt(0))).Text = "TRAY OUT";
            ((Telerik.WinControls.UI.RadButtonElement)(this.radbtn_TrayOut.GetChildAt(0))).Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_TrayOut.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(57)))), ((int)(((byte)(53)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_TrayOut.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(57)))), ((int)(((byte)(53)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_TrayOut.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(154)))), ((int)(((byte)(154)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_TrayOut.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(154)))), ((int)(((byte)(154)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_TrayOut.GetChildAt(0).GetChildAt(0))).CanFocus = true;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radbtn_TrayOut.GetChildAt(0).GetChildAt(1).GetChildAt(1))).TextWrap = true;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radbtn_TrayOut.GetChildAt(0).GetChildAt(1).GetChildAt(1))).LineLimit = true;
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radbtn_TrayOut.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radbtn_TrayOut.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Padding = new System.Windows.Forms.Padding(4);
            ((Telerik.WinControls.Primitives.TextPrimitive)(this.radbtn_TrayOut.GetChildAt(0).GetChildAt(1).GetChildAt(1))).Alignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // radbtn_Remeasure
            // 
            this.radbtn_Remeasure.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.radbtn_Remeasure.ForeColor = System.Drawing.Color.White;
            this.radbtn_Remeasure.Location = new System.Drawing.Point(222, 15);
            this.radbtn_Remeasure.Margin = new System.Windows.Forms.Padding(4);
            this.radbtn_Remeasure.Name = "radbtn_Remeasure";
            this.radbtn_Remeasure.Size = new System.Drawing.Size(200, 60);
            this.radbtn_Remeasure.TabIndex = 55;
            this.radbtn_Remeasure.Text = "REMEASURE";
            this.radbtn_Remeasure.ThemeName = "ControlDefault";
            this.radbtn_Remeasure.Click += new System.EventHandler(this.radbtn_Remeasure_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radbtn_Remeasure.GetChildAt(0))).Text = "REMEASURE";
            ((Telerik.WinControls.UI.RadButtonElement)(this.radbtn_Remeasure.GetChildAt(0))).Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_Remeasure.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_Remeasure.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_Remeasure.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_Remeasure.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            // 
            // radbtn_AllRemeasure
            // 
            this.radbtn_AllRemeasure.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.radbtn_AllRemeasure.ForeColor = System.Drawing.Color.Black;
            this.radbtn_AllRemeasure.Location = new System.Drawing.Point(12, 15);
            this.radbtn_AllRemeasure.Margin = new System.Windows.Forms.Padding(4);
            this.radbtn_AllRemeasure.Name = "radbtn_AllRemeasure";
            this.radbtn_AllRemeasure.Size = new System.Drawing.Size(200, 60);
            this.radbtn_AllRemeasure.TabIndex = 56;
            this.radbtn_AllRemeasure.Text = "ALL REMEASURE";
            this.radbtn_AllRemeasure.ThemeName = "ControlDefault";
            this.radbtn_AllRemeasure.Click += new System.EventHandler(this.radbtn_AllRemeasure_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radbtn_AllRemeasure.GetChildAt(0))).Text = "ALL REMEASURE";
            ((Telerik.WinControls.UI.RadButtonElement)(this.radbtn_AllRemeasure.GetChildAt(0))).Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_AllRemeasure.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(241)))), ((int)(((byte)(252)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_AllRemeasure.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(226)))), ((int)(((byte)(244)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_AllRemeasure.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(211)))), ((int)(((byte)(226)))), ((int)(((byte)(244)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_AllRemeasure.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.White;
            // 
            // radpnl_RemeausreTitle
            // 
            this.radpnl_RemeausreTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.radpnl_RemeausreTitle.Font = new System.Drawing.Font("Arial", 24F);
            this.radpnl_RemeausreTitle.ForeColor = System.Drawing.Color.White;
            this.radpnl_RemeausreTitle.Location = new System.Drawing.Point(0, 0);
            this.radpnl_RemeausreTitle.Name = "radpnl_RemeausreTitle";
            this.radpnl_RemeausreTitle.Padding = new System.Windows.Forms.Padding(5);
            this.radpnl_RemeausreTitle.Size = new System.Drawing.Size(650, 60);
            this.radpnl_RemeausreTitle.TabIndex = 57;
            this.radpnl_RemeausreTitle.Text = "NG CHANNEL LIST";
            this.radpnl_RemeausreTitle.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radpnl_RemeausreTitle.ThemeName = "ControlDefault";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_RemeausreTitle.GetChildAt(0))).Text = "NG CHANNEL LIST";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_RemeausreTitle.GetChildAt(0))).BorderHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(14)))), ((int)(((byte)(248)))));
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_RemeausreTitle.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(5);
            // 
            // IROCVRemeasureInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DarkOrange;
            this.ClientSize = new System.Drawing.Size(664, 541);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "IROCVRemeasureInfo";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remeasure List";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPLC1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radbtn_TrayOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radbtn_Remeasure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radbtn_AllRemeasure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_RemeausreTitle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvPLC1;
        private Telerik.WinControls.UI.RadButton radbtn_AllRemeasure;
        private Telerik.WinControls.UI.RadButton radbtn_Remeasure;
        private Telerik.WinControls.UI.RadButton radbtn_TrayOut;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadPanel radpnl_RemeausreTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
    }
}