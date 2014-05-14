using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using System.Windows.Forms;

namespace SCAD
{
    public partial class SCADMain
    {
        private void SCADMain_Startup(object sender, System.EventArgs e)
        {
            // Testing to ensure add-in has loaded properly.
            Excel.Workbook Wbook = this.Application.Workbooks.Add(System.Type.Missing);
            MessageBox.Show("The SCAD add-in has been initiated.");

            // Writes to activesheet to verify permissions.
            Excel.Worksheet activeSheet = Application.ActiveSheet;
            Excel.Range FirstRow = activeSheet.get_Range("A1");
            FirstRow.Value2 = "Testing SCAD things.";
        }

        public string StudDesign()
        {
            // Testing interaction between SCADMain and SCADRibbon buttons.
            MessageBox.Show("This is in SCADMain.");
            Excel.Worksheet activeSheet = Application.ActiveSheet;
            Excel.Range FirstRow = activeSheet.get_Range("A1");
            FirstRow.Value2 = "Stud stuff has been done to this now.";
            return "Now back to SCADRibbon.";
        }

        public string LateralDesign()
        {
            // Testing interaction between SCADMain and SCADRibbon buttons.
            MessageBox.Show("This is in SCADMain.");
            Excel.Worksheet activeSheet = Application.ActiveSheet;
            Excel.Range FirstRow = activeSheet.get_Range("A1");
            FirstRow.Value2 = "Lateral stuff has been done to this now.";
            return "Now back to SCADRibbon.";
        }

        private void SCADMain_Shutdown(object sender, System.EventArgs e)
        {
            // Testing to ensure add-in unloads properly.
            Excel.Workbook Wbook = this.Application.Workbooks.Add(System.Type.Missing);
            MessageBox.Show("The SCAD add-in has been unloaded.");
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(SCADMain_Startup);
            this.Shutdown += new System.EventHandler(SCADMain_Shutdown);
        }
        
        #endregion
    }
}
