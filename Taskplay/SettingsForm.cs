using System;
using System.Windows.Forms;

namespace Taskplay
{
    public partial class SettingsForm : Form
    {
        Microsoft.Win32.RegistryKey autorun;
        Microsoft.Win32.RegistryKey settings;
        private readonly bool isDarkModeOn;
        private readonly bool areChangeSongButtonsShown;
        private readonly Action<bool> restartAction;
        private bool isRestartNeeded = false;

        public SettingsForm(bool isDarkModeOn, bool areChangeSongButtonsShown, Action<bool> restartAction)
        {
            InitializeComponent();
            autorun = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            settings = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Taskplay");
            this.isDarkModeOn = isDarkModeOn;
            this.restartAction = restartAction;
            this.areChangeSongButtonsShown = areChangeSongButtonsShown;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            labelVersion.Text = String.Format("Version {0}", Application.ProductVersion);
            checkBoxAutorun.Checked = (autorun.GetValue("Taskplay") != null);
            checkBoxDarkMode.Checked = isDarkModeOn;
            checkBoxShowSongChangeButtons.Checked = areChangeSongButtonsShown;
        }

        private void SaveSettings()
        {
            if (checkBoxAutorun.Checked)
                autorun.SetValue("Taskplay", Application.ExecutablePath);
            else
                autorun.DeleteValue("Taskplay", false);

            settings.SetValue("DarkMode", checkBoxDarkMode.Checked ? 1 : 0);
            settings.SetValue("ShowChangeSongButtons", checkBoxShowSongChangeButtons.Checked ? 1 : 0);

            isRestartNeeded = checkBoxDarkMode.Checked != isDarkModeOn
                || checkBoxShowSongChangeButtons.Checked != areChangeSongButtonsShown;
        }

        private void linkLabelGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/evilpro/Taskplay");
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            SaveSettings();
            Close();
            if (isRestartNeeded)
                restartAction(true);
        }
    }
}
