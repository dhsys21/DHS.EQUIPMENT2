using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DHS.EQUIPMENT
{
    public partial class PasswordForm : Form
    {
        public PasswordForm()
        {
            InitializeComponent();
        }

        private void radBtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PasswordForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
        private void CheckPassword()
        {
            if (tbPassword.Text == "morrow")
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                lblMsg.Text = "Please insert password - Password is invalid!";
                lblMsg.ForeColor = Color.Red;
            }
        }
        private void radBtnOk_Click(object sender, EventArgs e)
        {
            CheckPassword();
        }

        private void tbPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            lblMsg.Text = "Please insert password";
            lblMsg.ForeColor = Color.SteelBlue;

            if (e.KeyChar == (char)Keys.Enter)
            {
                CheckPassword();
            }
        }
    }
}
