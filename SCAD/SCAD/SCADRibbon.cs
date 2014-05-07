using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using System.Windows.Forms;

namespace SCAD
{
    public partial class SCADRibbon
    {
        private void SCADRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void LaunchStuds_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will launch Stud Design eventually");
        }

        private void SetStudCallout_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will set user-defined stud callouts for individual walls eventually");
        }

        private void RelaunchStud_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will relaunch Stud Design if already in the Stud Design workbook eventually");
        }
    }
}
