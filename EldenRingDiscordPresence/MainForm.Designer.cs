
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
            toggleStartOnStartup.Location = new Point(12, 49);
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
            delayComboBox.Location = new Point(94, 71);
            delayComboBox.Name = "delayComboBox";
            delayComboBox.Size = new Size(124, 23);
            delayComboBox.TabIndex = 4;
            delayComboBox.SelectedIndexChanged += delayChanged;
            // 
            // timeIntervalLabel
            // 
            timeIntervalLabel.AutoSize = true;
            timeIntervalLabel.Location = new Point(12, 74);
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
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(411, 109);
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

        private void FromLoad(object sender, EventArgs e)
        {
            delayComboBox.SelectedIndex = Program.ConfigurationManager.CurrentConfiguration.UpdateDelay;
            toggleStartOnStartup.Checked = Program.ConfigurationManager.CurrentConfiguration.StartWithWindows;

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
    }
}
