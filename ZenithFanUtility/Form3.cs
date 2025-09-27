using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZenithFanUtility
{
    public partial class Form3 : Form
    {
        public Form3(Form owner)
        {
            InitializeComponent();
            ApplyDarkMode();
            this.Owner = owner;
        }
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;

        private static bool SetDarkModeTitleBar(IntPtr handle)
        {
            if (Environment.OSVersion.Version.Major >= 10)
            {
                try
                {
                    int enabled = 1;
                    DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref enabled, sizeof(int));
                    DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, ref enabled, sizeof(int));
                    return true;
                }
                catch
                {
                    // fails on unsupported OS versions
                }
            }
            return false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void ApplyDarkMode()
        {
            var backgroundColor = Color.FromArgb(28, 28, 28);
            var foregroundColor = Color.White;
            var controlBackgroundColor = Color.FromArgb(30, 30, 30);
            var trackBarBackgroundColor = Color.FromArgb(30, 30, 30);
            var outlineColor = Color.DarkGray;
            var linkLabelColor = Color.LightBlue;

            this.BackColor = backgroundColor;
            this.ForeColor = foregroundColor;

            foreach (Control control in this.Controls)
            {
                if (control is Label || control is CheckBox || control is System.Windows.Forms.TextBox)
                {
                    control.BackColor = controlBackgroundColor;
                    control.ForeColor = foregroundColor;
                }
                else if (control is System.Windows.Forms.TrackBar)
                {
                    control.BackColor = trackBarBackgroundColor;
                }
                else if (control is System.Windows.Forms.Button button)
                {
                    button.BackColor = controlBackgroundColor;
                    button.ForeColor = foregroundColor;
                    button.FlatAppearance.BorderColor = outlineColor;
                    button.FlatAppearance.BorderSize = 1;
                    button.FlatStyle = FlatStyle.Flat;
                }
                else if (control is LinkLabel linkLabel)
                {
                    linkLabel.LinkColor = linkLabelColor;
                    linkLabel.ActiveLinkColor = linkLabelColor;
                    linkLabel.VisitedLinkColor = linkLabelColor;
                    linkLabel.BackColor = controlBackgroundColor;
                    linkLabel.ForeColor = foregroundColor;
                }
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // Apply the dark theme to the title bar first
            SetDarkModeTitleBar(this.Handle);

            // self positioning logic
            if (this.Owner != null)
            {
                this.StartPosition = FormStartPosition.Manual;
                int gap = 10;
                int newLeft = this.Owner.Left;
                int newTop = this.Owner.Top - this.Height - gap;

                // failsafe, if theres no room on top, place it below instead
                if (newTop < Screen.FromControl(this).WorkingArea.Top)
                {
                    newTop = this.Owner.Bottom + gap;
                }
                this.Location = new Point(newLeft, newTop);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Karmel0x/AsusFanControl");
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/Darren80/AsusFanControlEnhanced");
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/pgain88/AsusFanControlRemastered");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/realMoai");
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://discord.com/users/622764600475254784");
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:d44330019@gmail.com");
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.buymeacoffee.com/zenithfanutility");
        }
    }
}
