using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SCAD
{
    public partial class StudLaunch : Form
    {
        public bool StudCancel = false;
        public object[] arrDesignData= new object[61];

        public StudLaunch()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void CancelStud_Click(object sender, EventArgs e)
        {
            StudCancel = true;
            this.Close();
        }

        private void SubmitStud_Click(object sender, EventArgs e)
        {
            StudCancel = false;

            // Pass values from form into array arrDataSort[]
            arrDesignData[0] = this.textBoxJobName.Text;
            arrDesignData[1] = this.textBoxJobNumber.Text;
            arrDesignData[2] = this.textBoxInitials.Text;
            arrDesignData[3] = this.comboBoxCode.Text;
            arrDesignData[4] = this.textBoxLvl1.Text;
            arrDesignData[5] = this.textBoxLvl2.Text;
            arrDesignData[6] = this.textBoxLvl3.Text;
            arrDesignData[7] = this.textBoxLvl4.Text;
            arrDesignData[8] = this.textBoxLvl5.Text;
            arrDesignData[9] = this.textBoxLvl6.Text;
            arrDesignData[10] = this.textBoxBendCoef.Text;
            arrDesignData[11] = this.textBoxBuiltupCol.Text;
            arrDesignData[12] = this.textBoxRepMember.Text;
            arrDesignData[13] = this.textBoxWetSvc.Text;
            arrDesignData[14] = this.textBoxTempFact.Text;
            arrDesignData[15] = this.textBoxBeamStab.Text;
            arrDesignData[16] = this.textBoxBuckFact.Text;
            arrDesignData[17] = this.textBoxBearingFactor.Text;
            arrDesignData[18] = this.textBoxSeismicSDS.Text;
            arrDesignData[19] = this.comboBoxPdfReports.Text;
            arrDesignData[20] = this.textBoxIntWallWt.Text;
            arrDesignData[21] = this.textBoxExtWallWt.Text;
            arrDesignData[22] = this.textBoxWindPress.Text;
            arrDesignData[23] = this.textBoxSeismicPress.Text;
            arrDesignData[24] = this.textBoxIntWallPress.Text;
            arrDesignData[25] = this.textBoxRoofSL.Text;
            arrDesignData[26] = this.textBoxRoofRL.Text;
            arrDesignData[27] = this.textBoxRoofDL.Text;
            arrDesignData[28] = this.textBoxRoofLL.Text;
            arrDesignData[29] = this.textBoxUnitDL.Text;
            arrDesignData[30] = this.textBoxUnitLL.Text;
            arrDesignData[31] = this.textBoxBalcDL.Text;
            arrDesignData[32] = this.textBoxBalcLL.Text;
            arrDesignData[33] = this.textBoxCorrDL.Text;
            arrDesignData[34] = this.textBoxCorrLL.Text;
            arrDesignData[35] = this.textBoxOtherDL.Text;
            arrDesignData[36] = this.textBoxOtherLL.Text;
            arrDesignData[37] = this.comboBoxFloorLBSpec.Text;
            arrDesignData[38] = this.comboBoxFloorLBGrade.Text;
            arrDesignData[39] = this.comboBoxRoofLBSpec.Text;
            arrDesignData[40] = this.comboBoxRoofLBGrade.Text;
            arrDesignData[41] = this.comboBoxExteriorLBSpec.Text;
            arrDesignData[42] = this.comboBoxExteriorGrade.Text;
            arrDesignData[43] = this.textBoxLvl1Lx.Text;
            arrDesignData[44] = this.textBoxLvl2Lx.Text;
            arrDesignData[45] = this.textBoxLvl3Lx.Text;
            arrDesignData[46] = this.textBoxLvl4Lx.Text;
            arrDesignData[47] = this.textBoxLvl5Lx.Text;
            arrDesignData[48] = this.textBoxLvl6Lx.Text;
            arrDesignData[49] = this.textBoxLvl1Ly.Text;
            arrDesignData[50] = this.textBoxLvl2Ly.Text;
            arrDesignData[51] = this.textBoxLvl3Ly.Text;
            arrDesignData[52] = this.textBoxLvl4Ly.Text;
            arrDesignData[53] = this.textBoxLvl5Ly.Text;
            arrDesignData[54] = this.textBoxLvl6Ly.Text;
            arrDesignData[55] = this.checkBoxHorizMatch.Text;
            arrDesignData[56] = this.checkBoxVertMatch.Text;
            arrDesignData[57] = this.checkBoxMedSumm.Text;
            arrDesignData[58] = this.checkBoxMedArrays.Text;
            arrDesignData[59] = this.textBoxIntRatio.Text;
            arrDesignData[60] = this.checkBoxCompLimit.Text;

            this.Close();
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }
    }
}
