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
         *            tGLLLLCLLt:     ..........                   *
         *                :GGLLLLLLLGGCt1fCGGLttLi                  *
         *          i         iLCLLLLC          CLL                 *
         *          1C        LLC;tLLG.          ,G                 *
         *          1LLL    LCLLLiLC0.            ..                *
         *          t0  ;C0CLLLC0Gi             .                   *
         *                    1LLLL.           1LL                  *
         *                    :LLLLL         .tCLLL                 *
         *                     LLLLL,      ;fG,  LLiG               *
         *                       fCLLLt.  iLG.. .CLLt               *
         *                           tG00CC000CL.iLLCL              *
         *                              1LC       ;LLLC             *
         *                             tC1          LLLC.           *
         *                            tfG,            CLLL.         *
         *                           iLC;              fLLL;        *
         *                        :iiLCCCCC         fCCCCCCCCCC     *
         ************************** SCAD ****************************
         ********* STRUCTURAL CAD ANALYSIS & DESIGN TOOLS ***********
         * Version 2.0                       Release: May 2014      *   
         * Company: SCA Consulting Engineers          © 2014        *
         *          12511 Emily Court                               *
         *          Sugar Land, TX 77478                            *
         ************************************************************
         * Revision History:                                        *
         * + 2.0  May 2014, Shaun Smith                             *
         *      - Migration to C# and .NET framework                *
         *      - Optimizations and Enchancements                   *
         *      - Changed Lateral Reports from printing to PDF      *
         *          creation                                        *
         *      - Removed redundant horizontal matching reports     *
         *      - Resolved horiz. matching values for Y-dir studs   *
         ************************************************************/


        /* Initialization method for the SCAD Add-In*/
        private void SCADMain_Startup(object sender, System.EventArgs e)
        {

        }

        // MkReportDirs() -- Creates Unique Report Directories.
        public void MkReportDirs(string JobNumber)
        {
            /* MkReportDirs() -- Creates report directories for pdf reports to be
             * placed into from both Stud and Lateral reporting schemes. Uses the
             * Job Number supplied by the user as the directory name, and defaults
             * to Temp if none is given */

            // Check if Report Directories exists and delete them
            if (System.IO.Directory.Exists(@"C:\SCAD\Reports\" + JobNumber + @"\"))
            {
                System.IO.Directory.Delete(@"C:\SCAD\Reports\" + JobNumber + @"\", true);
            }
            if (System.IO.Directory.Exists(@"C:\SCAD\Reports\Temp\"))
            {
                System.IO.Directory.Delete(@"C:\SCAD\Reports\Temp\", true);
            }

            // Create new report directories, directory is "Temp" if no Job number exists
            if (JobNumber == "")
            {
                System.IO.Directory.CreateDirectory(@"C:\SCAD\Reports\Temp\");
            }
            else
            {
                System.IO.Directory.CreateDirectory(@"C:\SCAD\Reports\" + JobNumber + @"\");
            }

            return;
        }

        // RawLineData -- Class that caters to all the data for individual stud lines exported from AutoCAD
        public class RawLineData
        {
            public string label { get; set; }
            public string layer { get; set; }
            public bool angled { get; set; }
            public char direction { get; set; }
            public float length { get; set; }
            public float Xstart { get; set; }
            public float Xend { get; set; }
            public float Ystart { get; set; }
            public float Yend { get; set; }
            public float slope { get; set; }
            public float Yintercept { get; set; }
            public float startGapLength { get; set; }
            public float endGapLength { get; set; }
            public int level { get; set; }
            public char studClass { get; set; }                 // Designates Interior/Exterior Wall
            public int studThickness { get; set; }
            public List<trussMatch> trussMatches;

            public RawLineData()
            {
                this.trussMatches = new List<trussMatch>();
            }

            public void addTrussMatch(string label, char type, int length)
            {
                trussMatches.Add(new trussMatch()
                {
                    trussLabel = label,
                    trussType = type,
                    trussLength = length
                });
            }
        }

        public class trussMatch                                 // Stores matched truss data
        {
            public string trussLabel { get; set; }
            public char trussType { get; set; }
            public int trussLength { get; set; }
            public char trussAngled { get; set; }               // 'A' if both stud and truss are angled
        }

        /******************** STUD DESIGN methods *******************/

        // StudDesign() -- Begins initial Stud Design from Data.
        public string StudDesign()
        {
            /* StudDesign() -- called by clicking "Launch SCAD" button on SCAD Ribbon
             * Proceeds to process Raw AutoCAD Stud data from Excel file and passes design
             * information from the user.*/

            // Testing launching and return of StudLaunch Form
            SCAD.StudLaunch StudForm = new SCAD.StudLaunch();
            StudForm.ShowDialog();

            // If Cancel is clicked, so prompt isn't displayed.
            if (StudForm.StudCancel == true)
            {
                return null;
            }

            // Create local arrDataSort[] array from form values
            object[] arrDesignDataSort = new object[61];
            arrDesignDataSort = StudForm.arrDesignData;

            // Pass design parameter data array to sorting routine
            this.DataSort(arrDesignDataSort);
            
            return "Now back to SCADRibbon.";
        }

        // DataSort() -- Sorts Raw data from AutoCAD export file so it is ready for Horizontal and Vertical matching
        public void DataSort(object[] arrDesignData)
        {
            /* DataSort() -- called by StudDesign() after design parameters have been passed
                * from StudForm into arrRawData[] array. This routine then parses the AutoCAD
                * excel data file and sorts it into appropriate line type arrays (Stud Walls, Loads, etc). */

            // Declarations for arrays to hold sorted, unsorted, and line types.
            //List<StudLineData> arrSorted = new List<StudLineData>();
            List<RawLineData> arrNotSorted = new List<RawLineData>();
            List<RawLineData> arrStud = new List<RawLineData>();
            List<RawLineData> arrTruss = new List<RawLineData>();
            List<RawLineData> arrDiaphr = new List<RawLineData>();
            List<RawLineData> arrGap = new List<RawLineData>();
            List<RawLineData> arrShear = new List<RawLineData>();
            List<RawLineData> arrBeam = new List<RawLineData>();

            // Declarations for counters and sorting threshold constants
            float fReSort = new float();        // Temporary resorting container for coordinates
            int iLevel = new int();                 // Stores current working level for sorting
            const int iStraight = 5;                // Straight line threshold
            int j, k, m = new int();                // Counters to cycle through lines

            // Deactivate Screen Updating while sorting
            this.Application.ScreenUpdating = false;

            // Select Raw Data and begin assigning it to arrNotSorted, a list of the StudLineData class
            Excel.Worksheet wsRawData = Application.Worksheets.get_Item("Sheet1");
            int iColCount = wsRawData.UsedRange.Columns.Count + 9;
            int iRowCount = wsRawData.UsedRange.Rows.Count - 1;
            for (int i = 0; i < iRowCount; i++)
            {
                arrNotSorted.Add(new RawLineData()
                {
                    layer = wsRawData.get_Range("C" + (2 + i)).Text,
                    length = (float)wsRawData.get_Range("E" + (2+i)).Value,
                    Xstart = (float)wsRawData.get_Range("G" + (2 + i)).Value,
                    Xend = (float)wsRawData.get_Range("I" + (2 + i)).Value,
                    Ystart = (float)wsRawData.get_Range("H" + (2 + i)).Value,
                    Yend = (float)wsRawData.get_Range("J" + (2 + i)).Value
                });
            }

            // Open the Stud Design template from the share drive
            Excel.Workbook wbStudDesign = this.Application.Workbooks.Add(@"\\Fs1\ENGUSERS\DESIGN\SCAD Programs\Stud Program\Stud Templates\Stud_Design.xltm");

            /************ CATEGORIZATION OF DIRECTION/SLOPE/DOMINANT COORDS ************
                * Begin looping through arrNotSorted array to find: Angled Lines, Y vs X direction Lines, Slope, and re-arrange non-dominate coordinates
                * such that start coordinate is smaller than end coordinate.*/
            foreach (RawLineData lineNotSorted in arrNotSorted)
            {
                // Check for angled wall: If differences in both X/Y directions exceed iStraight threshold, declare angled
                if ((Math.Abs(lineNotSorted.Xstart - lineNotSorted.Xend)) > iStraight && (Math.Abs(lineNotSorted.Ystart - lineNotSorted.Yend)) > iStraight)
                {
                    lineNotSorted.angled = true;
                    lineNotSorted.direction = 'A';

                    // Re-sort so Start/End coordinates are in dominate order
                    if (lineNotSorted.Xend < lineNotSorted.Xstart)
                    {
                        fReSort = lineNotSorted.Xend;
                        lineNotSorted.Xend = lineNotSorted.Xstart;
                        lineNotSorted.Xstart = fReSort;

                        fReSort = lineNotSorted.Yend;
                        lineNotSorted.Yend = lineNotSorted.Ystart;
                        lineNotSorted.Ystart = fReSort;
                    }
                }

                // Assign false value for angled if line falls within iStraight threshold
                else
                {
                    lineNotSorted.angled = false;
                }

                // Re-sort so that X/Y Start/End coords are in dominate order
                if (lineNotSorted.Xend < lineNotSorted.Xstart)
                {
                    fReSort = lineNotSorted.Xend;
                    lineNotSorted.Xend = lineNotSorted.Xstart;
                    lineNotSorted.Xstart = fReSort;
                }
                if (lineNotSorted.Yend < lineNotSorted.Ystart)
                {
                    fReSort = lineNotSorted.Yend;
                    lineNotSorted.Yend = lineNotSorted.Ystart;
                    lineNotSorted.Ystart = fReSort;
                }

                // Determine line direction for straight lines
                if (lineNotSorted.angled == false)
                {
                    // Check if Y direction line (if X coords are within straight tolerance)
                    if ((lineNotSorted.Xend - lineNotSorted.Xstart) <= iStraight)
                    {
                        lineNotSorted.direction = 'Y';
                    }

                    // Else, since line is straight, assign direction as X, slope as zero, Y intercept as Start Y coordinate
                    else
                    {
                        lineNotSorted.direction = 'X';
                        lineNotSorted.slope = 0;
                        lineNotSorted.Yintercept = lineNotSorted.Ystart;
                    }
                }

                // If line is angled, find slope, Y-intercept and assign dominant direction (based on slope)
                if (lineNotSorted.angled == true)
                {
                    // Determine slope
                    lineNotSorted.slope = (lineNotSorted.Yend - lineNotSorted.Ystart) / (lineNotSorted.Xend - lineNotSorted.Xstart);

                    // Determine Y-intercept
                    lineNotSorted.Yintercept = lineNotSorted.Xstart * lineNotSorted.slope;
                    lineNotSorted.Yintercept = lineNotSorted.Yend - lineNotSorted.Yintercept;
                }
            }

            /************ SORTING BY Y COORDINATES ************/
            // Sort data from arrNotSorted into arrSorted, from smallest to largest Y end coord
            List<RawLineData> arrSorted = arrNotSorted.OrderBy(o => o.Yend).ToList();

            /************ IDENTIFY LEVELS BASED ON DIAPHRAGM LINES ************/
            iLevel = 0;
            // Identify Diaphragm Lines and assign level
            foreach (RawLineData lineSorted in arrSorted)
            {
                if (lineSorted.layer == "ENG_DIAPHR" && lineSorted.direction == 'Y')
                {
                    iLevel++;
                    lineSorted.level = iLevel;
                }
            }

            // Loop through arrSorted and apply level to each line based on Y-Coords of Diaphragm lines
            foreach (RawLineData diaphrElement in arrSorted)
            {
                // Find diaphragm lines
                if (diaphrElement.layer == "ENG_DIAPHR" && diaphrElement.direction == 'Y')                           
                {
                    foreach (RawLineData subElement in arrSorted)
                    {
                        // Match stud lines with diaphragm lines. Assign same level as diaphragm if falls between.
                        if (subElement.Ystart >= diaphrElement.Ystart && subElement.Yend <= diaphrElement.Yend)     
                        {
                            subElement.level = diaphrElement.level;                                                 
                        }
                    }
                }
            }

            /************ SUB DIVIDE INTO LINE TYPES LISTS ************/
            // Cycle through each line and Separate into different lists based on type/layer
            foreach (RawLineData element in arrSorted)
            {
                // Add element to arrGap list if Gap Line
                if (element.layer.Substring(0,7) == "ENG_GAP")
                {
                    arrGap.Add(new RawLineData()
                    {
                        layer = element.layer,
                        angled = element.angled,
                        direction = element.direction,
                        Xstart = element.Xstart,
                        Xend = element.Xend,
                        Ystart = element.Ystart,
                        Yend = element.Yend,
                        slope = element.slope,
                        Yintercept = element.Yintercept,
                        level = element.level
                    });
                }

                // Add element to arrDiaphr list if Diaphragm Line
                if (element.layer.Substring(0,7) == "ENG_DIA")
                {
                    arrDiaphr.Add(new RawLineData()
                    {
                        layer = element.layer,
                        angled = element.angled,
                        direction = element.direction,
                        Xstart = element.Xstart,
                        Xend = element.Xend,
                        Ystart = element.Ystart,
                        Yend = element.Yend,
                        slope = element.slope,
                        Yintercept = element.Yintercept,
                        level = element.level
                    });
                }

                // Add element to arrShear list if Shear Line
                if (element.layer.Substring(0,7) == "ENG_SHE")
                {
                    arrShear.Add(new RawLineData()
                    {
                        layer = element.layer,
                        angled = element.angled,
                        direction = element.direction,
                        Xstart = element.Xstart,
                        Xend = element.Xend,
                        Ystart = element.Ystart,
                        Yend = element.Yend,
                        slope = element.slope,
                        Yintercept = element.Yintercept,
                        level = element.level
                    });
                }

                // Add element to arrTruss list if Truss Line
                if (element.layer.Substring(0,7) == "ENG_TRU")
                {
                    arrTruss.Add(new RawLineData()
                    {
                        layer = element.layer,
                        length = element.length,
                        angled = element.angled,
                        direction = element.direction,
                        Xstart = element.Xstart,
                        Xend = element.Xend,
                        Ystart = element.Ystart,
                        Yend = element.Yend,
                        slope = element.slope,
                        Yintercept = element.Yintercept,
                        level = element.level
                    });
                }

                // Add element to arrStud list if Stud Line
                if (element.layer.Substring(0,7) == "ENG_STU")
                {
                    arrStud.Add(new RawLineData()
                    {
                        layer = element.layer,
                        angled = element.angled,
                        direction = element.direction,
                        Xstart = element.Xstart,
                        Xend = element.Xend,
                        Ystart = element.Ystart,
                        Yend = element.Yend,
                        slope = element.slope,
                        Yintercept = element.Yintercept,
                        level = element.level
                    });
                }

                // Add element to arrBeam list if Beam Line
                if (element.layer.Substring(0,7) == "ENG_BEA")
                {
                    arrBeam.Add(new RawLineData()
                    {
                        layer = element.layer,
                        angled = element.angled,
                        direction = element.direction,
                        Xstart = element.Xstart,
                        Xend = element.Xend,
                        Ystart = element.Ystart,
                        Yend = element.Yend,
                        slope = element.slope,
                        Yintercept = element.Yintercept,
                        level = element.level
                    });
                }
            }

            /************ DETERMINE GAP LENGTH FOR EACH LINE ************/
            // Loop through each Gap Line
            foreach (RawLineData gapElement in arrGap)
            {
                // Match each gap line with Diaphragm line on same level, calculate gap lengths
                foreach (RawLineData diaphrElement in arrDiaphr)
                {
                    if (diaphrElement.level == gapElement.level)
                    {
                        diaphrElement.startGapLength = (float)Math.Pow((Math.Pow((gapElement.Xstart - diaphrElement.Xstart), 2) + Math.Pow((gapElement.Ystart - diaphrElement.Ystart), 2)), .5);
                        diaphrElement.endGapLength = (float)Math.Pow((Math.Pow((gapElement.Xstart - diaphrElement.Xend), 2) + Math.Pow((gapElement.Ystart - diaphrElement.Yend), 2)), .5);
                    }
                }

                // Match each gap line with Shear line on same level, calculate gap lengths
                foreach (RawLineData shearElement in arrShear)
                {
                    if (shearElement.level == gapElement.level)
                    {
                        shearElement.startGapLength = (float)Math.Pow((Math.Pow((gapElement.Xstart - shearElement.Xstart), 2) + Math.Pow((gapElement.Ystart - shearElement.Ystart), 2)), .5);
                        shearElement.endGapLength = (float)Math.Pow((Math.Pow((gapElement.Xstart - shearElement.Xend), 2) + Math.Pow((gapElement.Ystart - shearElement.Yend), 2)), .5);
                    }
                }

                // Match each gap line with Truss line on same level, calculate gap lengths
                foreach (RawLineData trussElement in arrTruss)
                {
                    if (trussElement.level == gapElement.level)
                    {
                        trussElement.startGapLength = (float)Math.Pow((Math.Pow((gapElement.Xstart - trussElement.Xstart), 2) + Math.Pow((gapElement.Ystart - trussElement.Ystart), 2)), .5);
                        trussElement.endGapLength = (float)Math.Pow((Math.Pow((gapElement.Xstart - trussElement.Xend), 2) + Math.Pow((gapElement.Ystart - trussElement.Yend), 2)), .5);
                    }
                }

                // Match each gap line with Stud line on same level, calculate gap lengths
                foreach (RawLineData studElement in arrStud)
                {
                    if (studElement.level == gapElement.level)
                    {
                        studElement.startGapLength = (float)Math.Pow((Math.Pow((gapElement.Xstart - studElement.Xstart), 2) + Math.Pow((gapElement.Ystart - studElement.Ystart), 2)), .5);
                        studElement.endGapLength = (float)Math.Pow((Math.Pow((gapElement.Xstart - studElement.Xend), 2) + Math.Pow((gapElement.Ystart - studElement.Yend), 2)), .5);
                    }
                }

                // Match each gap line with Beam line on same level, calculate gap lengths
                foreach (RawLineData beamElement in arrBeam)
                {
                    if (beamElement.level == gapElement.level)
                    {
                        beamElement.startGapLength = (float)Math.Pow((Math.Pow((gapElement.Xstart - beamElement.Xstart), 2) + Math.Pow((gapElement.Ystart - beamElement.Ystart), 2)), .5);
                        beamElement.endGapLength = (float)Math.Pow((Math.Pow((gapElement.Xstart - beamElement.Xend), 2) + Math.Pow((gapElement.Ystart - beamElement.Yend), 2)), .5);
                    }
                }
            }

            /************ CREATE NEW LABELS FOR LINE DATA ************/
            // Cycle through each level in order to generate new labels based on SCA standard (i.e. X/Y-START_LINE-TYPE_LEVEL_NUMBER)
            for (int level = 1; level <= iLevel; level++)
            {
                // Cycle through each gap line to apply new label determined by direction and level
                j = 1;              // Counters used to indicate line number in particular direction
                k = 1;
                m = 1;
                foreach (RawLineData gapElement in arrGap)
                {
                    if (gapElement.direction == 'X' && gapElement.level == level)
                    {
                        gapElement.label = "X_" + Math.Round(gapElement.Xstart, 2) + "_G_" + level + "_" + j;
                        j++;
                    }
                    if (gapElement.direction == 'Y' && gapElement.level == level)
                    {
                        gapElement.label = "Y_" + Math.Round(gapElement.Ystart, 2) + "_G_" + level + "_" + k;
                        k++;
                    }
                    if (gapElement.direction == 'A' && gapElement.level == level)
                    {
                        gapElement.label = "A_" + Math.Round(gapElement.Ystart, 2) + "_G_" + level + "_" + m;
                        m++;
                    }
                }

                // Cycle through each diaphragm line to apply new label determined by direction and level
                j = 1;              // Counters used to indicate line number in particular direction
                k = 1;
                m = 1;
                foreach (RawLineData diaphrElement in arrDiaphr)
                {
                    if (diaphrElement.direction == 'X' && diaphrElement.level == level)
                    {
                        diaphrElement.label = "X_" + Math.Round(diaphrElement.Xstart, 2) + "_D_" + level + "_" + j;
                        j++;
                    }
                    if (diaphrElement.direction == 'Y' && diaphrElement.level == level)
                    {
                        diaphrElement.label = "Y_" + Math.Round(diaphrElement.Ystart, 2) + "_D_" + level + "_" + k;
                        k++;
                    }
                    if (diaphrElement.direction == 'A' && diaphrElement.level == level)
                    {
                        diaphrElement.label = "A_" + Math.Round(diaphrElement.Ystart, 2) + "_D_" + level + "_" + m;
                        m++;
                    }
                }

                // Cycle through each truss line to apply new label determined by direction and level and truss type
                j = 1;              // Counters used to indicate line number in particular direction
                k = 1;
                m = 1;
                foreach (RawLineData trussElement in arrTruss)
                {
                    if (trussElement.direction == 'X' && trussElement.level == level)
                    {
                        switch (trussElement.layer)
                        {
                            case "ENG_TRUSS_BALC":
                                trussElement.label = "X_TB_" + Math.Round(trussElement.Xstart, 2) + "_" + level + "_" + j;
                                j++;
                                continue;
                            case "ENG_TRUSS_CORR":
                                trussElement.label = "X_TC_" + Math.Round(trussElement.Xstart, 2) + "_" + level + "_" + j;
                                j++;
                                continue;
                            case "ENG_TRUSS_ROOF":
                                trussElement.label = "X_TR_" + Math.Round(trussElement.Xstart, 2) + "_" + level + "_" + j;
                                j++;
                                continue;
                            case "ENG_TRUSS_UNIT":
                                trussElement.label = "X_TU_" + Math.Round(trussElement.Xstart, 2) + "_" + level + "_" + j;
                                j++;
                                continue;
                            default:
                                trussElement.label = "X_TO_" + Math.Round(trussElement.Xstart, 2) + "_" + level + "_" + j;
                                j++;
                                continue;
                        }
                    }
                    if (trussElement.direction == 'Y' && trussElement.level == level)
                    {
                        switch (trussElement.layer)
                        {
                            case "ENG_TRUSS_BALC":
                                trussElement.label = "Y_TB_" + Math.Round(trussElement.Ystart, 2) + "_" + level + "_" + k;
                                k++;
                                continue;
                            case "ENG_TRUSS_CORR":
                                trussElement.label = "Y_TC_" + Math.Round(trussElement.Ystart, 2) + "_" + level + "_" + k;
                                k++;
                                continue;
                            case "ENG_TRUSS_ROOF":
                                trussElement.label = "Y_TR_" + Math.Round(trussElement.Ystart, 2) + "_" + level + "_" + k;
                                k++;
                                continue;
                            case "ENG_TRUSS_UNIT":
                                trussElement.label = "Y_TU_" + Math.Round(trussElement.Ystart, 2) + "_" + level + "_" + k;
                                k++;
                                continue;
                            default:
                                trussElement.label = "Y_TO_" + Math.Round(trussElement.Ystart, 2) + "_" + level + "_" + k;
                                k++;
                                continue;
                        }
                    }
                    if (trussElement.direction == 'A' && trussElement.level == level)
                    {
                        switch (trussElement.layer)
                        {
                            case "ENG_TRUSS_BALC":
                                trussElement.label = "A_TB_" + Math.Round(trussElement.Ystart, 2) + "_" + level + "_" + m;
                                m++;
                                continue;
                            case "ENG_TRUSS_CORR":
                                trussElement.label = "A_TC_" + Math.Round(trussElement.Ystart, 2) + "_" + level + "_" + m;
                                m++;
                                continue;
                            case "ENG_TRUSS_ROOF":
                                trussElement.label = "A_TR_" + Math.Round(trussElement.Ystart, 2) + "_" + level + "_" + m;
                                m++;
                                continue;
                            case "ENG_TRUSS_UNIT":
                                trussElement.label = "A_TU_" + Math.Round(trussElement.Ystart, 2) + "_" + level + "_" + m;
                                m++;
                                continue;
                            default:
                                trussElement.label = "A_TO_" + Math.Round(trussElement.Ystart, 2) + "_" + level + "_" + m;
                                m++;
                                continue;
                        }
                    }
                }
            }

            // Cycle through each stud line to apply new label determined by direction and level
            j = 1;              // Counters used to indicate line number in particular direction
            k = 1;
            m = 1;
            foreach (RawLineData studElement in arrStud)
            {
                if (studElement.direction == 'X')
                {
                    // Determine if exterior or interior
                    if (studElement.layer == "ENG_STUD_EXT" || studElement.layer == "ENG_STUD_4_EXT" ||
                        studElement.layer == "ENG_STUD_6_EXT" || studElement.layer == "ENG_STUD_8_EXT")
                    {
                        studElement.label = "X_" + Math.Round(studElement.Xstart, 2) + "_SE_" + j;
                        studElement.studClass = 'E';
                        j++;
                    }
                    else
                    {
                        studElement.label = "X_" + Math.Round(studElement.Xstart, 2) + "_SI_" + j;
                        studElement.studClass = 'I';
                        j++;
                    }
                }
                if (studElement.direction == 'Y')
                {
                    // Determine if exterior or interior
                    if (studElement.layer == "ENG_STUD_EXT" || studElement.layer == "ENG_STUD_4_EXT" ||
                        studElement.layer == "ENG_STUD_6_EXT" || studElement.layer == "ENG_STUD_8_EXT")
                    {
                        studElement.label = "Y_" + Math.Round(studElement.Ystart, 2) + "_SE_" + k;
                        studElement.studClass = 'E';
                        k++;
                    }
                    else
                    {
                        studElement.label = "Y_" + Math.Round(studElement.Ystart, 2) + "_SI_" + k;
                        studElement.studClass = 'I';
                        k++;
                    }
                }
                if (studElement.direction == 'A')
                {
                    // Determine if exterior or interior
                    if (studElement.layer == "ENG_STUD_EXT" || studElement.layer == "ENG_STUD_4_EXT" ||
                        studElement.layer == "ENG_STUD_6_EXT" || studElement.layer == "ENG_STUD_8_EXT")
                    {
                        studElement.label = "A_" + Math.Round(studElement.Ystart, 2) + "_SE_" + m;
                        studElement.studClass = 'E';
                        m++;
                    }
                    else
                    {
                        studElement.label = "A_" + Math.Round(studElement.Ystart, 2) + "_SI_" + m;
                        studElement.studClass = 'I';
                        m++;
                    }
                }
            }

            // Cycle through each shear line to apply new label determined by direction and level
            j = 1;              // Counters used to indicate line number in particular direction
            k = 1;
            m = 1;
            foreach (RawLineData shearElement in arrShear)
            {
                if (shearElement.direction == 'X')
                {
                    shearElement.label = "X_" + Math.Round(shearElement.Xstart, 2) + "_S_" + j;
                    j++;
                }
                if (shearElement.direction == 'Y')
                {
                    shearElement.label = "Y_" + Math.Round(shearElement.Ystart, 2) + "_S_" + k;
                    k++;
                }
                if (shearElement.direction == 'A')
                {
                    shearElement.label = "A_" + Math.Round(shearElement.Ystart, 2) + "_S_" + m;
                    m++;
                }
            }

            // Cycle through each beam line to apply new label determined by direction and level
            j = 1;              // Counters used to indicate line number in particular direction
            k = 1;
            m = 1;
            foreach (RawLineData beamElement in arrBeam)
            {
                if (beamElement.direction == 'X')
                {
                    beamElement.label = "X_" + Math.Round(beamElement.Xstart, 2) + "_B_" + j;
                    j++;
                }
                if (beamElement.direction == 'Y')
                {
                    beamElement.label = "Y_" + Math.Round(beamElement.Ystart, 2) + "_B_" + k;
                    k++;
                }
                if (beamElement.direction == 'A')
                {
                    beamElement.label = "A_" + Math.Round(beamElement.Ystart, 2) + "_B_" + m;
                    m++;
                }
            }

            /************ CREATE MEDIATION INPUT ARRAYS WORKSHEET ************/
            // Create optional mediation input arrays worksheet if box is checked
            if ((bool)arrDesignData[58] == true)
            {
                Arrays(arrSorted, arrDiaphr, arrGap, arrShear, arrTruss, arrStud, arrBeam);
            }

            /************ FORMAT LEVEL-SPECIFIC CALC TABLES ************/
            // Format workbook for level-specific calc tables
            SCADBuild(arrDesignData, arrStud.Count(), iLevel);          

            // Reactivate Screen Updating after sorting
            this.Application.ScreenUpdating = true;

            /************ START STUD MATCHING ROUTINES ************/
            // Load Progress Bar and set final value
            SCAD.MediationProgressBar MediationProgress = new MediationProgressBar();
            MediationProgress.Show();
            MediationProgress.progressBar.Maximum = (iLevel * arrTruss.Count() * 2) + arrStud.Count() * 5 + (iLevel - 1) * arrStud.Count() + 42;

            // Send arrStud, arrTruss, arrGap to Horizonal Matching Routine
            HSM(ref arrStud, ref arrTruss, arrDesignData, iLevel, ref MediationProgress);

            // Send arrStud, arrTruss, arrGap to Vertical Matching Routine
            VSM(ref arrStud, ref arrTruss, arrDesignData, iLevel, ref MediationProgress);
            
            // Unload Progress Bar
            MediationProgress.Close();

            // Finalize design of stud workbook
            // AutoDesign();

            return;
        }

        // SCADBuild() -- Creates calc worksheets for each level in the Stud Design Workbook.
        public void SCADBuild(object[] arrDesignData, int iStud, int iLevel)
        {
            // Loop through each level to create a new calc table for that floor
            for (int i = 1; i <= iLevel; i++)
            {
                // Create new calc table worksheet and format it
                Excel.Worksheet wsCalcTable = (Excel.Worksheet)this.Application.Worksheets.Add();
                wsCalcTable.Name = "L" + i + " Calc Table";
                wsCalcTable.Tab.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent3;
                wsCalcTable.Tab.TintAndShade = -0.5;

                // Freeze the header row
                ((Excel._Worksheet)wsCalcTable).Activate();
                wsCalcTable.Application.ActiveWindow.SplitRow = 6;
                wsCalcTable.Application.ActiveWindow.FreezePanes = true;

                // Populate header titles
                wsCalcTable.get_Range("A2").Value = "Building Code";
                wsCalcTable.get_Range("A3").Value = "Stud Species:";
                wsCalcTable.get_Range("A5").Value = "Print Line";
                wsCalcTable.get_Range("B2").Value = "=INPUT!D9";
                wsCalcTable.get_Range("B3").Value = "VARIES:";
                wsCalcTable.get_Range("B5").Value = "Stud Line";
                wsCalcTable.get_Range("C5").Value = "Int or Ext (I/E)";
                wsCalcTable.get_Range("D2").Value = "Stud Grade:";
                wsCalcTable.get_Range("D3").Value = "Level:";
                wsCalcTable.get_Range("D5").Value = "# Levels";
                wsCalcTable.get_Range("E2").Value = "VARIES:";
                wsCalcTable.get_Range("E3").Value = i;
                wsCalcTable.get_Range("E5").Value = "Wall Thickness (in)";
                wsCalcTable.get_Range("F5").Value = "Stud Species";
                wsCalcTable.get_Range("G2").Value = "Typ. Wall Height (ft):";
                wsCalcTable.get_Range("G5").Value = "Stud Grade";
                wsCalcTable.get_Range("H5").Value = "Add'l DL (plf)";
                wsCalcTable.get_Range("I5").Value = "Add'l LL (plf)";
                wsCalcTable.get_Range("J4").Value = "Trib. Lengths for Current Level (ft):";
                wsCalcTable.get_Range("J5").Value = "Roof";
                wsCalcTable.get_Range("K5").Value = "Unit";
                wsCalcTable.get_Range("L5").Value = "Balcony";
                wsCalcTable.get_Range("M5").Value = "Corridor";
                wsCalcTable.get_Range("N5").Value = "Other";
                wsCalcTable.get_Range("O5").Value = "Wall Height (ft)";
                wsCalcTable.get_Range("P4").Value = "Reactions From Levels Above (plf)";
                wsCalcTable.get_Range("P5").Value = "Roof DL Rxn";
                wsCalcTable.get_Range("Q5").Value = "Roof LL Rxn";
                wsCalcTable.get_Range("R5").Value = "Unit DL Rxn";
                wsCalcTable.get_Range("S5").Value = "Unit LL Rxn";
                wsCalcTable.get_Range("T5").Value = "Balc. DL Rxn";
                wsCalcTable.get_Range("U5").Value = "Balc. LL Rxn";
                wsCalcTable.get_Range("V5").Value = "Corr. DL Rxn";
                wsCalcTable.get_Range("W5").Value = "Corr. LL Rxn";
                wsCalcTable.get_Range("X5").Value = "Other DL Rxn";
                wsCalcTable.get_Range("Y5").Value = "Other LL Rxn";
                wsCalcTable.get_Range("Z5").Value = "Unbraced Column Length - Lx (ft)";
                wsCalcTable.get_Range("AA5").Value = "Unbraced Column Length - Ly (in)";
                wsCalcTable.get_Range("AB5").Value = "Choose Callout";
                wsCalcTable.get_Range("AC5").Value = "Stud Callout";
                wsCalcTable.get_Range("AD5").Value = "Stud Size";
                wsCalcTable.get_Range("AE5").Value = "Stud Spacing";
                wsCalcTable.get_Range("AF5").Value = "Bending Unity";
                wsCalcTable.get_Range("AG5").Value = "Compression Unity";
                wsCalcTable.get_Range("AH5").Value = "Interaction Unity";
                wsCalcTable.get_Range("AI5").Value = "Actual Defl. (L/x)";
                wsCalcTable.get_Range("AJ5").Value = "Allow Defl. (L/x)";
                wsCalcTable.get_Range("AK5").Value = "Check";
                wsCalcTable.get_Range("AQ5").Value = "Next Match Above";
                wsCalcTable.get_Range("AR5").Value = "Next Match Below";
                wsCalcTable.get_Range("AS5").Value = "Roof DL Rxn";
                wsCalcTable.get_Range("AT5").Value = "Roof LL Rxn";
                wsCalcTable.get_Range("AU5").Value = "Unit DL Rxn";
                wsCalcTable.get_Range("AV5").Value = "Unit LL Rxn";
                wsCalcTable.get_Range("AW5").Value = "Balc. DL Rxn";
                wsCalcTable.get_Range("AX5").Value = "Balc. LL Rxn";
                wsCalcTable.get_Range("AY5").Value = "Corr. DL Rxn";
                wsCalcTable.get_Range("AZ5").Value = "Corr. LL Rxn";
                wsCalcTable.get_Range("BA5").Value = "Other DL Rxn";
                wsCalcTable.get_Range("BB5").Value = "Other LL Rxn";
                wsCalcTable.get_Range("BC5").Value = "Start X";
                wsCalcTable.get_Range("BD5").Value = "Start Y";
                wsCalcTable.get_Range("BE5").Value = "End X";
                wsCalcTable.get_Range("BF5").Value = "End Y";
            }

            // Adds level specific checks to Calc Tables
            Excel.Worksheet wsCalcTableN = this.Application.Worksheets.get_Item("L1 Calc Table");
            switch (iLevel)
            {
                case 6:
                    wsCalcTableN = this.Application.Worksheets.get_Item("L6 Calc Table");
                    wsCalcTableN.get_Range("AL5").Value = "L1 Check";
                    wsCalcTableN.get_Range("AM5").Value = "L2 Check";
                    wsCalcTableN.get_Range("AN5").Value = "L3 Check";
                    wsCalcTableN.get_Range("AO5").Value = "L4 Check";
                    wsCalcTableN.get_Range("AP5").Value = "L5 Check";

                    wsCalcTableN = this.Application.Worksheets.get_Item("L1 Calc Table");
                    wsCalcTableN.get_Range("AP5").Value = "L6 Check";

                    wsCalcTableN = this.Application.Worksheets.get_Item("L2 Calc Table");
                    wsCalcTableN.get_Range("AP5").Value = "L6 Check";

                    wsCalcTableN = this.Application.Worksheets.get_Item("L3 Calc Table");
                    wsCalcTableN.get_Range("AP5").Value = "L6 Check";

                    wsCalcTableN = this.Application.Worksheets.get_Item("L4 Calc Table");
                    wsCalcTableN.get_Range("AP5").Value = "L6 Check";

                    wsCalcTableN = this.Application.Worksheets.get_Item("L5 Calc Table");
                    wsCalcTableN.get_Range("AP5").Value = "L6 Check";
                    goto case 5;
                case 5:
                    wsCalcTableN = this.Application.Worksheets.get_Item("L5 Calc Table");
                    wsCalcTableN.get_Range("AL5").Value = "L1 Check";
                    wsCalcTableN.get_Range("AM5").Value = "L2 Check";
                    wsCalcTableN.get_Range("AN5").Value = "L3 Check";
                    wsCalcTableN.get_Range("AO5").Value = "L4 Check";

                    wsCalcTableN = this.Application.Worksheets.get_Item("L1 Calc Table");
                    wsCalcTableN.get_Range("AO5").Value = "L5 Check";

                    wsCalcTableN = this.Application.Worksheets.get_Item("L2 Calc Table");
                    wsCalcTableN.get_Range("AO5").Value = "L5 Check";

                    wsCalcTableN = this.Application.Worksheets.get_Item("L3 Calc Table");
                    wsCalcTableN.get_Range("AO5").Value = "L5 Check";

                    wsCalcTableN = this.Application.Worksheets.get_Item("L4 Calc Table");
                    wsCalcTableN.get_Range("AO5").Value = "L5 Check";
                    goto case 4;
                case 4:
                    wsCalcTableN = this.Application.Worksheets.get_Item("L4 Calc Table");
                    wsCalcTableN.get_Range("AL5").Value = "L1 Check";
                    wsCalcTableN.get_Range("AM5").Value = "L2 Check";
                    wsCalcTableN.get_Range("AN5").Value = "L3 Check";

                    wsCalcTableN = this.Application.Worksheets.get_Item("L1 Calc Table");
                    wsCalcTableN.get_Range("AN5").Value = "L4 Check";

                    wsCalcTableN = this.Application.Worksheets.get_Item("L2 Calc Table");
                    wsCalcTableN.get_Range("AN5").Value = "L4 Check";

                    wsCalcTableN = this.Application.Worksheets.get_Item("L3 Calc Table");
                    wsCalcTableN.get_Range("AN5").Value = "L4 Check";
                    goto case 3;
                case 3:
                    wsCalcTableN = this.Application.Worksheets.get_Item("L3 Calc Table");
                    wsCalcTableN.get_Range("AL5").Value = "L1 Check";
                    wsCalcTableN.get_Range("AM5").Value = "L2 Check";

                    wsCalcTableN = this.Application.Worksheets.get_Item("L1 Calc Table");
                    wsCalcTableN.get_Range("AM5").Value = "L3 Check";

                    wsCalcTableN = this.Application.Worksheets.get_Item("L2 Calc Table");
                    wsCalcTableN.get_Range("AM5").Value = "L3 Check";
                    goto case 2;
                case 2:
                    wsCalcTableN = this.Application.Worksheets.get_Item("L2 Calc Table");
                    wsCalcTableN.get_Range("AL5").Value = "L2 Check";

                    wsCalcTableN = this.Application.Worksheets.get_Item("L1 Calc Table");
                    wsCalcTableN.get_Range("AL5").Value = "L1 Check";
                    break;
            }

            // Populate INPUT worksheet with Design Data from initial form
            Excel.Worksheet wsInput = this.Application.Worksheets.get_Item("INPUT");

            wsInput.get_Range("D4").Value = arrDesignData[0];           // Job Name
            wsInput.get_Range("J4").Value = arrDesignData[1];           // Job Number
            wsInput.get_Range("H4").Value = arrDesignData[2];           // Initials
            wsInput.get_Range("D7").Value = iLevel;                     // # of Levels
            wsInput.get_Range("I7").Value = arrDesignData[59];          // Allowable Interaction Ratio
            wsInput.get_Range("D9").Value = arrDesignData[3];           // Building Code
            wsInput.get_Range("D12").Value = arrDesignData[37];         // Stud Species 1
            wsInput.get_Range("D13").Value = arrDesignData[38];         // Stud Grade 1
            wsInput.get_Range("F12").Value = arrDesignData[39];         // Stud Species 2
            wsInput.get_Range("H12").Value = arrDesignData[40];         // Stud Grade 2
            wsInput.get_Range("F13").Value = arrDesignData[41];         // Stud Species 3
            wsInput.get_Range("H13").Value = arrDesignData[42];         // Stud Grade 3
            wsInput.get_Range("D16").Value = arrDesignData[43];          // Lx1
            wsInput.get_Range("E16").Value = arrDesignData[44];          // Lx2
            wsInput.get_Range("F16").Value = arrDesignData[45];          // Lx3
            wsInput.get_Range("G16").Value = arrDesignData[46];          // Lx4
            wsInput.get_Range("H16").Value = arrDesignData[47];          // Lx5
            wsInput.get_Range("I16").Value = arrDesignData[48];          // Lx6
            wsInput.get_Range("D17").Value = arrDesignData[49];          // Ly1
            wsInput.get_Range("E17").Value = arrDesignData[50];          // Ly2
            wsInput.get_Range("F17").Value = arrDesignData[51];          // Ly3
            wsInput.get_Range("G17").Value = arrDesignData[52];          // Ly4
            wsInput.get_Range("H17").Value = arrDesignData[53];          // Ly5
            wsInput.get_Range("I17").Value = arrDesignData[54];          // Ly6
            wsInput.get_Range("D19").Value = arrDesignData[22];          // Wind Pressure
            wsInput.get_Range("D20").Value = arrDesignData[23];          // Seismic Pressure
            wsInput.get_Range("D21").Value = arrDesignData[24];          // Internal Wall Pressure
            wsInput.get_Range("D23").Value = arrDesignData[25];          // Roof SL
            wsInput.get_Range("D24").Value = arrDesignData[26];          // Roof RL
            wsInput.get_Range("D25").Value = arrDesignData[27];          // Roof DL
            wsInput.get_Range("D26").Value = arrDesignData[28];          // Roof LL
            wsInput.get_Range("D27").Value = arrDesignData[29];          // Unit DL
            wsInput.get_Range("D28").Value = arrDesignData[30];          // Unit LL
            wsInput.get_Range("D29").Value = arrDesignData[31];          // Balc DL
            wsInput.get_Range("D30").Value = arrDesignData[32];          // Balc LL
            wsInput.get_Range("D31").Value = arrDesignData[33];          // Corr DL
            wsInput.get_Range("D32").Value = arrDesignData[34];          // Corr LL
            wsInput.get_Range("D33").Value = arrDesignData[35];          // Other DL
            wsInput.get_Range("D34").Value = arrDesignData[36];          // Other LL
            wsInput.get_Range("D35").Value = arrDesignData[20];          // Interior Wall Wt
            wsInput.get_Range("D36").Value = arrDesignData[21];          // Exterior Wall Wt
            wsInput.get_Range("I24").Value = arrDesignData[10];          // Bending Coefficient
            wsInput.get_Range("I25").Value = arrDesignData[11];          // Built up Col Factor
            wsInput.get_Range("I26").Value = arrDesignData[12];          // Repetitive Member
            wsInput.get_Range("I27").Value = arrDesignData[13];          // Wet Service Factor
            wsInput.get_Range("I28").Value = arrDesignData[14];          // Temp Factor
            wsInput.get_Range("I29").Value = arrDesignData[15];          // Beam Stability Factor
            wsInput.get_Range("I30").Value = arrDesignData[16];          // Buckling Factor
            wsInput.get_Range("I31").Value = arrDesignData[17];          // Bearing Area Factor
            wsInput.get_Range("I33").Value = arrDesignData[18];          // Seismic SDS
            if (arrDesignData[60].ToString() == "True")                             // Compression Limit
            {
                wsInput.get_Range("I20").Value = "Yes";
            }
            else
            {
                wsInput.get_Range("I20").Value = "No";
            }

            // Create report directories if print all lines flag is activated
            if (arrDesignData[19].ToString() == "Yes")
            {
                MkReportDirs((string)arrDesignData[1]);
            }

            return;
        }

        // Arrays() -- Creates optional worksheet for all of the sorted raw data
        public void Arrays(List<RawLineData> arrSorted, List<RawLineData> arrDiaphr, List<RawLineData> arrGap, List<RawLineData> arrShear, List<RawLineData> arrTruss, List<RawLineData> arrStud, List<RawLineData> arrBeam)
        {
            // Create new Array worksheet to output lines
            Excel.Worksheet wsArrays = (Excel.Worksheet)this.Application.Worksheets.Add();
            wsArrays.Name = "Arrays";
            wsArrays.Tab.ThemeColor = Excel.XlThemeColor.xlThemeColorAccent3;
            wsArrays.Tab.TintAndShade = 0.5;

            // Freeze the header row
            ((Excel._Worksheet)wsArrays).Activate();
            wsArrays.Application.ActiveWindow.SplitRow = 1;
            wsArrays.Application.ActiveWindow.FreezePanes = true;

            int i = 2;  // Counter to increment rows

            // Header Row for Array worksheet
            wsArrays.get_Range("B1").Value = "Label";
            wsArrays.get_Range("D1").Value = "Xstart";
            wsArrays.get_Range("E1").Value = "Ystart";
            wsArrays.get_Range("G1").Value = "Xend";
            wsArrays.get_Range("H1").Value = "Yend";
            wsArrays.get_Range("I1").Value = "level";
            wsArrays.get_Range("J1").Value = "direction";
            wsArrays.get_Range("K1").Value = "studClass";
            wsArrays.get_Range("L1").Value = "studThickness";
            wsArrays.get_Range("M1").Value = "angled";
            wsArrays.get_Range("N1").Value = "startGapLength";
            wsArrays.get_Range("O1").Value = "endGapLength";
            wsArrays.get_Range("P1").Value = "Yintercept";
            wsArrays.get_Range("Q1").Value = "slope";

            // Loop through arrSorted
            wsArrays.get_Range("A" + i).Value = "arrSorted";
            foreach (RawLineData element in arrSorted)
            {
                wsArrays.get_Range("C" + i).Value = element.label;
                wsArrays.get_Range("D" + i).Value = element.Xstart;
                wsArrays.get_Range("E" + i).Value = element.Ystart;
                wsArrays.get_Range("G" + i).Value = element.Xend;
                wsArrays.get_Range("H" + i).Value = element.Yend;
                wsArrays.get_Range("I" + i).Value = element.level;
                wsArrays.get_Range("J" + i).Value = "" + element.direction;
                wsArrays.get_Range("K" + i).Value = "" + element.studClass;
                wsArrays.get_Range("L" + i).Value = element.studThickness;
                wsArrays.get_Range("M" + i).Value = element.angled;
                wsArrays.get_Range("N" + i).Value = element.startGapLength;
                wsArrays.get_Range("O" + i).Value = element.endGapLength;
                wsArrays.get_Range("P" + i).Value = element.Yintercept;
                wsArrays.get_Range("Q" + i).Value = element.slope;
                i++;
            }

            // Loop through arrStud
            i++;
            wsArrays.get_Range("A" + i).Value = "arrStud";
            foreach (RawLineData element in arrStud)
            {
                wsArrays.get_Range("C" + i).Value = element.label;
                wsArrays.get_Range("D" + i).Value = element.Xstart;
                wsArrays.get_Range("E" + i).Value = element.Ystart;
                wsArrays.get_Range("G" + i).Value = element.Xend;
                wsArrays.get_Range("H" + i).Value = element.Yend;
                wsArrays.get_Range("I" + i).Value = element.level;
                wsArrays.get_Range("J" + i).Value = "" + element.direction;
                wsArrays.get_Range("K" + i).Value = "" + element.studClass;
                wsArrays.get_Range("L" + i).Value = element.studThickness;
                wsArrays.get_Range("M" + i).Value = element.angled;
                wsArrays.get_Range("N" + i).Value = element.startGapLength;
                wsArrays.get_Range("O" + i).Value = element.endGapLength;
                wsArrays.get_Range("P" + i).Value = element.Yintercept;
                wsArrays.get_Range("Q" + i).Value = element.slope;
                i++;
            }

            // Loop through arrTruss
            i++;
            wsArrays.get_Range("A" + i).Value = "arrTruss";
            foreach (RawLineData element in arrTruss)
            {
                wsArrays.get_Range("C" + i).Value = element.label;
                wsArrays.get_Range("D" + i).Value = element.Xstart;
                wsArrays.get_Range("E" + i).Value = element.Ystart;
                wsArrays.get_Range("G" + i).Value = element.Xend;
                wsArrays.get_Range("H" + i).Value = element.Yend;
                wsArrays.get_Range("I" + i).Value = element.level;
                wsArrays.get_Range("J" + i).Value = "" + element.direction;
                wsArrays.get_Range("K" + i).Value = "" + element.studClass;
                wsArrays.get_Range("L" + i).Value = element.studThickness;
                wsArrays.get_Range("M" + i).Value = element.angled;
                wsArrays.get_Range("N" + i).Value = element.startGapLength;
                wsArrays.get_Range("O" + i).Value = element.endGapLength;
                wsArrays.get_Range("P" + i).Value = element.Yintercept;
                wsArrays.get_Range("Q" + i).Value = element.slope;
                i++;
            }

            // Loop through arrGap
            i++;
            wsArrays.get_Range("A" + i).Value = "arrGap";
            foreach (RawLineData element in arrGap)
            {
                wsArrays.get_Range("C" + i).Value = element.label;
                wsArrays.get_Range("D" + i).Value = element.Xstart;
                wsArrays.get_Range("E" + i).Value = element.Ystart;
                wsArrays.get_Range("G" + i).Value = element.Xend;
                wsArrays.get_Range("H" + i).Value = element.Yend;
                wsArrays.get_Range("I" + i).Value = element.level;
                wsArrays.get_Range("J" + i).Value = "" + element.direction;
                wsArrays.get_Range("K" + i).Value = "" + element.studClass;
                wsArrays.get_Range("L" + i).Value = element.studThickness;
                wsArrays.get_Range("M" + i).Value = element.angled;
                wsArrays.get_Range("N" + i).Value = element.startGapLength;
                wsArrays.get_Range("O" + i).Value = element.endGapLength;
                wsArrays.get_Range("P" + i).Value = element.Yintercept;
                wsArrays.get_Range("Q" + i).Value = element.slope;
                i++;
            }

            // Loop through arrDiaphr
            i++;
            wsArrays.get_Range("A" + i).Value = "arrDiaphr";
            foreach (RawLineData element in arrDiaphr)
            {
                wsArrays.get_Range("C" + i).Value = element.label;
                wsArrays.get_Range("D" + i).Value = element.Xstart;
                wsArrays.get_Range("E" + i).Value = element.Ystart;
                wsArrays.get_Range("G" + i).Value = element.Xend;
                wsArrays.get_Range("H" + i).Value = element.Yend;
                wsArrays.get_Range("I" + i).Value = element.level;
                wsArrays.get_Range("J" + i).Value = "" + element.direction;
                wsArrays.get_Range("K" + i).Value = "" + element.studClass;
                wsArrays.get_Range("L" + i).Value = element.studThickness;
                wsArrays.get_Range("M" + i).Value = element.angled;
                wsArrays.get_Range("N" + i).Value = element.startGapLength;
                wsArrays.get_Range("O" + i).Value = element.endGapLength;
                wsArrays.get_Range("P" + i).Value = element.Yintercept;
                wsArrays.get_Range("Q" + i).Value = element.slope;
                i++;
            }

            // Loop through arrBeam
            i++;
            wsArrays.get_Range("A" + i).Value = "arrBeam";
            foreach (RawLineData element in arrBeam)
            {
                wsArrays.get_Range("C" + i).Value = element.label;
                wsArrays.get_Range("D" + i).Value = element.Xstart;
                wsArrays.get_Range("E" + i).Value = element.Ystart;
                wsArrays.get_Range("G" + i).Value = element.Xend;
                wsArrays.get_Range("H" + i).Value = element.Yend;
                wsArrays.get_Range("I" + i).Value = element.level;
                wsArrays.get_Range("J" + i).Value = "" + element.direction;
                wsArrays.get_Range("K" + i).Value = "" + element.studClass;
                wsArrays.get_Range("L" + i).Value = element.studThickness;
                wsArrays.get_Range("M" + i).Value = element.angled;
                wsArrays.get_Range("N" + i).Value = element.startGapLength;
                wsArrays.get_Range("O" + i).Value = element.endGapLength;
                wsArrays.get_Range("P" + i).Value = element.Yintercept;
                wsArrays.get_Range("Q" + i).Value = element.slope;
                i++;
            }

            // Loop through arrShear
            i++;
            wsArrays.get_Range("A" + i).Value = "arrShear";
            foreach (RawLineData element in arrShear)
            {
                wsArrays.get_Range("C" + i).Value = element.label;
                wsArrays.get_Range("D" + i).Value = element.Xstart;
                wsArrays.get_Range("E" + i).Value = element.Ystart;
                wsArrays.get_Range("G" + i).Value = element.Xend;
                wsArrays.get_Range("H" + i).Value = element.Yend;
                wsArrays.get_Range("I" + i).Value = element.level;
                wsArrays.get_Range("J" + i).Value = "" + element.direction;
                wsArrays.get_Range("K" + i).Value = "" + element.studClass;
                wsArrays.get_Range("L" + i).Value = element.studThickness;
                wsArrays.get_Range("M" + i).Value = element.angled;
                wsArrays.get_Range("N" + i).Value = element.startGapLength;
                wsArrays.get_Range("O" + i).Value = element.endGapLength;
                wsArrays.get_Range("P" + i).Value = element.Yintercept;
                wsArrays.get_Range("Q" + i).Value = element.slope;
                i++;
            }
        }

        // HSM_Step1() -- Handles horizontal matching of stud and truss lines
        public void HSM(ref List<RawLineData> arrStud, ref List<RawLineData> arrTruss, Object[] arrDesignData, int iLevel, ref SCAD.MediationProgressBar MediationProgress)
        {
            // Declarations
            double dIntersect = new double();                           // Used to store intersection of stud and truss line
            int jx = new int(); int jy = new int();                     // Counters to iterate through stud worksheet cells
            int kx = new int(); int ky = new int();                     // Counters to iterate through truss worksheet cells

            /************ MATCH Y-TRUSSES WITH X-STUD LINES FOR EACH LEVEL ************/
            for (int i = 1; i <= iLevel; i++)
            {
                // If Horizontal Matching summary is checked, create and format matching datasheets for output
                if ((bool)arrDesignData[55] == true)
                {
                    // Create new worksheet for X-Direction matches for each level and format it initially
                    Excel.Worksheet wsXdir = this.Application.Worksheets.Add();
                    wsXdir.Name = "X-DIR L" + i;
                    wsXdir.Tab.ThemeColor = Excel.XlThemeColor.xlThemeColorLight2;
                    wsXdir.Tab.TintAndShade = 0.4;
                    wsXdir.get_Range("B1").Font.Bold = true;
                    wsXdir.get_Range("B1").Font.Name = "Arial";
                    wsXdir.get_Range("B1").Font.Size = 12;
                    wsXdir.get_Range("D9").EntireColumn.Activate();
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).Weight = Excel.XlBorderWeight.xlThin;
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;
                    wsXdir.get_Range("C8").EntireRow.Activate();
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).Weight = Excel.XlBorderWeight.xlThin;
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;
                    wsXdir.get_Range("D7", "D8").Activate();
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).Weight = Excel.XlBorderWeight.xlThin;
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).Weight = Excel.XlBorderWeight.xlThin;

                    // Populate headers
                    wsXdir.get_Range("B1").Value = "X DIRECTION STUD LINES MATCHING";
                    wsXdir.get_Range("B2").Value = "X Direction Stud Count:";
                    wsXdir.get_Range("B3").Value = "Angled Stud Count:";
                    wsXdir.get_Range("B4").Value = "Y Direction Truss Count:";
                    wsXdir.get_Range("B5").Value = "Angled Truss Count:";
                    wsXdir.get_Range("D7").Value = "X DIR. STUD LINES";
                    wsXdir.get_Range("C8").Value = "Y DIR. TRUSS LINES";
                    wsXdir.get_Range("E8").Value = "Roof";
                    wsXdir.get_Range("F8").Value = "Unit";
                    wsXdir.get_Range("G8").Value = "Balcony";
                    wsXdir.get_Range("H8").Value = "Corridor";
                    wsXdir.get_Range("I8").Value = "Other";
                    wsXdir.get_Range("C2").Value = arrStud.Count(n => n.direction == 'X' && n.level == i);
                    wsXdir.get_Range("C3").Value = arrStud.Count(n => n.direction == 'A' && n.level == i);
                    wsXdir.get_Range("C4").Value = arrTruss.Count(n => n.direction == 'Y' && n.level == i);
                    wsXdir.get_Range("C5").Value = arrTruss.Count(n => n.direction == 'A' && n.level == i);
                    wsXdir.get_Range("A1", "NN1").EntireColumn.AutoFit();

                    // Create new worksheet for Y-Direction matches for each level and format it initially
                    Excel.Worksheet wsYdir = this.Application.Worksheets.Add();
                    wsYdir.Name = "Y-DIR L" + i;
                    wsYdir.Tab.ThemeColor = Excel.XlThemeColor.xlThemeColorLight2;
                    wsYdir.Tab.TintAndShade = 0.4;
                    wsYdir.get_Range("B1").Font.Bold = true;
                    wsYdir.get_Range("B1").Font.Name = "Arial";
                    wsYdir.get_Range("B1").Font.Size = 12;
                    wsYdir.get_Range("D9").EntireColumn.Activate();
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).Weight = Excel.XlBorderWeight.xlThin;
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;
                    wsYdir.get_Range("C8").EntireRow.Activate();
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).Weight = Excel.XlBorderWeight.xlThin;
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).ColorIndex = Excel.XlColorIndex.xlColorIndexAutomatic;
                    wsYdir.get_Range("D7", "D8").Activate();
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeBottom).Weight = Excel.XlBorderWeight.xlThin;
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                    this.Application.Selection.Borders(Excel.XlBordersIndex.xlEdgeRight).Weight = Excel.XlBorderWeight.xlThin;

                    // Populate headers
                    wsYdir.get_Range("B1").Value = "Y DIRECTION STUD LINES MATCHING";
                    wsYdir.get_Range("B2").Value = "Y Direction Stud Count:";
                    wsYdir.get_Range("B3").Value = "Angled Stud Count:";
                    wsYdir.get_Range("B4").Value = "X Direction Truss Count:";
                    wsYdir.get_Range("B5").Value = "Angled Truss Count:";
                    wsYdir.get_Range("D7").Value = "Y DIR. STUD LINES";
                    wsYdir.get_Range("C8").Value = "X DIR. TRUSS LINES";
                    wsYdir.get_Range("E8").Value = "Roof";
                    wsYdir.get_Range("F8").Value = "Unit";
                    wsYdir.get_Range("G8").Value = "Balcony";
                    wsYdir.get_Range("H8").Value = "Corridor";
                    wsYdir.get_Range("I8").Value = "Other";
                    wsYdir.get_Range("C2").Value = arrStud.Count(n => n.direction == 'Y' && n.level == i);
                    wsYdir.get_Range("C3").Value = arrStud.Count(n => n.direction == 'A' && n.level == i);
                    wsYdir.get_Range("C4").Value = arrTruss.Count(n => n.direction == 'X' && n.level == i);
                    wsYdir.get_Range("C5").Value = arrTruss.Count(n => n.direction == 'A' && n.level == i);
                    wsYdir.get_Range("A1", "NN1").EntireColumn.AutoFit();
                }

                // Cycle through matchings of X-Direction stud lines, begin with the truss element then match it to individual stud elements
                jx = 0; jy = 0; kx = 0; ky = 0;
                foreach (RawLineData trussElement in arrTruss)
                {
                    foreach (RawLineData studElement in arrStud)
                    {
                        /* X-DIRECTION MATCHING */
                        // If stud line is X direction and on same level and truss line is Y direction and on same level
                        if (studElement.direction == 'X' && studElement.level == i && trussElement.direction == 'Y' && trussElement.level == i)
                        {
                            // Check to see if truss line falls within limits of the stud line
                            if (trussElement.Xstart >= studElement.Xstart && trussElement.Xstart <= studElement.Xend &&
                                studElement.Ystart >= trussElement.Ystart && studElement.Ystart <= trussElement.Yend)
                            {
                                // Assign truss match label, type, and length for stud line
                                switch (trussElement.label[3])
                                {
                                    case 'R':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'R',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'U':
                                        studElement.trussMatches.Add(new trussMatch()
                                            {
                                                trussLabel = trussElement.label,
                                                trussType = 'U',
                                                trussLength = (int)(trussElement.length / 2)
                                            });
                                        break;
                                    case 'B':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'B',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'C':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'C',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'O':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'O',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    default:
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'X',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                }
                            }
                        }

                        // If stud line is X direction and on same level and truss line is A direction and on same level
                        if (studElement.direction == 'X' && studElement.level == i && trussElement.direction == 'A' && trussElement.level == i)
                        {
                            // Determine intersection of stud and truss line
                            dIntersect = (trussElement.Yintercept - studElement.Ystart) / (-1 * trussElement.slope);

                            // Check to see if truss line falls within limits of the stud line, case is if intersection is negative
                            if (dIntersect < 0 && studElement.Xstart <= (dIntersect + 1) && studElement.Xend >= (dIntersect - 1) &&
                                trussElement.Ystart >= studElement.Ystart && trussElement.Yend <= studElement.Yend)
                            {
                                // Assign truss match label, type, and length for stud line
                                switch (trussElement.label[3])
                                {
                                    case 'R':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'R',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'U':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'U',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'B':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'B',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'C':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'C',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'O':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'O',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    default:
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'X',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                }
                            }

                            // Check to see if truss line falls within limits of the stud line, case is if intersection is positive
                            if (dIntersect > 0 && studElement.Xstart <= (dIntersect + 1) && studElement.Xend >= (dIntersect - 1) &&
                                trussElement.Ystart <= studElement.Ystart && trussElement.Yend >= studElement.Yend)
                            {
                                // Assign truss match label, type, and length for stud line
                                switch (trussElement.label[3])
                                {
                                    case 'R':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'R',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'U':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'U',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'B':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'B',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'C':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'C',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'O':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'O',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    default:
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'X',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                }
                            }
                        }

                        // If stud line is angled and truss line is Y direction, check to see if within bounds of stud line
                        if (studElement.direction == 'A' && studElement.level == i && trussElement.direction == 'Y' && trussElement.level == i)
                        {
                            // Determine intersection of angled stud and Y direction truss
                            dIntersect = studElement.slope * trussElement.Xstart - studElement.Yintercept;

                            if ((dIntersect + 1) >= trussElement.Ystart && (dIntersect - 1) <= trussElement.Yend && trussElement.Xstart >= studElement.Xstart
                                && trussElement.Xstart <= studElement.Xend)
                            {
                                // Assign truss match label, type, and length for stud line
                                switch (trussElement.label[3])
                                {
                                    case 'R':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'R',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'U':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'U',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'B':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'B',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'C':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'C',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'O':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'O',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    default:
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'X',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                }
                            }
                        }

                        // If stud is angled and truss is angled, check to see if within bounds of stud
                        if (studElement.direction == 'A' && studElement.level == i && trussElement.direction == 'A' && trussElement.level == i)
                        {
                            double dIntersectX = new double();        // Used to store potential x intersection coord

                            // Determine potential intersection X coord
                            if ((trussElement.slope - studElement.slope) == 0)
                            {
                                dIntersectX = 0;
                            }
                            else
                            {
                                dIntersectX = (studElement.Yintercept - trussElement.Yintercept) / (trussElement.slope - studElement.slope);
                            }

                            dIntersect = (studElement.slope * dIntersectX) + studElement.Yintercept;

                            // Check if intersection falls within angled stud and truss lines, case is stud slope is NEGATIVE and truss slope is POSITIVE
                            if (studElement.slope < 0 && trussElement.slope > 0 && (dIntersectX + 1) >= studElement.Xstart && (dIntersectX - 1) <= studElement.Xend &&
                                (dIntersect - 1) <= studElement.Ystart && (dIntersect + 1) >= studElement.Yend && (dIntersectX + 1) >= trussElement.Xstart &&
                                (dIntersectX - 1) <= trussElement.Xend && (dIntersect + 1) >= trussElement.Ystart && (dIntersect - 1) <= trussElement.Yend)
                            {
                                // Assign truss match label, type, and length for stud line
                                switch (trussElement.label[3])
                                {
                                    case 'R':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'R',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'U':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'U',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'B':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'B',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'C':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'C',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'O':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'O',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    default:
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'X',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                }
                            }

                            // Check if intersection falls within angled stud and truss lines, case is stud slope is NEGATIVE and truss slope is NEGATIVE
                            if (studElement.slope < 0 && trussElement.slope < 0 && (dIntersectX + 1) >= studElement.Xstart && (dIntersectX - 1) <= studElement.Xend &&
                                (dIntersect - 1) <= studElement.Ystart && (dIntersect + 1) >= studElement.Yend && (dIntersectX + 1) >= trussElement.Xstart &&
                                (dIntersectX - 1) <= trussElement.Xend && (dIntersect - 1) <= trussElement.Ystart && (dIntersect + 1) >= trussElement.Yend)
                            {
                                // Assign truss match label, type, and length for stud line
                                switch (trussElement.label[3])
                                {
                                    case 'R':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'R',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'U':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'U',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'B':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'B',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'C':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'C',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'O':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'O',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    default:
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'X',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                }
                            }

                            // Check if intersection falls within angled stud and truss lines, case is stud slope is POSITVE and truss slope is POSITIVE
                            if (studElement.slope > 0 && trussElement.slope > 0 && (dIntersectX + 1) >= studElement.Xstart && (dIntersectX - 1) <= studElement.Xend &&
                                (dIntersect + 1) >= studElement.Ystart && (dIntersect - 1) <= studElement.Yend && (dIntersectX + 1) >= trussElement.Xstart &&
                                (dIntersectX - 1) <= trussElement.Xend && (dIntersect + 1) >= trussElement.Ystart && (dIntersect - 1) <= trussElement.Yend)
                            {
                                // Assign truss match label, type, and length for stud line
                                switch (trussElement.label[3])
                                {
                                    case 'R':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'R',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'U':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'U',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'B':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'B',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'C':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'C',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'O':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'O',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    default:
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'X',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                }
                            }

                            // Check if intersection falls within angled stud and truss lines, case is stud slope is POSITIVE and truss slope is NEGATIVE
                            if (studElement.slope > 0 && trussElement.slope < 0 && (dIntersectX + 1) >= studElement.Xstart && (dIntersectX - 1) <= studElement.Xend &&
                                (dIntersect + 1) >= studElement.Ystart && (dIntersect - 1) <= studElement.Yend && (dIntersectX + 1) >= trussElement.Xstart &&
                                (dIntersectX - 1) <= trussElement.Xend && (dIntersect - 1) <= trussElement.Ystart && (dIntersect + 1) >= trussElement.Yend)
                            {
                                // Assign truss match label, type, and length for stud line
                                switch (trussElement.label[3])
                                {
                                    case 'R':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'R',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'U':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'U',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'B':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'B',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'C':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'C',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    case 'O':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'O',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                    default:
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'X',
                                            trussLength = (int)(trussElement.length / 2),
                                            trussAngled = 'A'
                                        });
                                        break;
                                }
                            }
                        }

                        /* Y-DIRECTION MATCHING */
                        // If stud line is Y direction and on same level and truss line is X direction and on same level
                        if (studElement.direction == 'Y' && studElement.level == i && trussElement.direction == 'X' && trussElement.level == i)
                        {
                            // Check to see if truss line falls within limits of the stud line
                            if ((studElement.Xstart >= trussElement.Xstart) && (studElement.Xstart <= trussElement.Xend) &&
                                (trussElement.Ystart >= studElement.Ystart) && (trussElement.Yend <= studElement.Yend))
                            {
                                // Assign truss match label, type, and length for stud line
                                switch (trussElement.label[3])
                                {
                                    case 'R':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'R',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'U':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'U',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'B':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'B',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'C':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'C',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'O':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'O',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    default:
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'X',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                }
                            }
                        }

                        // If stud line is Y direction and on same level and truss line is A direction and on same level
                        if (studElement.direction == 'X' && studElement.level == i && trussElement.direction == 'A' && trussElement.level == i)
                        {
                            // Determine intersection of stud and truss line
                            dIntersect = (trussElement.slope * studElement.Xstart) + trussElement.Yintercept;

                            // Check to see if truss line falls within limits of the stud line, case is if intersection is negative
                            if ((studElement.Ystart <= dIntersect + 1) && (studElement.Yend >= dIntersect - 1) && (trussElement.Xstart <= studElement.Xstart)
                                && trussElement.Xend >= studElement.Xend)
                            {
                                // Assign truss match label, type, and length for stud line
                                switch (trussElement.label[3])
                                {
                                    case 'R':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'R',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'U':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'U',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'B':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'B',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'C':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'C',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'O':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'O',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    default:
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'X',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                }
                            }

                            // Check to see if truss line falls within limits of the stud line, case is if intersection is positive
                            if (dIntersect > 0 && studElement.Xstart <= (dIntersect + 1) && studElement.Xend >= (dIntersect - 1) &&
                                trussElement.Ystart <= studElement.Ystart && trussElement.Yend >= studElement.Yend)
                            {
                                // Assign truss match label, type, and length for stud line
                                switch (trussElement.label[3])
                                {
                                    case 'R':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'R',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'U':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'U',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'B':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'B',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'C':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'C',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'O':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'O',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    default:
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'X',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                }
                            }
                        }

                        // If stud line is angled and truss line is X direction, check to see if within bounds of stud line
                        if (studElement.direction == 'A' && studElement.level == i && trussElement.direction == 'X' && trussElement.level == i)
                        {
                            // Determine intersection of angled stud and Y direction truss
                            dIntersect = (studElement.Yintercept - trussElement.Ystart) / (-1 * studElement.slope);

                            if (((studElement.slope > 0) && (trussElement.Xstart <= dIntersect + 1) && (trussElement.Xend >= dIntersect - 1) &&
                                (trussElement.Ystart >= studElement.Ystart) && (trussElement.Ystart <= studElement.Yend))
                                || ((studElement.slope < 0) && (trussElement.Xstart <= dIntersect + 1) && (trussElement.Xend >= dIntersect - 1) &&
                                (trussElement.Ystart <= studElement.Ystart) && (trussElement.Ystart >= studElement.Yend)))
                            {
                                // Assign truss match label, type, and length for stud line
                                switch (trussElement.label[3])
                                {
                                    case 'R':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'R',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'U':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'U',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'B':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'B',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'C':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'C',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    case 'O':
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'O',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                    default:
                                        studElement.trussMatches.Add(new trussMatch()
                                        {
                                            trussLabel = trussElement.label,
                                            trussType = 'X',
                                            trussLength = (int)(trussElement.length / 2)
                                        });
                                        break;
                                }
                            }
                        }

                        // Add X/A direction stud label to match worksheet if matching report is desired (only completed on first truss pass)
                        if ((bool)arrDesignData[55] == true && kx == 0 && studElement.level == i && (studElement.direction == 'X' || studElement.direction == 'A'))
                        {
                            Excel.Worksheet wsMediationX = Application.Worksheets.get_Item("X-DIR L" + i);
                            wsMediationX.get_Range("D" + (9 + jx)).Value = studElement.label;
                        }

                        // Add truss tributary load to matching X direction stud label on match worksheet if matching report is desired
                        if ((bool)arrDesignData[55] == true && studElement.level == i && (studElement.direction == 'X' || studElement.direction == 'A')
                            && studElement.trussMatches.Any(r => r.trussLabel == trussElement.label))
                        {
                            Excel.Worksheet wsMediationX = Application.Worksheets.get_Item("X-DIR L" + i);
                            wsMediationX.get_Range("J" + (9 + jx)).Offset[0, kx].Value = (int)(trussElement.length / 2);
                            // Add cumulative tributary load totals for each stud line
                            switch ((char)trussElement.label[3])
                            {
                                case 'U':
                                    wsMediationX.get_Range("F" + (9 + jx)).Value = (wsMediationX.get_Range("F" + (9 + jx)).Value == null) ?
                                        (int)(trussElement.length / 2) :
                                        (int)wsMediationX.get_Range("F" + (9 + jx)).Value2 + (int)(trussElement.length / 2);
                                    break;
                                case 'R':
                                    wsMediationX.get_Range("E" + (9 + jx)).Value = (wsMediationX.get_Range("E" + (9 + jx)).Value == null) ?
                                        (int)(trussElement.length / 2) :
                                        (int)wsMediationX.get_Range("E" + (9 + jx)).Value2 + (int)(trussElement.length / 2);
                                    break;
                                case 'B':
                                    wsMediationX.get_Range("G" + (9 + jx)).Value = (wsMediationX.get_Range("G" + (9 + jx)).Value == null) ?
                                        (int)(trussElement.length / 2) :
                                        (int)wsMediationX.get_Range("G" + (9 + jx)).Value2 + (int)(trussElement.length / 2);
                                    break;
                                case 'C':
                                    wsMediationX.get_Range("H" + (9 + jx)).Value = (wsMediationX.get_Range("H" + (9 + jx)).Value == null) ?
                                        (int)(trussElement.length / 2) :
                                        (int)wsMediationX.get_Range("H" + (9 + jx)).Value2 + (int)(trussElement.length / 2);
                                    break;
                                case 'O':
                                    wsMediationX.get_Range("I" + (9 + jx)).Value = (wsMediationX.get_Range("I" + (9 + jx)).Value == null) ?
                                        (int)(trussElement.length / 2) :
                                        (int)wsMediationX.get_Range("I" + (9 + jx)).Value2 + (int)(trussElement.length / 2);
                                    break;
                            }
                        }

                        // Add Y direction stud label to match worksheet if matching report is desired (only completed on first truss pass)
                        if ((bool)arrDesignData[55] == true && ky == 0 && studElement.level == i && (studElement.direction == 'Y' || studElement.direction == 'A'))
                        {
                            Excel.Worksheet wsMediationY = Application.Worksheets.get_Item("Y-DIR L" + i);
                            wsMediationY.get_Range("D" + (9 + jy)).Value = studElement.label;
                        }

                        // Add truss tributary load to matching Y direction stud label on match worksheet if matching report is desired
                        if ((bool)arrDesignData[55] == true && studElement.level == i && (studElement.direction == 'Y' || studElement.direction == 'A')
                            && studElement.trussMatches.Any(r => r.trussLabel == trussElement.label))
                        {
                            Excel.Worksheet wsMediationY = Application.Worksheets.get_Item("Y-DIR L" + i);
                            wsMediationY.get_Range("J" + (9 + jy)).Offset[0, ky].Value = (int)(trussElement.length / 2);
                            // Add cumulative tributary load totals for each stud line
                            switch ((char)trussElement.label[3])
                            {
                                case 'U':
                                    wsMediationY.get_Range("F" + (9 + jy)).Value = (wsMediationY.get_Range("F" + (9 + jy)).Value == null) ?
                                        (int)(trussElement.length / 2) :
                                        (int)wsMediationY.get_Range("F" + (9 + jy)).Value2 + (int)(trussElement.length / 2);
                                    break;
                                case 'R':
                                    wsMediationY.get_Range("E" + (9 + jy)).Value = (wsMediationY.get_Range("E" + (9 + jy)).Value == null) ?
                                        (int)(trussElement.length / 2) :
                                        (int)wsMediationY.get_Range("E" + (9 + jy)).Value2 + (int)(trussElement.length / 2);
                                    break;
                                case 'B':
                                    wsMediationY.get_Range("G" + (9 + jy)).Value = (wsMediationY.get_Range("G" + (9 + jy)).Value == null) ?
                                        (int)(trussElement.length / 2) :
                                        (int)wsMediationY.get_Range("G" + (9 + jy)).Value2 + (int)(trussElement.length / 2);
                                    break;
                                case 'C':
                                    wsMediationY.get_Range("H" + (9 + jy)).Value = (wsMediationY.get_Range("H" + (9 + jy)).Value == null) ?
                                        (int)(trussElement.length / 2) :
                                        (int)wsMediationY.get_Range("H" + (9 + jy)).Value2 + (int)(trussElement.length / 2);
                                    break;
                                case 'O':
                                    wsMediationY.get_Range("I" + (9 + jy)).Value = (wsMediationY.get_Range("I" + (9 + jy)).Value == null) ?
                                        (int)(trussElement.length / 2) :
                                        (int)wsMediationY.get_Range("I" + (9 + jy)).Value2 + (int)(trussElement.length / 2);
                                    break;
                            }
                        }

                        // Increment stud counters
                        if ((bool)arrDesignData[55] == true && studElement.level == i && (studElement.direction == 'X' || studElement.direction == 'A'))
                        {
                            jx++;
                        }
                        if ((bool)arrDesignData[55] == true && studElement.level == i && (studElement.direction == 'Y' || studElement.direction == 'A'))
                        {
                            jy++;
                        }
                    }

                    // Add Y/A direction truss label to match worksheet if matching report is desired
                    if ((bool)arrDesignData[55] == true && trussElement.level == i && (trussElement.direction == 'Y' || trussElement.direction == 'A'))
                    {
                        Excel.Worksheet wsMediationX = Application.Worksheets.get_Item("X-DIR L" + i);
                        wsMediationX.get_Range("J" + 8).Offset[0, kx].Value = trussElement.label;
                        kx++;
                    }

                    // Add X direction truss label to match worksheet if matching report is desired
                    if ((bool)arrDesignData[55] == true && trussElement.level == i && trussElement.direction == 'X')
                    {
                        Excel.Worksheet wsMediationY = Application.Worksheets.get_Item("Y-DIR L" + i);
                        wsMediationY.get_Range("J" + 8).Offset[0, ky].Value = trussElement.label;
                        ky++;
                    }

                    // Reset stud counters
                    if ((bool)arrDesignData[55] == true)
                    {
                        jx = 0;
                        jy = 0;
                    }

                    // Increment progress bar
                    MediationProgress.progressBar.Increment(1);
                }

                // Resize colums on matching reports
                if ((bool)arrDesignData[55] == true)
                {
                    Excel.Worksheet wsMediation = Application.Worksheets.get_Item("X-DIR L" + i);
                    wsMediation.get_Range("A1", "NN1").EntireColumn.AutoFit();
                    wsMediation = Application.Worksheets.get_Item("Y-DIR L" + i);
                    wsMediation.get_Range("A1", "NN1").EntireColumn.AutoFit();
                }
            }
        return;
        }

        // VSM_Step1() -- Handles vertical matching of stud and truss lines
        public void VSM(ref List<RawLineData> arrStud, ref List<RawLineData> arrTruss, Object[] arrDesignData, int iLevel, ref SCAD.MediationProgressBar MediationProgress)
        {
            try
            {

            }
            catch (Exception e) { MessageBox.Show(e.Message); }
            return;
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
            /* StudLineReports() -- Called by clicking "Create Stud Reports" on
             * the SCAD Ribbon. Routine cycles through existing Stud lines and 
             * will, depending on user input, either create Key plan numbers for
             * unique stud walls on each level, or simply create pdfs of those walls
             * already selected in the "Ln Calc Table" worksheets. They then placed
             * in a directory under C:\SCAD\Reports and the folder is opened */

            // Call Stud Report Dialog
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
                // Declarations
                Excel.Application xlApp = this.Application;
                Excel.Worksheet wsInput = Application.Worksheets.get_Item("INPUT");
                string JobNumber = System.Convert.ToString(wsInput.get_Range("J4").Value);
                int levels = (int)wsInput.get_Range("D7").Value;

                // Deactivate Screen Updating while reports are made
                xlApp.ScreenUpdating = false;

                // Create Report Directories for job PDF files
                this.MkReportDirs(JobNumber);

                // Create PDF reports for each level according to the line's print flags
                for (int i = 1; i <= levels; i++)
                {
                    this.StudLevelReports(i,JobNumber);
                }

                // Activate Screen Updating after reports are made
                xlApp.ScreenUpdating = true;

                // Open folder where reports are held
                if (JobNumber == "")
                {
                   System.Diagnostics.Process.Start("explorer.exe", @"/select, C:\SCAD\Reports\Temp\");
                }
                else
                {
                    System.Diagnostics.Process.Start("explorer.exe", @"/select, C:\SCAD\Reports\" + JobNumber + @"\");
                }
            }

            return "Now back to SCAD Ribbon";
        }

        // StudLevelReports() -- Creates the actual PDF reports for a given level
        public void StudLevelReports(int level, string JobNumber)
        {
            /* StudLevelReports() -- Called by StudLineReports() to create the actual
             * PDF reports for each stud line on a given level, if it is flagged to be 
             * printed. */

            // Worksheet Declarations
            Excel.Worksheet wsInput = Application.Worksheets.get_Item("INPUT");
            Excel.Worksheet wsCalcTable = new Excel.Worksheet();
            Excel.Worksheet wsStudAnalysis = Application.Worksheets.get_Item("STUD ANALYSIS");
            foreach (Excel.Worksheet sheet in this.Application.Sheets)
            {
                if (sheet.Name == "L" + level + " Calc Table")
                {
                    wsCalcTable = sheet;
                    break;
                }
            }

            // Value Declarations
            int iStudn = (wsCalcTable.UsedRange.Rows.Count - 5);                                                    // Total Number of stud lines on Calc sheet
            string PrintAll = System.Convert.ToString(wsCalcTable.get_Range("B4").Value2);                          // Holds Print All flag for level
            System.Object[,] LineLabels = new System.Object[iStudn,1];                                              // Stores Key Plan numbers if they exist
            bool KeyPlansExist = false;
            if (wsCalcTable.get_Range("BS6").Text != "")
            {
                // Checks if Key plans exist, if they do, assign to LineLabels
                KeyPlansExist = true;
                LineLabels = (System.Object[,])wsCalcTable.get_Range("BS6", "BS" + (6 + iStudn)).Value2;
            }
            System.Object[,] StudLines = (System.Object[,])wsCalcTable.get_Range("A6", "AE" + (6 + iStudn)).Value2; // Holds stud line data for reports
                
            // Set Calculation to Manual while information is copied
            Globals.SCADMain.Application.Calculation = Excel.XlCalculation.xlCalculationManual;

            // Iterate through each stud line on Calc Table and create PDF report for it if flagged
            for (int i = 1; i <= iStudn; i++)
            {
                if (StudLines[i,1].ToString() == "Yes" || PrintAll == "Yes")
                {
                    // Populate Stud Analysis worksheet with line information to create PDF
                    {
                        // Determine Interior/Exterior Flag
                        if (StudLines[i, 3].ToString() == "I")
                        {
                            wsStudAnalysis.get_Range("C6").Value = "Interior";
                        }
                        else
                        {
                            wsStudAnalysis.get_Range("C6").Value = "Exterior";
                        }

                        wsStudAnalysis.get_Range("D6").Value = StudLines[i, 2].ToString();      // Wall Label
                        wsStudAnalysis.get_Range("G6").Value = level;                           // Floor Level
                        wsStudAnalysis.get_Range("J21").Value = StudLines[i, 15].ToString();    // Wall Height
                        wsStudAnalysis.get_Range("D17").Value = StudLines[i, 16].ToString();    // Roof DL Reaction
                        wsStudAnalysis.get_Range("E17").Value = StudLines[i, 17].ToString();    // Roof LL Reaction
                        wsStudAnalysis.get_Range("F10").Value = StudLines[i, 10].ToString();    // Roof Length
                        wsStudAnalysis.get_Range("D18").Value = StudLines[i, 18].ToString();    // Unit DL Reaction
                        wsStudAnalysis.get_Range("E18").Value = StudLines[i, 19].ToString();    // Unit LL Reaction
                        wsStudAnalysis.get_Range("F11").Value = StudLines[i, 11].ToString();    // Unit Length
                        wsStudAnalysis.get_Range("D19").Value = StudLines[i, 20].ToString();    // Balcony DL Reaction
                        wsStudAnalysis.get_Range("E19").Value = StudLines[i, 21].ToString();    // Balcony LL Reaction
                        wsStudAnalysis.get_Range("F12").Value = StudLines[i, 12].ToString();    // Balcony Length
                        wsStudAnalysis.get_Range("D20").Value = StudLines[i, 22].ToString();    // Corridor DL Reaction
                        wsStudAnalysis.get_Range("E20").Value = StudLines[i, 23].ToString();    // Corridor LL Reaction
                        wsStudAnalysis.get_Range("F13").Value = StudLines[i, 13].ToString();    // Corridor Length
                        wsStudAnalysis.get_Range("D21").Value = StudLines[i, 24].ToString();    // Other DL Reaction
                        wsStudAnalysis.get_Range("E21").Value = StudLines[i, 25].ToString();    // Other LL Reaction
                        wsStudAnalysis.get_Range("F14").Value = StudLines[i, 14].ToString();    // Other Length
                        wsStudAnalysis.get_Range("E28").Value = StudLines[i, 26].ToString();    // Unbraced Column Length Lx
                        wsStudAnalysis.get_Range("E29").Value = StudLines[i, 27].ToString();    // Unbraced Column Length Ly
                        wsStudAnalysis.get_Range("J11").Value = StudLines[i, 30].ToString();    // Stud Size
                        wsStudAnalysis.get_Range("J12").Value = StudLines[i, 31].ToString();    // Stud Spacing
                    }

                    // Create PDF file of Stud Analysis Report for each line
                    {
                        // Label used for individual report file names, uses Key plan numbers if exist, otherwise Temp name
                        string StudFileName = "Temp";
                        if (KeyPlansExist == false)
                        {
                            StudFileName = System.Convert.ToString(StudFileName + i);
                        }
                        else
                        {
                            StudFileName = LineLabels[i,1].ToString();
                        }

                        // Place in Temp folder if no Job Number given
                        if (JobNumber == "")                                
                        {
                            wsStudAnalysis.ExportAsFixedFormat(
                                Type: Excel.XlFixedFormatType.xlTypePDF, 
                                Filename: @"C:\SCAD\Reports\Temp\" + StudFileName + @".pdf", 
                                Quality: Excel.XlFixedFormatQuality.xlQualityMinimum,
                                IgnorePrintAreas:false,
                                OpenAfterPublish:false);
                        }
                        // Otherwise place in Job Number folder
                        else 
                        {
                            wsStudAnalysis.ExportAsFixedFormat(
                                Type: Excel.XlFixedFormatType.xlTypePDF, 
                                Filename: @"C:\SCAD\Reports\" + JobNumber + @"\" + StudFileName + @".pdf", 
                                Quality: Excel.XlFixedFormatQuality.xlQualityMinimum,
                                IgnorePrintAreas: false, 
                                OpenAfterPublish: false);
                        }
                    }
                }
            }

            // Return to Automatic Calculation after reports are made
            Globals.SCADMain.Application.Calculation = Excel.XlCalculation.xlCalculationAutomatic;
            return;
        }

        // StudUniqueReports() -- Sets up workbook to make reports for all unique walls on each level.
        public void StudUniqueReports(int levels)
        {
            /* StudUniqeReports() -- Called by StudLineReports() if the "Create Reports for
             * all unique stud lines" option is selected by the user. This creates a Key Plan
             * scheme and formula for each level to determine which walls are unique. Those 
             * unique walls are then flagged to be printed before the routine returns to the
             * normal reporting workflow. */

            // Declarations
            int iStud; // Stores total number of stud lines for each level

            Excel.Worksheet wsOutput = Application.Worksheets.get_Item("OUTPUT");
            Excel.Worksheet wsInput = Application.Worksheets.get_Item("INPUT");
            Excel.Worksheet wsAnalysis = Application.Worksheets.get_Item("STUD ANALYSIS");

            // Establish total number of stud walls and those on level 1 through used row ranges
            iStud = wsOutput.UsedRange.Rows.Count - 2;

            Globals.SCADMain.Application.Calculation = Excel.XlCalculation.xlCalculationManual; // Set to manual calculation while flagging lines
            
            // Treatment of the workbook and formulas that accomodate all levels
            {
                // Copy and Paste Job Number to remove formatting
                wsInput.get_Range("J4").Copy();
                wsInput.get_Range("J4").PasteSpecial(Excel.XlPasteType.xlPasteValues);

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

            // Iterate through level specific formulas and treatment of Stud Reporting and Calc Table worksheets
            for (int CurrentLevel = 1; CurrentLevel <= levels; CurrentLevel++)
            {
                this.StudTreatLevels(CurrentLevel);
            }

            // Return to Automatic Calculation mode
            Globals.SCADMain.Application.Calculation = Excel.XlCalculation.xlCalculationAutomatic;

            return;
        }

        // StudTreatLevels() - Routine that handles level specific treatment of Stud Reports.
        public void StudTreatLevels(int level)
        {
            /* StudTreatLevels() -- Called by StudUniqueReports() to handle repeated level specfic
             * routines that were long enough to warrant its own method. Sets up each level to
             * reflect Key Plan numbers and formulas for print flags */

            // Declarations
            Excel.Worksheet wsInput = Application.Worksheets.get_Item("INPUT");
            Excel.Worksheet wsCalcTable = new Excel.Worksheet();
            foreach (Excel.Worksheet sheet in this.Application.Sheets)
            {
                if (sheet.Name == "L" + level + " Calc Table")
                {
                    wsCalcTable = sheet;
                    break;
                }
            }

            // Establish total number of stud walls on level through used row ranges
            int iStudn = wsCalcTable.UsedRange.Rows.Count - 5;

            // Formula to determine if "Print All" is enabled, or if stud line is unique on its floor
            wsCalcTable.get_Range("A6").Formula = @"=IF($B$4=""Yes"",""Yes"",IF(BS6>0,""Yes"",""No""))";
            wsCalcTable.get_Range("A6").AutoFill(wsCalcTable.get_Range("A6", "A" + (iStudn + 6)));

            // Formula for assigning Key Plan numbers to walls
            wsCalcTable.get_Range("BS6").Value = (level * 100) + 1;
            wsCalcTable.get_Range("BS7").Formula = @"=IFERROR(IF(MATCH(MAX(BT7:EP7),BS$6:BS6,0)>0,0,0),MAX(BT7:EP7))";
            wsCalcTable.get_Range("BS7").AutoFill(wsCalcTable.get_Range("BS7", "BS" + (iStudn + 6)));

            // Null columns and rows while formula determines Key Plan numbers
            wsCalcTable.get_Range("BT6").Value = 0;
            wsCalcTable.get_Range("BT7").Value = 0;
            wsCalcTable.get_Range("BT6", "BT7").AutoFill(wsCalcTable.get_Range("BT6", "BT" + (iStudn + 6)));
            wsCalcTable.get_Range("BV6").Value = 0;
            wsCalcTable.get_Range("BW6").Value = 0;
            wsCalcTable.get_Range("BV6", "BW6").AutoFill(wsCalcTable.get_Range("BV6", "HN6"));

            // Sets the initial unique Key Plan number
            wsCalcTable.get_Range("BU6").Formula = "=BT6";

            // Creates Header Row for Key Plan Numbers
            wsCalcTable.get_Range("BU5").Value = (level * 100) + 1;
            wsCalcTable.get_Range("BV5").Value = (level * 100) + 2;
            wsCalcTable.get_Range("BU5", "BV5").AutoFill(wsCalcTable.get_Range("BU5", "HN5"));

            // Formula for determining unique Key Plan numbers for each wall
            wsCalcTable.get_Range("BU7").Formula = @"=IF(AND(SUM($BT7:BT7)=0,SUM(BU$6:BU6)=0),BU$5,IFERROR(IF(AND($C7=(INDIRECT(""C""&MATCH(BU$5,BU$6:BU6,0)+5)),"
                + @"$AD7=(INDIRECT(""AD""&MATCH(BU$5,BU$6:BU6,0)+5)),$AE7=(INDIRECT(""AE""&MATCH(BU$5,BU$6:BU6,0)+5)),$Z7=(INDIRECT(""Z""&MATCH(BU$5,BU$6:BU6,0)+5))"
                + @",ABS($H7-(INDIRECT(""H""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($I7-(INDIRECT(""I""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($J7-(INDIRECT(""J""&MATCH"
                + @"(BU$5,BU$6:BU6,0)+5)))<=1,ABS($K7-(INDIRECT(""K""&MATCH(BU$5,BU$6:BU6,0)+5)))<=1,ABS($L7-(INDIRECT(""L""&MATCH(BU$5,BU$6:BU6,0)+5))<=1),ABS($M7-"
                + @"(INDIRECT(""M""&MATCH(BU$5,BU$6:BU6,0)+5)))<=1,ABS($N7-(INDIRECT(""N""&MATCH(BU$5,BU$6:BU6,0)+5)))<=1,ABS($O7-(INDIRECT(""O""&MATCH(BU$5,BU$6:BU6,0)"
                + @"+5)))<=100,ABS($P7-(INDIRECT(""P""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($Q7-(INDIRECT(""Q""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($R7-(INDIRECT(""R""&"
                + @"MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($S7-(INDIRECT(""S""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($T7-(INDIRECT(""T""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS"
                + @"($U7-(INDIRECT(""U""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($V7-(INDIRECT(""V""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($W7-(INDIRECT(""W""&MATCH(BU$5,BU$6:BU6,0)+5)))"
                + @"<=100,ABS($X7-(INDIRECT(""X""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100,ABS($Y7-(INDIRECT(""Y""&MATCH(BU$5,BU$6:BU6,0)+5)))<=100),BU$5,0),0))";
            wsCalcTable.get_Range("BU7").AutoFill(wsCalcTable.get_Range("BU7", "HN7"));
            wsCalcTable.get_Range("BU7", "HN7").AutoFill(wsCalcTable.get_Range("BU7", "HN" + (iStudn + 6)));

            ((Excel._Worksheet)wsCalcTable).Calculate();

            // Copy and Paste formula Values to reduce file size and increase reporting speed
            wsCalcTable.get_Range("BS6", "HN" + (iStudn + 6)).Copy();
            wsCalcTable.get_Range("BS6", "HN" + (iStudn + 6)).PasteSpecial(Excel.XlPasteType.xlPasteValues);

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

        // LateralReportPacks() -- Creates PDF reports of Lateral Design packs
        public string LateralReportPacks()
        {
            /* LateralReportPacks() -- Creates the PDF reports of Lateral Design packs
             * for the imported CAD data, Seismic Load Input, Wind Load Input, and
             * Wall Geometry Input data.*/

            // Open confirm dialog box.
            SCAD.LateralReportConfirm ConfirmBox = new SCAD.LateralReportConfirm();
            ConfirmBox.ShowDialog();

            if (ConfirmBox.ReportConfirm == false)
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

            // Declarations
            Excel.Worksheet wsCAD = Application.Worksheets.get_Item("CAD IMPORT");
            Excel.Worksheet wsCode = Application.Worksheets.get_Item("Code Info");
            Excel.Worksheet wsWind = Application.Worksheets.get_Item("Wind Load Input");
            Excel.Worksheet wsSeismic = Application.Worksheets.get_Item("Seismic Load Input");
            Excel.Worksheet wsWallGeom = Application.Worksheets.get_Item("Wall Geometry Input");
            Excel.Worksheet wsIteration = Application.Worksheets.get_Item("Iteration");

            int xWalls = (int)wsCAD.get_Range("BK7").Value2; // Stores the total number of Shear Walls in either X Direction
            int yWalls = (int)wsCAD.get_Range("BW7").Value2; // Stores the total number of Shear Walls in either Y Direction
            int levels = (int)wsCAD.get_Range("I3").Value2; // Stores the total number of levels in the project
            string JobNumber = wsCode.get_Range("R4").Value2.ToString(); // Stores the Job Number for the project

            // Make Directories for Reports
            this.MkReportDirs(JobNumber);

            // Create a PDF report of the "Code Info" worksheet, the print range is specified first
            Excel.Range rngPrint = wsCode.get_Range("B2", "U104");
            rngPrint.ExportAsFixedFormat(
                                Type: Excel.XlFixedFormatType.xlTypePDF,
                                Filename: @"C:\SCAD\Reports\" + JobNumber + @"\Code_Info.pdf",
                                Quality: Excel.XlFixedFormatQuality.xlQualityMinimum,
                                IgnorePrintAreas: false,
                                OpenAfterPublish: false);

            // Create a PDF report of the "Wind Load Input" worksheet, the print range is specified first depending on levels
            rngPrint = wsWind.get_Range("C2", "U70");
            switch (levels)
            {
                case 3:
                case 4:
                    rngPrint = wsWind.get_Range("C2", "U132");
                    break;
                case 5:
                case 6:
                    rngPrint = wsWind.get_Range("C2", "U194");
                    break;
            }
            rngPrint.ExportAsFixedFormat(
                                Type: Excel.XlFixedFormatType.xlTypePDF,
                                Filename: @"C:\SCAD\Reports\" + JobNumber + @"\Wind_Load_Input.pdf",
                                Quality: Excel.XlFixedFormatQuality.xlQualityMinimum,
                                IgnorePrintAreas: false,
                                OpenAfterPublish: false);

            // Create a PDF report of the "Seismic Load Input" worksheet, the print range is specified first depending on levels
            rngPrint = wsSeismic.get_Range("C2", "U70");
            switch (levels)
            {
                case 3:
                case 4:
                    rngPrint = wsSeismic.get_Range("C2", "U132");
                    break;
                case 5:
                case 6:
                    rngPrint = wsSeismic.get_Range("C2", "U194");
                    break;
            }
            rngPrint.ExportAsFixedFormat(
                                Type: Excel.XlFixedFormatType.xlTypePDF,
                                Filename: @"C:\SCAD\Reports\" + JobNumber + @"\Seismic_Load_Input.pdf",
                                Quality: Excel.XlFixedFormatQuality.xlQualityMinimum,
                                IgnorePrintAreas: false,
                                OpenAfterPublish: false);

            // Create a PDF report of the wall lines on  "Wall Geometry Input" worksheet
            // The print range is specified first depending on number of X direction walls and Level
            for (int i = 0; i < levels; i++)
            {
                rngPrint = wsWallGeom.get_Range("B" + (2 + (i * 213)), "Q" + (78 + (i * 213)));

                if (xWalls >= 68)
                {
                    rngPrint = wsWallGeom.get_Range("B" + (2 + (i * 213)), "Q" + (145 + (i * 213)));
                }

                if (xWalls >= 134)
                {
                    rngPrint = wsWallGeom.get_Range("B" + (2 + (i * 213)), "Q" + (212 + (i * 213)));
                }

                rngPrint.ExportAsFixedFormat(
                        Type: Excel.XlFixedFormatType.xlTypePDF,
                        Filename: @"C:\SCAD\Reports\" + JobNumber + @"\Wall_Geometry_Output_L" + (i + 1) + @"X.pdf",
                        Quality: Excel.XlFixedFormatQuality.xlQualityMinimum,
                        IgnorePrintAreas: false,
                        OpenAfterPublish: false);
            }

            // Create a PDF report of the wall lines on "Wall Geometry Input" worksheet, 
            // The print range is specified first depending on number of Y direction walls and level
            for (int i = 0; i < levels; i++)
            {
                rngPrint = wsWallGeom.get_Range("S" + (2 + (i * 213)), "AH" + (78 + (i * 213)));

                if (yWalls >= 68)
                {
                    rngPrint = wsWallGeom.get_Range("S" + (2 + (i * 213)), "AH" + (145 + (i * 213)));
                }

                if (yWalls >= 134)
                {
                    rngPrint = wsWallGeom.get_Range("S" + (2 + (i * 213)), "AH" + (212 + (i * 213)));
                }

                rngPrint.ExportAsFixedFormat(
                        Type: Excel.XlFixedFormatType.xlTypePDF,
                        Filename: @"C:\SCAD\Reports\" + JobNumber + @"\Wall_Geometry_Output_L" + (i + 1) + @"Y.pdf",
                        Quality: Excel.XlFixedFormatQuality.xlQualityMinimum,
                        IgnorePrintAreas: false,
                        OpenAfterPublish: false);
            }

            // Create a PDF report of the wall lines on "Iteration" worksheet
            // The print range is specified first depending on number of X direction walls and Level
            string CellColumn1 = "C";
            string CellColumn2 = "AA";

            for (int i = 0; i < levels; i++)
            {
                switch (i)              // Select proper cell column range depending on level
                {
                    case 0:
                        CellColumn1 = "C";
                        CellColumn2 = "AA";
                        break;
                    case 1:
                        CellColumn1 = "BZ";
                        CellColumn2 = "CX";
                        break;
                    case 2:
                        CellColumn1 = "EW";
                        CellColumn2 = "FU";
                        break;
                    case 3:
                        CellColumn1 = "HT";
                        CellColumn2 = "IR";
                        break;
                    case 4:
                        CellColumn1 = "KQ";
                        CellColumn2 = "LO";
                        break;
                    case 5:
                        CellColumn1 = "NN";
                        CellColumn2 = "OL";
                        break;
                }

                //Select Row Range depending on wall count
                rngPrint = wsIteration.get_Range(CellColumn1 + 15, CellColumn2 + 64);

                if (xWalls >= 50)
                {
                    rngPrint = wsIteration.get_Range(CellColumn1 + 15, CellColumn2 + 114);
                }

                if (xWalls >= 100)
                {
                    rngPrint = wsIteration.get_Range(CellColumn1 + 15, CellColumn2 + 164);
                }

                if (xWalls >= 150)
                {
                    rngPrint = wsIteration.get_Range(CellColumn1 + 15, CellColumn2 + 214);
                }

                // Create PDF Report
                rngPrint.ExportAsFixedFormat(
                        Type: Excel.XlFixedFormatType.xlTypePDF,
                        Filename: @"C:\SCAD\Reports\" + JobNumber + @"\Iteration_L" + (i + 1) + @"X.pdf",
                        Quality: Excel.XlFixedFormatQuality.xlQualityMinimum,
                        IgnorePrintAreas: false,
                        OpenAfterPublish: false);
            }

            // Create a PDF report of the wall lines on "Iteraion" worksheet, 
            // The print range is specified first depending on number of Y direction walls and level
            for (int i = 0; i < levels; i++)
            {
                switch (i)          // Select proper cell column range depending on level
                {
                    case 0:
                        CellColumn1 = "AG";
                        CellColumn2 = "BE";
                        break;
                    case 1:
                        CellColumn1 = "DD";
                        CellColumn2 = "EB";
                        break;
                    case 2:
                        CellColumn1 = "GA";
                        CellColumn2 = "GY";
                        break;
                    case 3:
                        CellColumn1 = "IX";
                        CellColumn2 = "JV";
                        break;
                    case 4:
                        CellColumn1 = "LU";
                        CellColumn2 = "MS";
                        break;
                    case 5:
                        CellColumn1 = "OR";
                        CellColumn2 = "PP";
                        break;
                }

                //Select Row Range depending on wall count
                rngPrint = wsIteration.get_Range(CellColumn1 + 15, CellColumn2 + 64);

                if (yWalls >= 50)   
                {
                    rngPrint = wsIteration.get_Range(CellColumn1 + 15, CellColumn2 + 114);
                }

                if (yWalls >= 100)
                {
                    rngPrint = wsIteration.get_Range(CellColumn1 + 15, CellColumn2 + 164);
                }

                if (yWalls >= 150)
                {
                    rngPrint = wsIteration.get_Range(CellColumn1 + 15, CellColumn2 + 214);
                }

                // Create PDF Report
                rngPrint.ExportAsFixedFormat(
                    Type: Excel.XlFixedFormatType.xlTypePDF,
                    Filename: @"C:\SCAD\Reports\" + JobNumber + @"\Iteration_L" + (i + 1) + @"Y.pdf",
                    Quality: Excel.XlFixedFormatQuality.xlQualityMinimum,
                    IgnorePrintAreas: false,
                    OpenAfterPublish: false);
            }

            // Open folder where reports are held
            if (JobNumber == "")
            {
                System.Diagnostics.Process.Start("explorer.exe", @"/select, C:\SCAD\Reports\Temp\");
            }
            else
            {
                System.Diagnostics.Process.Start("explorer.exe", @"/select, C:\SCAD\Reports\" + JobNumber + @"\");
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
