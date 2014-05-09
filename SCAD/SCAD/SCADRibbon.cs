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

        private void FinalizeSchedule_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will Finalize Dynamic Scheduling eventually");
        }

        private void StartDynamicSchedule_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will Start Dynamic Scheduling eventually");
        }

        private void CreateScriptStud_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will Create the Stud Script File eventually");
        }

        private void PrintStudLines_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will create Stud PDF reports eventually");
        }

        private void PrelimLateral_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will begin Preliminary Lateral Design eventually");
        }

        private void FullLateral_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will Finalize Lateral Design eventually");
        }

        private void SetupWalls_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will Resetup shear walls eventually");
        }

        private void CreateScriptLateral_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will create the lateral design script eventually");
        }

        private void PrintReportsLateral_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will create lateral PDF reports eventually");
        }

        private void ExportRISADiaphragm_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will export RISA Diaphragm information eventually");
        }

        private void OpenChecklistSpecs_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will open Checklist and Specs eventually");
        }

        private void OpenBeam_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will open the Beam worksheet eventually");
        }

        private void OpenColumn_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will open the Column worksheet eventually");
        }

        private void OpenStud_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will open the Stud worksheet eventually");
        }

        private void OpenWind_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will open the Wind worksheet eventually");
        }

        private void OpenSeismic_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will open the Seismic worksheet eventually");
        }

        private void OpenSeismic2_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will open the Seismic2 worksheet eventually");
        }

        private void OpenWoodBrickDiff_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will open the Wood/Brick Differential worksheet eventually");
        }

        private void OpenPeriod_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will open the Building Period worksheet eventually");
        }
    }
}
