namespace SCAD
{
    partial class StudExport
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
            this.ExportOptionsLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ChooseItemsLabel = new System.Windows.Forms.Label();
            this.StudNameCheck = new System.Windows.Forms.CheckBox();
            this.StudDesignCheck = new System.Windows.Forms.CheckBox();
            this.StudKeyCheck = new System.Windows.Forms.CheckBox();
            this.StudEndpointCheck = new System.Windows.Forms.CheckBox();
            this.StudFoundCheck = new System.Windows.Forms.CheckBox();
            this.StudScheduleCheck = new System.Windows.Forms.CheckBox();
            this.StudExportSubmit = new System.Windows.Forms.Button();
            this.StudExportCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // ExportOptionsLabel
            // 
            this.ExportOptionsLabel.AutoSize = true;
            this.ExportOptionsLabel.Font = new System.Drawing.Font("Arial Black", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ExportOptionsLabel.Location = new System.Drawing.Point(112, -1);
            this.ExportOptionsLabel.Name = "ExportOptionsLabel";
            this.ExportOptionsLabel.Size = new System.Drawing.Size(307, 100);
            this.ExportOptionsLabel.TabIndex = 0;
            this.ExportOptionsLabel.Text = "Stud\r\nExport Options";
            this.ExportOptionsLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SCAD.Properties.Resources.SCALogo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(81, 75);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // ChooseItemsLabel
            // 
            this.ChooseItemsLabel.AutoSize = true;
            this.ChooseItemsLabel.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChooseItemsLabel.Location = new System.Drawing.Point(26, 99);
            this.ChooseItemsLabel.Name = "ChooseItemsLabel";
            this.ChooseItemsLabel.Size = new System.Drawing.Size(393, 17);
            this.ChooseItemsLabel.TabIndex = 2;
            this.ChooseItemsLabel.Text = "Choose which items below should be exported to AutoCAD:";
            // 
            // StudNameCheck
            // 
            this.StudNameCheck.AutoSize = true;
            this.StudNameCheck.Location = new System.Drawing.Point(81, 133);
            this.StudNameCheck.Name = "StudNameCheck";
            this.StudNameCheck.Size = new System.Drawing.Size(103, 17);
            this.StudNameCheck.TabIndex = 3;
            this.StudNameCheck.Text = "Stud Wall Name";
            this.StudNameCheck.UseVisualStyleBackColor = true;
            // 
            // StudDesignCheck
            // 
            this.StudDesignCheck.AutoSize = true;
            this.StudDesignCheck.Checked = true;
            this.StudDesignCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.StudDesignCheck.Location = new System.Drawing.Point(81, 156);
            this.StudDesignCheck.Name = "StudDesignCheck";
            this.StudDesignCheck.Size = new System.Drawing.Size(108, 17);
            this.StudDesignCheck.TabIndex = 4;
            this.StudDesignCheck.Text = "Stud Wall Design";
            this.StudDesignCheck.UseVisualStyleBackColor = true;
            // 
            // StudKeyCheck
            // 
            this.StudKeyCheck.AutoSize = true;
            this.StudKeyCheck.Location = new System.Drawing.Point(81, 179);
            this.StudKeyCheck.Name = "StudKeyCheck";
            this.StudKeyCheck.Size = new System.Drawing.Size(113, 17);
            this.StudKeyCheck.TabIndex = 5;
            this.StudKeyCheck.Text = "Key Plan Numbers";
            this.StudKeyCheck.UseVisualStyleBackColor = true;
            // 
            // StudEndpointCheck
            // 
            this.StudEndpointCheck.AutoSize = true;
            this.StudEndpointCheck.Checked = true;
            this.StudEndpointCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.StudEndpointCheck.Location = new System.Drawing.Point(254, 133);
            this.StudEndpointCheck.Name = "StudEndpointCheck";
            this.StudEndpointCheck.Size = new System.Drawing.Size(122, 17);
            this.StudEndpointCheck.TabIndex = 6;
            this.StudEndpointCheck.Text = "Stud Wall Endpoints";
            this.StudEndpointCheck.UseVisualStyleBackColor = true;
            // 
            // StudFoundCheck
            // 
            this.StudFoundCheck.AutoSize = true;
            this.StudFoundCheck.Location = new System.Drawing.Point(254, 156);
            this.StudFoundCheck.Name = "StudFoundCheck";
            this.StudFoundCheck.Size = new System.Drawing.Size(130, 17);
            this.StudFoundCheck.TabIndex = 7;
            this.StudFoundCheck.Text = "Foundation Reactions";
            this.StudFoundCheck.UseVisualStyleBackColor = true;
            // 
            // StudScheduleCheck
            // 
            this.StudScheduleCheck.AutoSize = true;
            this.StudScheduleCheck.Checked = true;
            this.StudScheduleCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.StudScheduleCheck.Location = new System.Drawing.Point(254, 179);
            this.StudScheduleCheck.Name = "StudScheduleCheck";
            this.StudScheduleCheck.Size = new System.Drawing.Size(96, 17);
            this.StudScheduleCheck.TabIndex = 8;
            this.StudScheduleCheck.Text = "Stud Schedule";
            this.StudScheduleCheck.UseVisualStyleBackColor = true;
            // 
            // StudExportSubmit
            // 
            this.StudExportSubmit.Location = new System.Drawing.Point(104, 215);
            this.StudExportSubmit.Name = "StudExportSubmit";
            this.StudExportSubmit.Size = new System.Drawing.Size(90, 28);
            this.StudExportSubmit.TabIndex = 9;
            this.StudExportSubmit.Text = "Submit";
            this.StudExportSubmit.UseVisualStyleBackColor = true;
            this.StudExportSubmit.Click += new System.EventHandler(this.StudExportSubmit_Click);
            // 
            // StudExportCancel
            // 
            this.StudExportCancel.Location = new System.Drawing.Point(254, 215);
            this.StudExportCancel.Name = "StudExportCancel";
            this.StudExportCancel.Size = new System.Drawing.Size(90, 28);
            this.StudExportCancel.TabIndex = 10;
            this.StudExportCancel.Text = "Cancel";
            this.StudExportCancel.UseVisualStyleBackColor = true;
            this.StudExportCancel.Click += new System.EventHandler(this.StudExportCancel_Click_1);
            // 
            // StudExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(442, 260);
            this.Controls.Add(this.StudExportCancel);
            this.Controls.Add(this.StudExportSubmit);
            this.Controls.Add(this.StudScheduleCheck);
            this.Controls.Add(this.StudFoundCheck);
            this.Controls.Add(this.StudEndpointCheck);
            this.Controls.Add(this.StudKeyCheck);
            this.Controls.Add(this.StudDesignCheck);
            this.Controls.Add(this.StudNameCheck);
            this.Controls.Add(this.ChooseItemsLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ExportOptionsLabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "StudExport";
            this.Text = "Stud Export Options - AutCAD Script";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label ExportOptionsLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label ChooseItemsLabel;
        private System.Windows.Forms.CheckBox StudNameCheck;
        private System.Windows.Forms.CheckBox StudDesignCheck;
        private System.Windows.Forms.CheckBox StudKeyCheck;
        private System.Windows.Forms.CheckBox StudEndpointCheck;
        private System.Windows.Forms.CheckBox StudFoundCheck;
        private System.Windows.Forms.CheckBox StudScheduleCheck;
        private System.Windows.Forms.Button StudExportSubmit;
        private System.Windows.Forms.Button StudExportCancel;
    }
}