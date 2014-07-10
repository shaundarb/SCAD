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
        public object[] arrDataSort = new object[61];

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

            try
            {
                // Pass values from form into array arrDataSort[]
                arrDataSort[0] = this.textBoxJobName.Text;
                arrDataSort[1] = this.textBoxJobNumber.Text;
                arrDataSort[2] = this.textBoxInitials.Text;
                arrDataSort[3] = this.comboBoxCode.Text;
                arrDataSort[4] = this.textBoxLvl1.Text;
                arrDataSort[5] = this.textBoxLvl2.Text;
                arrDataSort[6] = this.textBoxLvl3.Text;
                arrDataSort[7] = this.textBoxLvl4.Text;
                arrDataSort[8] = this.textBoxLvl5.Text;
                arrDataSort[9] = this.textBoxLvl6.Text;
                arrDataSort[10] = this.textBoxBendCoef.Text;
                arrDataSort[11] = this.textBoxBuiltupCol.Text;
                arrDataSort[12] = this.textBoxRepMember.Text;
                arrDataSort[13] = this.textBoxWetSvc.Text;
                arrDataSort[14] = this.textBoxTempFact.Text;
                arrDataSort[15] = this.textBoxBeamStab.Text;
                arrDataSort[16] = this.textBoxBuckFact.Text;
                arrDataSort[17] = this.textBoxBearingFactor.Text;
                arrDataSort[18] = this.textBoxSeismicSDS.Text;
                arrDataSort[19] = this.comboBoxPdfReports.Text;
                arrDataSort[20] = this.textBoxIntWallWt.Text;
                arrDataSort[21] = this.textBoxExtWallWt.Text;
                arrDataSort[22] = this.textBoxWindPress.Text;
                arrDataSort[23] = this.textBoxSeismicPress.Text;
                arrDataSort[24] = this.textBoxIntWallPress.Text;
                arrDataSort[25] = this.textBoxRoofSL.Text;
                arrDataSort[26] = this.textBoxRoofRL.Text;
                arrDataSort[27] = this.textBoxRoofDL.Text;
                arrDataSort[28] = this.textBoxRoofLL.Text;
                arrDataSort[29] = this.textBoxUnitDL.Text;
                arrDataSort[30] = this.textBoxUnitLL.Text;
                arrDataSort[31] = this.textBoxBalcDL.Text;
                arrDataSort[32] = this.textBoxBalcLL.Text;
                arrDataSort[33] = this.textBoxCorrDL.Text;
                arrDataSort[34] = this.textBoxCorrLL.Text;
                arrDataSort[35] = this.textBoxOtherDL.Text;
                arrDataSort[36] = this.textBoxOtherLL.Text;
                arrDataSort[37] = this.comboBoxFloorLBSpec.Text;
                arrDataSort[38] = this.comboBoxFloorLBGrade.Text;
                arrDataSort[39] = this.comboBoxRoofLBSpec.Text;
                arrDataSort[40] = this.comboBoxRoofLBGrade.Text;
                arrDataSort[41] = this.comboBoxExteriorLBSpec.Text;
                arrDataSort[42] = this.comboBoxExteriorGrade.Text;
                arrDataSort[43] = this.textBoxLvl1Lx.Text;
                arrDataSort[44] = this.textBoxLvl2Lx.Text;
                arrDataSort[45] = this.textBoxLvl3Lx.Text;
                arrDataSort[46] = this.textBoxLvl4Lx.Text;
                arrDataSort[47] = this.textBoxLvl5Lx.Text;
                arrDataSort[48] = this.textBoxLvl6Lx.Text;
                arrDataSort[49] = this.textBoxLvl1Ly.Text;
                arrDataSort[50] = this.textBoxLvl2Ly.Text;
                arrDataSort[51] = this.textBoxLvl3Ly.Text;
                arrDataSort[52] = this.textBoxLvl4Ly.Text;
                arrDataSort[53] = this.textBoxLvl5Ly.Text;
                arrDataSort[54] = this.textBoxLvl6Ly.Text;
                arrDataSort[55] = this.checkBoxHorizMatch.Text;
                arrDataSort[56] = this.checkBoxVertMatch.Text;
                arrDataSort[57] = this.checkBoxMedSumm.Text;
                arrDataSort[58] = this.checkBoxMedArrays.Text;
                arrDataSort[59] = this.textBoxIntRatio.Text;
                arrDataSort[60] = this.checkBoxCompLimit.Text;

                this.Close();
            }
            catch (Exception except) { MessageBox.Show(except.Message); }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {

        }

        private void label29_Click(object sender, EventArgs e)
        {

        }
    }
}
