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
            this.TabSCAD = this.Factory.CreateRibbonTab();
            this.groupStuds = this.Factory.CreateRibbonGroup();
            this.LaunchStuds = this.Factory.CreateRibbonButton();
            this.SetStudCallout = this.Factory.CreateRibbonButton();
            this.RelaunchStud = this.Factory.CreateRibbonButton();
            this.TabSCAD.SuspendLayout();
            this.groupStuds.SuspendLayout();
            // 
            // TabSCAD
            // 
            this.TabSCAD.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.TabSCAD.Groups.Add(this.groupStuds);
            this.TabSCAD.Label = "SCAD 2.0";
            this.TabSCAD.Name = "TabSCAD";
            this.TabSCAD.Position = this.Factory.RibbonPosition.BeforeOfficeId("TabHome");
            // 
            // groupStuds
            // 
            this.groupStuds.Items.Add(this.LaunchStuds);
            this.groupStuds.Items.Add(this.SetStudCallout);
            this.groupStuds.Items.Add(this.RelaunchStud);
            this.groupStuds.Label = "Stud Tools";
            this.groupStuds.Name = "groupStuds";
            // 
            // LaunchStuds
            // 
            this.LaunchStuds.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.LaunchStuds.Image = global::SCAD.Properties.Resources.SCAbutton;
            this.LaunchStuds.Label = "Stud Design";
            this.LaunchStuds.Name = "LaunchStuds";
            this.LaunchStuds.ShowImage = true;
            this.LaunchStuds.SuperTip = "Launches initial stud design process.";
            this.LaunchStuds.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.LaunchStuds_Click);
            // 
            // SetStudCallout
            // 
            this.SetStudCallout.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.SetStudCallout.Label = "Set Callouts";
            this.SetStudCallout.Name = "SetStudCallout";
            this.SetStudCallout.OfficeImageId = "Refresh";
            this.SetStudCallout.ShowImage = true;
            this.SetStudCallout.SuperTip = "Recalculates stud wall data with new user-defined callout for individual walls (o" +
    "n \"Ln Calc Table\" worksheets)";
            this.SetStudCallout.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SetStudCallout_Click);
            // 
            // RelaunchStud
            // 
            this.RelaunchStud.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.RelaunchStud.Label = "Relaunch Design";
            this.RelaunchStud.Name = "RelaunchStud";
            this.RelaunchStud.OfficeImageId = "Repeat";
            this.RelaunchStud.ScreenTip = "Relaunch Stud Design with CAD data.";
            this.RelaunchStud.ShowImage = true;
            this.RelaunchStud.SuperTip = "Relaunches the Stud Design with CAD data and Project Settings (Must be in Stud De" +
    "sign workbook)";
            this.RelaunchStud.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.RelaunchStud_Click);
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

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab TabSCAD;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupStuds;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton LaunchStuds;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton SetStudCallout;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton RelaunchStud;
    }

    partial class ThisRibbonCollection
    {
        internal SCADRibbon Ribbon1
        {
            get { return this.GetRibbon<SCADRibbon>(); }
        }
    }
}
