namespace SCAD
{
    partial class SCADRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public SCADRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SCADRibbon));
            this.TabSCAD = this.Factory.CreateRibbonTab();
            this.groupStuds = this.Factory.CreateRibbonGroup();
            this.LaunchStuds = this.Factory.CreateRibbonButton();
            this.RelaunchStud = this.Factory.CreateRibbonButton();
            this.SetStudCallout = this.Factory.CreateRibbonButton();
            this.StartDynamicSchedule = this.Factory.CreateRibbonButton();
            this.FinalizeSchedule = this.Factory.CreateRibbonButton();
            this.CreateScriptStud = this.Factory.CreateRibbonButton();
            this.PrintStudLines = this.Factory.CreateRibbonButton();
            this.LateralTools = this.Factory.CreateRibbonGroup();
            this.PrelimLateral = this.Factory.CreateRibbonButton();
            this.FullLateral = this.Factory.CreateRibbonButton();
            this.SetupWalls = this.Factory.CreateRibbonButton();
            this.CreateScriptLateral = this.Factory.CreateRibbonButton();
            this.PrintReportsLateral = this.Factory.CreateRibbonButton();
            this.ExportRISADiaphragm = this.Factory.CreateRibbonButton();
            this.StandaloneTools = this.Factory.CreateRibbonGroup();
            this.OpenChecklistSpecs = this.Factory.CreateRibbonButton();
            this.OpenBeam = this.Factory.CreateRibbonButton();
            this.OpenColumn = this.Factory.CreateRibbonButton();
            this.OpenStud = this.Factory.CreateRibbonButton();
            this.OpenWind = this.Factory.CreateRibbonButton();
            this.OpenSeismic = this.Factory.CreateRibbonButton();
            this.OpenSeismic2 = this.Factory.CreateRibbonButton();
            this.OpenWoodBrickDiff = this.Factory.CreateRibbonButton();
            this.OpenPeriod = this.Factory.CreateRibbonButton();
            this.TabSCAD.SuspendLayout();
            this.groupStuds.SuspendLayout();
            this.LateralTools.SuspendLayout();
            this.StandaloneTools.SuspendLayout();
            // 
            // TabSCAD
            // 
            this.TabSCAD.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.TabSCAD.Groups.Add(this.groupStuds);
            this.TabSCAD.Groups.Add(this.LateralTools);
            this.TabSCAD.Groups.Add(this.StandaloneTools);
            this.TabSCAD.Label = "SCAD 2.0";
            this.TabSCAD.Name = "TabSCAD";
            this.TabSCAD.Position = this.Factory.RibbonPosition.BeforeOfficeId("TabHome");
            // 
            // groupStuds
            // 
            this.groupStuds.Items.Add(this.LaunchStuds);
            this.groupStuds.Items.Add(this.RelaunchStud);
            this.groupStuds.Items.Add(this.SetStudCallout);
            this.groupStuds.Items.Add(this.StartDynamicSchedule);
            this.groupStuds.Items.Add(this.FinalizeSchedule);
            this.groupStuds.Items.Add(this.CreateScriptStud);
            this.groupStuds.Items.Add(this.PrintStudLines);
            this.groupStuds.Label = "Stud Tools";
            this.groupStuds.Name = "groupStuds";
            // 
            // LaunchStuds
            // 
            this.LaunchStuds.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.LaunchStuds.Image = ((System.Drawing.Image)(resources.GetObject("LaunchStuds.Image")));
            this.LaunchStuds.Label = "Stud Design";
            this.LaunchStuds.Name = "LaunchStuds";
            this.LaunchStuds.ScreenTip = "Launch Stud Design";
            this.LaunchStuds.ShowImage = true;
            this.LaunchStuds.SuperTip = "Launches initial stud design process. Must be called from the raw CAD data Excel " +
    "file generated from the project data (Data Required: Stud Walls, Load Lines, Gap" +
    " Lines, Diaphragm Lines)";
            this.LaunchStuds.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.LaunchStuds_Click);
            // 
            // RelaunchStud
            // 
            this.RelaunchStud.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.RelaunchStud.Label = "Relaunch Design";
            this.RelaunchStud.Name = "RelaunchStud";
            this.RelaunchStud.OfficeImageId = "Repeat";
            this.RelaunchStud.ScreenTip = "Relaunch Design";
            this.RelaunchStud.ShowImage = true;
            this.RelaunchStud.SuperTip = "Relaunches the Stud Design with CAD data and Project Settings (Must be in Stud De" +
    "sign workbook)";
            this.RelaunchStud.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.RelaunchStud_Click);
            // 
            // SetStudCallout
            // 
            this.SetStudCallout.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.SetStudCallout.Label = "Set Callouts";
            this.SetStudCallout.Name = "SetStudCallout";
            this.SetStudCallout.OfficeImageId = "Refresh";
            this.SetStudCallout.ScreenTip = "Sets user-defined callout for individual wall";
            this.SetStudCallout.ShowImage = true;
            this.SetStudCallout.SuperTip = "Recalculates stud wall data with new user-defined callout for individual walls (o" +
    "n \"Ln Calc Table\" worksheets)";
            this.SetStudCallout.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SetStudCallout_Click);
            // 
            // StartDynamicSchedule
            // 
            this.StartDynamicSchedule.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.StartDynamicSchedule.Label = "Start Dynamic Schedule";
            this.StartDynamicSchedule.Name = "StartDynamicSchedule";
            this.StartDynamicSchedule.OfficeImageId = "OutlineGroup";
            this.StartDynamicSchedule.ScreenTip = "Start Dynamic Schedule";
            this.StartDynamicSchedule.ShowImage = true;
            this.StartDynamicSchedule.SuperTip = "Begins generating a projet stud schedule.  The designer will then consolidate the" +
    "m into the final schedule.";
            this.StartDynamicSchedule.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.StartDynamicSchedule_Click);
            // 
            // FinalizeSchedule
            // 
            this.FinalizeSchedule.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.FinalizeSchedule.Label = "Finish Dynamic Schedule";
            this.FinalizeSchedule.Name = "FinalizeSchedule";
            this.FinalizeSchedule.OfficeImageId = "Consolidate";
            this.FinalizeSchedule.ScreenTip = "Finalizes User-Defined Stud Schedule";
            this.FinalizeSchedule.ShowImage = true;
            this.FinalizeSchedule.SuperTip = "After the designer creates anc consolidates the indtended schedule for the projec" +
    "t, this is conducted to update the workbook so the new callouts will be applied." +
    "";
            this.FinalizeSchedule.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.FinalizeSchedule_Click);
            // 
            // CreateScriptStud
            // 
            this.CreateScriptStud.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.CreateScriptStud.Label = "Create Script File";
            this.CreateScriptStud.Name = "CreateScriptStud";
            this.CreateScriptStud.OfficeImageId = "ViewPageLayoutView";
            this.CreateScriptStud.ScreenTip = "Create Stud CAD Script File";
            this.CreateScriptStud.ShowImage = true;
            this.CreateScriptStud.SuperTip = "Creates a CAD script file to copy the stud design data into the project drawing. " +
    "Allows the designer to specify what data to copy over (callouts, wall name, key " +
    "plan, schedule, endpoints, reactions)";
            this.CreateScriptStud.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.CreateScriptStud_Click);
            // 
            // PrintStudLines
            // 
            this.PrintStudLines.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.PrintStudLines.Label = "Create Stud Reports";
            this.PrintStudLines.Name = "PrintStudLines";
            this.PrintStudLines.OfficeImageId = "FilePrintPreview";
            this.PrintStudLines.ScreenTip = "Create Stud PDF reports";
            this.PrintStudLines.ShowImage = true;
            this.PrintStudLines.SuperTip = "Creates PDF report files of all designed stud walls.";
            this.PrintStudLines.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.PrintStudLines_Click);
            // 
            // LateralTools
            // 
            this.LateralTools.Items.Add(this.PrelimLateral);
            this.LateralTools.Items.Add(this.FullLateral);
            this.LateralTools.Items.Add(this.SetupWalls);
            this.LateralTools.Items.Add(this.CreateScriptLateral);
            this.LateralTools.Items.Add(this.PrintReportsLateral);
            this.LateralTools.Items.Add(this.ExportRISADiaphragm);
            this.LateralTools.Label = "Lateral Design Tools";
            this.LateralTools.Name = "LateralTools";
            // 
            // PrelimLateral
            // 
            this.PrelimLateral.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.PrelimLateral.Image = ((System.Drawing.Image)(resources.GetObject("PrelimLateral.Image")));
            this.PrelimLateral.Label = "Prelim Lateral Design";
            this.PrelimLateral.Name = "PrelimLateral";
            this.PrelimLateral.ScreenTip = "Preliminary Lateral Design";
            this.PrelimLateral.ShowImage = true;
            this.PrelimLateral.SuperTip = "Begins Preliminary Lateral Design from Raw Shear CAD data set. Loads the lateral " +
    "design workbook.";
            this.PrelimLateral.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.PrelimLateral_Click);
            // 
            // FullLateral
            // 
            this.FullLateral.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.FullLateral.Label = "Finalize Lateral Design";
            this.FullLateral.Name = "FullLateral";
            this.FullLateral.OfficeImageId = "TracePrecedentsRemoveArrows";
            this.FullLateral.ScreenTip = "Finalize Lateral Design";
            this.FullLateral.ShowImage = true;
            this.FullLateral.SuperTip = "Finalizes Lateral Design after wind, seismic, and wall geometry has been designed" +
    " for project. Develops holddown design data.";
            this.FullLateral.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.FullLateral_Click);
            // 
            // SetupWalls
            // 
            this.SetupWalls.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.SetupWalls.Label = "ReSetup Walls";
            this.SetupWalls.Name = "SetupWalls";
            this.SetupWalls.OfficeImageId = "ChartTypeOtherInsertDialog";
            this.SetupWalls.ScreenTip = "ReSetup Walls";
            this.SetupWalls.ShowImage = true;
            this.SetupWalls.SuperTip = "Recalculates Iteration data values if new walls are created or if wind/seismic/wa" +
    "ll geometry values are changed.";
            this.SetupWalls.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SetupWalls_Click);
            // 
            // CreateScriptLateral
            // 
            this.CreateScriptLateral.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.CreateScriptLateral.Label = "Create Script File";
            this.CreateScriptLateral.Name = "CreateScriptLateral";
            this.CreateScriptLateral.OfficeImageId = "ViewPageLayoutView";
            this.CreateScriptLateral.ScreenTip = "Create Script File";
            this.CreateScriptLateral.ShowImage = true;
            this.CreateScriptLateral.SuperTip = "Creates CAD Script file for Lateral Design Data which includes Shear Wall Names, " +
    "Length, Design, Anchors, Endpoints, and Drag Forces.";
            this.CreateScriptLateral.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.CreateScriptLateral_Click);
            // 
            // PrintReportsLateral
            // 
            this.PrintReportsLateral.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.PrintReportsLateral.Label = "Create Lateral Reports";
            this.PrintReportsLateral.Name = "PrintReportsLateral";
            this.PrintReportsLateral.OfficeImageId = "FilePrintPreview";
            this.PrintReportsLateral.ScreenTip = "Create Lateral PDF Report";
            this.PrintReportsLateral.ShowImage = true;
            this.PrintReportsLateral.SuperTip = "Creates PDF reports for Lateral Design data. Must be called from the Full Lateral" +
    " Design workbook. ";
            this.PrintReportsLateral.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.PrintReportsLateral_Click);
            // 
            // ExportRISADiaphragm
            // 
            this.ExportRISADiaphragm.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.ExportRISADiaphragm.Label = "Send to RISA Diaphragm";
            this.ExportRISADiaphragm.Name = "ExportRISADiaphragm";
            this.ExportRISADiaphragm.OfficeImageId = "SlicerConnectionsMenu";
            this.ExportRISADiaphragm.ScreenTip = "Export to RISA Diaphragm workbbok";
            this.ExportRISADiaphragm.ShowImage = true;
            this.ExportRISADiaphragm.SuperTip = "Exports lateral diapgrahm data to RISA Diaphragm conversion workbook.";
            this.ExportRISADiaphragm.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ExportRISADiaphragm_Click);
            // 
            // StandaloneTools
            // 
            this.StandaloneTools.Items.Add(this.OpenChecklistSpecs);
            this.StandaloneTools.Items.Add(this.OpenBeam);
            this.StandaloneTools.Items.Add(this.OpenColumn);
            this.StandaloneTools.Items.Add(this.OpenStud);
            this.StandaloneTools.Items.Add(this.OpenWind);
            this.StandaloneTools.Items.Add(this.OpenSeismic);
            this.StandaloneTools.Items.Add(this.OpenSeismic2);
            this.StandaloneTools.Items.Add(this.OpenWoodBrickDiff);
            this.StandaloneTools.Items.Add(this.OpenPeriod);
            this.StandaloneTools.Label = "Standalone Tools";
            this.StandaloneTools.Name = "StandaloneTools";
            // 
            // OpenChecklistSpecs
            // 
            this.OpenChecklistSpecs.Label = "Checklist/Specs";
            this.OpenChecklistSpecs.Name = "OpenChecklistSpecs";
            this.OpenChecklistSpecs.OfficeImageId = "Spelling";
            this.OpenChecklistSpecs.ShowImage = true;
            this.OpenChecklistSpecs.SuperTip = "Opens Project Checklist & Specs";
            this.OpenChecklistSpecs.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenChecklistSpecs_Click);
            // 
            // OpenBeam
            // 
            this.OpenBeam.Label = "Beam (13 AISC/2005 NDS)";
            this.OpenBeam.Name = "OpenBeam";
            this.OpenBeam.OfficeImageId = "PageScaleToFitWidth";
            this.OpenBeam.ShowImage = true;
            this.OpenBeam.SuperTip = "Opens 13th Ed AISC/2005 NDS Individual Beam workbook.";
            this.OpenBeam.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenBeam_Click);
            // 
            // OpenColumn
            // 
            this.OpenColumn.Label = "Column (2005 NDS)";
            this.OpenColumn.Name = "OpenColumn";
            this.OpenColumn.OfficeImageId = "PageScaleToFitHeight";
            this.OpenColumn.ShowImage = true;
            this.OpenColumn.SuperTip = "Opens 2005 NDS Individual Column workbook.";
            this.OpenColumn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenColumn_Click);
            // 
            // OpenStud
            // 
            this.OpenStud.Label = "Stud (2005 NDS)";
            this.OpenStud.Name = "OpenStud";
            this.OpenStud.OfficeImageId = "Consolidate";
            this.OpenStud.ShowImage = true;
            this.OpenStud.SuperTip = "Opens 2005 NDS Individual Stud workbook.";
            this.OpenStud.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenStud_Click);
            // 
            // OpenWind
            // 
            this.OpenWind.Label = "Wind (ASCE 7-10/05/02)";
            this.OpenWind.Name = "OpenWind";
            this.OpenWind.OfficeImageId = "HyperlinkInsert";
            this.OpenWind.ShowImage = true;
            this.OpenWind.SuperTip = "Opens ASCE 7-10/05/02 Wind workbook.";
            this.OpenWind.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenWind_Click);
            // 
            // OpenSeismic
            // 
            this.OpenSeismic.Label = "Seismic (ASCE 7-05)";
            this.OpenSeismic.Name = "OpenSeismic";
            this.OpenSeismic.OfficeImageId = "SparklineLineInsert";
            this.OpenSeismic.ShowImage = true;
            this.OpenSeismic.SuperTip = "Opens ASCE 7-05 Seismic workbook.";
            this.OpenSeismic.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenSeismic_Click);
            // 
            // OpenSeismic2
            // 
            this.OpenSeismic2.Label = "Seismic (ASCE 7-10)";
            this.OpenSeismic2.Name = "OpenSeismic2";
            this.OpenSeismic2.OfficeImageId = "SparklineLineInsert";
            this.OpenSeismic2.ShowImage = true;
            this.OpenSeismic2.SuperTip = "Opens ASCE 7-10 Seismic workbook.";
            this.OpenSeismic2.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenSeismic2_Click);
            // 
            // OpenWoodBrickDiff
            // 
            this.OpenWoodBrickDiff.Label = "Wood/Brick Differential";
            this.OpenWoodBrickDiff.Name = "OpenWoodBrickDiff";
            this.OpenWoodBrickDiff.OfficeImageId = "DesignMode";
            this.OpenWoodBrickDiff.ShowImage = true;
            this.OpenWoodBrickDiff.SuperTip = "Opens Wood/Brick Differential workbook.";
            this.OpenWoodBrickDiff.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenWoodBrickDiff_Click);
            // 
            // OpenPeriod
            // 
            this.OpenPeriod.Label = "Building Period";
            this.OpenPeriod.Name = "OpenPeriod";
            this.OpenPeriod.OfficeImageId = "ReviewShareWorkbook";
            this.OpenPeriod.ShowImage = true;
            this.OpenPeriod.SuperTip = "Opens Building Period workbook.";
            this.OpenPeriod.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenPeriod_Click);
            // 
            // SCADRibbon
            // 
            this.Name = "SCADRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.TabSCAD);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.SCADRibbon_Load);
            this.TabSCAD.ResumeLayout(false);
            this.TabSCAD.PerformLayout();
            this.groupStuds.ResumeLayout(false);
            this.groupStuds.PerformLayout();
            this.LateralTools.ResumeLayout(false);
            this.LateralTools.PerformLayout();
            this.StandaloneTools.ResumeLayout(false);
            this.StandaloneTools.PerformLayout();

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab TabSCAD;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupStuds;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton LaunchStuds;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton SetStudCallout;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton RelaunchStud;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton StartDynamicSchedule;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton CreateScriptStud;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup LateralTools;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton PrelimLateral;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton FullLateral;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup StandaloneTools;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton PrintStudLines;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton SetupWalls;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton CreateScriptLateral;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton PrintReportsLateral;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ExportRISADiaphragm;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton OpenChecklistSpecs;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton OpenBeam;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton OpenColumn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton OpenStud;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton OpenWind;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton OpenSeismic;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton OpenSeismic2;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton OpenWoodBrickDiff;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton OpenPeriod;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton FinalizeSchedule;
    }

    partial class ThisRibbonCollection
    {
        internal SCADRibbon Ribbon1
        {
            get { return this.GetRibbon<SCADRibbon>(); }
        }
    }
}
