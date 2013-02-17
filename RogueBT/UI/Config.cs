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

        private void buttonApply_Click(object sender, EventArgs e)
        {

            Settings.Mode.mUsePoisons[(int)Helpers.Enum.LocationContext.Raid] = checkBoxRaidPoison.Checked;
            Settings.Mode.mUsePoisons[(int)Helpers.Enum.LocationContext.Arena] = checkBoxArenaPoison.Checked;
            Settings.Mode.mUsePoisons[(int)Helpers.Enum.LocationContext.Dungeon] = checkBoxDungeonPoison.Checked;
            Settings.Mode.mUsePoisons[(int)Helpers.Enum.LocationContext.Battleground] = checkBoxBgPoison.Checked;
            Settings.Mode.mUsePoisons[(int)Helpers.Enum.LocationContext.World] = checkBoxLevelPoison.Checked;

            Settings.Mode.mPoisonsMain[(int)Helpers.Enum.LocationContext.Raid] = (Helpers.Enum.LeathalPoisonSpellId)comboBoxRaidPoison1.SelectedItem;
            Settings.Mode.mPoisonsOff[(int)Helpers.Enum.LocationContext.Raid] = (Helpers.Enum.NonLeathalPoisonSpellId)comboBoxRaidPoison2.SelectedItem;
            Settings.Mode.mPoisonsMain[(int)Helpers.Enum.LocationContext.Arena] = (Helpers.Enum.LeathalPoisonSpellId)comboBoxArenaPoison1.SelectedItem;
            Settings.Mode.mPoisonsOff[(int)Helpers.Enum.LocationContext.Arena] = (Helpers.Enum.NonLeathalPoisonSpellId)comboBoxArenaPoison2.SelectedItem;
            Settings.Mode.mPoisonsMain[(int)Helpers.Enum.LocationContext.Dungeon] = (Helpers.Enum.LeathalPoisonSpellId)comboBoxDungeonPoison1.SelectedItem;
            Settings.Mode.mPoisonsOff[(int)Helpers.Enum.LocationContext.Dungeon] = (Helpers.Enum.NonLeathalPoisonSpellId)comboBoxDungeonPoison2.SelectedItem;
            Settings.Mode.mPoisonsMain[(int)Helpers.Enum.LocationContext.Battleground] = (Helpers.Enum.LeathalPoisonSpellId)comboBoxBgPoison1.SelectedItem;
            Settings.Mode.mPoisonsOff[(int)Helpers.Enum.LocationContext.Battleground] = (Helpers.Enum.NonLeathalPoisonSpellId)comboBoxBgPoison2.SelectedItem;
            Settings.Mode.mPoisonsMain[(int)Helpers.Enum.LocationContext.World] = (Helpers.Enum.LeathalPoisonSpellId)comboBoxLevelPoison1.SelectedItem;
            Settings.Mode.mPoisonsOff[(int)Helpers.Enum.LocationContext.World] = (Helpers.Enum.NonLeathalPoisonSpellId)comboBoxLevelPoison2.SelectedItem;

            Settings.Mode.mOverrideContext = !radioButtonAuto.Checked;

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

            if (radioSapAdds.Checked)
            {
                Settings.Mode.mSap = Helpers.Enum.Saps.Adds;
            }
            else if (radioSapTarget.Checked)
            {
                Settings.Mode.mSap = Helpers.Enum.Saps.Target;
            }
            else
            {
                Settings.Mode.mSap = Helpers.Enum.Saps.Never;
            }


            Settings.Mode.mTargeting = targeting.Checked;
            Settings.Mode.mUseMovement = movement.Checked;
            Settings.Mode.mMoveBehind = moveBehind.Checked;
            Settings.Mode.mMoveBackwards = moveBackwards.Checked;
            Settings.Mode.mAlwaysStealth = alwaysStealth.Checked;
            Settings.Mode.mNeverStealth = neverStealth.Checked;
            Settings.Mode.mUseAoe = aoe.Checked;
            Settings.Mode.mCrowdControl = crowdControl.Checked;
            Settings.Mode.mPickPocket = pickPocket.Checked;
            Settings.Mode.mSWPick = swPick.Checked;
            Settings.Mode.mFeint = feint.Checked;
            Settings.Mode.mFoKPull = FoKPull.Checked;
            Settings.Mode.mDistract = distract.Checked;
            Settings.Mode.mVanish = vanish.Checked;


            Close();
        }
        private void Config_Load(object sender, EventArgs e)
        {


            distract.Checked = Settings.Mode.mDistract;
            vanish.Checked = Settings.Mode.mVanish;
            targeting.Checked = Settings.Mode.mTargeting;
            movement.Checked = Settings.Mode.mUseMovement;
            moveBehind.Checked = Settings.Mode.mMoveBehind;
            moveBackwards.Checked = Settings.Mode.mMoveBackwards;
            alwaysStealth.Checked = Settings.Mode.mAlwaysStealth;
            neverStealth.Checked = Settings.Mode.mNeverStealth;
            aoe.Checked = Settings.Mode.mUseAoe;
            crowdControl.Checked = Settings.Mode.mCrowdControl;
            pickPocket.Checked = Settings.Mode.mPickPocket;
            swPick.Checked = Settings.Mode.mSWPick;
            feint.Checked = Settings.Mode.mFeint;
            FoKPull.Checked = Settings.Mode.mFoKPull;

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

            switch (Settings.Mode.mSap)
            {
                case Helpers.Enum.Saps.Never:

                    radioSapNever.Checked = true;
                    break;

                case Helpers.Enum.Saps.Adds:

                    radioSapAdds.Checked = true;
                    break;

                case Helpers.Enum.Saps.Target:

                    radioSapTarget.Checked = true;
                    break;
            }
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


        private void checkAlwaysStealth_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Mode.mAlwaysStealth = alwaysStealth.Checked;
        }

        private void checkAoe_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Mode.mUseAoe = aoe.Checked;
        }

        private void checkCrowdControl_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Mode.mCrowdControl = crowdControl.Checked;
        }

        private void checkPickPocket_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Mode.mPickPocket = pickPocket.Checked;

        }

        private void checkSWPick_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Mode.mSWPick = swPick.Checked;

        }

        private void checkFeint_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Mode.mFeint = pickPocket.Checked;

        }
        
        private void checkMovement_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Mode.mUseMovement = movement.Checked;
        }

        private void checkMoveBehind_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Mode.mMoveBehind = moveBehind.Checked;
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
