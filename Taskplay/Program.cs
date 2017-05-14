using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Taskplay
{
    static class Program
    {
        static bool _isMusicPlaying = false;    // Bool to keep in check if the user is playing music

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Create system tray icons
            NotifyIcon previousIcon = new NotifyIcon();
            NotifyIcon playIcon = new NotifyIcon();
            NotifyIcon nextIcon = new NotifyIcon();
            //Create the context menu and its items
            ContextMenu contextMenu = new ContextMenu();
            MenuItem contextItemSettings = new MenuItem();
            MenuItem contextItemExit = new MenuItem();
            //Setup the context menu items
            contextItemSettings.Text = "&Settings";
            contextItemExit.Text = "&Exit";
            contextItemSettings.Click += new EventHandler(contextMenuSettings_Click);
            contextItemExit.Click += new EventHandler(contextMenuExit_Click);
            //Add the context menu items to the context menu
            contextMenu.MenuItems.Add(contextItemSettings);
            contextMenu.MenuItems.Add(contextItemExit);
            //Setup nextIcon
            nextIcon.Icon = Properties.Resources.Forward;
            nextIcon.Text = "Next";
            nextIcon.Visible = true;
            nextIcon.MouseClick += new MouseEventHandler(nextIcon_MouseClick);
            nextIcon.ContextMenu = contextMenu;
            //Setup playIcon
            playIcon.Icon = Properties.Resources.Play;
            playIcon.Text = "Play / Pause";
            playIcon.Visible = true;
            playIcon.MouseClick += new MouseEventHandler(playIcon_MouseClick);
            playIcon.ContextMenu = contextMenu;
            //Setup previousIcon
            previousIcon.Icon = Properties.Resources.Backward;
            previousIcon.Text = "Previous";
            previousIcon.Visible = true;
            previousIcon.MouseClick += new MouseEventHandler(previousIcon_MouseClick);
            previousIcon.ContextMenu = contextMenu;

            //Launch
            Application.Run();
        }

        private static void previousIcon_MouseClick(object sender, MouseEventArgs e)
        {
            //Send Media Key Previous Track
            if (e.Button == MouseButtons.Left)
            {
                keybd_event(0xB1, 0, 0x0001, IntPtr.Zero);
                keybd_event(0xB1, 0, 0x0002, IntPtr.Zero);
            }
        }

        private static void playIcon_MouseClick(object sender, MouseEventArgs e)
        {
            //Send Media Key Play
            if (e.Button == MouseButtons.Left)
            {
                keybd_event(0xB3, 0, 0x0001, IntPtr.Zero);
                keybd_event(0xB3, 0, 0x0002, IntPtr.Zero);

                // Get the Play-button
                var playIcon = (NotifyIcon)sender;

                if (_isMusicPlaying == false)
                {
                    // Start playing music and show the pause-icon
                    playIcon.Icon = Properties.Resources.Pause;
                    _isMusicPlaying = true;
                }
                else
                {
                    // Pause the music and display the Play-icon
                    playIcon.Icon = Properties.Resources.Play;
                    _isMusicPlaying = false;
                }
            }

            // PLEASE NOTE; this method does NOT check whether the music has been paused by an external source.
            //  This could be added by building in an check that listens to the state of the systems MusicPlayer
            //   I hope this gives a nice base to start with. :)
        }

        private static void nextIcon_MouseClick(object sender, MouseEventArgs e)
        {
            //Send Media Key Next Track
            if (e.Button == MouseButtons.Left)
            {
                keybd_event(0xB0, 0, 0x0001, IntPtr.Zero);
                keybd_event(0xB0, 0, 0x0002, IntPtr.Zero);
            }
        }

        private static void contextMenuSettings_Click(object sender, System.EventArgs e)
        {
            //Show Settings form
            new SettingsForm().ShowDialog();
        }

        private static void contextMenuExit_Click(object sender, System.EventArgs e)
        {
            //Exit the app
            Application.Exit();
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);
    }
}
