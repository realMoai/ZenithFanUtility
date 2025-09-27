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
            ApplyCurrentMode();
            this.Owner = owner; // Set the owner
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
                    // Fails on unsupported OS versions
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
            var outlineColor = Color.DarkGray; // Set the outline color to dark grey
            var linkLabelColor = Color.LightBlue; // Set link label color to light blue

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

        public void ApplyLightMode()
        {
            // Define light mode colors
            var backgroundColor = System.Drawing.Color.White;
            var foregroundColor = System.Drawing.Color.Black;
            var controlBackgroundColor = System.Drawing.Color.White;
            var trackBarBackgroundColor = System.Drawing.Color.White;
            var menuStripBackgroundColor = System.Drawing.Color.LightGray;
            var menuStripForegroundColor = System.Drawing.Color.Black;

            // Apply colors to the form
            this.BackColor = backgroundColor;
            this.ForeColor = foregroundColor;

            // Apply colors to controls
            foreach (Control control in this.Controls)
            {
                if (control is Label || control is Button || control is CheckBox || control is TextBox)
                {
                    control.BackColor = controlBackgroundColor;
                    control.ForeColor = foregroundColor;
                }
                else if (control is TrackBar)
                {
                    control.BackColor = trackBarBackgroundColor;
                }
                else if (control is LinkLabel linkLabel)
                {
                    linkLabel.LinkColor = foregroundColor;
                    linkLabel.ActiveLinkColor = foregroundColor;
                    linkLabel.VisitedLinkColor = foregroundColor;
                    linkLabel.BackColor = controlBackgroundColor;
                    linkLabel.ForeColor = foregroundColor;
                }
            }

            // Apply colors to the menu strip if it exists
            if (this.MainMenuStrip != null)
            {
                this.MainMenuStrip.BackColor = menuStripBackgroundColor;
                this.MainMenuStrip.ForeColor = menuStripForegroundColor;
            }
        }

        private void ApplyCurrentMode()
        {
            // Logic to determine and apply the current mode (dark or light)
            // For example, you can check a setting or a user preference
            bool isDarkMode = true; // This should be replaced with actual logic

            if (isDarkMode)
            {
                ApplyDarkMode();
            }
            else
            {
                ApplyLightMode();
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            // Apply the dark theme to the title bar first
            SetDarkModeTitleBar(this.Handle);

            // --- START: Self-Positioning Logic ---
            if (this.Owner != null)
            {
                // Set the start position to Manual to override any defaults
                this.StartPosition = FormStartPosition.Manual;

                // Define the gap between the forms
                int gap = 10;

                // Calculate the desired location above the owner (Form1)
                int newLeft = this.Owner.Left;
                int newTop = this.Owner.Top - this.Height - gap;

                // Failsafe: If there's no room on top, place it below instead
                if (newTop < Screen.FromControl(this).WorkingArea.Top)
                {
                    newTop = this.Owner.Bottom + gap;
                }

                // Set our own location
                this.Location = new Point(newLeft, newTop);
            }
            // --- END: Self-Positioning Logic ---
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

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

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
