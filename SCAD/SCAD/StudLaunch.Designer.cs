namespace SCAD
{
    partial class StudLaunch
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SCALogo = new System.Windows.Forms.PictureBox();
            this.StudDesignTitle = new System.Windows.Forms.Label();
            this.tabStud = new System.Windows.Forms.TabPage();
            this.tabInput = new System.Windows.Forms.TabPage();
            this.STUDTabs = new System.Windows.Forms.TabControl();
            this.SubmitStud = new System.Windows.Forms.Button();
            this.CancelStud = new System.Windows.Forms.Button();
            this.labelVersion = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.SCALogo)).BeginInit();
            this.STUDTabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // SCALogo
            // 
            this.SCALogo.Image = global::SCAD.Properties.Resources.SCALogo;
            this.SCALogo.Location = new System.Drawing.Point(11, 12);
            this.SCALogo.Name = "SCALogo";
            this.SCALogo.Size = new System.Drawing.Size(67, 70);
            this.SCALogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.SCALogo.TabIndex = 0;
            this.SCALogo.TabStop = false;
            // 
            // StudDesignTitle
            // 
            this.StudDesignTitle.AutoSize = true;
            this.StudDesignTitle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.StudDesignTitle.Font = new System.Drawing.Font("Arial Black", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StudDesignTitle.Location = new System.Drawing.Point(78, 22);
            this.StudDesignTitle.Name = "StudDesignTitle";
            this.StudDesignTitle.Size = new System.Drawing.Size(338, 45);
            this.StudDesignTitle.TabIndex = 1;
            this.StudDesignTitle.Text = "SCAD Stud Design";
            // 
            // tabStud
            // 
            this.tabStud.BackColor = System.Drawing.Color.Transparent;
            this.tabStud.Location = new System.Drawing.Point(4, 22);
            this.tabStud.Name = "tabStud";
            this.tabStud.Padding = new System.Windows.Forms.Padding(3);
            this.tabStud.Size = new System.Drawing.Size(398, 431);
            this.tabStud.TabIndex = 1;
            this.tabStud.Text = "STUD";
            // 
            // tabInput
            // 
            this.tabInput.Location = new System.Drawing.Point(4, 22);
            this.tabInput.Name = "tabInput";
            this.tabInput.Padding = new System.Windows.Forms.Padding(3);
            this.tabInput.Size = new System.Drawing.Size(398, 431);
            this.tabInput.TabIndex = 0;
            this.tabInput.Text = "INPUTS";
            this.tabInput.UseVisualStyleBackColor = true;
            // 
            // STUDTabs
            // 
            this.STUDTabs.Controls.Add(this.tabInput);
            this.STUDTabs.Controls.Add(this.tabStud);
            this.STUDTabs.Location = new System.Drawing.Point(12, 95);
            this.STUDTabs.Name = "STUDTabs";
            this.STUDTabs.SelectedIndex = 0;
            this.STUDTabs.Size = new System.Drawing.Size(406, 457);
            this.STUDTabs.TabIndex = 2;
            // 
            // SubmitStud
            // 
            this.SubmitStud.Location = new System.Drawing.Point(92, 566);
            this.SubmitStud.Name = "SubmitStud";
            this.SubmitStud.Size = new System.Drawing.Size(75, 23);
            this.SubmitStud.TabIndex = 3;
            this.SubmitStud.Text = "Submit";
            this.SubmitStud.UseVisualStyleBackColor = true;
            this.SubmitStud.Click += new System.EventHandler(this.SubmitStud_Click);
            // 
            // CancelStud
            // 
            this.CancelStud.Location = new System.Drawing.Point(253, 566);
            this.CancelStud.Name = "CancelStud";
            this.CancelStud.Size = new System.Drawing.Size(75, 23);
            this.CancelStud.TabIndex = 4;
            this.CancelStud.Text = "Cancel";
            this.CancelStud.UseVisualStyleBackColor = true;
            this.CancelStud.Click += new System.EventHandler(this.CancelStud_Click);
            // 
            // labelVersion
            // 
            this.labelVersion.AutoSize = true;
            this.labelVersion.Location = new System.Drawing.Point(376, 67);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(28, 13);
            this.labelVersion.TabIndex = 5;
            this.labelVersion.Text = "v2.0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(195, 592);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(219, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Copyright SCA Consulting Engineers © 2014 ";
            // 
            // StudLaunch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(430, 611);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.CancelStud);
            this.Controls.Add(this.SubmitStud);
            this.Controls.Add(this.STUDTabs);
            this.Controls.Add(this.StudDesignTitle);
            this.Controls.Add(this.SCALogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "StudLaunch";
            this.Text = "SCAD Stud Design";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SCALogo)).EndInit();
            this.STUDTabs.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox SCALogo;
        private System.Windows.Forms.Label StudDesignTitle;
        private System.Windows.Forms.TabPage tabStud;
        private System.Windows.Forms.TabPage tabInput;
        private System.Windows.Forms.TabControl STUDTabs;
        private System.Windows.Forms.Button SubmitStud;
        private System.Windows.Forms.Button CancelStud;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label label1;
    }
}