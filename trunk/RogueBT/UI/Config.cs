//////////////////////////////////////////////////
//                Config.cs                     //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

using System;
using System.Windows.Forms;

namespace RogueBT.UI
{
    public partial class Config : Form
    {
        public Config()
        {
            InitializeComponent();
        }

        private void Config_Load(object sender, EventArgs e)
        {
            comboBoxRaidPoison1.DataSource = Enum.GetValues(typeof(Helpers.Enum.LeathalPoisonSpellId));
            comboBoxRaidPoison2.DataSource = Enum.GetValues(typeof(Helpers.Enum.NonLeathalPoisonSpellId));
            comboBoxArenaPoison1.DataSource = Enum.GetValues(typeof(Helpers.Enum.LeathalPoisonSpellId));
            comboBoxArenaPoison2.DataSource = Enum.GetValues(typeof(Helpers.Enum.NonLeathalPoisonSpellId));
            comboBoxDungeonPoison1.DataSource = Enum.GetValues(typeof(Helpers.Enum.LeathalPoisonSpellId));
            comboBoxDungeonPoison2.DataSource = Enum.GetValues(typeof(Helpers.Enum.NonLeathalPoisonSpellId));
            comboBoxBgPoison1.DataSource = Enum.GetValues(typeof(Helpers.Enum.LeathalPoisonSpellId));
            comboBoxBgPoison2.DataSource = Enum.GetValues(typeof(Helpers.Enum.NonLeathalPoisonSpellId));
            comboBoxLevelPoison1.DataSource = Enum.GetValues(typeof(Helpers.Enum.LeathalPoisonSpellId));
            comboBoxLevelPoison2.DataSource = Enum.GetValues(typeof(Helpers.Enum.NonLeathalPoisonSpellId));

            checkBoxRaidPoison.Checked    = Settings.Mode.mUsePoisons[(int) Helpers.Enum.LocationContext.Raid];
            checkBoxArenaPoison.Checked  = Settings.Mode.mUsePoisons[(int) Helpers.Enum.LocationContext.Arena];
            checkBoxDungeonPoison.Checked = Settings.Mode.mUsePoisons[(int) Helpers.Enum.LocationContext.Dungeon];
            checkBoxBgPoison.Checked      = Settings.Mode.mUsePoisons[(int) Helpers.Enum.LocationContext.Battleground];
            checkBoxLevelPoison.Checked   = Settings.Mode.mUsePoisons[(int) Helpers.Enum.LocationContext.World];

            comboBoxRaidPoison1.SelectedItem    = Settings.Mode.mPoisonsMain[(int) Helpers.Enum.LocationContext.Raid];
            comboBoxRaidPoison2.SelectedItem    = Settings.Mode.mPoisonsOff[(int) Helpers.Enum.LocationContext.Raid];
            comboBoxArenaPoison1.SelectedItem  = Settings.Mode.mPoisonsMain[(int) Helpers.Enum.LocationContext.Arena];
            comboBoxArenaPoison2.SelectedItem  = Settings.Mode.mPoisonsOff[(int) Helpers.Enum.LocationContext.Arena];
            comboBoxDungeonPoison1.SelectedItem = Settings.Mode.mPoisonsMain[(int) Helpers.Enum.LocationContext.Dungeon];
            comboBoxDungeonPoison2.SelectedItem = Settings.Mode.mPoisonsOff[(int) Helpers.Enum.LocationContext.Dungeon];
            comboBoxBgPoison1.SelectedItem      = Settings.Mode.mPoisonsMain[(int) Helpers.Enum.LocationContext.Battleground];
            comboBoxBgPoison2.SelectedItem      = Settings.Mode.mPoisonsOff[(int) Helpers.Enum.LocationContext.Battleground];
            comboBoxLevelPoison1.SelectedItem   = Settings.Mode.mPoisonsMain[(int) Helpers.Enum.LocationContext.World];
            comboBoxLevelPoison2.SelectedItem   = Settings.Mode.mPoisonsOff[(int) Helpers.Enum.LocationContext.World];

            panelRaidPoison.Enabled = checkBoxRaidPoison.Checked;
            panelArenaPoison.Enabled = checkBoxArenaPoison.Checked;
            panelDungeonPoison.Enabled = checkBoxDungeonPoison.Checked;
            panelBgPoison.Enabled = checkBoxBgPoison.Checked;
            panelLevelPoison.Enabled = checkBoxBgPoison.Checked;
     
            if (!Settings.Mode.mOverrideContext)
            {
                radioButtonAuto.Checked = true;
            }
            else
            {
                switch (Settings.Mode.mLocationSettings)
                {
                    case Helpers.Enum.LocationContext.Raid:

                        radioButtonRaid.Checked = true;
                        break;

                    case Helpers.Enum.LocationContext.Arena:

                        radioButtonArena.Checked = true;
                        break;

                    case Helpers.Enum.LocationContext.Dungeon:

                        radioButtonDungeon.Checked = true;
                        break;

                    case Helpers.Enum.LocationContext.HeroicDungeon:

                        radioButtonDungeon.Checked = true;
                        break;

                    case Helpers.Enum.LocationContext.Battleground:

                        radioButtonBattleground.Checked = true;
                        break;

                    case Helpers.Enum.LocationContext.World:

                        radioButtonLevel.Checked = true;
                        break;
                }
            }

            if (Settings.Mode.mUseMovement)
            {
                radioButtonMoveOn.Checked = true;
            }
            else
            {
                radioButtonMoveOff.Checked = true;
            }

            if (Settings.Mode.mUseAoe)
            {
                radioButtonAoeOn.Checked = true;
            }
            else
            {
                radioButtonAoeOff.Checked = true;
            }

            switch (Settings.Mode.mCooldownUse)
            {
                case Helpers.Enum.CooldownUse.Always:

                    radioCooldownAlways.Checked = true;
                    break;

                case Helpers.Enum.CooldownUse.ByFocus:

                    radioCooldownByFocus.Checked = true;
                    break;

                case Helpers.Enum.CooldownUse.OnlyOnBosses:

                    radioCooldownByBoss.Checked = true;
                    break;

                case Helpers.Enum.CooldownUse.Never:

                    radioCooldownNever.Checked = true;
                    break;
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            if (checkBoxRaidPoison.Checked || checkBoxArenaPoison.Checked)
            {
                var input = MessageBox.Show("WARNING: You have enabled poisons for Raid mode or Arena mode. Using Apoc's Raid Bot as your botbase will result in RogueBT being " +
                                             "unable to apply poisons, regardless of context, due to a design limitation with Raid Bot. ",
                                             "Warning",
                                             MessageBoxButtons.OKCancel,
                                             MessageBoxIcon.Exclamation);

                if (input == DialogResult.Cancel) return;
            }

            Settings.Mode.mUsePoisons[(int) Helpers.Enum.LocationContext.Raid]          = checkBoxRaidPoison.Checked;
            Settings.Mode.mUsePoisons[(int) Helpers.Enum.LocationContext.Arena]         = checkBoxArenaPoison.Checked;
            Settings.Mode.mUsePoisons[(int) Helpers.Enum.LocationContext.Dungeon]       = checkBoxDungeonPoison.Checked;
            Settings.Mode.mUsePoisons[(int) Helpers.Enum.LocationContext.Battleground]  = checkBoxBgPoison.Checked;
            Settings.Mode.mUsePoisons[(int) Helpers.Enum.LocationContext.World]         = checkBoxLevelPoison.Checked;

            Settings.Mode.mPoisonsMain[(int)Helpers.Enum.LocationContext.Raid]          = (Helpers.Enum.LeathalPoisonSpellId)comboBoxRaidPoison1.SelectedItem;
            Settings.Mode.mPoisonsOff[(int)Helpers.Enum.LocationContext.Raid]           = (Helpers.Enum.NonLeathalPoisonSpellId)comboBoxRaidPoison2.SelectedItem;
            Settings.Mode.mPoisonsMain[(int)Helpers.Enum.LocationContext.Arena]         = (Helpers.Enum.LeathalPoisonSpellId)comboBoxArenaPoison1.SelectedItem;
            Settings.Mode.mPoisonsOff[(int)Helpers.Enum.LocationContext.Arena]          = (Helpers.Enum.NonLeathalPoisonSpellId)comboBoxArenaPoison2.SelectedItem;
            Settings.Mode.mPoisonsMain[(int)Helpers.Enum.LocationContext.Dungeon]       = (Helpers.Enum.LeathalPoisonSpellId)comboBoxDungeonPoison1.SelectedItem;
            Settings.Mode.mPoisonsOff[(int)Helpers.Enum.LocationContext.Dungeon]        = (Helpers.Enum.NonLeathalPoisonSpellId)comboBoxDungeonPoison2.SelectedItem;
            Settings.Mode.mPoisonsMain[(int)Helpers.Enum.LocationContext.Battleground]  = (Helpers.Enum.LeathalPoisonSpellId)comboBoxBgPoison1.SelectedItem;
            Settings.Mode.mPoisonsOff[(int)Helpers.Enum.LocationContext.Battleground]   = (Helpers.Enum.NonLeathalPoisonSpellId)comboBoxBgPoison2.SelectedItem;
            Settings.Mode.mPoisonsMain[(int)Helpers.Enum.LocationContext.World]         = (Helpers.Enum.LeathalPoisonSpellId)comboBoxLevelPoison1.SelectedItem;
            Settings.Mode.mPoisonsOff[(int)Helpers.Enum.LocationContext.World]          = (Helpers.Enum.NonLeathalPoisonSpellId)comboBoxLevelPoison2.SelectedItem;

            Settings.Mode.mOverrideContext = !radioButtonAuto.Checked;
            Settings.Mode.mUseMovement = radioButtonMoveOn.Checked;
            Settings.Mode.mUseAoe = radioButtonAoeOn.Checked;

            if (radioButtonRaid.Checked)
            {
                Settings.Mode.mLocationSettings = Helpers.Enum.LocationContext.Raid;
            }
            else if (radioButtonArena.Checked)
            {
                Settings.Mode.mLocationSettings = Helpers.Enum.LocationContext.Arena;
            }
            else if (radioButtonDungeon.Checked)
            {
                Settings.Mode.mLocationSettings = Helpers.Enum.LocationContext.Dungeon;
            }
            else if (radioButtonBattleground.Checked)
            {
                Settings.Mode.mLocationSettings = Helpers.Enum.LocationContext.Battleground;
            }
            else if (radioButtonLevel.Checked)
            {
                Settings.Mode.mLocationSettings = Helpers.Enum.LocationContext.World;
            }

            if (radioCooldownAlways.Checked)
            {
                Settings.Mode.mCooldownUse = Helpers.Enum.CooldownUse.Always;
            }
            else if (radioCooldownByFocus.Checked)
            {
                Settings.Mode.mCooldownUse = Helpers.Enum.CooldownUse.ByFocus;
            }
            else if (radioCooldownByBoss.Checked)
            {
                Settings.Mode.mCooldownUse = Helpers.Enum.CooldownUse.OnlyOnBosses;  
            }
            else
            {
                Settings.Mode.mCooldownUse = Helpers.Enum.CooldownUse.Never;
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

        private void checkBoxRaidPoison_CheckedChanged(object sender, EventArgs e)
        {
            panelRaidPoison.Enabled = checkBoxRaidPoison.Checked;
        }

        private void checkBoxArenaPoison_CheckedChanged(object sender, EventArgs e)
        {
            panelArenaPoison.Enabled = checkBoxArenaPoison.Checked;
        }

        private void checkBoxDungeonPoison_CheckedChanged(object sender, EventArgs e)
        {
            panelDungeonPoison.Enabled = checkBoxDungeonPoison.Checked;
        }

        private void checkBoxBgPoison_CheckedChanged(object sender, EventArgs e)
        {
            panelBgPoison.Enabled = checkBoxBgPoison.Checked;
        }

        private void checkBoxLevelPoison_CheckedChanged(object sender, EventArgs e)
        {
            panelLevelPoison.Enabled = checkBoxBgPoison.Checked;
        }
    }
}
