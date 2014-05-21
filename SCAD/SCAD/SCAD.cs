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

            return null;
        }

        // StudLineReports() -- Creates PDF reports of flagged stud lines.
        public string StudLineReports()
        {
            SCAD.StudPrintLines StudReportForm = new SCAD.StudPrintLines();
            StudReportForm.ShowDialog();

            // Return to main if the Cancel button is clicked.
            if (StudReportForm.PrintCurrent == false && StudReportForm.PrintUnique == false)
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
       
            // Routine if unique stud lines option is selected.
            if (StudReportForm.PrintUnique == true)
                {
                    Excel.Worksheet wsInput = Application.Worksheets.get_Item("INPUT");
                    int levels = (int)wsInput.get_Range("D7").Value;
                    Globals.SCADMain.StudUniqueReports(levels);
                }

            // Routine to make reports of all flagged lines.
            {
                MessageBox.Show("Print Selected Lines.");
            }

            return "Now back to SCAD Ribbon";
        }

        // StudUniqueReports() -- Sets up workbook to make reports for all unique walls on each level.
        public void StudUniqueReports(int levels)
        {
            // Declarations
            int iStud, iStud1, iStud2, iStud3, iStud4, iStud5, iStud6; // Stores total number of stud lines for each level

            Excel.Worksheet wsOutput = Application.Worksheets.get_Item("OUTPUT");
            Excel.Worksheet wsInput = Application.Worksheets.get_Item("INPUT");
            Excel.Worksheet wsAnalysis = Application.Worksheets.get_Item("STUD ANALYSIS");
            Excel.Worksheet wsCalcTable1 = Application.Worksheets.get_Item("L1 Calc Table");

            // Establish total number of stud walls and those on level 1 through used row ranges
            iStud = wsOutput.UsedRange.Rows.Count - 2;
            iStud1 = wsCalcTable1.UsedRange.Rows.Count - 5;

            Globals.SCADMain.Application.Calculation = Excel.XlCalculation.xlCalculationManual; // Set to manual calculation while flagging lines
            
            // Treatment of the workbook and formulas that accomodate all levels
            {
                // Append Level to Job # so folder is not overwritten when reports are made
                wsInput.get_Range("K4").Formula = @"=CONCATENATE(IFERROR(LEFT(J4,FIND(""-"",J4,1)-1),J4),""-1"")";
                wsInput.get_Range("K4").Copy();
                wsInput.get_Range("J4").PasteSpecial(Excel.XlPasteType.xlPasteValues);
                wsInput.get_Range("K4").ClearContents();

                // Setting up Stud Analysis worksheet to include Key Plan # in reporting documents
                wsAnalysis.get_Range("B5").Value = "Key Plan #:";
                wsAnalysis.get_Range("C5").Formula = @"=IFERROR(INDEX('OUTPUT'!AJ3:AJ" + (iStud + 2) + @",MATCH(D6,'OUTPUT'!AI3:AI" + (iStud + 2) + @",0),1),"""")";
                wsAnalysis.get_Range("C5").Font.Name = "Arial";
                wsAnalysis.get_Range("C5").Font.Size = 14;
                wsAnalysis.get_Range("C5").Font.Bold = true;

                // Setup OUTPUT sheet to make create reports with Key Plan numbers for Level 1
                wsOutput.get_Range("AK1").Value = "KEY PLAN NUMBERS";
                wsOutput.get_Range("AJ2").Value = "#";
                wsOutput.get_Range("AK2").Value = "X-Coord.";
                wsOutput.get_Range("AL2").Value = "Y-Coord.";
                wsOutput.get_Range("AM2").Value = "Z-Coord.";
                wsOutput.get_Range("AI3").Formula = "=C3";
                wsOutput.get_Range("AI3").AutoFill(wsOutput.get_Range("AI3", "AI" + (iStud + 2)));

                // Establish formula for Key Plan report numbers, dependent on number of levels
                switch (levels)
                {
                    case 1 :
                        wsOutput.get_Range("AJ3").Formula = @"=IFERROR(MAX(INDIRECT(CONCATENATE(""'L1 Calc Table'!BS"",MATCH(AI3,'L1 Calc Table'!$B$6:$B$" + 
                            (iStud + 2) + @",0)+5)):INDIRECT(CONCATENATE(""'L1 Calc Table'!HN"",MATCH(AI3,'L1 Calc Table'!$B$6:$B$" + (iStud + 2) + @",0)+5))),"""")";
                        break;
                    case 2 :
                        wsOutput.get_Range("AJ3").Formula = @"=IFERROR(MAX(INDIRECT(CONCATENATE(""'L1 Calc Table'!BS"",MATCH(AI3,'L1 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L1 Calc Table'!HN"",MATCH(AI3,'L1 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5))),IFERROR(MAX(INDIRECT(CONCATENATE(""'L2 Calc Table'!BS"",MATCH(AI3,'L2 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L2 Calc Table'!HN"",MATCH(AI3,'L2 Calc Table'!$B$6:$B$" + (iStud + 2) + @",0)+5))),""""))";
                        break;
                    case 3 :
                        wsOutput.get_Range("AJ3").Formula = @"=IFERROR(MAX(INDIRECT(CONCATENATE(""'L1 Calc Table'!BS"",MATCH(AI3,'L1 Calc Table'!$B$6:$B$" + (iStud + 2) 
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L1 Calc Table'!HN"",MATCH(AI3,'L1 Calc Table'!$B$6:$B$" + (iStud + 2) 
                            + @",0)+5))),IFERROR(MAX(INDIRECT(CONCATENATE(""'L2 Calc Table'!BS"",MATCH(AI3,'L2 Calc Table'!$B$6:$B$" + (iStud + 2) 
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L2 Calc Table'!HN"",MATCH(AI3,'L2 Calc Table'!$B$6:$B$" + (iStud + 2) 
                            + @",0)+5))),IFERROR(MAX(INDIRECT(CONCATENATE(""'L3 Calc Table'!BS"",MATCH(AI3,'L3 Calc Table'!$B$6:$B$" + (iStud + 2) 
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L3 Calc Table'!HN"",MATCH(AI3,'L3 Calc Table'!$B$6:$B$" + (iStud + 2) + @",0)+5))),"""")))";
                        break;
                    case 4 :
                        wsOutput.get_Range("AJ3").Formula = @"=IFERROR(MAX(INDIRECT(CONCATENATE(""'L1 Calc Table'!BS"",MATCH(AI3,'L1 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L1 Calc Table'!HN"",MATCH(AI3,'L1 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5))),IFERROR(MAX(INDIRECT(CONCATENATE(""'L2 Calc Table'!BS"",MATCH(AI3,'L2 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L2 Calc Table'!HN"",MATCH(AI3,'L2 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5))),IFERROR(MAX(INDIRECT(CONCATENATE(""'L3 Calc Table'!BS"",MATCH(AI3,'L3 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L3 Calc Table'!HN"",MATCH(AI3,'L3 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5))),IFERROR(MAX(INDIRECT(CONCATENATE(""'L4 Calc Table'!BS"",MATCH(AI3,'L4 Calc Table'!$B$6:$B$" + (iStud + 2) 
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L4 Calc Table'!HN"",MATCH(AI3,'L4 Calc Table'!$B$6:$B$" + (iStud + 2) + @",0)+5))),""""))))";
                        break;
                    case 5 :
                        wsOutput.get_Range("AJ3").Formula = @"=IFERROR(MAX(INDIRECT(CONCATENATE(""'L1 Calc Table'!BS"",MATCH(AI3,'L1 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L1 Calc Table'!HN"",MATCH(AI3,'L1 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5))),IFERROR(MAX(INDIRECT(CONCATENATE(""'L2 Calc Table'!BS"",MATCH(AI3,'L2 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L2 Calc Table'!HN"",MATCH(AI3,'L2 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5))),IFERROR(MAX(INDIRECT(CONCATENATE(""'L3 Calc Table'!BS"",MATCH(AI3,'L3 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L3 Calc Table'!HN"",MATCH(AI3,'L3 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5))),IFERROR(MAX(INDIRECT(CONCATENATE(""'L4 Calc Table'!BS"",MATCH(AI3,'L4 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L4 Calc Table'!HN"",MATCH(AI3,'L4 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5))),IFERROR(MAX(INDIRECT(CONCATENATE(""'L5 Calc Table'!BS"",MATCH(AI3,'L5 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L5 Calc Table'!HN"",MATCH(AI3,'L5 Calc Table'!$B$6:$B$" + (iStud + 2) + @",0)+5))),"""")))))";
                        break;
                    case 6 :
                        wsOutput.get_Range("AJ3").Formula = @"=IFERROR(MAX(INDIRECT(CONCATENATE(""'L1 Calc Table'!BS"",MATCH(AI3,'L1 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L1 Calc Table'!HN"",MATCH(AI3,'L1 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5))),IFERROR(MAX(INDIRECT(CONCATENATE(""'L2 Calc Table'!BS"",MATCH(AI3,'L2 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L2 Calc Table'!HN"",MATCH(AI3,'L2 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5))),IFERROR(MAX(INDIRECT(CONCATENATE(""'L3 Calc Table'!BS"",MATCH(AI3,'L3 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L3 Calc Table'!HN"",MATCH(AI3,'L3 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5))),IFERROR(MAX(INDIRECT(CONCATENATE(""'L4 Calc Table'!BS"",MATCH(AI3,'L4 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L4 Calc Table'!HN"",MATCH(AI3,'L4 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5))),IFERROR(MAX(INDIRECT(CONCATENATE(""'L5 Calc Table'!BS"",MATCH(AI3,'L5 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L5 Calc Table'!HN"",MATCH(AI3,'L5 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5))),IFERROR(MAX(INDIRECT(CONCATENATE(""'L6 Calc Table'!BS"",MATCH(AI3,'L6 Calc Table'!$B$6:$B$" + (iStud + 2)
                            + @",0)+5)):INDIRECT(CONCATENATE(""'L6 Calc Table'!HN"",MATCH(AI3,'L6 Calc Table'!$B$6:$B$" + (iStud + 2) + @",0)+5))),""""))))))";
                        break;
                }
                wsOutput.get_Range("AJ3").AutoFill(wsOutput.get_Range("AJ3", "AJ" + iStud + 2));

                // Create Formula and Autofill for X-Coordinates of each wall
                wsOutput.get_Range("AK3").Formula = @"=INDEX($K$3:$K$" + (iStud + 2) + @",MATCH(AI3,$C$3:$C$" + (iStud + 2) + @",0),1)";
                wsOutput.get_Range("AK3").AutoFill(wsOutput.get_Range("AK3", "AK" + iStud + 2));

                // Create Formula and Autofill for Y-Coordinates of each wall
                wsOutput.get_Range("AL3").Formula = @"=INDEX($L$3:$L$" + (iStud + 2) + @",MATCH(AI3,$C$3:$C$" + (iStud + 2) + @",0),1)";
                wsOutput.get_Range("AL3").AutoFill(wsOutput.get_Range("AL3", "AL" + iStud + 2));

                // Place zero values for Z-Coordinates of each wall
                wsOutput.get_Range("AM3").Formula = "0";
                wsOutput.get_Range("AM4").Formula = "0";
                wsOutput.get_Range("AM3","AM4").AutoFill(wsOutput.get_Range("AM3", "AM" + iStud + 2));

                ((Excel._Worksheet)wsOutput).Calculate();
            }

            // Level 1 treatment of flagging lines
            {
                // Formula to determine if "Print All" is enabled, or if stud line is unique on its floor
                wsCalcTable1.get_Range("A6").Formula = @"=IF($B$4=""Yes"",""Yes"",IF(BS6>0,""Yes"",""No""))";
                wsCalcTable1.get_Range("A6").AutoFill(wsCalcTable1.get_Range("A6","A" + (iStud1 + 6)));

                // Formula for assigning Key Plan numbers to walls
                wsCalcTable1.get_Range("BS6").Value = 101;
                wsCalcTable1.get_Range("BS7").Formula = @"=IFERROR(IF(MATCH(MAX(BT7:EP7),BS$6:BS6,0)>0,0,0),MAX(BT7:EP7))";
                wsCalcTable1.get_Range("BS7").AutoFill(wsCalcTable1.get_Range("BS7", "BS" + (iStud1 + 6)));

                // Null columns and rows while formula determines Key Plan numbers
                wsCalcTable1.get_Range("BT6").Value = 0;
                wsCalcTable1.get_Range("BT7").Value = 0;
                wsCalcTable1.get_Range("BT6","BT7").AutoFill(wsCalcTable1.get_Range("BT6","BT" + (iStud1 + 6)));
                wsCalcTable1.get_Range("BV6").Value = 0;
                wsCalcTable1.get_Range("BW6").Value = 0;
                wsCalcTable1.get_Range("BV6", "BW6").AutoFill(wsCalcTable1.get_Range("BV6", "HN6"));

                // Sets the initial unique Key Plan number
                wsCalcTable1.get_Range("BU6").Formula = "=BT6";

                // Creates Header Row for Key Plan Numbers
                wsCalcTable1.get_Range("BU5").Value = 101;
                wsCalcTable1.get_Range("BV5").Value = 102;
                wsCalcTable1.get_Range("BU5","BV5").AutoFill(wsCalcTable1.get_Range("BU5","HN5"));

                // Formula for determining unique Key Plan numbers for each wall
                wsCalcTable1.get_Range("BU7").Formula = @"=IF(AND(SUM($BT7:BT7)=0,SUM(BU$6:BU6)=0),BU$5,IFERROR(IF(AND($C7=(INDIRECT(""C""&MATCH(BU$5,BU$6:BU6,0)+5)),"
                    + @"$AD7=(INDIRECT(""AD""&MATCH(BU$5,BU$6:BU6,0)+5)),$AE7=(INDIRECT(""AE""&MATCH(BU$5,BU$6:BU6,0)+5)),$Z7=(INDIRECT(""Z""&MATCH(BU$5,BU$6:BU6,0)+5))"
                    + @",ABS($H7-(INDIRECT(""H""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($I7-(INDIRECT(""I""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($J7-(INDIRECT(""J""&MATCH"
                    + @"(BU$5,BU$6:BU6,0)+5)))<=1,ABS($K7-(INDIRECT(""K""&MATCH(BU$5,BU$6:BU6,0)+5)))<=1,ABS($L7-(INDIRECT(""L""&MATCH(BU$5,BU$6:BU6,0)+5))<=1),ABS($M7-"
                    + @"(INDIRECT(""M""&MATCH(BU$5,BU$6:BU6,0)+5)))<=1,ABS($N7-(INDIRECT(""N""&MATCH(BU$5,BU$6:BU6,0)+5)))<=1,ABS($O7-(INDIRECT(""O""&MATCH(BU$5,BU$6:BU6,0)"
                    + @"+5)))<=100,ABS($P7-(INDIRECT(""P""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($Q7-(INDIRECT(""Q""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($R7-(INDIRECT(""R""&"
                    + @"MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($S7-(INDIRECT(""S""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($T7-(INDIRECT(""T""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS"
                    + @"($U7-(INDIRECT(""U""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($V7-(INDIRECT(""V""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($W7-(INDIRECT(""W""&MATCH(BU$5,BU$6:BU6,0)+5)))"
                    + @"<=100,ABS($X7-(INDIRECT(""X""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($Y7-(INDIRECT(""Y""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100),BU$5,0),0))";
                wsCalcTable1.get_Range("BU7").AutoFill(wsCalcTable1.get_Range("BU7", "HN7"));
                wsCalcTable1.get_Range("BU7", "HN7").AutoFill(wsCalcTable1.get_Range("BU7", "HN" + (iStud1 + 6)));

                ((Excel._Worksheet)wsCalcTable1).Calculate();

                // Copy and Paste formula Values to reduce file size and increase reporting speed
                wsCalcTable1.get_Range("BS6", "HN" + (iStud1 + 6)).Copy();
                wsCalcTable1.get_Range("BS6", "HN" + (iStud1 + 6)).PasteSpecial(Excel.XlPasteType.xlPasteValues);

                // Return to Automatic Calculation mode
                Globals.SCADMain.Application.Calculation = Excel.XlCalculation.xlCalculationAutomatic;
            }

            // Level 2 treatment of flagging lines
            if (levels > 1)
            {
                // Establish total number of stud walls on level 2 through used row ranges
                Excel.Worksheet wsCalcTable2 = Application.Worksheets.get_Item("L2 Calc Table");
                iStud2 = wsCalcTable2.UsedRange.Rows.Count - 5;
            }

            // Level 3 treatment of flagging lines
            if (levels > 2)
            {
                // Establish total number of stud walls on level 3 through used row ranges
                Excel.Worksheet wsCalcTable3 = Application.Worksheets.get_Item("L3 Calc Table");
                iStud3 = wsCalcTable3.UsedRange.Rows.Count - 5;
            }

            // Level 4 treatment of flagging lines
            if (levels > 3)
            {
                // Establish total number of stud walls on level 4 through used row ranges
                Excel.Worksheet wsCalcTable4 = Application.Worksheets.get_Item("L4 Calc Table");
                iStud4 = wsCalcTable4.UsedRange.Rows.Count - 5;
            }

            // Level 5 treatment of flagging lines
            if (levels > 4)
            {
                // Establish total number of stud walls on level 5 through used row ranges
                Excel.Worksheet wsCalcTable5 = Application.Worksheets.get_Item("L5 Calc Table");
                iStud5 = wsCalcTable5.UsedRange.Rows.Count - 5;
            }

            // Level 6 treatment of flagging lines
            if (levels > 5)
            {
                // Establish total number of stud walls on level 6 through used row ranges
                Excel.Worksheet wsCalcTable6 = Application.Worksheets.get_Item("L6 Calc Table");
                iStud6 = wsCalcTable6.UsedRange.Rows.Count - 5;
            }
            
            return;
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
