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
        /************************************************************
         *            ;tCG0GCG0CLffLi                               *
         *          ;LC0         ,CCi                               *
         *          tLLL:          fi                               *
         *           LLLLLLt,                                       *
         *            tGLLLLCLLt:     ...      ..                   *
         *                :GGLLLLLLLGGCt1fCGGLttLi                  *
         *          i         iLCLLLLC          CLL                 *
         *          1C       fLLC;tLLG.          ,G                 *
         *          1LLL    LCLLLiLC0.            ..                *
         *          t0  ;C0CLLLC0Gi             .                   *
         *                    1LLLL.           1LL                  *
         *                    :LLLLL         .tCLLL                 *
         *                     LLLLL,      ;fG,  LLiG               *
         *                       fCLLLt.  iLG.  tCLLt               *
         *                           tG00CC000CL.iLLCL              *
         *                              1LC       ;LLLC             *
         *                             tC1          LLLC.           *
         *                             fG,            CLLL.         *
         *                           iLC;              fLLLf        *
         *                        :iiLCCCCC         fCCCCCCCCCC     *
         ********* STRUCTURAL CAD ANALYSIS & DESIGN TOOLS ***********
         * Version 2.0                       Release: May 2014      *   
         * Company: SCA Consulting Engineers          © 2014        *
         *          12511 Emily Court                               *
         *          Sugar Land, TX 77478                            *
         ************************************************************
         * Revision History:                                        *
         * + 2.0 - May 2014, Shaun Smith                            *
         *      - Migration to C# and .NET framework                *
         *      - Optimizations and Enchancements                   *
         ************************************************************/


        /* Initialization method for the SCAD Add-In*/
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

        /******************** STUD DESIGN methods *******************/

        // StudDesign() -- Begins initial Stud Design from Data.
        public string StudDesign()
        {
            /* StudDesign() -- called by clicking "Launch SCAD" button on SCAD Ribbon
             * Proceeds to process Raw AutoCAD Stud data from Excel file and passes design
             * information from the user.*/

            // Testing interaction between SCADMain and SCADRibbon buttons.
            MessageBox.Show("This is in SCADMain.");
            Excel.Worksheet activeSheet = Application.ActiveSheet;
            Excel.Range FirstRow = activeSheet.get_Range("A1");
            FirstRow.Value2 = "Stud stuff has been done to this now.";

            // Testing launching and return of StudLaunch Form
            SCAD.StudLaunch StudForm = new SCAD.StudLaunch();
            StudForm.ShowDialog();

            return "Now back to SCADRibbon.";
        }

        // StudExport() -- Creates an AutoCAD script file of Stud Design.
        public string StudExport()
        {
            /* StudExport() -- called by clicking "Create Script" on SCAD Ribbon.
             * Passes desired script options from form and then copies existing script
             * data on Stud Design workbook into a AutoCAD script file (*.scr) in Notepad.*/

            // Create instance of StudExport form in Modal mode.
            SCAD.StudExport StudExportForm = new SCAD.StudExport();
            StudExportForm.ShowDialog();

            // If Cancel is clicked, so prompt isn't displayed.
            if (StudExportForm.StudExportOptions[6] == true)
            {
                return null;
            }

            // Check to see if in Stud Design workbook
            {
                bool found = false;
                foreach (Excel.Worksheet sheet in this.Application.Sheets)
                {
                    if (sheet.Name == "STUD ANALYSIS")
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return "This routine may only be called from the Stud Design workbook after initial SCAD stud design has been completed.";
                }
            }

            // Select 'Create Script' worksheet and then autofill existing formulas to populate script data.
            Excel.Worksheet CreateScript = Application.Worksheets.get_Item("Create Script");
            CreateScript.Select();

            if (CreateScript.get_Range("B7").Value == null) // Check if autofill already completed
            {
                CreateScript.get_Range("B6").AutoFill(CreateScript.get_Range("B6", "B10005"));    // Master Script file data
                CreateScript.get_Range("C6").AutoFill(CreateScript.get_Range("C6", "C10005"));    // Rank of Master data
                CreateScript.get_Range("E6").AutoFill(CreateScript.get_Range("E6", "E2405"));     // Stud Wall Design data
                CreateScript.get_Range("F6").AutoFill(CreateScript.get_Range("F6", "F2405"));     // Rank of Wall Design data
                CreateScript.get_Range("H6").AutoFill(CreateScript.get_Range("H6", "H2405"));     // Stud Endpoint data
                CreateScript.get_Range("I6").AutoFill(CreateScript.get_Range("I6", "I2405"));     // Rank of Endpoint data
                CreateScript.get_Range("K6").AutoFill(CreateScript.get_Range("K6", "K2405"));     // Stud Endpoint data - Column 2
                CreateScript.get_Range("N6").AutoFill(CreateScript.get_Range("N6", "N2405"));     // Foundation Reaction data
                CreateScript.get_Range("O6").AutoFill(CreateScript.get_Range("O6", "O2405"));     // Rank of Foundation Reaction data
                CreateScript.get_Range("Q6").AutoFill(CreateScript.get_Range("Q6", "Q2405"));     // Stud Wall Name data
                CreateScript.get_Range("R6").AutoFill(CreateScript.get_Range("R6", "R2405"));     // Rank of Wall Name data
                CreateScript.get_Range("T6").AutoFill(CreateScript.get_Range("T6", "T2405"));     // Keyplan data
                CreateScript.get_Range("U6").AutoFill(CreateScript.get_Range("U6", "U2405"));     // Rank of Keyplan data
            }

            // Assign bool values from Form to determine what to include in Script, then ReCalculates formulas on worksheet
            {
                CreateScript.get_Range("R1").Value2 = StudExportForm.StudExportOptions[0];  // Stud Wall Names
                CreateScript.get_Range("F1").Value2 = StudExportForm.StudExportOptions[1];  // Stud Wall Design
                CreateScript.get_Range("I1").Value2 = StudExportForm.StudExportOptions[2];  // Key Plan Numbers
                CreateScript.get_Range("L1").Value2 = StudExportForm.StudExportOptions[2];  // Key Plan Numbers
                CreateScript.get_Range("O1").Value2 = StudExportForm.StudExportOptions[3];  // Stud Wall Endpoints
                CreateScript.get_Range("U1").Value2 = StudExportForm.StudExportOptions[4];  // Foundation Reactions
                CreateScript.get_Range("X1").Value2 = StudExportForm.StudExportOptions[5];  // Stud Schedule

                ((Excel._Worksheet)CreateScript).Calculate();
            }

            // Determines number of rows in master data list and copies it into Notepad as "Template.scr"
            try
            {
                string maxLines = System.Convert.ToString(CreateScript.get_Range("A2").Value + 5);                  // Find Max range of Column B
                Excel.Range ScriptRange = CreateScript.get_Range("B1", "B" + maxLines);                            
                System.Array ScriptVals = (System.Array)ScriptRange.Value;                                          // Copy range and convert to string array
                string[] ScriptText = ScriptVals.OfType<object>().Select(o => o.ToString()).ToArray();

                string userName = Environment.UserName;                                                             // Determine user name to find directory
                string fileName = "Template.scr";
                int i = 1;

                while (System.IO.File.Exists(@"C:\Users\" + userName + @"\Desktop\" + fileName))                    // Check to see if file exists. Increment if it does.  
                {
                    fileName = "Template" + i + ".scr";
                    i++;
                }

                System.IO.File.WriteAllLines(@"C:\Users\" + userName + @"\Desktop\" + fileName, ScriptText);        // Create Script file on User's Desktop and display the file location.
                MessageBox.Show("The AutoCAD Script file has been created and can be found on the Desktop at:\n"
                    + @"C:\Users\" + userName + @"\Desktop\" + fileName);
                System.Diagnostics.Process.Start("explorer.exe", @"/select, C:\Users\" + userName + @"\Desktop\" + fileName);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }

            return null;
        }

        /***************** End STUD DESIGN methods ******************/

        /***************** LATERAL DESIGN methods *******************/

        // LateralDesign() -- Begins Prelim Lateral Design from Data.
        public string LateralDesign()
        {
            // Testing interaction between SCADMain and SCADRibbon buttons.
            MessageBox.Show("This is in SCADMain.");
            Excel.Worksheet activeSheet = Application.ActiveSheet;
            Excel.Range FirstRow = activeSheet.get_Range("A1");
            FirstRow.Value2 = "Lateral stuff has been done to this now.";
            return "Now back to SCADRibbon.";
        }

        // LateralExport() -- Creates AutoCAD script file of Lateral Design.
        public string LateralExport()
        {
            /* LateralExport() -- called by clicking "Create Script" on SCAD Ribbon in Lateral 
             * tools. Passes desired script options from form and then copies existing script
             * data on Lateral Design workbook into a AutoCAD script file (*.scr) in Notepad.*/

            // Create instance of LateralExport form in Modal mode.
            SCAD.LateralExport LateralExportForm = new SCAD.LateralExport();
            LateralExportForm.ShowDialog();

            // If Cancel is clicked, so prompt isn't displayed.
            if (LateralExportForm.LateralExportOptions[6] == true)
            {
                return null;
            }

            // Check to see if in Lateral Design workbook
            {
                bool found = false;
                foreach (Excel.Worksheet sheet in this.Application.Sheets)
                {
                    if (sheet.Name == "Iteration")
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    return "This routine may only be called from the Lateral Design workbook after preliminary SCAD stud design has been completed.";
                }
            }

            // Select 'Script File' worksheet and assign bool values from Form to determine what to include in Script
            Excel.Worksheet wsScriptFile = Application.Worksheets.get_Item("Script File");
            wsScriptFile.Select();
            {
                wsScriptFile.get_Range("O1").Value2 = LateralExportForm.LateralExportOptions[0];  // Shear Wall Names
                wsScriptFile.get_Range("F1").Value2 = LateralExportForm.LateralExportOptions[1];  // Shear Wall Design
                wsScriptFile.get_Range("R1").Value2 = LateralExportForm.LateralExportOptions[2];  // Shear Wall Length
                wsScriptFile.get_Range("I1").Value2 = LateralExportForm.LateralExportOptions[3];  // Shear Wall Anchors
                wsScriptFile.get_Range("L1").Value2 = LateralExportForm.LateralExportOptions[4];  // Shear Wall Endpoints
                wsScriptFile.get_Range("U1").Value2 = LateralExportForm.LateralExportOptions[5];  // Drag Forces

                ((Excel._Worksheet)wsScriptFile).Calculate();
            }

            // Determines number of rows in master data list and copies it into Notepad as "Template.scr"
            {
                string maxLines = System.Convert.ToString(wsScriptFile.get_Range("A2").Value + 4);                  // Find Max range of Column B
                Excel.Range ScriptRange = wsScriptFile.get_Range("B1", "B" + maxLines);
                System.Array ScriptVals = (System.Array)ScriptRange.Value;                                          // Copy range and convert to string array
                string[] ScriptText = ScriptVals.OfType<object>().Select(o => o.ToString()).ToArray();

                string userName = Environment.UserName;                                                             // Determine user name to find directory
                string fileName = "Template.scr";
                int i = 1;

                while (System.IO.File.Exists(@"C:\Users\" + userName + @"\Desktop\" + fileName))                    // Check to see if file exists. Increment if it does.  
                {
                    fileName = "Template" + i + ".scr";
                    i++;
                }

                System.IO.File.WriteAllLines(@"C:\Users\" + userName + @"\Desktop\" + fileName, ScriptText);        // Create Script file on User's Desktop and display the file location.
                MessageBox.Show("The AutoCAD Script file has been created and can be found on the Desktop at:\n"
                    + @"C:\Users\" + userName + @"\Desktop\" + fileName);
                System.Diagnostics.Process.Start("explorer.exe", @"/select, C:\Users\" + userName + @"\Desktop\" + fileName);
            }

            return null;
        }
        /***************** End LATERAL DESIGN methods ***************/

        /* Shutdown method for the SCAD Add-In */
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
