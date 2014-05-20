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
    public partial class LateralExport : Form
    {
        public bool[] LateralExportOptions = new bool[7]; // Stores the user's lateral export preferences from form.

        public LateralExport()
        {
            InitializeComponent();
        }

        private void LateralExportSubmit_Click(object sender, EventArgs e)
        {
            /* Passes the user's lateral export preferences back to add-in */
            LateralExportOptions[0] = this.checkShearName.Checked;      // Shear Wall Names
            LateralExportOptions[1] = this.checkShearDesign.Checked;    // Shear Wall Design
            LateralExportOptions[2] = this.checkShearLength.Checked;    // Shear Wall Length
            LateralExportOptions[3] = this.checkShearAnchors.Checked;   // Shear Wall Anchors
            LateralExportOptions[4] = this.checkShearEndpoints.Checked; // Shear Wall Endpoints
            LateralExportOptions[5] = this.checkDrag.Checked;           // Drag Forces

            this.Close();
        }

        private void LateralExportCancel_Click(object sender, EventArgs e)
        {
            LateralExportOptions[6] = true;
            this.Close();
        }
    }
}
