//////////////////////////////////////////////////
//             Config.Designer.cs               //
//      Part of PallyRaidBT by fiftypence        //
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioCooldownNever = new System.Windows.Forms.RadioButton();
            this.radioCooldownByBoss = new System.Windows.Forms.RadioButton();
            this.radioCooldownByFocus = new System.Windows.Forms.RadioButton();
            this.radioCooldownAlways = new System.Windows.Forms.RadioButton();
            this.labelModePrompt = new System.Windows.Forms.Label();
            this.radioButtonLevel = new System.Windows.Forms.RadioButton();
            this.radioButtonDungeon = new System.Windows.Forms.RadioButton();
            this.radioButtonRaid = new System.Windows.Forms.RadioButton();
            this.radioButtonPvpMoveOff = new System.Windows.Forms.RadioButton();
            this.radioButtonPvpMoveOn = new System.Windows.Forms.RadioButton();
            this.radioButtonHolderPanel = new System.Windows.Forms.Panel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1.SuspendLayout();
            this.radioButtonHolderPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonApply
            // 
            this.buttonApply.Location = new System.Drawing.Point(9, 148);
            this.buttonApply.Name = "buttonApply";
            this.buttonApply.Size = new System.Drawing.Size(95, 23);
            this.buttonApply.TabIndex = 2;
            this.buttonApply.Text = "Apply and Close";
            this.buttonApply.UseVisualStyleBackColor = true;
            this.buttonApply.Click += new System.EventHandler(this.buttonApply_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(252, 148);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonLaunchCombatControl
            // 
            this.buttonLaunchCombatControl.Location = new System.Drawing.Point(110, 148);
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
            this.labelUseCooldowns.Location = new System.Drawing.Point(12, 9);
            this.labelUseCooldowns.Name = "labelUseCooldowns";
            this.labelUseCooldowns.Size = new System.Drawing.Size(145, 24);
            this.labelUseCooldowns.TabIndex = 5;
            this.labelUseCooldowns.Text = "Use cooldowns:";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioCooldownNever);
            this.panel1.Controls.Add(this.radioCooldownByBoss);
            this.panel1.Controls.Add(this.radioCooldownByFocus);
            this.panel1.Controls.Add(this.radioCooldownAlways);
            this.panel1.Location = new System.Drawing.Point(34, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(123, 96);
            this.panel1.TabIndex = 6;
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
            this.labelModePrompt.Location = new System.Drawing.Point(27, 269);
            this.labelModePrompt.Name = "labelModePrompt";
            this.labelModePrompt.Size = new System.Drawing.Size(276, 24);
            this.labelModePrompt.TabIndex = 0;
            this.labelModePrompt.Text = "Set my mode to (non functional)";
            // 
            // radioButtonLevel
            // 
            this.radioButtonLevel.AutoSize = true;
            this.radioButtonLevel.Location = new System.Drawing.Point(3, 3);
            this.radioButtonLevel.Name = "radioButtonLevel";
            this.radioButtonLevel.Size = new System.Drawing.Size(95, 17);
            this.radioButtonLevel.TabIndex = 4;
            this.radioButtonLevel.TabStop = true;
            this.radioButtonLevel.Text = "Leveling Mode";
            this.radioButtonLevel.UseVisualStyleBackColor = true;
            // 
            // radioButtonDungeon
            // 
            this.radioButtonDungeon.AutoSize = true;
            this.radioButtonDungeon.Location = new System.Drawing.Point(3, 72);
            this.radioButtonDungeon.Name = "radioButtonDungeon";
            this.radioButtonDungeon.Size = new System.Drawing.Size(99, 17);
            this.radioButtonDungeon.TabIndex = 1;
            this.radioButtonDungeon.TabStop = true;
            this.radioButtonDungeon.Text = "Dungeon Mode";
            this.radioButtonDungeon.UseVisualStyleBackColor = true;
            // 
            // radioButtonRaid
            // 
            this.radioButtonRaid.AutoSize = true;
            this.radioButtonRaid.Location = new System.Drawing.Point(3, 95);
            this.radioButtonRaid.Name = "radioButtonRaid";
            this.radioButtonRaid.Size = new System.Drawing.Size(77, 17);
            this.radioButtonRaid.TabIndex = 0;
            this.radioButtonRaid.TabStop = true;
            this.radioButtonRaid.Text = "Raid Mode";
            this.radioButtonRaid.UseVisualStyleBackColor = true;
            // 
            // radioButtonPvpMoveOff
            // 
            this.radioButtonPvpMoveOff.AutoSize = true;
            this.radioButtonPvpMoveOff.Location = new System.Drawing.Point(3, 49);
            this.radioButtonPvpMoveOff.Name = "radioButtonPvpMoveOff";
            this.radioButtonPvpMoveOff.Size = new System.Drawing.Size(148, 17);
            this.radioButtonPvpMoveOff.TabIndex = 2;
            this.radioButtonPvpMoveOff.TabStop = true;
            this.radioButtonPvpMoveOff.Text = "PvP Mode (movement off)";
            this.radioButtonPvpMoveOff.UseVisualStyleBackColor = true;
            // 
            // radioButtonPvpMoveOn
            // 
            this.radioButtonPvpMoveOn.AutoSize = true;
            this.radioButtonPvpMoveOn.Location = new System.Drawing.Point(3, 26);
            this.radioButtonPvpMoveOn.Name = "radioButtonPvpMoveOn";
            this.radioButtonPvpMoveOn.Size = new System.Drawing.Size(148, 17);
            this.radioButtonPvpMoveOn.TabIndex = 3;
            this.radioButtonPvpMoveOn.TabStop = true;
            this.radioButtonPvpMoveOn.Text = "PvP Mode (movement on)";
            this.radioButtonPvpMoveOn.UseVisualStyleBackColor = true;
            // 
            // radioButtonHolderPanel
            // 
            this.radioButtonHolderPanel.Controls.Add(this.radioButtonPvpMoveOn);
            this.radioButtonHolderPanel.Controls.Add(this.radioButtonPvpMoveOff);
            this.radioButtonHolderPanel.Controls.Add(this.radioButtonRaid);
            this.radioButtonHolderPanel.Controls.Add(this.radioButtonDungeon);
            this.radioButtonHolderPanel.Controls.Add(this.radioButtonLevel);
            this.radioButtonHolderPanel.Location = new System.Drawing.Point(46, 296);
            this.radioButtonHolderPanel.Name = "radioButtonHolderPanel";
            this.radioButtonHolderPanel.Size = new System.Drawing.Size(178, 116);
            this.radioButtonHolderPanel.TabIndex = 1;
            // 
            // Config
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 178);
            this.Controls.Add(this.panel1);
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
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.radioButtonHolderPanel.ResumeLayout(false);
            this.radioButtonHolderPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonApply;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonLaunchCombatControl;
        private System.Windows.Forms.Label labelUseCooldowns;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radioCooldownNever;
        private System.Windows.Forms.RadioButton radioCooldownByBoss;
        private System.Windows.Forms.RadioButton radioCooldownByFocus;
        private System.Windows.Forms.RadioButton radioCooldownAlways;
        private System.Windows.Forms.Label labelModePrompt;
        private System.Windows.Forms.RadioButton radioButtonLevel;
        private System.Windows.Forms.RadioButton radioButtonDungeon;
        private System.Windows.Forms.RadioButton radioButtonRaid;
        private System.Windows.Forms.RadioButton radioButtonPvpMoveOff;
        private System.Windows.Forms.RadioButton radioButtonPvpMoveOn;
        private System.Windows.Forms.Panel radioButtonHolderPanel;
        private System.Windows.Forms.Timer timer1;
    }
}