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
            this.STUDtab = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.STUDTabs = new System.Windows.Forms.TabControl();
            this.SubmitStud = new System.Windows.Forms.Button();
            this.CancelStud = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.SCALogo)).BeginInit();
            this.STUDTabs.SuspendLayout();
            this.SuspendLayout();
            // 
            // SCALogo
            // 
            this.SCALogo.Image = global::SCAD.Properties.Resources.SCALogo;
            this.SCALogo.Location = new System.Drawing.Point(12, 12);
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
            this.StudDesignTitle.Font = new System.Drawing.Font("Stencil", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StudDesignTitle.Location = new System.Drawing.Point(85, 22);
            this.StudDesignTitle.Name = "StudDesignTitle";
            this.StudDesignTitle.Size = new System.Drawing.Size(332, 42);
            this.StudDesignTitle.TabIndex = 1;
            this.StudDesignTitle.Text = "SCAD Stud Design";
            // 
            // STUDtab
            // 
            this.STUDtab.BackColor = System.Drawing.Color.Transparent;
            this.STUDtab.Location = new System.Drawing.Point(4, 22);
            this.STUDtab.Name = "STUDtab";
            this.STUDtab.Padding = new System.Windows.Forms.Padding(3);
            this.STUDtab.Size = new System.Drawing.Size(398, 431);
            this.STUDtab.TabIndex = 1;
            this.STUDtab.Text = "STUD";
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(398, 431);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "INPUTS";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // STUDTabs
            // 
            this.STUDTabs.Controls.Add(this.tabPage1);
            this.STUDTabs.Controls.Add(this.STUDtab);
            this.STUDTabs.Location = new System.Drawing.Point(12, 95);
            this.STUDTabs.Name = "STUDTabs";
            this.STUDTabs.SelectedIndex = 0;
            this.STUDTabs.Size = new System.Drawing.Size(406, 457);
            this.STUDTabs.TabIndex = 2;
            // 
            // SubmitStud
            // 
            this.SubmitStud.Location = new System.Drawing.Point(92, 574);
            this.SubmitStud.Name = "SubmitStud";
            this.SubmitStud.Size = new System.Drawing.Size(75, 23);
            this.SubmitStud.TabIndex = 3;
            this.SubmitStud.Text = "Submit";
            this.SubmitStud.UseVisualStyleBackColor = true;
            // 
            // CancelStud
            // 
            this.CancelStud.Location = new System.Drawing.Point(253, 574);
            this.CancelStud.Name = "CancelStud";
            this.CancelStud.Size = new System.Drawing.Size(75, 23);
            this.CancelStud.TabIndex = 4;
            this.CancelStud.Text = "Cancel";
            this.CancelStud.UseVisualStyleBackColor = true;
            // 
            // StudLaunch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(430, 611);
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
        private System.Windows.Forms.TabPage STUDtab;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl STUDTabs;
        private System.Windows.Forms.Button SubmitStud;
        private System.Windows.Forms.Button CancelStud;
    }
}