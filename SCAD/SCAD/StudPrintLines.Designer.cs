namespace SCAD
{
    partial class StudPrintLines
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
            this.pictureSCALogo = new System.Windows.Forms.PictureBox();
            this.labelStudOptions = new System.Windows.Forms.Label();
            this.buttonPrintUnique = new System.Windows.Forms.Button();
            this.buttonPrintCurrent = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSCALogo)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureSCALogo
            // 
            this.pictureSCALogo.Image = global::SCAD.Properties.Resources.SCALogo;
            this.pictureSCALogo.Location = new System.Drawing.Point(12, 12);
            this.pictureSCALogo.Name = "pictureSCALogo";
            this.pictureSCALogo.Size = new System.Drawing.Size(81, 75);
            this.pictureSCALogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureSCALogo.TabIndex = 0;
            this.pictureSCALogo.TabStop = false;
            // 
            // labelStudOptions
            // 
            this.labelStudOptions.AutoSize = true;
            this.labelStudOptions.Font = new System.Drawing.Font("Arial Black", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelStudOptions.Location = new System.Drawing.Point(99, 9);
            this.labelStudOptions.Name = "labelStudOptions";
            this.labelStudOptions.Size = new System.Drawing.Size(309, 100);
            this.labelStudOptions.TabIndex = 1;
            this.labelStudOptions.Text = "Stud Line\r\nReport Options";
            this.labelStudOptions.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // buttonPrintUnique
            // 
            this.buttonPrintUnique.Location = new System.Drawing.Point(66, 124);
            this.buttonPrintUnique.Name = "buttonPrintUnique";
            this.buttonPrintUnique.Size = new System.Drawing.Size(280, 43);
            this.buttonPrintUnique.TabIndex = 2;
            this.buttonPrintUnique.Text = "Print Unique Stud Lines Only\r\n(From All Levels)";
            this.buttonPrintUnique.UseVisualStyleBackColor = true;
            this.buttonPrintUnique.Click += new System.EventHandler(this.buttonPrintUnique_Click);
            // 
            // buttonPrintCurrent
            // 
            this.buttonPrintCurrent.Location = new System.Drawing.Point(66, 173);
            this.buttonPrintCurrent.Name = "buttonPrintCurrent";
            this.buttonPrintCurrent.Size = new System.Drawing.Size(280, 43);
            this.buttonPrintCurrent.TabIndex = 3;
            this.buttonPrintCurrent.Text = "Print Lines Currently Selected in the File";
            this.buttonPrintCurrent.UseVisualStyleBackColor = true;
            this.buttonPrintCurrent.Click += new System.EventHandler(this.buttonPrintCurrent_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(170, 222);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 4;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // StudPrintLines
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(416, 254);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonPrintCurrent);
            this.Controls.Add(this.buttonPrintUnique);
            this.Controls.Add(this.labelStudOptions);
            this.Controls.Add(this.pictureSCALogo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "StudPrintLines";
            this.Text = "Print Stud Line Reports";
            ((System.ComponentModel.ISupportInitialize)(this.pictureSCALogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureSCALogo;
        private System.Windows.Forms.Label labelStudOptions;
        private System.Windows.Forms.Button buttonPrintUnique;
        private System.Windows.Forms.Button buttonPrintCurrent;
        private System.Windows.Forms.Button buttonCancel;
    }
}