//////////////////////////////////////////////////
//                Config.cs                     //
//      Part of PallyRaidBT by fiftypence        //
//////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace PallyRaidBT.UI
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }

        private void Config_Load(object sender, EventArgs e)
        {
            switch (Settings.Mode.mCurMode)
            {
                case Settings.Mode.Modes.Raid:

                    radioButtonRaid.Checked = true;
                    break;

                case Settings.Mode.Modes.Dungeon:

                    radioButtonDungeon.Checked = true;
                    break;

                case Settings.Mode.Modes.PvPMoveOff:

                    radioButtonPvpMoveOff.Checked = true;
                    break;

                case Settings.Mode.Modes.PvPMoveOn:

                    radioButtonPvpMoveOn.Checked = true;
                    break;

                case Settings.Mode.Modes.Level:

                    radioButtonLevel.Checked = true;
                    break;
            }

            switch(Settings.Mode.mCooldownUse)
            {
                case Settings.Mode.CooldownUse.Always:

                    radioCooldownAlways.Checked = true;
                    break;

                case Settings.Mode.CooldownUse.ByFocus:

                    radioCooldownByFocus.Checked = true;
                    break;

                case Settings.Mode.CooldownUse.OnlyOnBosses:

                    radioCooldownByBoss.Checked = true;
                    break;

                case Settings.Mode.CooldownUse.Never:

                    radioCooldownNever.Checked = true;
                    break;
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (radioButtonRaid.Checked)
            {
                Settings.Mode.mCurMode = Settings.Mode.Modes.Raid;
            }
            else if (radioButtonDungeon.Checked)
            {
                Settings.Mode.mCurMode = Settings.Mode.Modes.Dungeon;
            }
            else if (radioButtonPvpMoveOff.Checked)
            {
                Settings.Mode.mCurMode = Settings.Mode.Modes.PvPMoveOff;
            }
            else if (radioButtonPvpMoveOn.Checked)
            {
                Settings.Mode.mCurMode = Settings.Mode.Modes.PvPMoveOn;
            }
            else
            {
                Settings.Mode.mCurMode = Settings.Mode.Modes.Level;
            }

            if (radioCooldownAlways.Checked)
            {
                Settings.Mode.mCooldownUse = Settings.Mode.CooldownUse.Always;
            }
            else if (radioCooldownByFocus.Checked)
            {
                Settings.Mode.mCooldownUse = Settings.Mode.CooldownUse.ByFocus;
            }
            else if (radioCooldownByBoss.Checked)
            {
                Settings.Mode.mCooldownUse = Settings.Mode.CooldownUse.OnlyOnBosses;  
            }
            else
            {
                Settings.Mode.mCooldownUse = Settings.Mode.CooldownUse.Never;
            }

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonLaunchCombatControl_Click(object sender, EventArgs e)
        {
            var combatControl = new CombatControl();
            combatControl.Show();
        }
    }
}
