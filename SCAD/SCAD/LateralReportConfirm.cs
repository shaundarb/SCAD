using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAD
{
    public partial class LateralReportConfirm : Form
    {
        public bool ReportConfirm = false;

        public LateralReportConfirm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            ReportConfirm = true;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            ReportConfirm = false;
            this.Close();
        }
    }
}
