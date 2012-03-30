//////////////////////////////////////////////////
//             Config.Designer.cs               //
//        Part of PallyRaidBT by kaihaider      //
//////////////////////////////////////////////////
//   Originally from MutaRaidBT by fiftypence.  //
//    Reused with permission from the author.   //
//////////////////////////////////////////////////
namespace PallyRaidBT.UI
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
            this.radioButtonHeroicDungeon = new System.Windows.Forms.RadioButton();
            this.radioButtonRaid = new System.Windows.Forms.RadioButton();
            this.radioButtonAuto = new System.Windows.Forms.RadioButton();
            this.radioButtonBattleground = new System.Windows.Forms.RadioButton();
            this.radioButtonHolderPanel = new System.Windows.Forms.Panel();
            this.radioButtonLevel = new System.Windows.Forms.RadioButton();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.labelUseMovement = new System.Windows.Forms.Label();
            this.panelMovement = new System.Windows.Forms.Panel();
            this.radioButtonMoveOff = new System.Windows.Forms.RadioButton();
            this.radioButtonMoveOn = new System.Windows.Forms.RadioButton();
            this.panelUseAoe = new System.Windows.Forms.Panel();
            this.radioButtonAoeOff = new System.Windows.Forms.RadioButton();
            this.radioButtonAoeOn = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.panelCooldowns.SuspendLayout();
            this.radioButtonHolderPanel.SuspendLayout();
            this.panelMovement.SuspendLayout();
            this.panelUseAoe.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(12, 361);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(95, 23);
            this.buttonApply.TabIndex = 2;
            this.buttonApply.Text = "Apply and Close";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(309, 361);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonLaunchCombatControl
            // 
            this.buttonLaunchCombatControl.Location = new System.Drawing.Point(143, 361);
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
            this.panelCooldowns.Size = new System.Drawing.Size(103, 96);
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
            // radioButtonHeroicDungeon
            // 
            this.radioButtonHeroicDungeon.AutoSize = true;
            this.radioButtonHeroicDungeon.Location = new System.Drawing.Point(3, 49);
            this.radioButtonHeroicDungeon.Name = "radioButtonHeroicDungeon";
            this.radioButtonHeroicDungeon.Size = new System.Drawing.Size(132, 17);
            this.radioButtonHeroicDungeon.TabIndex = 1;
            this.radioButtonHeroicDungeon.TabStop = true;
            this.radioButtonHeroicDungeon.Text = "Heroic Dungeon mode";
            this.radioButtonHeroicDungeon.UseVisualStyleBackColor = true;
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
            this.radioButtonHolderPanel.Controls.Add(this.radioButtonHeroicDungeon);
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
            this.radioButtonLevel.Size = new System.Drawing.Size(80, 17);
            this.radioButtonLevel.TabIndex = 5;
            this.radioButtonLevel.TabStop = true;
            this.radioButtonLevel.Text = "Level mode";
            this.radioButtonLevel.UseVisualStyleBackColor = true;
            // 
            // labelUseMovement
            // 
            this.labelUseMovement.AutoSize = true;
            this.labelUseMovement.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUseMovement.Location = new System.Drawing.Point(287, 9);
            this.labelUseMovement.Name = "labelUseMovement";
            this.labelUseMovement.Size = new System.Drawing.Size(99, 24);
            this.labelUseMovement.TabIndex = 7;
            this.labelUseMovement.Text = "Movement";
            // 
            // panelMovement
            // 
            this.panelMovement.Controls.Add(this.radioButtonMoveOff);
            this.panelMovement.Controls.Add(this.radioButtonMoveOn);
            this.panelMovement.Location = new System.Drawing.Point(291, 36);
            this.panelMovement.Name = "panelMovement";
            this.panelMovement.Size = new System.Drawing.Size(51, 55);
            this.panelMovement.TabIndex = 8;
            // 
            // radioButtonMoveOff
            // 
            this.radioButtonMoveOff.AutoSize = true;
            this.radioButtonMoveOff.Location = new System.Drawing.Point(3, 26);
            this.radioButtonMoveOff.Name = "radioButtonMoveOff";
            this.radioButtonMoveOff.Size = new System.Drawing.Size(39, 17);
            this.radioButtonMoveOff.TabIndex = 1;
            this.radioButtonMoveOff.TabStop = true;
            this.radioButtonMoveOff.Text = "Off";
            this.radioButtonMoveOff.UseVisualStyleBackColor = true;
            // 
            // radioButtonMoveOn
            // 
            this.radioButtonMoveOn.AutoSize = true;
            this.radioButtonMoveOn.Location = new System.Drawing.Point(3, 3);
            this.radioButtonMoveOn.Name = "radioButtonMoveOn";
            this.radioButtonMoveOn.Size = new System.Drawing.Size(39, 17);
            this.radioButtonMoveOn.TabIndex = 0;
            this.radioButtonMoveOn.TabStop = true;
            this.radioButtonMoveOn.Text = "On";
            this.radioButtonMoveOn.UseVisualStyleBackColor = true;
            // 
            // panelUseAoe
            // 
            this.panelUseAoe.Controls.Add(this.radioButtonAoeOff);
            this.panelUseAoe.Controls.Add(this.radioButtonAoeOn);
            this.panelUseAoe.Location = new System.Drawing.Point(291, 131);
            this.panelUseAoe.Name = "panelUseAoe";
            this.panelUseAoe.Size = new System.Drawing.Size(51, 55);
            this.panelUseAoe.TabIndex = 10;
            // 
            // radioButtonAoeOff
            // 
            this.radioButtonAoeOff.AutoSize = true;
            this.radioButtonAoeOff.Location = new System.Drawing.Point(3, 26);
            this.radioButtonAoeOff.Name = "radioButtonAoeOff";
            this.radioButtonAoeOff.Size = new System.Drawing.Size(39, 17);
            this.radioButtonAoeOff.TabIndex = 1;
            this.radioButtonAoeOff.TabStop = true;
            this.radioButtonAoeOff.Text = "Off";
            this.radioButtonAoeOff.UseVisualStyleBackColor = true;
            // 
            // radioButtonAoeOn
            // 
            this.radioButtonAoeOn.AutoSize = true;
            this.radioButtonAoeOn.Location = new System.Drawing.Point(3, 3);
            this.radioButtonAoeOn.Name = "radioButtonAoeOn";
            this.radioButtonAoeOn.Size = new System.Drawing.Size(39, 17);
            this.radioButtonAoeOn.TabIndex = 0;
            this.radioButtonAoeOn.TabStop = true;
            this.radioButtonAoeOn.Text = "On";
            this.radioButtonAoeOn.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(287, 104);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 24);
            this.label10.TabIndex = 9;
            this.label10.Text = "AoE";
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 389);
            this.Controls.Add(this.panelUseAoe);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panelMovement);
            this.Controls.Add(this.labelUseMovement);
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
            this.Text = "PallyRaidBT Settings";
            this.Load += new System.EventHandler(this.Config_Load);
            this.panelCooldowns.ResumeLayout(false);
            this.panelCooldowns.PerformLayout();
            this.radioButtonHolderPanel.ResumeLayout(false);
            this.radioButtonHolderPanel.PerformLayout();
            this.panelMovement.ResumeLayout(false);
            this.panelMovement.PerformLayout();
            this.panelUseAoe.ResumeLayout(false);
            this.panelUseAoe.PerformLayout();
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
        private System.Windows.Forms.RadioButton radioButtonHeroicDungeon;
        private System.Windows.Forms.RadioButton radioButtonRaid;
        private System.Windows.Forms.RadioButton radioButtonAuto;
        private System.Windows.Forms.RadioButton radioButtonBattleground;
        private System.Windows.Forms.Panel radioButtonHolderPanel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.RadioButton radioButtonLevel;
        private System.Windows.Forms.Label labelUseMovement;
        private System.Windows.Forms.Panel panelMovement;
        private System.Windows.Forms.RadioButton radioButtonMoveOff;
        private System.Windows.Forms.RadioButton radioButtonMoveOn;
        private System.Windows.Forms.Panel panelUseAoe;
        private System.Windows.Forms.RadioButton radioButtonAoeOff;
        private System.Windows.Forms.RadioButton radioButtonAoeOn;
        private System.Windows.Forms.Label label10;
    }
}