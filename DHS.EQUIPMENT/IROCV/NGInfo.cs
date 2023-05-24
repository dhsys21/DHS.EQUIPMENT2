using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DHS.EQUIPMENT.Common;

namespace DHS.EQUIPMENT
{
    public partial class NGInfo : Form
    {
        Util util = new Util();
        CEquipmentData _system;

        public int _iStage;
        int LABEL_TITLE_HEIGHT = 42;
        int LABEL_DATA_HEIGHT = 32;
        private Label[] lblNGInfo = new Label[_Constant.ChannelCount];
        private Label[] lblUseInfo = new Label[_Constant.ChannelCount];

        private static NGInfo ngInfoForm = new NGInfo();
        public static NGInfo GetInstance()
        {
            if (ngInfoForm == null) ngInfoForm = new NGInfo();
            return ngInfoForm;
        }
        public NGInfo()
        {
            InitializeComponent();
            _system = CEquipmentData.GetInstance();

            //* ng info panel
            MakeLabelTitle();
            MakeLabelData();
        }

        public void SetStageNo(int stageno)
        {
            _iStage = stageno;
            lblTitle.Text = "STAGE " + (stageno + 1);
        }

        #region Make Panel - ir/ocv values
        private void MakeLabelTitle()
        {
            int nx = 3;
            int ny = 2;
            int width = 76;
            int height = LABEL_TITLE_HEIGHT;
            for (int nIndex = 0; nIndex < 32;)
            {
                Label lbl = new Label();
                SetLabelOption_Title(lbl, width, height, nx, ny, (nIndex + 1).ToString(), nIndex);
                lbl.DoubleClick += new System.EventHandler(this.lblNGInfo_Click);

                nIndex = nIndex + 1;
                nx = nx + lbl.Width + 2;
                if (nIndex % 4 == 0) nx += 2;
                if (nIndex % 16 == 0)
                {
                    ny = ny + LABEL_TITLE_HEIGHT + (LABEL_DATA_HEIGHT * 2) + 4;
                    nx = 3;
                }
            }
        }
        private void MakeLabelData()
        {
            int nx = 3;
            int ny = LABEL_TITLE_HEIGHT + 3;
            int width = 76;
            int height = LABEL_DATA_HEIGHT;
            string label_text = string.Empty;
            for (int nIndex = 0; nIndex < _Constant.ChannelCount;)
            {
                label_text = (nIndex / 16 + 1).ToString() + " - " + (nIndex % 16 + 1).ToString();

                lblUseInfo[nIndex] = new DoubleBufferedLabel();//new Label();
                SetLabelOption_Data(lblUseInfo[nIndex], width, height, nx, ny, label_text, _Constant.ColorOCV, nIndex);

                lblNGInfo[nIndex] = new DoubleBufferedLabel();//new Label();
                SetLabelOption_Data(lblNGInfo[nIndex], width, height, nx, ny + height + 1, label_text, _Constant.ColorOCV, nIndex);

                nIndex = nIndex + 1;
                nx = nx + width + 2;
                if (nIndex % 4 == 0) nx += 2;
                if (nIndex % 16 == 0)
                {
                    ny = ny + LABEL_TITLE_HEIGHT + (LABEL_DATA_HEIGHT * 2) + 4;
                    nx = 3;
                }
            }
        }

        private void SetLabelOption_Title(Label lbl, int width, int height, int left, int top, string text, int index)
        {
            lbl.Visible = true;
            lbl.BackColor = Color.DarkGray;
            lbl.Width = width;
            lbl.Height = height;
            lbl.Font = new Font("Arial", 18, FontStyle.Bold);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Text = text;
            pBase.Controls.Add(lbl);
            lbl.Left = left;
            lbl.Top = top;
            lbl.Tag = index;
        }
        private void SetLabelOption_Data(Label lbl, int width, int height, int left, int top, string text, Color clr, int index)
        {
            lbl.Visible = true;
            lbl.BackColor = clr;
            lbl.Width = width;
            lbl.Height = height;
            lbl.Font = new Font("Arial", 12, FontStyle.Bold);
            lbl.TextAlign = ContentAlignment.MiddleCenter;
            lbl.Text = text;
            pBase.Controls.Add(lbl);
            lbl.Left = left;
            lbl.Top = top;
            lbl.Tag = index;
        }
        #endregion

        #region Control Event
        private void NGInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void radBtnInit_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you want to initialize all channel records?", "Initialization", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                InitChannelAll();
            }
        }
        private void lblNGInfo_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            int channel_no = int.Parse(lbl.Tag.ToString());
            string msg = "Are you want to initialize channel " + (channel_no + 1) + " records?";
            var result = MessageBox.Show(msg, "Initialization", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                InitChanelEach(channel_no);
            }
            
        }
        #endregion

        #region Method
        private void InitChannelAll()
        {
            for(int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
            {
                lblUseInfo[nIndex].Text = "0";
                lblNGInfo[nIndex].Text = "0";
                _system.REMEASUREUSE[nIndex] = 0;
                _system.REMEASURENG[nIndex] = 0;
            }

            util.SaveNGInfo(this._iStage, false, _system.REMEASUREUSE, _system.REMEASURENG);
        }
        private void InitChanelEach(int channel)
        {
            lblUseInfo[channel].Text = "0";
            lblNGInfo[channel].Text = "0";

            _system.REMEASUREUSE[channel] = 0;
            _system.REMEASURENG[channel] = 0;

            util.SaveNGInfo(this._iStage, false, _system.REMEASUREUSE, _system.REMEASURENG);
        }
        public void SetNGInfo(int stageno)
        {
            if(stageno == this._iStage)
            {
                for(int nIndex = 0; nIndex < _Constant.ChannelCount; nIndex++)
                {
                    lblUseInfo[nIndex].Text = _system.REMEASUREUSE[nIndex].ToString();
                    lblNGInfo[nIndex].Text = _system.REMEASURENG[nIndex].ToString();
                }
            }
        }
        #endregion

    }
}
