//////////////////////////////////////////////////
//             Config.Designer.cs               //
//      Part of RogueBT by kaihaider        //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////

namespace RogueBT.UI
{
    partial class Config
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
            this.components = new System.ComponentModel.Container();
            this.buttonApply = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonLaunchCombatControl = new System.Windows.Forms.Button();
            this.labelUseCooldowns = new System.Windows.Forms.Label();
            this.panelCooldowns = new System.Windows.Forms.Panel();
            this.radioCooldownNever = new System.Windows.Forms.RadioButton();
            this.radioCooldownByBoss = new System.Windows.Forms.RadioButton();
            this.radioCooldownByFocus = new System.Windows.Forms.RadioButton();
            this.radioCooldownAlways = new System.Windows.Forms.RadioButton();
            this.labelModePrompt = new System.Windows.Forms.Label();
            this.radioButtonDungeon = new System.Windows.Forms.RadioButton();
            this.radioButtonArena = new System.Windows.Forms.RadioButton();
            this.radioButtonRaid = new System.Windows.Forms.RadioButton();
            this.radioButtonAuto = new System.Windows.Forms.RadioButton();
            this.radioButtonBattleground = new System.Windows.Forms.RadioButton();
            this.radioButtonHolderPanel = new System.Windows.Forms.Panel();
            this.radioButtonLevel = new System.Windows.Forms.RadioButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelApplyPoisons = new System.Windows.Forms.Label();
            this.panelRaidPoison = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxRaidPoison2 = new System.Windows.Forms.ComboBox();
            this.labelRaidPoisonMain = new System.Windows.Forms.Label();
            this.comboBoxRaidPoison1 = new System.Windows.Forms.ComboBox();
            this.checkBoxRaidPoison = new System.Windows.Forms.CheckBox();
            this.panelArenaPoison = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxArenaPoison2 = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBoxArenaPoison1 = new System.Windows.Forms.ComboBox();
            this.checkBoxArenaPoison = new System.Windows.Forms.CheckBox();
            this.panelBgPoison = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBoxBgPoison2 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxBgPoison1 = new System.Windows.Forms.ComboBox();
            this.checkBoxBgPoison = new System.Windows.Forms.CheckBox();
            this.panelDungeonPoison = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxDungeonPoison2 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxDungeonPoison1 = new System.Windows.Forms.ComboBox();
            this.checkBoxDungeonPoison = new System.Windows.Forms.CheckBox();
            this.panelLevelPoison = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.comboBoxLevelPoison2 = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.comboBoxLevelPoison1 = new System.Windows.Forms.ComboBox();
            this.checkBoxLevelPoison = new System.Windows.Forms.CheckBox();
            this.alwaysStealth = new System.Windows.Forms.CheckBox();
            this.pickPocket = new System.Windows.Forms.CheckBox();
            this.moveBehind = new System.Windows.Forms.CheckBox();
            this.aoe = new System.Windows.Forms.CheckBox();
            this.movement = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.crowdControl = new System.Windows.Forms.CheckBox();
            this.targeting = new System.Windows.Forms.CheckBox();
            this.moveBackwards = new System.Windows.Forms.CheckBox();
            this.radioSapNever = new System.Windows.Forms.RadioButton();
            this.panelSap = new System.Windows.Forms.Panel();
            this.radioSapTarget = new System.Windows.Forms.RadioButton();
            this.radioSapAdds = new System.Windows.Forms.RadioButton();
            this.label11 = new System.Windows.Forms.Label();
            this.feint = new System.Windows.Forms.CheckBox();
            this.swPick = new System.Windows.Forms.CheckBox();
            this.FoKPull = new System.Windows.Forms.CheckBox();
            this.neverStealth = new System.Windows.Forms.CheckBox();
            this.distract = new System.Windows.Forms.CheckBox();
            this.vanish = new System.Windows.Forms.CheckBox();
            this.panelCooldowns.SuspendLayout();
            this.radioButtonHolderPanel.SuspendLayout();
            this.panelRaidPoison.SuspendLayout();
            this.panelArenaPoison.SuspendLayout();
            this.panelBgPoison.SuspendLayout();
            this.panelDungeonPoison.SuspendLayout();
            this.panelLevelPoison.SuspendLayout();
            this.panelSap.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(19, 369);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(95, 23);
            this.buttonApply.TabIndex = 2;
            this.buttonApply.Text = "Apply and Close";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(412, 369);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonLaunchCombatControl
            // 
            this.buttonLaunchCombatControl.Location = new System.Drawing.Point(209, 369);
            this.buttonLaunchCombatControl.Name = "buttonLaunchCombatControl";
            this.buttonLaunchCombatControl.Size = new System.Drawing.Size(136, 23);
            this.buttonLaunchCombatControl.TabIndex = 4;
            this.buttonLaunchCombatControl.Text = "Launch Combat Control";
            this.buttonLaunchCombatControl.UseVisualStyleBackColor = true;
            this.buttonLaunchCombatControl.Click += new System.EventHandler(this.buttonLaunchCombatControl_Click);
            // 
            // labelUseCooldowns
            // 
            this.labelUseCooldowns.AutoSize = true;
            this.labelUseCooldowns.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUseCooldowns.Location = new System.Drawing.Point(160, 9);
            this.labelUseCooldowns.Name = "labelUseCooldowns";
            this.labelUseCooldowns.Size = new System.Drawing.Size(105, 24);
            this.labelUseCooldowns.TabIndex = 5;
            this.labelUseCooldowns.Text = "Cooldowns";
            // 
            // panelCooldowns
            // 
            this.panelCooldowns.Controls.Add(this.radioCooldownNever);
            this.panelCooldowns.Controls.Add(this.radioCooldownByBoss);
            this.panelCooldowns.Controls.Add(this.radioCooldownByFocus);
            this.panelCooldowns.Controls.Add(this.radioCooldownAlways);
            this.panelCooldowns.Location = new System.Drawing.Point(164, 36);
            this.panelCooldowns.Name = "panelCooldowns";
            this.panelCooldowns.Size = new System.Drawing.Size(103, 92);
            this.panelCooldowns.TabIndex = 6;
            // 
            // radioCooldownNever
            // 
            this.radioCooldownNever.AutoSize = true;
            this.radioCooldownNever.Location = new System.Drawing.Point(4, 72);
            this.radioCooldownNever.Name = "radioCooldownNever";
            this.radioCooldownNever.Size = new System.Drawing.Size(54, 17);
            this.radioCooldownNever.TabIndex = 3;
            this.radioCooldownNever.TabStop = true;
            this.radioCooldownNever.Text = "Never";
            this.radioCooldownNever.UseVisualStyleBackColor = true;
            // 
            // radioCooldownByBoss
            // 
            this.radioCooldownByBoss.AutoSize = true;
            this.radioCooldownByBoss.Location = new System.Drawing.Point(4, 49);
            this.radioCooldownByBoss.Name = "radioCooldownByBoss";
            this.radioCooldownByBoss.Size = new System.Drawing.Size(97, 17);
            this.radioCooldownByBoss.TabIndex = 2;
            this.radioCooldownByBoss.TabStop = true;
            this.radioCooldownByBoss.Text = "Only on bosses";
            this.radioCooldownByBoss.UseVisualStyleBackColor = true;
            // 
            // radioCooldownByFocus
            // 
            this.radioCooldownByFocus.AutoSize = true;
            this.radioCooldownByFocus.Location = new System.Drawing.Point(4, 26);
            this.radioCooldownByFocus.Name = "radioCooldownByFocus";
            this.radioCooldownByFocus.Size = new System.Drawing.Size(66, 17);
            this.radioCooldownByFocus.TabIndex = 1;
            this.radioCooldownByFocus.TabStop = true;
            this.radioCooldownByFocus.Text = "By focus";
            this.radioCooldownByFocus.UseVisualStyleBackColor = true;
            // 
            // radioCooldownAlways
            // 
            this.radioCooldownAlways.AutoSize = true;
            this.radioCooldownAlways.Location = new System.Drawing.Point(4, 4);
            this.radioCooldownAlways.Name = "radioCooldownAlways";
            this.radioCooldownAlways.Size = new System.Drawing.Size(58, 17);
            this.radioCooldownAlways.TabIndex = 0;
            this.radioCooldownAlways.TabStop = true;
            this.radioCooldownAlways.Text = "Always";
            this.radioCooldownAlways.UseVisualStyleBackColor = true;
            // 
            // labelModePrompt
            // 
            this.labelModePrompt.AutoSize = true;
            this.labelModePrompt.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelModePrompt.Location = new System.Drawing.Point(12, 9);
            this.labelModePrompt.Name = "labelModePrompt";
            this.labelModePrompt.Size = new System.Drawing.Size(59, 24);
            this.labelModePrompt.TabIndex = 0;
            this.labelModePrompt.Text = "Mode";
            // 
            // radioButtonDungeon
            // 
            this.radioButtonDungeon.AutoSize = true;
            this.radioButtonDungeon.Location = new System.Drawing.Point(3, 72);
            this.radioButtonDungeon.Name = "radioButtonDungeon";
            this.radioButtonDungeon.Size = new System.Drawing.Size(98, 17);
            this.radioButtonDungeon.TabIndex = 4;
            this.radioButtonDungeon.TabStop = true;
            this.radioButtonDungeon.Text = "Dungeon mode";
            this.radioButtonDungeon.UseVisualStyleBackColor = true;
            // 
            // radioButtonArena
            // 
            this.radioButtonArena.AutoSize = true;
            this.radioButtonArena.Location = new System.Drawing.Point(3, 49);
            this.radioButtonArena.Name = "radioButtonArena";
            this.radioButtonArena.Size = new System.Drawing.Size(82, 17);
            this.radioButtonArena.TabIndex = 1;
            this.radioButtonArena.TabStop = true;
            this.radioButtonArena.Text = "Arena mode";
            this.radioButtonArena.UseVisualStyleBackColor = true;
            // 
            // radioButtonRaid
            // 
            this.radioButtonRaid.AutoSize = true;
            this.radioButtonRaid.Location = new System.Drawing.Point(3, 26);
            this.radioButtonRaid.Name = "radioButtonRaid";
            this.radioButtonRaid.Size = new System.Drawing.Size(76, 17);
            this.radioButtonRaid.TabIndex = 0;
            this.radioButtonRaid.TabStop = true;
            this.radioButtonRaid.Text = "Raid mode";
            this.radioButtonRaid.UseVisualStyleBackColor = true;
            // 
            // radioButtonAuto
            // 
            this.radioButtonAuto.AutoSize = true;
            this.radioButtonAuto.Location = new System.Drawing.Point(3, 4);
            this.radioButtonAuto.Name = "radioButtonAuto";
            this.radioButtonAuto.Size = new System.Drawing.Size(72, 17);
            this.radioButtonAuto.TabIndex = 2;
            this.radioButtonAuto.TabStop = true;
            this.radioButtonAuto.Text = "Automatic";
            this.radioButtonAuto.UseVisualStyleBackColor = true;
            // 
            // radioButtonBattleground
            // 
            this.radioButtonBattleground.AutoSize = true;
            this.radioButtonBattleground.Location = new System.Drawing.Point(3, 95);
            this.radioButtonBattleground.Name = "radioButtonBattleground";
            this.radioButtonBattleground.Size = new System.Drawing.Size(114, 17);
            this.radioButtonBattleground.TabIndex = 3;
            this.radioButtonBattleground.TabStop = true;
            this.radioButtonBattleground.Text = "Battleground mode";
            this.radioButtonBattleground.UseVisualStyleBackColor = true;
            // 
            // radioButtonHolderPanel
            // 
            this.radioButtonHolderPanel.Controls.Add(this.radioButtonLevel);
            this.radioButtonHolderPanel.Controls.Add(this.radioButtonBattleground);
            this.radioButtonHolderPanel.Controls.Add(this.radioButtonAuto);
            this.radioButtonHolderPanel.Controls.Add(this.radioButtonRaid);
            this.radioButtonHolderPanel.Controls.Add(this.radioButtonArena);
            this.radioButtonHolderPanel.Controls.Add(this.radioButtonDungeon);
            this.radioButtonHolderPanel.Location = new System.Drawing.Point(16, 36);
            this.radioButtonHolderPanel.Name = "radioButtonHolderPanel";
            this.radioButtonHolderPanel.Size = new System.Drawing.Size(136, 138);
            this.radioButtonHolderPanel.TabIndex = 1;
            // 
            // radioButtonLevel
            // 
            this.radioButtonLevel.AutoSize = true;
            this.radioButtonLevel.Location = new System.Drawing.Point(3, 118);
            this.radioButtonLevel.Name = "radioButtonLevel";
            this.radioButtonLevel.Size = new System.Drawing.Size(82, 17);
            this.radioButtonLevel.TabIndex = 5;
            this.radioButtonLevel.TabStop = true;
            this.radioButtonLevel.Text = "World mode";
            this.radioButtonLevel.UseVisualStyleBackColor = true;
            // 
            // labelApplyPoisons
            // 
            this.labelApplyPoisons.AutoSize = true;
            this.labelApplyPoisons.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApplyPoisons.Location = new System.Drawing.Point(15, 206);
            this.labelApplyPoisons.Name = "labelApplyPoisons";
            this.labelApplyPoisons.Size = new System.Drawing.Size(77, 24);
            this.labelApplyPoisons.TabIndex = 9;
            this.labelApplyPoisons.Text = "Poisons";
            // 
            // panelRaidPoison
            // 
            this.panelRaidPoison.Controls.Add(this.label1);
            this.panelRaidPoison.Controls.Add(this.comboBoxRaidPoison2);
            this.panelRaidPoison.Controls.Add(this.labelRaidPoisonMain);
            this.panelRaidPoison.Controls.Add(this.comboBoxRaidPoison1);
            this.panelRaidPoison.Location = new System.Drawing.Point(111, 233);
            this.panelRaidPoison.Name = "panelRaidPoison";
            this.panelRaidPoison.Size = new System.Drawing.Size(278, 22);
            this.panelRaidPoison.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(148, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Offhand";
            // 
            // comboBoxRaidPoison2
            // 
            this.comboBoxRaidPoison2.FormattingEnabled = true;
            this.comboBoxRaidPoison2.Location = new System.Drawing.Point(193, 0);
            this.comboBoxRaidPoison2.Name = "comboBoxRaidPoison2";
            this.comboBoxRaidPoison2.Size = new System.Drawing.Size(76, 21);
            this.comboBoxRaidPoison2.TabIndex = 3;
            // 
            // labelRaidPoisonMain
            // 
            this.labelRaidPoisonMain.AutoSize = true;
            this.labelRaidPoisonMain.Location = new System.Drawing.Point(10, 5);
            this.labelRaidPoisonMain.Name = "labelRaidPoisonMain";
            this.labelRaidPoisonMain.Size = new System.Drawing.Size(54, 13);
            this.labelRaidPoisonMain.TabIndex = 2;
            this.labelRaidPoisonMain.Text = "Mainhand";
            // 
            // comboBoxRaidPoison1
            // 
            this.comboBoxRaidPoison1.AllowDrop = true;
            this.comboBoxRaidPoison1.FormattingEnabled = true;
            this.comboBoxRaidPoison1.Location = new System.Drawing.Point(70, 0);
            this.comboBoxRaidPoison1.Name = "comboBoxRaidPoison1";
            this.comboBoxRaidPoison1.Size = new System.Drawing.Size(76, 21);
            this.comboBoxRaidPoison1.TabIndex = 1;
            // 
            // checkBoxRaidPoison
            // 
            this.checkBoxRaidPoison.AutoSize = true;
            this.checkBoxRaidPoison.Location = new System.Drawing.Point(22, 237);
            this.checkBoxRaidPoison.Name = "checkBoxRaidPoison";
            this.checkBoxRaidPoison.Size = new System.Drawing.Size(48, 17);
            this.checkBoxRaidPoison.TabIndex = 0;
            this.checkBoxRaidPoison.Text = "Raid";
            this.checkBoxRaidPoison.UseVisualStyleBackColor = true;
            this.checkBoxRaidPoison.CheckedChanged += new System.EventHandler(this.checkBoxRaidPoison_CheckedChanged);
            // 
            // panelArenaPoison
            // 
            this.panelArenaPoison.Controls.Add(this.label2);
            this.panelArenaPoison.Controls.Add(this.comboBoxArenaPoison2);
            this.panelArenaPoison.Controls.Add(this.label3);
            this.panelArenaPoison.Controls.Add(this.comboBoxArenaPoison1);
            this.panelArenaPoison.Location = new System.Drawing.Point(111, 260);
            this.panelArenaPoison.Name = "panelArenaPoison";
            this.panelArenaPoison.Size = new System.Drawing.Size(278, 22);
            this.panelArenaPoison.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(148, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Offhand";
            // 
            // comboBoxArenaPoison2
            // 
            this.comboBoxArenaPoison2.FormattingEnabled = true;
            this.comboBoxArenaPoison2.Location = new System.Drawing.Point(193, 0);
            this.comboBoxArenaPoison2.Name = "comboBoxArenaPoison2";
            this.comboBoxArenaPoison2.Size = new System.Drawing.Size(76, 21);
            this.comboBoxArenaPoison2.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Mainhand";
            // 
            // comboBoxArenaPoison1
            // 
            this.comboBoxArenaPoison1.FormattingEnabled = true;
            this.comboBoxArenaPoison1.Location = new System.Drawing.Point(70, 0);
            this.comboBoxArenaPoison1.Name = "comboBoxArenaPoison1";
            this.comboBoxArenaPoison1.Size = new System.Drawing.Size(76, 21);
            this.comboBoxArenaPoison1.TabIndex = 1;
            // 
            // checkBoxArenaPoison
            // 
            this.checkBoxArenaPoison.AutoSize = true;
            this.checkBoxArenaPoison.Location = new System.Drawing.Point(22, 264);
            this.checkBoxArenaPoison.Name = "checkBoxArenaPoison";
            this.checkBoxArenaPoison.Size = new System.Drawing.Size(54, 17);
            this.checkBoxArenaPoison.TabIndex = 0;
            this.checkBoxArenaPoison.Text = "Arena";
            this.checkBoxArenaPoison.UseVisualStyleBackColor = true;
            this.checkBoxArenaPoison.CheckedChanged += new System.EventHandler(this.checkBoxArenaPoison_CheckedChanged);
            // 
            // panelBgPoison
            // 
            this.panelBgPoison.Controls.Add(this.label4);
            this.panelBgPoison.Controls.Add(this.comboBoxBgPoison2);
            this.panelBgPoison.Controls.Add(this.label5);
            this.panelBgPoison.Controls.Add(this.comboBoxBgPoison1);
            this.panelBgPoison.Location = new System.Drawing.Point(111, 314);
            this.panelBgPoison.Name = "panelBgPoison";
            this.panelBgPoison.Size = new System.Drawing.Size(278, 22);
            this.panelBgPoison.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(148, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Offhand";
            // 
            // comboBoxBgPoison2
            // 
            this.comboBoxBgPoison2.FormattingEnabled = true;
            this.comboBoxBgPoison2.Location = new System.Drawing.Point(193, 0);
            this.comboBoxBgPoison2.Name = "comboBoxBgPoison2";
            this.comboBoxBgPoison2.Size = new System.Drawing.Size(76, 21);
            this.comboBoxBgPoison2.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Mainhand";
            // 
            // comboBoxBgPoison1
            // 
            this.comboBoxBgPoison1.FormattingEnabled = true;
            this.comboBoxBgPoison1.Location = new System.Drawing.Point(70, 0);
            this.comboBoxBgPoison1.Name = "comboBoxBgPoison1";
            this.comboBoxBgPoison1.Size = new System.Drawing.Size(76, 21);
            this.comboBoxBgPoison1.TabIndex = 1;
            // 
            // checkBoxBgPoison
            // 
            this.checkBoxBgPoison.AutoSize = true;
            this.checkBoxBgPoison.Location = new System.Drawing.Point(22, 318);
            this.checkBoxBgPoison.Name = "checkBoxBgPoison";
            this.checkBoxBgPoison.Size = new System.Drawing.Size(86, 17);
            this.checkBoxBgPoison.TabIndex = 0;
            this.checkBoxBgPoison.Text = "Battleground";
            this.checkBoxBgPoison.UseVisualStyleBackColor = true;
            this.checkBoxBgPoison.CheckedChanged += new System.EventHandler(this.checkBoxBgPoison_CheckedChanged);
            // 
            // panelDungeonPoison
            // 
            this.panelDungeonPoison.Controls.Add(this.label6);
            this.panelDungeonPoison.Controls.Add(this.comboBoxDungeonPoison2);
            this.panelDungeonPoison.Controls.Add(this.label7);
            this.panelDungeonPoison.Controls.Add(this.comboBoxDungeonPoison1);
            this.panelDungeonPoison.Location = new System.Drawing.Point(111, 287);
            this.panelDungeonPoison.Name = "panelDungeonPoison";
            this.panelDungeonPoison.Size = new System.Drawing.Size(278, 22);
            this.panelDungeonPoison.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(148, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(45, 13);
            this.label6.TabIndex = 4;
            this.label6.Text = "Offhand";
            // 
            // comboBoxDungeonPoison2
            // 
            this.comboBoxDungeonPoison2.FormattingEnabled = true;
            this.comboBoxDungeonPoison2.Location = new System.Drawing.Point(193, 0);
            this.comboBoxDungeonPoison2.Name = "comboBoxDungeonPoison2";
            this.comboBoxDungeonPoison2.Size = new System.Drawing.Size(76, 21);
            this.comboBoxDungeonPoison2.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 5);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Mainhand";
            // 
            // comboBoxDungeonPoison1
            // 
            this.comboBoxDungeonPoison1.FormattingEnabled = true;
            this.comboBoxDungeonPoison1.Location = new System.Drawing.Point(70, 0);
            this.comboBoxDungeonPoison1.Name = "comboBoxDungeonPoison1";
            this.comboBoxDungeonPoison1.Size = new System.Drawing.Size(76, 21);
            this.comboBoxDungeonPoison1.TabIndex = 1;
            // 
            // checkBoxDungeonPoison
            // 
            this.checkBoxDungeonPoison.AutoSize = true;
            this.checkBoxDungeonPoison.Location = new System.Drawing.Point(22, 291);
            this.checkBoxDungeonPoison.Name = "checkBoxDungeonPoison";
            this.checkBoxDungeonPoison.Size = new System.Drawing.Size(70, 17);
            this.checkBoxDungeonPoison.TabIndex = 0;
            this.checkBoxDungeonPoison.Text = "Dungeon";
            this.checkBoxDungeonPoison.UseVisualStyleBackColor = true;
            this.checkBoxDungeonPoison.CheckedChanged += new System.EventHandler(this.checkBoxDungeonPoison_CheckedChanged);
            // 
            // panelLevelPoison
            // 
            this.panelLevelPoison.Controls.Add(this.label8);
            this.panelLevelPoison.Controls.Add(this.comboBoxLevelPoison2);
            this.panelLevelPoison.Controls.Add(this.label9);
            this.panelLevelPoison.Controls.Add(this.comboBoxLevelPoison1);
            this.panelLevelPoison.Location = new System.Drawing.Point(111, 341);
            this.panelLevelPoison.Name = "panelLevelPoison";
            this.panelLevelPoison.Size = new System.Drawing.Size(278, 22);
            this.panelLevelPoison.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(148, 5);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 4;
            this.label8.Text = "Offhand";
            // 
            // comboBoxLevelPoison2
            // 
            this.comboBoxLevelPoison2.FormattingEnabled = true;
            this.comboBoxLevelPoison2.Location = new System.Drawing.Point(193, 0);
            this.comboBoxLevelPoison2.Name = "comboBoxLevelPoison2";
            this.comboBoxLevelPoison2.Size = new System.Drawing.Size(76, 21);
            this.comboBoxLevelPoison2.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 5);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Mainhand";
            // 
            // comboBoxLevelPoison1
            // 
            this.comboBoxLevelPoison1.FormattingEnabled = true;
            this.comboBoxLevelPoison1.Location = new System.Drawing.Point(70, 0);
            this.comboBoxLevelPoison1.Name = "comboBoxLevelPoison1";
            this.comboBoxLevelPoison1.Size = new System.Drawing.Size(76, 21);
            this.comboBoxLevelPoison1.TabIndex = 1;
            // 
            // checkBoxLevelPoison
            // 
            this.checkBoxLevelPoison.AutoSize = true;
            this.checkBoxLevelPoison.Location = new System.Drawing.Point(22, 345);
            this.checkBoxLevelPoison.Name = "checkBoxLevelPoison";
            this.checkBoxLevelPoison.Size = new System.Drawing.Size(52, 17);
            this.checkBoxLevelPoison.TabIndex = 0;
            this.checkBoxLevelPoison.Text = "Level";
            this.checkBoxLevelPoison.UseVisualStyleBackColor = true;
            this.checkBoxLevelPoison.CheckedChanged += new System.EventHandler(this.checkBoxLevelPoison_CheckedChanged);
            // 
            // alwaysStealth
            // 
            this.alwaysStealth.AutoSize = true;
            this.alwaysStealth.Location = new System.Drawing.Point(404, 40);
            this.alwaysStealth.Name = "alwaysStealth";
            this.alwaysStealth.Size = new System.Drawing.Size(95, 17);
            this.alwaysStealth.TabIndex = 24;
            this.alwaysStealth.Text = "Always Stealth";
            this.alwaysStealth.UseVisualStyleBackColor = true;
            // 
            // pickPocket
            // 
            this.pickPocket.AutoSize = true;
            this.pickPocket.Location = new System.Drawing.Point(403, 131);
            this.pickPocket.Name = "pickPocket";
            this.pickPocket.Size = new System.Drawing.Size(84, 17);
            this.pickPocket.TabIndex = 23;
            this.pickPocket.Text = "Pick Pocket";
            this.pickPocket.UseVisualStyleBackColor = true;
            // 
            // moveBehind
            // 
            this.moveBehind.AutoSize = true;
            this.moveBehind.Location = new System.Drawing.Point(271, 108);
            this.moveBehind.Name = "moveBehind";
            this.moveBehind.Size = new System.Drawing.Size(89, 17);
            this.moveBehind.TabIndex = 22;
            this.moveBehind.Text = "Move Behind";
            this.moveBehind.UseVisualStyleBackColor = true;
            // 
            // aoe
            // 
            this.aoe.AutoSize = true;
            this.aoe.Location = new System.Drawing.Point(403, 85);
            this.aoe.Name = "aoe";
            this.aoe.Size = new System.Drawing.Size(45, 17);
            this.aoe.TabIndex = 21;
            this.aoe.Text = "Aoe";
            this.aoe.UseVisualStyleBackColor = true;
            // 
            // movement
            // 
            this.movement.AutoSize = true;
            this.movement.Location = new System.Drawing.Point(271, 63);
            this.movement.Name = "movement";
            this.movement.Size = new System.Drawing.Size(76, 17);
            this.movement.TabIndex = 20;
            this.movement.Text = "Movement";
            this.movement.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(328, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 24);
            this.label10.TabIndex = 25;
            this.label10.Text = "Toggles";
            // 
            // crowdControl
            // 
            this.crowdControl.AutoSize = true;
            this.crowdControl.Location = new System.Drawing.Point(403, 108);
            this.crowdControl.Name = "crowdControl";
            this.crowdControl.Size = new System.Drawing.Size(92, 17);
            this.crowdControl.TabIndex = 26;
            this.crowdControl.Text = "Crowd Control";
            this.crowdControl.UseVisualStyleBackColor = true;
            // 
            // targeting
            // 
            this.targeting.AutoSize = true;
            this.targeting.Location = new System.Drawing.Point(271, 40);
            this.targeting.Name = "targeting";
            this.targeting.Size = new System.Drawing.Size(71, 17);
            this.targeting.TabIndex = 27;
            this.targeting.Text = "Targeting";
            this.targeting.UseVisualStyleBackColor = true;
            // 
            // moveBackwards
            // 
            this.moveBackwards.AutoSize = true;
            this.moveBackwards.Location = new System.Drawing.Point(271, 85);
            this.moveBackwards.Name = "moveBackwards";
            this.moveBackwards.Size = new System.Drawing.Size(109, 17);
            this.moveBackwards.TabIndex = 28;
            this.moveBackwards.Text = "Move Backwards";
            this.moveBackwards.UseVisualStyleBackColor = true;
            // 
            // radioSapNever
            // 
            this.radioSapNever.AutoSize = true;
            this.radioSapNever.Location = new System.Drawing.Point(4, 3);
            this.radioSapNever.Name = "radioSapNever";
            this.radioSapNever.Size = new System.Drawing.Size(54, 17);
            this.radioSapNever.TabIndex = 30;
            this.radioSapNever.TabStop = true;
            this.radioSapNever.Text = "Never";
            this.radioSapNever.UseVisualStyleBackColor = true;
            // 
            // panelSap
            // 
            this.panelSap.Controls.Add(this.radioSapTarget);
            this.panelSap.Controls.Add(this.radioSapAdds);
            this.panelSap.Controls.Add(this.radioSapNever);
            this.panelSap.Location = new System.Drawing.Point(164, 158);
            this.panelSap.Name = "panelSap";
            this.panelSap.Size = new System.Drawing.Size(62, 69);
            this.panelSap.TabIndex = 31;
            // 
            // radioSapTarget
            // 
            this.radioSapTarget.AutoSize = true;
            this.radioSapTarget.Location = new System.Drawing.Point(3, 48);
            this.radioSapTarget.Name = "radioSapTarget";
            this.radioSapTarget.Size = new System.Drawing.Size(56, 17);
            this.radioSapTarget.TabIndex = 32;
            this.radioSapTarget.TabStop = true;
            this.radioSapTarget.Text = "Target";
            this.radioSapTarget.UseVisualStyleBackColor = true;
            // 
            // radioSapAdds
            // 
            this.radioSapAdds.AutoSize = true;
            this.radioSapAdds.Location = new System.Drawing.Point(4, 26);
            this.radioSapAdds.Name = "radioSapAdds";
            this.radioSapAdds.Size = new System.Drawing.Size(49, 17);
            this.radioSapAdds.TabIndex = 31;
            this.radioSapAdds.TabStop = true;
            this.radioSapAdds.Text = "Adds";
            this.radioSapAdds.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(160, 131);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(43, 24);
            this.label11.TabIndex = 32;
            this.label11.Text = "Sap";
            // 
            // feint
            // 
            this.feint.AutoSize = true;
            this.feint.Location = new System.Drawing.Point(403, 154);
            this.feint.Name = "feint";
            this.feint.Size = new System.Drawing.Size(49, 17);
            this.feint.TabIndex = 33;
            this.feint.Text = "Feint";
            this.feint.UseVisualStyleBackColor = true;
            // 
            // swPick
            // 
            this.swPick.AutoSize = true;
            this.swPick.Location = new System.Drawing.Point(271, 131);
            this.swPick.Name = "swPick";
            this.swPick.Size = new System.Drawing.Size(114, 17);
            this.swPick.TabIndex = 34;
            this.swPick.Text = "Stop/Wait for Pick";
            this.swPick.UseVisualStyleBackColor = true;
            // 
            // FoKPull
            // 
            this.FoKPull.AutoSize = true;
            this.FoKPull.Location = new System.Drawing.Point(403, 177);
            this.FoKPull.Name = "FoKPull";
            this.FoKPull.Size = new System.Drawing.Size(65, 17);
            this.FoKPull.TabIndex = 35;
            this.FoKPull.Text = "FoK Pull";
            this.FoKPull.UseVisualStyleBackColor = true;
            // 
            // neverStealth
            // 
            this.neverStealth.AutoSize = true;
            this.neverStealth.Location = new System.Drawing.Point(403, 63);
            this.neverStealth.Name = "neverStealth";
            this.neverStealth.Size = new System.Drawing.Size(91, 17);
            this.neverStealth.TabIndex = 36;
            this.neverStealth.Text = "Never Stealth";
            this.neverStealth.UseVisualStyleBackColor = true;
            // 
            // distract
            // 
            this.distract.AutoSize = true;
            this.distract.Location = new System.Drawing.Point(403, 201);
            this.distract.Name = "distract";
            this.distract.Size = new System.Drawing.Size(93, 43);
            this.distract.TabIndex = 37;
            this.distract.Text = "Distract after\r\n20 ticks\r\nwithout pulling";
            this.distract.UseVisualStyleBackColor = true;
            // 
            // vanish
            // 
            this.vanish.AutoSize = true;
            this.vanish.Location = new System.Drawing.Point(403, 248);
            this.vanish.Name = "vanish";
            this.vanish.Size = new System.Drawing.Size(76, 30);
            this.vanish.TabIndex = 38;
            this.vanish.Text = "Vanish on \r\nLow Life";
            this.vanish.UseVisualStyleBackColor = true;
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(502, 403);
            this.Controls.Add(this.vanish);
            this.Controls.Add(this.distract);
            this.Controls.Add(this.neverStealth);
            this.Controls.Add(this.FoKPull);
            this.Controls.Add(this.swPick);
            this.Controls.Add(this.feint);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.panelSap);
            this.Controls.Add(this.moveBackwards);
            this.Controls.Add(this.targeting);
            this.Controls.Add(this.crowdControl);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.alwaysStealth);
            this.Controls.Add(this.pickPocket);
            this.Controls.Add(this.moveBehind);
            this.Controls.Add(this.aoe);
            this.Controls.Add(this.movement);
            this.Controls.Add(this.panelLevelPoison);
            this.Controls.Add(this.panelDungeonPoison);
            this.Controls.Add(this.panelBgPoison);
            this.Controls.Add(this.panelArenaPoison);
            this.Controls.Add(this.checkBoxLevelPoison);
            this.Controls.Add(this.checkBoxBgPoison);
            this.Controls.Add(this.checkBoxDungeonPoison);
            this.Controls.Add(this.checkBoxArenaPoison);
            this.Controls.Add(this.checkBoxRaidPoison);
            this.Controls.Add(this.panelRaidPoison);
            this.Controls.Add(this.labelApplyPoisons);
            this.Controls.Add(this.panelCooldowns);
            this.Controls.Add(this.labelUseCooldowns);
            this.Controls.Add(this.buttonLaunchCombatControl);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonApply);
            this.Controls.Add(this.radioButtonHolderPanel);
            this.Controls.Add(this.labelModePrompt);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Config";
            this.Text = "RogueBT Settings";
            this.Load += new System.EventHandler(this.Config_Load);
            this.panelCooldowns.ResumeLayout(false);
            this.panelCooldowns.PerformLayout();
            this.radioButtonHolderPanel.ResumeLayout(false);
            this.radioButtonHolderPanel.PerformLayout();
            this.panelRaidPoison.ResumeLayout(false);
            this.panelRaidPoison.PerformLayout();
            this.panelArenaPoison.ResumeLayout(false);
            this.panelArenaPoison.PerformLayout();
            this.panelBgPoison.ResumeLayout(false);
            this.panelBgPoison.PerformLayout();
            this.panelDungeonPoison.ResumeLayout(false);
            this.panelDungeonPoison.PerformLayout();
            this.panelLevelPoison.ResumeLayout(false);
            this.panelLevelPoison.PerformLayout();
            this.panelSap.ResumeLayout(false);
            this.panelSap.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonLaunchCombatControl;
        private System.Windows.Forms.Label labelUseCooldowns;
        private System.Windows.Forms.Panel panelCooldowns;
        private System.Windows.Forms.RadioButton radioCooldownNever;
        private System.Windows.Forms.RadioButton radioCooldownByBoss;
        private System.Windows.Forms.RadioButton radioCooldownByFocus;
        private System.Windows.Forms.RadioButton radioCooldownAlways;
        private System.Windows.Forms.Label labelModePrompt;
        private System.Windows.Forms.RadioButton radioButtonDungeon;
        private System.Windows.Forms.RadioButton radioButtonArena;
        private System.Windows.Forms.RadioButton radioButtonRaid;
        private System.Windows.Forms.RadioButton radioButtonAuto;
        private System.Windows.Forms.RadioButton radioButtonBattleground;
        private System.Windows.Forms.Panel radioButtonHolderPanel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.RadioButton radioButtonLevel;
        private System.Windows.Forms.Label labelApplyPoisons;
        private System.Windows.Forms.Panel panelRaidPoison;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxRaidPoison2;
        private System.Windows.Forms.Label labelRaidPoisonMain;
        private System.Windows.Forms.ComboBox comboBoxRaidPoison1;
        private System.Windows.Forms.CheckBox checkBoxRaidPoison;
        private System.Windows.Forms.Panel panelArenaPoison;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxArenaPoison2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBoxArenaPoison1;
        private System.Windows.Forms.CheckBox checkBoxArenaPoison;
        private System.Windows.Forms.Panel panelBgPoison;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxBgPoison2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxBgPoison1;
        private System.Windows.Forms.CheckBox checkBoxBgPoison;
        private System.Windows.Forms.Panel panelDungeonPoison;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxDungeonPoison2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxDungeonPoison1;
        private System.Windows.Forms.CheckBox checkBoxDungeonPoison;
        private System.Windows.Forms.Panel panelLevelPoison;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox comboBoxLevelPoison2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox comboBoxLevelPoison1;
        private System.Windows.Forms.CheckBox checkBoxLevelPoison;
        private System.Windows.Forms.CheckBox alwaysStealth;
        private System.Windows.Forms.CheckBox pickPocket;
        private System.Windows.Forms.CheckBox moveBehind;
        private System.Windows.Forms.CheckBox aoe;
        private System.Windows.Forms.CheckBox movement;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox crowdControl;
        private System.Windows.Forms.CheckBox targeting;
        private System.Windows.Forms.CheckBox moveBackwards;
        private System.Windows.Forms.RadioButton radioSapNever;
        private System.Windows.Forms.Panel panelSap;
        private System.Windows.Forms.RadioButton radioSapTarget;
        private System.Windows.Forms.RadioButton radioSapAdds;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox feint;
        private System.Windows.Forms.CheckBox swPick;
        private System.Windows.Forms.CheckBox FoKPull;
        private System.Windows.Forms.CheckBox neverStealth;
        private System.Windows.Forms.CheckBox distract;
        private System.Windows.Forms.CheckBox vanish;
    }
}