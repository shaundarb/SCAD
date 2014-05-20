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
    public partial class StudExport : Form
    {
        public bool[] StudExportOptions = new bool[7]; // Stores the user's export preferences from form.

        public StudExport()
        {
            InitializeComponent();
        }

        private void StudExportSubmit_Click(object sender, EventArgs e)
        {
            /* Passes the user's stud export preferences back to add-in */
            StudExportOptions[0] = this.StudNameCheck.Checked;      // Stud Wall Names
            StudExportOptions[1] = this.StudDesignCheck.Checked;    // Stud Wall Design
            StudExportOptions[2] = this.StudKeyCheck.Checked;       // Key Plan Numbers
            StudExportOptions[3] = this.StudEndpointCheck.Checked;  // Stud Wall Endpoints
            StudExportOptions[4] = this.StudFoundCheck.Checked;     // Foundation Reactions
            StudExportOptions[5] = this.StudScheduleCheck.Checked;  // Stud Schedule

            this.Close();
        }

        private void StudExportCancel_Click_1(object sender, EventArgs e)
        {
            StudExportOptions[6] = true;
            this.Close();
        }
    }
}
