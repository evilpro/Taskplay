using System;
using System.Windows.Forms;

namespace Taskplay
{
    public partial class SettingsForm : Form
    {
        Microsoft.Win32.RegistryKey autorun;
        private readonly bool isDarkModeOn;
        private readonly Action<bool> restartAction;
        private bool isRestartNeeded = false;

        public SettingsForm(bool isDarkModeOn, Action<bool> restartAction)
        {
            InitializeComponent();
            autorun = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
            this.isDarkModeOn = isDarkModeOn;
            this.restartAction = restartAction;
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            labelVersion.Text = String.Format("Version {0}", Application.ProductVersion);
            checkBoxAutorun.Checked = (autorun.GetValue("Taskplay") != null);
            checkBoxDarkMode.Checked = isDarkModeOn;
        }

        private void SaveSettings()
        {
            if (checkBoxAutorun.Checked)
                autorun.SetValue("Taskplay", Application.ExecutablePath);
            else
                autorun.DeleteValue("Taskplay", false);

            var subKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Taskplay", true);
            subKey.SetValue("DarkMode", checkBoxDarkMode.Checked ? 1 : 0);

            isRestartNeeded = checkBoxDarkMode.Checked != isDarkModeOn;
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
