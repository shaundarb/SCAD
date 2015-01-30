using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;
using System.Windows.Forms;

namespace SCAD
{
    public partial class SCADRibbon
    {
        private void SCADRibbon_Load(object sender, RibbonUIEventArgs e)
        {
            this.TabSCAD.Position = this.Factory.RibbonPosition.BeforeOfficeId("TabHome");
        }

        private void LaunchStuds_Click(object sender, RibbonControlEventArgs e)
        {
            /* Calls the StudDesign() function from SCADMain. To begin.*/
            Globals.SCADMain.StudDesign();
        }

        private void SetStudCallout_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will set user-defined stud callouts for individual walls eventually");
        }

        private void RelaunchStud_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will relaunch Stud Design eventually");
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
            /* Calls the StudExport() function from SCADMain. This function 
             * returns a string to ensure it has been called successfully
             * and then displays it.*/
            string Response = Globals.SCADMain.StudExport();
            if (Response != null)
            {
                MessageBox.Show(Response);
            }
        }

        private void PrintStudLines_Click(object sender, RibbonControlEventArgs e)
        {
            /* Calls the StudPrintReports() function from SCADMain. This function 
             * returns a string to ensure it has been called successfully
             * and then displays it.*/
            string Response = Globals.SCADMain.StudLineReports();
            if (Response != null)
            {
                MessageBox.Show(Response);
            }
        }

        private void PrelimLateral_Click(object sender, RibbonControlEventArgs e)
        {
            /* Calls the LateralDesign() function from SCADMain. This function 
             * returns a string if something unexpected is encountered.*/
            string Response = Globals.SCADMain.LateralDesign();
            if (Response != null)
            {
                MessageBox.Show(Response);
            }
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
            /* Calls the LateralExport() function from SCADMain. This function 
             * returns a string if something unexpected is encountered.*/
            string Response = Globals.SCADMain.LateralExport();
            if (Response != null)
            {
                MessageBox.Show(Response);
            }
        }

        private void PrintReportsLateral_Click(object sender, RibbonControlEventArgs e)
        {
            string Response = Globals.SCADMain.LateralReportPacks();
            if (Response != null)
            {
                MessageBox.Show(Response);
            }
        }

        private void ExportRISADiaphragm_Click(object sender, RibbonControlEventArgs e)
        {
            MessageBox.Show("This will export RISA Diaphragm information eventually");
        }

        private void OpenChecklistSpecs_Click(object sender, RibbonControlEventArgs e)
        {
            /* Opens the Checklist and Specs workbook that exists on the SCA "ENGUSERS" server.*/
            Excel.Application xlApp = (Excel.Application)Globals.SCADMain.Application;
            Excel.Workbook Specs = xlApp.Workbooks.Open(@"\\Fs1\ENGUSERS\DESIGN\Specifications\Checklist & Specs_BETA.xlsm");
        }

        private void OpenBeam_Click(object sender, RibbonControlEventArgs e)
        {
            /* Opens the 2005 NDS/13th AISC Beam workbook that exists on the SCA "ENGUSERS" server.*/
            Excel.Application xlApp = (Excel.Application)Globals.SCADMain.Application;
            Excel.Workbook Beam = xlApp.Workbooks.Open(@"\\Fs1\ENGUSERS\DESIGN\Beam\Beam Analysis v1.7.xlsm");
        }

        private void OpenColumn_Click(object sender, RibbonControlEventArgs e)
        {
            /* Opens the 2005 NDS Column workbook that exists on the SCA "ENGUSERS" server.*/
            Excel.Application xlApp = (Excel.Application)Globals.SCADMain.Application;
            Excel.Workbook Column = xlApp.Workbooks.Open(@"\\Fs1\ENGUSERS\DESIGN\Studs\2005 NDS Column Interaction.xlsm");
        }

        private void OpenStud_Click(object sender, RibbonControlEventArgs e)
        {
            /* Opens the 2005 NDS Stud workbook that exists on the SCA "ENGUSERS" server.*/
            Excel.Application xlApp = (Excel.Application)Globals.SCADMain.Application;
            Excel.Workbook Stud = xlApp.Workbooks.Open(@"\\Fs1\ENGUSERS\DESIGN\SCAD Programs\Stud Program\Stud Templates\Stud_Design.xltm");
        }

        private void OpenWind_Click(object sender, RibbonControlEventArgs e)
        {
            /* Opens the ASCE 7-02/05/10 Wind workbook that exists on the SCA "ENGUSERS" server.*/
            Excel.Application xlApp = (Excel.Application)Globals.SCADMain.Application;
            Excel.Workbook Wind = xlApp.Workbooks.Open(@"\\Fs1\ENGUSERS\DESIGN\XXXXX New Wood Project Folder\3a Wind Loads\Wind Spreadsheetv3.0.xlsm");
        }

        private void OpenSeismic_Click(object sender, RibbonControlEventArgs e)
        {
            /* Opens the ASCE 7-05 Seismic workbook that exists on the SCA "ENGUSERS" server.*/
            Excel.Application xlApp = (Excel.Application)Globals.SCADMain.Application;
            Excel.Workbook Seismic = xlApp.Workbooks.Open(@"\\Fs1\ENGUSERS\DESIGN\XXXXX New Wood Project Folder\3b Seismic Loads\IBC 2006 Seismic.xlsm");
        }

        private void OpenSeismic2_Click(object sender, RibbonControlEventArgs e)
        {
            /* Opens the ASCE 7-10 Seismic workbook that exists on the SCA "ENGUSERS" server.*/
            Excel.Application xlApp = (Excel.Application)Globals.SCADMain.Application;
            Excel.Workbook Seismic2 = xlApp.Workbooks.Open(@"\\Fs1\ENGUSERS\DESIGN\XXXXX New Wood Project Folder\3b Seismic Loads\IBC 2012 Seismic.xlsm");
        }

        private void OpenWoodBrickDiff_Click(object sender, RibbonControlEventArgs e)
        {
            /* Opens the Wood/Brick Differential/Shrinkage workbook that exists on the SCA "ENGUSERS" server.*/
            Excel.Application xlApp = (Excel.Application)Globals.SCADMain.Application;
            Excel.Workbook BrickDiff = xlApp.Workbooks.Open(@"\\Fs1\ENGUSERS\DESIGN\XXXXX New Wood Project Folder\7c Brick-Wood Differential\Shrinkagev1.0.xlsm");
        }

        private void OpenPeriod_Click(object sender, RibbonControlEventArgs e)
        {
            /* Opens the Building Period/T Calcs workbook that exists on the SCA "ENGUSERS" server.*/
            Excel.Application xlApp = (Excel.Application)Globals.SCADMain.Application;
            Excel.Workbook Period = xlApp.Workbooks.Open(@"\\Fs1\ENGUSERS\DESIGN\Codes (Wind and Seismic)\Seismic\T calcs.xlsm");
        }
    }
}
