//////////////////////////////////////////////////
//              CombatControl.cs                //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace RogueBT.UI
{
    public partial class CombatControl : Form
    {
        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        public CombatControl()
        {
            InitializeComponent();
        }
        private void returnControl()
        {

            Process[] p = Process.GetProcessesByName("Wow");

            // Activate the first application we find with this name
            if (p.Count() > 0)
                SetForegroundWindow(p[0].MainWindowHandle);
        }

        private void buttonToggleCd_Click(object sender, EventArgs e)
        {
            Settings.Mode.mUseCooldowns = !Settings.Mode.mUseCooldowns;
            labelCdStatus.Text = "CDs: " + Settings.Mode.mUseCooldowns;
            returnControl();
        }

        private void buttonToggleCombat_Click(object sender, EventArgs e)
        {
            Settings.Mode.mUseCombat = !Settings.Mode.mUseCombat;
            labelCombatStatus.Text = "Combat: " + Settings.Mode.mUseCombat;
            returnControl();
        }

        private void buttonToggleBehindTar_Click(object sender, EventArgs e)
        {
            Settings.Mode.mForceBehind = !Settings.Mode.mForceBehind;
            labelBehindTarStatus.Text = "Behind: " + Settings.Mode.mForceBehind;
            returnControl();
        }

        private void CombatControl_Load(object sender, EventArgs e)
        {
            TopMost = true;

            labelCdStatus.Text = "CDs: " + Settings.Mode.mUseCooldowns;
            labelCombatStatus.Text = "Combat: " + Settings.Mode.mUseCombat;
            labelBehindTarStatus.Text = "Behind: " + Settings.Mode.mForceBehind;
        }

    }
}
