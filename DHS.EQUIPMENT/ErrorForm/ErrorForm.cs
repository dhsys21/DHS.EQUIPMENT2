using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DHS.EQUIPMENT.Common;

namespace DHS.EQUIPMENT
{
    public partial class ErrorForm : Form
    {
        private static ErrorForm errorform = new ErrorForm();
        public static ErrorForm GetInstance()
        {
            if (errorform == null) errorform = new ErrorForm();
            return errorform;
        }
        public ErrorForm()
        {
            InitializeComponent();

            ShowMessage();
        }
        public void HideMessage2(bool show)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Form, bool>((formInstance, isShow) =>
                {
                    if (isShow) formInstance.Show();
                    else formInstance.Hide();
                }), this, show);
            }
            else
            {
                if (show) this.Show();
                else this.Hide();
            }
        }
        private void ShowMessage()
        {
            string title = "Error";
            string errmsg = "There is an error.";
            string checkmsg = "Please check the error.";
            lblTitle.Text = title;
            lblErrMsg.Text = errmsg;
            lblCheckMsg.Text = checkmsg;
        }

        public void ShowMessage(string title, string errormsg, string checkmsg)
        {
            lblTitle.Text = title;
            lblErrMsg.Text = errormsg;
            lblCheckMsg.Text = checkmsg;
            try
            {
                this.Show();
            }
            catch (Exception ex) { }

        }

        internal void ShowMessage(enumStageError _StageError, int stageno)
        {
            string errmsg = string.Empty;
            if (this.Visible == true) return;

            if (_StageError == enumStageError.NoError)
            {
                if (this.Visible)
                    HideMessage2(false);
            }
            else if (_StageError == enumStageError.IROCVDisconnected)
            {
                errmsg = "IR/OCV Stage No." + stageno.ToString() + " is not connected."; 
                //* for test 2024 05 31 주석처리
                ShowMessage("IR/OCV Connection Error", "IR/OCV is not connected.", "Please check the IR/OCV and restart IR/OCV Controller.");
            }
            else if (_StageError == enumStageError.IROCVNoResponse)
            {
                errmsg = "IR/OCV Stage No." + stageno.ToString() + " is not response.";
                ShowMessage("IR/OCV Response Error", "IR/OCV is not response.", "Please re-start IR/OCV Controller.");
            }
            else if (_StageError == enumStageError.IROCVNotRemote)
            {
                errmsg = "IR/OCV Stage No." + stageno.ToString() + " is not remote mode.";
                ShowMessage("IR/OCV Response Error", "IR/OCV is not remote mode.", "Please click reset button or re-start IR/OCV Controller.");
            }
            else if (_StageError == enumStageError.PLCDisconnected)
            {
                ShowMessage("PLC Connection Error", "PLC is not connected.", "Please check the PLC connection.");
            }
            else if (_StageError == enumStageError.MESDisconnected)
            {
                ShowMessage("MES Connection Error", "MES is not connected.", "Please check the MES connection.");
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
