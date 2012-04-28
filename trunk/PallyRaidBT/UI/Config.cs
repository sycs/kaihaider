//////////////////////////////////////////////////
//                Config.cs                     //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
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
          
     
            if (!Settings.Mode.mOverrideContext)
            {
                radioButtonAuto.Checked = true;
            }
            else
            {
                switch (Settings.Mode.mLocationSettings)
                {
                    case Helpers.Enumeration.LocationContext.Raid:

                        radioButtonRaid.Checked = true;
                        break;

                    case Helpers.Enumeration.LocationContext.HeroicDungeon:

                        radioButtonHeroicDungeon.Checked = true;
                        break;

                    case Helpers.Enumeration.LocationContext.Dungeon:

                        radioButtonDungeon.Checked = true;
                        break;

                    case Helpers.Enumeration.LocationContext.Battleground:

                        radioButtonBattleground.Checked = true;
                        break;

                    case Helpers.Enumeration.LocationContext.World:

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
                case Helpers.Enumeration.CooldownUse.Always:

                    radioCooldownAlways.Checked = true;
                    break;

                case Helpers.Enumeration.CooldownUse.ByFocus:

                    radioCooldownByFocus.Checked = true;
                    break;

                case Helpers.Enumeration.CooldownUse.OnlyOnBosses:

                    radioCooldownByBoss.Checked = true;
                    break;

                case Helpers.Enumeration.CooldownUse.Never:

                    radioCooldownNever.Checked = true;
                    break;
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            
            Settings.Mode.mOverrideContext = !radioButtonAuto.Checked;
            Settings.Mode.mUseMovement = radioButtonMoveOn.Checked;
            Settings.Mode.mUseAoe = radioButtonAoeOn.Checked;

            if (radioButtonRaid.Checked)
            {
                Settings.Mode.mLocationSettings = Helpers.Enumeration.LocationContext.Raid;
            }
            else if (radioButtonHeroicDungeon.Checked)
            {
                Settings.Mode.mLocationSettings = Helpers.Enumeration.LocationContext.HeroicDungeon;
            }
            else if (radioButtonDungeon.Checked)
            {
                Settings.Mode.mLocationSettings = Helpers.Enumeration.LocationContext.Dungeon;
            }
            else if (radioButtonBattleground.Checked)
            {
                Settings.Mode.mLocationSettings = Helpers.Enumeration.LocationContext.Battleground;
            }
            else if (radioButtonLevel.Checked)
            {
                Settings.Mode.mLocationSettings = Helpers.Enumeration.LocationContext.World;
            }

            if (radioCooldownAlways.Checked)
            {
                Settings.Mode.mCooldownUse = Helpers.Enumeration.CooldownUse.Always;
            }
            else if (radioCooldownByFocus.Checked)
            {
                Settings.Mode.mCooldownUse = Helpers.Enumeration.CooldownUse.ByFocus;
            }
            else if (radioCooldownByBoss.Checked)
            {
                Settings.Mode.mCooldownUse = Helpers.Enumeration.CooldownUse.OnlyOnBosses;  
            }
            else
            {
                Settings.Mode.mCooldownUse = Helpers.Enumeration.CooldownUse.Never;
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
