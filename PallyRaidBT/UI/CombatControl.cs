//////////////////////////////////////////////////
//              CombatControl.cs                //
//      Part of PallyRaidBT by fiftypence        //
//////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace PallyRaidBT.UI
{
    public partial class CombatControl : Form
    {
        public CombatControl()
        {
            InitializeComponent();
        }

        private void buttonToggleCd_Click(object sender, EventArgs e)
        {
            Settings.Mode.mUseCooldowns = !Settings.Mode.mUseCooldowns;
            labelCdStatus.Text = "CDs: " + Settings.Mode.mUseCooldowns;
        }

        private void buttonToggleAoe_Click(object sender, EventArgs e)
        {
            Settings.Mode.mUseAoe = !Settings.Mode.mUseAoe;
            labelAoeStatus.Text = "AoE: " + Settings.Mode.mUseAoe;
        }

        private void buttonToggleCombat_Click(object sender, EventArgs e)
        {
            Settings.Mode.mUseCombat = !Settings.Mode.mUseCombat;
            labelCombatStatus.Text = "Combat: " + Settings.Mode.mUseCombat;
        }

        private void CombatControl_Load(object sender, EventArgs e)
        {
            TopMost = true;

            labelCdStatus.Text = "CDs: " + Settings.Mode.mUseCooldowns;
            labelAoeStatus.Text = "AoE: " + Settings.Mode.mUseAoe;
            labelCombatStatus.Text = "Combat: " + Settings.Mode.mUseCombat;
            labelBehindTarStatus.Text = "Behind: " + Settings.Mode.mForceBehind;
        }

        private void buttonToggleBehindTar_Click(object sender, EventArgs e)
        {
            Settings.Mode.mForceBehind = !Settings.Mode.mForceBehind;
            labelBehindTarStatus.Text = "Behind: " + Settings.Mode.mForceBehind;
        }

        private void labelAoeStatus_Click(object sender, EventArgs e)
        {

        }
    }
}
