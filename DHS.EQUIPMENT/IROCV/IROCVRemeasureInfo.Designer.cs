
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvNGList = new System.Windows.Forms.DataGridView();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.radbtn_TrayOut = new Telerik.WinControls.UI.RadButton();
            this.radbtn_RemeasureAll = new Telerik.WinControls.UI.RadButton();
            this.radpnl_RemeausreTitle = new Telerik.WinControls.UI.RadPanel();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNGList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.radbtn_TrayOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radbtn_RemeasureAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_RemeausreTitle)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Orange;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.dgvNGList);
            this.panel1.Controls.Add(this.radPanel1);
            this.panel1.Controls.Add(this.radpnl_RemeausreTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(6, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(588, 529);
            this.panel1.TabIndex = 0;
            // 
            // dgvNGList
            // 
            this.dgvNGList.AllowUserToAddRows = false;
            this.dgvNGList.AllowUserToDeleteRows = false;
            this.dgvNGList.AllowUserToResizeColumns = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Gainsboro;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Empty;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvNGList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvNGList.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNGList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvNGList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNGList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column3,
            this.Column4,
            this.Column5});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("굴림", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvNGList.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvNGList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvNGList.GridColor = System.Drawing.Color.White;
            this.dgvNGList.Location = new System.Drawing.Point(0, 60);
            this.dgvNGList.Name = "dgvNGList";
            this.dgvNGList.ReadOnly = true;
            this.dgvNGList.RowHeadersVisible = false;
            this.dgvNGList.RowHeadersWidth = 51;
            this.dgvNGList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.Black;
            this.dgvNGList.RowTemplate.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.White;
            this.dgvNGList.RowTemplate.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            this.dgvNGList.RowTemplate.Height = 23;
            this.dgvNGList.RowTemplate.ReadOnly = true;
            this.dgvNGList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNGList.Size = new System.Drawing.Size(586, 377);
            this.dgvNGList.TabIndex = 52;
            this.dgvNGList.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvNGList_RowsAdded);
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.radbtn_TrayOut);
            this.radPanel1.Controls.Add(this.radbtn_RemeasureAll);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.radPanel1.Font = new System.Drawing.Font("Arial", 24F);
            this.radPanel1.ForeColor = System.Drawing.Color.White;
            this.radPanel1.Location = new System.Drawing.Point(0, 437);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Padding = new System.Windows.Forms.Padding(5);
            this.radPanel1.Size = new System.Drawing.Size(586, 90);
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
            this.radbtn_TrayOut.Location = new System.Drawing.Point(320, 21);
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
            // radbtn_RemeasureAll
            // 
            this.radbtn_RemeasureAll.Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            this.radbtn_RemeasureAll.ForeColor = System.Drawing.Color.White;
            this.radbtn_RemeasureAll.Location = new System.Drawing.Point(80, 21);
            this.radbtn_RemeasureAll.Margin = new System.Windows.Forms.Padding(4);
            this.radbtn_RemeasureAll.Name = "radbtn_RemeasureAll";
            this.radbtn_RemeasureAll.Size = new System.Drawing.Size(200, 60);
            this.radbtn_RemeasureAll.TabIndex = 55;
            this.radbtn_RemeasureAll.Text = "REMEASURE";
            this.radbtn_RemeasureAll.ThemeName = "ControlDefault";
            this.radbtn_RemeasureAll.Click += new System.EventHandler(this.radbtn_RemeasureAll_Click);
            ((Telerik.WinControls.UI.RadButtonElement)(this.radbtn_RemeasureAll.GetChildAt(0))).Text = "REMEASURE";
            ((Telerik.WinControls.UI.RadButtonElement)(this.radbtn_RemeasureAll.GetChildAt(0))).Font = new System.Drawing.Font("Arial", 14F, System.Drawing.FontStyle.Bold);
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_RemeasureAll.GetChildAt(0).GetChildAt(0))).BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_RemeasureAll.GetChildAt(0).GetChildAt(0))).BackColor3 = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_RemeasureAll.GetChildAt(0).GetChildAt(0))).BackColor4 = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            ((Telerik.WinControls.Primitives.FillPrimitive)(this.radbtn_RemeasureAll.GetChildAt(0).GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(202)))), ((int)(((byte)(249)))));
            // 
            // radpnl_RemeausreTitle
            // 
            this.radpnl_RemeausreTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.radpnl_RemeausreTitle.Font = new System.Drawing.Font("Arial", 24F);
            this.radpnl_RemeausreTitle.ForeColor = System.Drawing.Color.White;
            this.radpnl_RemeausreTitle.Location = new System.Drawing.Point(0, 0);
            this.radpnl_RemeausreTitle.Name = "radpnl_RemeausreTitle";
            this.radpnl_RemeausreTitle.Padding = new System.Windows.Forms.Padding(5);
            this.radpnl_RemeausreTitle.Size = new System.Drawing.Size(586, 60);
            this.radpnl_RemeausreTitle.TabIndex = 57;
            this.radpnl_RemeausreTitle.Text = "NG CHANNEL LIST";
            this.radpnl_RemeausreTitle.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.radpnl_RemeausreTitle.ThemeName = "ControlDefault";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_RemeausreTitle.GetChildAt(0))).Text = "NG CHANNEL LIST";
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_RemeausreTitle.GetChildAt(0))).BorderHighlightColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(14)))), ((int)(((byte)(248)))));
            ((Telerik.WinControls.UI.RadPanelElement)(this.radpnl_RemeausreTitle.GetChildAt(0))).Padding = new System.Windows.Forms.Padding(5);
            // 
            // Column1
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.Column1.DefaultCellStyle = dataGridViewCellStyle3;
            this.Column1.HeaderText = "Channel";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 120;
            // 
            // Column3
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold);
            this.Column3.DefaultCellStyle = dataGridViewCellStyle4;
            this.Column3.HeaderText = "NG Name";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 220;
            // 
            // Column4
            // 
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold);
            this.Column4.DefaultCellStyle = dataGridViewCellStyle5;
            this.Column4.HeaderText = "IR";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Width = 120;
            // 
            // Column5
            // 
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("굴림", 11.25F, System.Drawing.FontStyle.Bold);
            this.Column5.DefaultCellStyle = dataGridViewCellStyle6;
            this.Column5.HeaderText = "OCV";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.Width = 120;
            // 
            // IROCVRemeasureInfo
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.DarkOrange;
            this.ClientSize = new System.Drawing.Size(600, 541);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "IROCVRemeasureInfo";
            this.Padding = new System.Windows.Forms.Padding(6);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remeasure List";
            this.TopMost = true;
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNGList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.radbtn_TrayOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radbtn_RemeasureAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radpnl_RemeausreTitle)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvNGList;
        private Telerik.WinControls.UI.RadButton radbtn_RemeasureAll;
        private Telerik.WinControls.UI.RadButton radbtn_TrayOut;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadPanel radpnl_RemeausreTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
    }
}