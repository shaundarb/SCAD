namespace SCAD
{
    partial class LateralExport
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
            this.pictureSCA = new System.Windows.Forms.PictureBox();
            this.labelLateralExport = new System.Windows.Forms.Label();
            this.checkShearName = new System.Windows.Forms.CheckBox();
            this.checkShearDesign = new System.Windows.Forms.CheckBox();
            this.checkShearLength = new System.Windows.Forms.CheckBox();
            this.checkShearAnchors = new System.Windows.Forms.CheckBox();
            this.checkShearEndpoints = new System.Windows.Forms.CheckBox();
            this.checkDrag = new System.Windows.Forms.CheckBox();
            this.LateralExportSubmit = new System.Windows.Forms.Button();
            this.LateralExportCancel = new System.Windows.Forms.Button();
            this.ChooseItemsLabel2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureSCA)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureSCA
            // 
            this.pictureSCA.Image = global::SCAD.Properties.Resources.SCALogo;
            this.pictureSCA.Location = new System.Drawing.Point(12, 12);
            this.pictureSCA.Name = "pictureSCA";
            this.pictureSCA.Size = new System.Drawing.Size(81, 75);
            this.pictureSCA.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureSCA.TabIndex = 0;
            this.pictureSCA.TabStop = false;
            // 
            // labelLateralExport
            // 
            this.labelLateralExport.AutoSize = true;
            this.labelLateralExport.Font = new System.Drawing.Font("Arial Black", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLateralExport.Location = new System.Drawing.Point(136, 9);
            this.labelLateralExport.Name = "labelLateralExport";
            this.labelLateralExport.Size = new System.Drawing.Size(280, 90);
            this.labelLateralExport.TabIndex = 1;
            this.labelLateralExport.Text = "Lateral Design\r\nExport Options";
            // 
            // checkShearName
            // 
            this.checkShearName.AutoSize = true;
            this.checkShearName.Location = new System.Drawing.Point(88, 133);
            this.checkShearName.Name = "checkShearName";
            this.checkShearName.Size = new System.Drawing.Size(109, 17);
            this.checkShearName.TabIndex = 3;
            this.checkShearName.Text = "Shear Wall Name";
            this.checkShearName.UseVisualStyleBackColor = true;
            // 
            // checkShearDesign
            // 
            this.checkShearDesign.AutoSize = true;
            this.checkShearDesign.Checked = true;
            this.checkShearDesign.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkShearDesign.Location = new System.Drawing.Point(88, 156);
            this.checkShearDesign.Name = "checkShearDesign";
            this.checkShearDesign.Size = new System.Drawing.Size(114, 17);
            this.checkShearDesign.TabIndex = 4;
            this.checkShearDesign.Text = "Shear Wall Design";
            this.checkShearDesign.UseVisualStyleBackColor = true;
            // 
            // checkShearLength
            // 
            this.checkShearLength.AutoSize = true;
            this.checkShearLength.Location = new System.Drawing.Point(88, 179);
            this.checkShearLength.Name = "checkShearLength";
            this.checkShearLength.Size = new System.Drawing.Size(114, 17);
            this.checkShearLength.TabIndex = 5;
            this.checkShearLength.Text = "Shear Wall Length";
            this.checkShearLength.UseVisualStyleBackColor = true;
            // 
            // checkShearAnchors
            // 
            this.checkShearAnchors.AutoSize = true;
            this.checkShearAnchors.Checked = true;
            this.checkShearAnchors.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkShearAnchors.Location = new System.Drawing.Point(273, 133);
            this.checkShearAnchors.Name = "checkShearAnchors";
            this.checkShearAnchors.Size = new System.Drawing.Size(120, 17);
            this.checkShearAnchors.TabIndex = 6;
            this.checkShearAnchors.Text = "Shear Wall Anchors";
            this.checkShearAnchors.UseVisualStyleBackColor = true;
            // 
            // checkShearEndpoints
            // 
            this.checkShearEndpoints.AutoSize = true;
            this.checkShearEndpoints.Location = new System.Drawing.Point(273, 156);
            this.checkShearEndpoints.Name = "checkShearEndpoints";
            this.checkShearEndpoints.Size = new System.Drawing.Size(128, 17);
            this.checkShearEndpoints.TabIndex = 7;
            this.checkShearEndpoints.Text = "Shear Wall Endpoints";
            this.checkShearEndpoints.UseVisualStyleBackColor = true;
            // 
            // checkDrag
            // 
            this.checkDrag.AutoSize = true;
            this.checkDrag.Checked = true;
            this.checkDrag.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkDrag.Location = new System.Drawing.Point(273, 179);
            this.checkDrag.Name = "checkDrag";
            this.checkDrag.Size = new System.Drawing.Size(84, 17);
            this.checkDrag.TabIndex = 8;
            this.checkDrag.Text = "Drag Forces";
            this.checkDrag.UseVisualStyleBackColor = true;
            // 
            // LateralExportSubmit
            // 
            this.LateralExportSubmit.Location = new System.Drawing.Point(110, 212);
            this.LateralExportSubmit.Name = "LateralExportSubmit";
            this.LateralExportSubmit.Size = new System.Drawing.Size(90, 28);
            this.LateralExportSubmit.TabIndex = 10;
            this.LateralExportSubmit.Text = "Submit";
            this.LateralExportSubmit.UseVisualStyleBackColor = true;
            this.LateralExportSubmit.Click += new System.EventHandler(this.LateralExportSubmit_Click);
            // 
            // LateralExportCancel
            // 
            this.LateralExportCancel.Location = new System.Drawing.Point(263, 212);
            this.LateralExportCancel.Name = "LateralExportCancel";
            this.LateralExportCancel.Size = new System.Drawing.Size(90, 28);
            this.LateralExportCancel.TabIndex = 11;
            this.LateralExportCancel.Text = "Cancel";
            this.LateralExportCancel.UseVisualStyleBackColor = true;
            this.LateralExportCancel.Click += new System.EventHandler(this.LateralExportCancel_Click);
            // 
            // ChooseItemsLabel2
            // 
            this.ChooseItemsLabel2.AutoSize = true;
            this.ChooseItemsLabel2.Font = new System.Drawing.Font("Arial", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ChooseItemsLabel2.Location = new System.Drawing.Point(23, 105);
            this.ChooseItemsLabel2.Name = "ChooseItemsLabel2";
            this.ChooseItemsLabel2.Size = new System.Drawing.Size(393, 17);
            this.ChooseItemsLabel2.TabIndex = 12;
            this.ChooseItemsLabel2.Text = "Choose which items below should be exported to AutoCAD:";
            // 
            // LateralExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(448, 256);
            this.Controls.Add(this.ChooseItemsLabel2);
            this.Controls.Add(this.LateralExportCancel);
            this.Controls.Add(this.LateralExportSubmit);
            this.Controls.Add(this.checkDrag);
            this.Controls.Add(this.checkShearEndpoints);
            this.Controls.Add(this.checkShearAnchors);
            this.Controls.Add(this.checkShearLength);
            this.Controls.Add(this.checkShearDesign);
            this.Controls.Add(this.checkShearName);
            this.Controls.Add(this.labelLateralExport);
            this.Controls.Add(this.pictureSCA);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "LateralExport";
            this.Text = "Lateral Design Export Options -- AutoCAD Script";
            ((System.ComponentModel.ISupportInitialize)(this.pictureSCA)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureSCA;
        private System.Windows.Forms.Label labelLateralExport;
        private System.Windows.Forms.CheckBox checkShearName;
        private System.Windows.Forms.CheckBox checkShearDesign;
        private System.Windows.Forms.CheckBox checkShearLength;
        private System.Windows.Forms.CheckBox checkShearAnchors;
        private System.Windows.Forms.CheckBox checkShearEndpoints;
        private System.Windows.Forms.CheckBox checkDrag;
        private System.Windows.Forms.Button LateralExportSubmit;
        private System.Windows.Forms.Button LateralExportCancel;
        private System.Windows.Forms.Label ChooseItemsLabel2;
    }
}