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
    public partial class StudPrintLines : Form
    {
        public bool PrintUnique = true;

        public StudPrintLines()
        {
            InitializeComponent();
        }

        private void buttonPrintUnique_Click(object sender, EventArgs e)
        {
            PrintUnique = true;
            this.Close();
        }

        private void buttonPrintCurrent_Click(object sender, EventArgs e)
        {
            PrintUnique = false;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
