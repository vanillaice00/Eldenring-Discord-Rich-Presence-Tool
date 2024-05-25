
using IWshRuntimeLibrary;
using System.Diagnostics;
using System.Windows.Forms;

namespace EldenRingDiscordPresence
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            gameStatusTitleLabel = new Label();
            toggleStartOnStartup = new CheckBox();
            delayComboBox = new ComboBox();
            timeIntervalLabel = new Label();
            notifyIcon = new NotifyIcon(components);
            statusLabel = new Label();
            graceIdTitle = new Label();
            graceID = new Label();
            showTimeElapsed = new CheckBox();
            presenceSettings = new Label();
            showImages = new CheckBox();
            showGraceName = new CheckBox();
            SuspendLayout();
            // 
            // gameStatusTitleLabel
            // 
            gameStatusTitleLabel.AutoSize = true;
            gameStatusTitleLabel.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            gameStatusTitleLabel.Location = new Point(5, 7);
            gameStatusTitleLabel.Name = "gameStatusTitleLabel";
            gameStatusTitleLabel.Size = new Size(74, 30);
            gameStatusTitleLabel.TabIndex = 1;
            gameStatusTitleLabel.Text = "Status:";
            // 
            // toggleStartOnStartup
            // 
            toggleStartOnStartup.AutoSize = true;
            toggleStartOnStartup.Location = new Point(5, 217);
            toggleStartOnStartup.Name = "toggleStartOnStartup";
            toggleStartOnStartup.Size = new Size(187, 19);
            toggleStartOnStartup.TabIndex = 3;
            toggleStartOnStartup.Text = "Start minimized with Windows";
            toggleStartOnStartup.UseVisualStyleBackColor = true;
            toggleStartOnStartup.CheckedChanged += toggleStartOnStartupCheckedChange;
            // 
            // delayComboBox
            // 
            delayComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            delayComboBox.FormattingEnabled = true;
            delayComboBox.Items.AddRange(new object[] { "10 Seconds", "30 Seconds", "60 Seconds" });
            delayComboBox.Location = new Point(94, 143);
            delayComboBox.Name = "delayComboBox";
            delayComboBox.Size = new Size(124, 23);
            delayComboBox.TabIndex = 4;
            delayComboBox.SelectedIndexChanged += delayChanged;
            // 
            // timeIntervalLabel
            // 
            timeIntervalLabel.AutoSize = true;
            timeIntervalLabel.Location = new Point(12, 146);
            timeIntervalLabel.Name = "timeIntervalLabel";
            timeIntervalLabel.Size = new Size(76, 15);
            timeIntervalLabel.TabIndex = 5;
            timeIntervalLabel.Text = "Update every";
            // 
            // notifyIcon
            // 
            notifyIcon.Icon = (Icon)resources.GetObject("notifyIcon.Icon");
            notifyIcon.Text = "ELDEN RING Discord Presence (Click to show)";
            notifyIcon.MouseClick += notifyIconClick;
            // 
            // statusLabel
            // 
            statusLabel.AutoSize = true;
            statusLabel.BackColor = Color.Transparent;
            statusLabel.Font = new Font("Segoe UI", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            statusLabel.ForeColor = Color.DarkGoldenrod;
            statusLabel.Location = new Point(70, 7);
            statusLabel.Name = "statusLabel";
            statusLabel.Size = new Size(223, 30);
            statusLabel.TabIndex = 6;
            statusLabel.Text = "WAITING FOR GAME...";
            // 
            // graceIdTitle
            // 
            graceIdTitle.AutoSize = true;
            graceIdTitle.Location = new Point(302, 206);
            graceIdTitle.Name = "graceIdTitle";
            graceIdTitle.Size = new Size(97, 15);
            graceIdTitle.TabIndex = 7;
            graceIdTitle.Text = "Current Grace ID:";
            // 
            // graceID
            // 
            graceID.AutoSize = true;
            graceID.Location = new Point(302, 221);
            graceID.Name = "graceID";
            graceID.Size = new Size(58, 15);
            graceID.TabIndex = 8;
            graceID.Text = "Unknown";
            // 
            // showTimeElapsed
            // 
            showTimeElapsed.AutoSize = true;
            showTimeElapsed.Location = new Point(12, 74);
            showTimeElapsed.Name = "showTimeElapsed";
            showTimeElapsed.Size = new Size(127, 19);
            showTimeElapsed.TabIndex = 9;
            showTimeElapsed.Text = "Show elapsed Time";
            showTimeElapsed.UseVisualStyleBackColor = true;
            showTimeElapsed.CheckedChanged += timeElapsedChanged;
            // 
            // presenceSettings
            // 
            presenceSettings.AutoSize = true;
            presenceSettings.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            presenceSettings.Location = new Point(5, 46);
            presenceSettings.Name = "presenceSettings";
            presenceSettings.Size = new Size(186, 25);
            presenceSettings.TabIndex = 10;
            presenceSettings.Text = "- Presence Settings -";
            // 
            // showImages
            // 
            showImages.AutoSize = true;
            showImages.Location = new Point(12, 99);
            showImages.Name = "showImages";
            showImages.Size = new Size(121, 19);
            showImages.TabIndex = 11;
            showImages.Text = "Show area images";
            showImages.UseVisualStyleBackColor = true;
            showImages.CheckedChanged += imageCheckChanged;
            // 
            // showGraceName
            // 
            showGraceName.AutoSize = true;
            showGraceName.Location = new Point(12, 124);
            showGraceName.Name = "showGraceName";
            showGraceName.Size = new Size(166, 19);
            showGraceName.TabIndex = 12;
            showGraceName.Text = "Show grace location name";
            showGraceName.UseVisualStyleBackColor = true;
            showGraceName.CheckedChanged += graceLocationChecked;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(411, 241);
            Controls.Add(showGraceName);
            Controls.Add(showImages);
            Controls.Add(presenceSettings);
            Controls.Add(showTimeElapsed);
            Controls.Add(graceID);
            Controls.Add(graceIdTitle);
            Controls.Add(statusLabel);
            Controls.Add(timeIntervalLabel);
            Controls.Add(delayComboBox);
            Controls.Add(toggleStartOnStartup);
            Controls.Add(gameStatusTitleLabel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            Text = "ELDEN RING - Discord Presence";
            Load += FromLoad;
            Resize += MainForm_Resize;
            ResumeLayout(false);
            PerformLayout();
        }

        private void graceLocationChecked(object sender, EventArgs e)
        {
            Program.ConfigurationManager.CurrentConfiguration.ShowGraceLocationName = showGraceName.Checked;
            Program.ConfigurationManager.updateConfigurationFile();
        }

        private void imageCheckChanged(object sender, EventArgs e)
        {
            Program.ConfigurationManager.CurrentConfiguration.ShowAreaImages = showImages.Checked;
            Program.ConfigurationManager.updateConfigurationFile();
        }

        private void timeElapsedChanged(object sender, EventArgs e)
        {
            Program.ConfigurationManager.CurrentConfiguration.ShowElapsedTime = showTimeElapsed.Checked;
            Program.ConfigurationManager.updateConfigurationFile();
        }

        private void FromLoad(object sender, EventArgs e)
        {
            delayComboBox.SelectedIndex = Program.ConfigurationManager.CurrentConfiguration.UpdateDelay;
            toggleStartOnStartup.Checked = Program.ConfigurationManager.CurrentConfiguration.StartWithWindows;
            showImages.Checked = Program.ConfigurationManager.CurrentConfiguration.ShowAreaImages;
            showTimeElapsed.Checked = Program.ConfigurationManager.CurrentConfiguration.ShowElapsedTime;
            showGraceName.Checked = Program.ConfigurationManager.CurrentConfiguration.ShowGraceLocationName;


        }

        private void delayChanged(object sender, EventArgs e)
        {
            Program.ConfigurationManager.CurrentConfiguration.UpdateDelay = delayComboBox.SelectedIndex;
            Program.ConfigurationManager.updateConfigurationFile();
            Program.StopTimer();
            Program.StartTimer();

        }

        private void toggleStartOnStartupCheckedChange(object sender, EventArgs e)
        {
            Program.ConfigurationManager.CurrentConfiguration.StartWithWindows = toggleStartOnStartup.Checked;
            Program.ConfigurationManager.updateConfigurationFile();

            string shortcutPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft\\Windows\\Start Menu\\Programs\\Startup\\EldenRingDiscordPresence.lnk");

            if (System.IO.File.Exists(shortcutPath) && !toggleStartOnStartup.Checked)
            {
                System.IO.File.Delete(shortcutPath);
            }
            else if(toggleStartOnStartup.Checked)
            {

                object shortCut = (object)"EldenRingDiscordPresence";
                WshShell shell = new WshShell();
                string shortcutAddress = (string)shell.SpecialFolders.Item(ref shortCut) + shortcutPath;
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                shortcut.TargetPath = Application.ExecutablePath;
                shortcut.WorkingDirectory = Application.StartupPath;
                shortcut.WindowStyle = 7;
                shortcut.Save();
            }
        }

        private void notifyIconClick(object sender, MouseEventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            this.Show();
            this.WindowState = FormWindowState.Normal;

            notifyIcon.Visible = false;
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
         
                if (FormWindowState.Minimized == this.WindowState)
                {
                notifyIcon.Visible = true;
                this.Hide();
                }
            
        }

        #endregion
        private Label gameStatusTitleLabel;
        private CheckBox toggleStartOnStartup;
        private ComboBox delayComboBox;
        private Label timeIntervalLabel;
        private NotifyIcon notifyIcon;
        private Label statusLabel;
        private Label graceIdTitle;
        private Label graceID;
        private CheckBox showTimeElapsed;
        private Label presenceSettings;
        private CheckBox showImages;
        private CheckBox showGraceName;
    }
}
