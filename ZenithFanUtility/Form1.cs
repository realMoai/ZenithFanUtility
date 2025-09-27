using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AsusFanControl;

namespace ZenithFanUtility
{
    public partial class Form1 : Form
    {
        private static Form1 instance;

        public static Form1 Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Form1(new string[0]);
                }
                return instance;
            }
        }

        [DllImport("dwmapi.dll")]
        private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attr, ref int attrValue, int attrSize);
        private bool isUpdatingStats = false;
        private ulong lastCpuTempValue = 0;
        private ulong lastGpuTempValue = 0;
        private string lastRpmString = "";
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private bool isReallyExiting = false;

        private static bool SetDarkModeTitleBar(IntPtr handle)
        {
            // this works on windows 11 and later builds of windows 10
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
                    // fails on unsupported os versions
                }
            }
            return false;
        }

        AsusControl asusControl = new AsusControl();
        int fanSpeed = 0;
        private Timer timer;
        NotifyIcon trayIcon;
        private int lastCpuTemp = 0;
        private bool isTempRising = true;
        private bool isInfoPopupVisible = false;
        private enum ActiveFanCurve
        {
            FanCurve1,
            FanCurve2
        }
        private ActiveFanCurve currentActiveFanCurve = ActiveFanCurve.FanCurve1;


        public Form1(string[] args)
        {
            // enable double buffering to reduce flicker
            this.SetStyle(ControlStyles.DoubleBuffer |
                            ControlStyles.UserPaint |
                            ControlStyles.AllPaintingInWmPaint,
                            true);
            this.UpdateStyles();

            InitializeComponent();
            menuStrip1.Renderer = new DarkMenuRenderer();
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);

            // create the tray icon here so it's never null, but keep it hidden
            trayIcon = new NotifyIcon()
            {
                Icon = Icon,
                Visible = false,
                ContextMenu = new ContextMenu(new MenuItem[] {
        new MenuItem("Show", (s1, e1) =>
        {
            Show();
            trayIcon.Visible = false;
        }),
        new MenuItem("Exit", (s1, e1) =>
        {
            isReallyExiting = true;
            Application.Exit();
        }),
    }),
            };
            trayIcon.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    // toggle the form's visibility
                    if (this.Visible)
                    {
                        this.Hide();
                    }
                    else
                    {
                        this.Show();
                        this.Activate(); // important: this brings the form to the front
                    }
                }
            };

            toolStripMenuItemTurnOffControlOnExit.Checked = Properties.Settings.Default.turnOffControlOnExit;
            toolStripMenuItemForbidUnsafeSettings.Checked = Properties.Settings.Default.forbidUnsafeSettings;
            toolStripMenuItemMinimizeToTrayOnClose.Checked = Properties.Settings.Default.minimizeToTrayOnClose;
            toolStripMenuItemAutoRefreshStats.Checked = Properties.Settings.Default.autoRefreshStats;
            trackBarFanSpeed.Value = Properties.Settings.Default.fanSpeed;

            numericUpDown1.Minimum = 1000;
            numericUpDown1.Maximum = 60000;

            // initialize the timer but do not subscribe or start it here
            timer = new Timer();

            Properties.Settings.Default.Reload();

            if (Properties.Settings.Default.TimerRefreshRate < numericUpDown1.Minimum ||
                Properties.Settings.Default.TimerRefreshRate > numericUpDown1.Maximum)
            {
                numericUpDown1.Value = 2500;
                Properties.Settings.Default.TimerRefreshRate = (int)numericUpDown1.Value;
                Properties.Settings.Default.Save();
            }
            else
            {
                numericUpDown1.Value = Properties.Settings.Default.TimerRefreshRate;
            }

            numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;

            LoadSettings();
        }

        protected override void SetVisibleCore(bool value)
        {
            // this is the definitive fix for the title bar flashing
            // we apply the dark theme before the form is made visible for the first time
            if (!this.IsHandleCreated)
            {
                this.CreateHandle();
                SetDarkModeTitleBar(this.Handle);
            }
            base.SetVisibleCore(value);
        }

        private void ButtonToggleFanCurve_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FanCurveEnabled = !Properties.Settings.Default.FanCurveEnabled;
            Properties.Settings.Default.Save();
            ShowInfoPopup($"Fan Curve is now {(Properties.Settings.Default.FanCurveEnabled ? "enabled" : "disabled")}");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
                timer?.Dispose();
                trayIcon?.Dispose();
                AppDomain.CurrentDomain.ProcessExit -= OnProcessExit; // unsubscribe processexit
                if (asusControl is IDisposable disposableControl)
                {
                    disposableControl.Dispose(); // dispose asuscontrol if idisposable
                }
            }
            base.Dispose(disposing);
        }

        private void OnProcessExit(object sender, EventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.turnOffControlOnExit)
                {
                    asusControl.SetFanSpeed(0, 0);
                    asusControl.SetFanSpeed(0, 1);
                }

                Properties.Settings.Default.Save();

                Console.WriteLine("Settings saved.");
            }
            catch (Exception ex)
            {
                ShowInfoPopup($"Error saving settings on exit: {ex.Message}", "Error");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trayIcon.Visible = true;
            timer.Tick += UpdateStats_Tick;
            timer.Interval = (int)numericUpDown1.Value;

            // important: only start the timer if the setting is already checked
            if (Properties.Settings.Default.autoRefreshStats)
            {
                timer.Start();
            }

            if (Environment.GetCommandLineArgs().Contains("-tray"))
            {
                WindowState = FormWindowState.Minimized;
                MinimizeToTray();
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // check our secret flag
            if (isReallyExiting)
            {
                // if we are really exiting, do nothing and let the app close
                return;
            }
            else
            {
                // if not, it means the user clicked the 'x' button
                // so, we cancel the close and hide the form instead
                e.Cancel = true;
                this.Hide();
            }
        }

        private void PositionFormNextToTray()
        {
            Rectangle workingArea = Screen.PrimaryScreen.WorkingArea;
            int newLeft = workingArea.Right - this.Width; // default to bottom-right
            int newTop = workingArea.Bottom - this.Height - 50;

            if (workingArea.Top > 0) // taskbar is at the top
            {
                newTop = workingArea.Top;
            }
            if (workingArea.Left > 0) // taskbar is on the left
            {
                newLeft = workingArea.Left;
            }

            this.Location = new Point(newLeft, newTop);
        }

        private void toolStripMenuItemTurnOffControlOnExit_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.turnOffControlOnExit = toolStripMenuItemTurnOffControlOnExit.Checked;
            Properties.Settings.Default.Save();
        }

        private void ToolStripMenuItemForbidUnsafeSettings_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.forbidUnsafeSettings = toolStripMenuItemForbidUnsafeSettings.Checked;
            Properties.Settings.Default.Save();
        }

        private void toolStripMenuItemMinimizeToTrayOnClose_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.minimizeToTrayOnClose = toolStripMenuItemMinimizeToTrayOnClose.Checked;
            Properties.Settings.Default.Save();
        }

        private void ToolStripMenuItemAutoRefreshStats_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.autoRefreshStats = toolStripMenuItemAutoRefreshStats.Checked;
            Properties.Settings.Default.Save();

            if (Properties.Settings.Default.autoRefreshStats)
            {
                // manually trigger a tick immediately so the user sees a change
                UpdateStats_Tick(null, null);
                timer.Start();
            }
            else
            {
                timer.Stop();
            }
        }

        private void toolStripMenuItemCheckForUpdates_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/realMoai/ZenithFanUtility");
        }

        private void RadioButton_Mode_CheckedChanged(object sender, EventArgs e)
        {
            // this method runs whenever any of the three radio buttons are checked
            // we only care about the one that is now 'checked'
            if (sender is RadioButton checkedButton && checkedButton.Checked)
            {
                string selectedMode = "";
                if (checkedButton == manualModeRadioButton)
                {
                    // --- manual mode ---
                    manualControlGroupBox.Enabled = true; // enable the slider panel
                    DeactivateFanCurve(); // turn off the fan curve
                    setFanSpeed(); // apply the current slider value
                    selectedMode = "Manual";
                }
                else if (checkedButton == fanCurve1RadioButton)
                {
                    // --- fan curve 1 ---
                    manualControlGroupBox.Enabled = false; // disable slider panel
                    Properties.Settings.Default.FanCurveEnabled = true;
                    Properties.Settings.Default.ActiveFanCurve = ActiveFanCurve.FanCurve1.ToString();
                    currentActiveFanCurve = ActiveFanCurve.FanCurve1;
                    selectedMode = "Curve1";
                }
                else if (checkedButton == fanCurve2RadioButton)
                {
                    // --- fan curve 2 ---
                    manualControlGroupBox.Enabled = false; // disable slider panel
                    Properties.Settings.Default.FanCurveEnabled = true;
                    Properties.Settings.Default.ActiveFanCurve = ActiveFanCurve.FanCurve2.ToString();
                    currentActiveFanCurve = ActiveFanCurve.FanCurve2;
                    selectedMode = "Curve2";
                }

                if (!string.IsNullOrEmpty(selectedMode))
                {
                    Properties.Settings.Default["LastSelectedMode"] = selectedMode;
                    Properties.Settings.Default.Save();
                }
            }
        }
        private void setFanSpeed()
        {
            var value = trackBarFanSpeed.Value;
            Properties.Settings.Default.fanSpeed = value;
            Properties.Settings.Default.Save();

            if (!checkBoxTurnOn.Checked)
            {
                value = 0;
            }

            if (fanSpeed == value)
                return;

            fanSpeed = value;

            if (Properties.Settings.Default.FanCurveEnabled)
            {
                ApplyFanCurve(Properties.Settings.Default.FanCurvePoints1, ActiveFanCurve.FanCurve1);
            }
            else
            {
                asusControl.SetFanSpeed(value, 0);
                asusControl.SetFanSpeed(value, 1);
                UpdateFanPercentageLabel(value, ActiveFanCurve.FanCurve1);
            }
        }

        // change the applyfancurve method to have a default parameter value
        private void ApplyFanCurve(System.Collections.Specialized.StringCollection fanCurve, ActiveFanCurve fanCurveType = ActiveFanCurve.FanCurve1)
        {
            if (fanCurve == null || fanCurve.Count == 0)
            {
                MessageBox.Show("Fan Curve is not enabled or points are empty.", "Debug", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var currentTemp = asusControl.Thermal_Read_Cpu_Temperature();
            int desiredFanSpeed = CalculateFanSpeedFromCurve((int)currentTemp, fanCurve);

            asusControl.SetFanSpeed(desiredFanSpeed, 0);
            asusControl.SetFanSpeed(desiredFanSpeed, 1);
            UpdateFanPercentageLabel(desiredFanSpeed, fanCurveType);

            currentActiveFanCurve = fanCurveType;
        }

        private int CalculateFanSpeedFromCurve(int currentTemp, System.Collections.Specialized.StringCollection fanCurve)
        {
            var points = fanCurve.Cast<string>()
                .Select(entry => entry.Split(','))
                .Select(parts => (Temperature: int.Parse(parts[1]), FanSpeed: int.Parse(parts[2])))
                .OrderBy(point => point.Temperature)
                .ToList();

            if (!points.Any())
            {
                throw new InvalidOperationException("Fan curve points are empty or invalid.");
            }

            var lowerPoint = points.LastOrDefault(point => point.Temperature <= currentTemp);
            var upperPoint = points.FirstOrDefault(point => point.Temperature >= currentTemp);

            if (lowerPoint == default)
            {
                return points.First().FanSpeed;
            }
            if (upperPoint == default)
            {
                return points.Last().FanSpeed;
            }
            if (lowerPoint.Temperature == upperPoint.Temperature)
            {
                return lowerPoint.FanSpeed;
            }

            double slope = (double)(upperPoint.FanSpeed - lowerPoint.FanSpeed) / (upperPoint.Temperature - lowerPoint.Temperature);
            return (int)(lowerPoint.FanSpeed + slope * (currentTemp - lowerPoint.Temperature));
        }
        private void UpdateFanPercentageLabel(int speed, ActiveFanCurve fanCurveType)
        {
            string fanCurveStatus = Properties.Settings.Default.FanCurveEnabled ? $" ({fanCurveType} Active)" : "";
            label1.Text = $"Fan Speed: {speed}%{fanCurveStatus}";

            // update trackbarfanspeed to match the label's value
            if (trackBarFanSpeed.Value != speed)
            {
                // temporarily unsubscribe from events to prevent unwanted triggers
                trackBarFanSpeed.MouseCaptureChanged -= trackBarFanSpeed_MouseCaptureChanged;
                trackBarFanSpeed.KeyUp -= trackBarFanSpeed_KeyUp;

                trackBarFanSpeed.Value = speed;

                // re-subscribe to the events
                trackBarFanSpeed.MouseCaptureChanged += trackBarFanSpeed_MouseCaptureChanged;
                trackBarFanSpeed.KeyUp += trackBarFanSpeed_KeyUp;
            }
        }
        private void checkBoxTurnOn_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.CustomControlEnabled = checkBoxTurnOn.Checked;
            Properties.Settings.Default.Save();

            // the original logic to apply the fan speed still runs
            setFanSpeed();
        }

        private void trackBarFanSpeed_MouseCaptureChanged(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.FanCurveEnabled)
            {
                ShowInfoPopup("Fan Curve is active. Please disable it to manually adjust the fan speed.", "Fan Curve Active");
                return;
            }

            if (Properties.Settings.Default.forbidUnsafeSettings)
            {
                if (trackBarFanSpeed.Value < 40)
                    trackBarFanSpeed.Value = 40;
                else if (trackBarFanSpeed.Value > 99)
                    trackBarFanSpeed.Value = 99;
            }

            setFanSpeed();
        }

        private void trackBarFanSpeed_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Left && e.KeyCode != Keys.Right)
                return;

            trackBarFanSpeed_MouseCaptureChanged(sender, e);
        }

        private void MaxFans_Click(object sender, EventArgs e)
        {
            if (checkBoxTurnOn.Checked)
            {
                DeactivateFanCurve();
                trackBarFanSpeed.Value = 100;
                setFanSpeed();
            }
        }

        private void MinimizeToTray()
        {
            if (trayIcon != null)
            {
                trayIcon.Visible = true;
            }
            this.Hide();
            this.ShowInTaskbar = false;
        }

        private void startWithWindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            startWithWindowsToolStripMenuItem.Checked = !startWithWindowsToolStripMenuItem.Checked;
            Properties.Settings.Default.boot = startWithWindowsToolStripMenuItem.Checked;
            Properties.Settings.Default.Save();
            SetStartup(startWithWindowsToolStripMenuItem.Checked);
        }
        private void SetStartup(bool enable)
        {
            string runKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Run";
            string appName = "AsusFanControl";

            // the path now includes the "-tray" argument for starting minimized
            // the quotes are added to handle spaces in the file path
            string appPath = $"\"{Application.ExecutablePath}\" -tray";

            using (Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(runKey, true))
            {
                if (enable)
                {
                    key.SetValue(appName, appPath);
                }
                else
                {
                    key.DeleteValue(appName, false);
                }
            }
        }

        private void ApplyDarkMode()
        {
            var backgroundColor = Color.FromArgb(28, 28, 28);
            var controlBackgroundColor = Color.FromArgb(28, 28, 28);
            var foregroundColor = Color.White;
            var outlineColor = Color.FromArgb(80, 80, 80);

            this.BackColor = backgroundColor;
            this.ForeColor = foregroundColor;

            foreach (Control control in this.Controls)
            {
                ApplyDarkModeToControl(control, backgroundColor, controlBackgroundColor, foregroundColor, outlineColor);
            }
            menuStrip1.Renderer = new DarkMenuRenderer();
        }

        // new helper method to apply dark mode recursively
        private void ApplyDarkModeToControl(Control control, Color backgroundColor, Color controlBackgroundColor, Color foregroundColor, Color outlineColor)
        {
            if (control is GroupBox)
            {
                control.BackColor = backgroundColor;
                control.ForeColor = foregroundColor;
            }
            else if (control is CheckBox || control is Label || control is RadioButton)
            {
                control.BackColor = Color.Transparent;
                control.ForeColor = foregroundColor;
            }
            else if (control is Button button)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.BackColor = controlBackgroundColor;
                button.ForeColor = foregroundColor;
                button.FlatAppearance.BorderColor = outlineColor;
                button.FlatAppearance.BorderSize = 1;
            }
            else if (control is NumericUpDown nud)
            {
                nud.BackColor = controlBackgroundColor;
                nud.ForeColor = foregroundColor;
            }
            else if (control is TrackBar trackBar)
            {
                trackBar.BackColor = controlBackgroundColor;
            }
            else if (control is TextBox textBox)
            {
                textBox.BackColor = controlBackgroundColor;
                textBox.ForeColor = foregroundColor;
                textBox.BorderStyle = BorderStyle.FixedSingle; // optional: give it a consistent border
            }

            // handle nested controls in groupboxes
            foreach (Control childControl in control.Controls)
            {
                ApplyDarkModeToControl(childControl, backgroundColor, controlBackgroundColor, foregroundColor, outlineColor);
            }
        }

        private void ButtonFanCurve_Click(object sender, EventArgs e)
        {
            using (var fanCurveForm = new Form2(this, Properties.Settings.Default.DarkMode, asusControl, this.timer))
            {
                fanCurveForm.PositionNextToOwner();
                fanCurveForm.ShowDialog();
            }
        }

        private void FanCurve_Click(object sender, EventArgs e)
        {
            using (var form2 = new Form2(this, Properties.Settings.Default.DarkMode, asusControl, this.timer))
            {
                form2.PositionNextToOwner();
                form2.ShowDialog();
            }
        }

        private void ToggleFanCurve_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FanCurveEnabled = true;
            Properties.Settings.Default.Save();
            ShowInfoPopup("Fan Curve 1 has been activated.", "Confirmation");

            // apply the fan curve with the correct activefancurve type
            ApplyFanCurve(Properties.Settings.Default.FanCurvePoints1, ActiveFanCurve.FanCurve1);

            // save the active fan curve
            Properties.Settings.Default.ActiveFanCurve = ActiveFanCurve.FanCurve1.ToString();
            Properties.Settings.Default.Save();
        }
        public void ApplyFanCurveFromForm2()
        {
            ApplyFanCurve(Properties.Settings.Default.FanCurvePoints1);
        }
        private void FanCurveOff_Click(object sender, EventArgs e)
        {
            DeactivateFanCurve();
            ShowInfoPopup("Fan Curve has been deactivated.", "Confirmation");

            // set the fan speed directly based on the slider value after deactivating the fan curve
            var value = trackBarFanSpeed.Value;
            asusControl.SetFanSpeed(value, 0);
            asusControl.SetFanSpeed(value, 1);
            UpdateFanPercentageLabel(value, ActiveFanCurve.FanCurve1);
        }
        private void DeactivateFanCurve()
        {
            Properties.Settings.Default.FanCurveEnabled = false;
            Properties.Settings.Default.Save();
            UpdateFanPercentageLabel(fanSpeed, currentActiveFanCurve); // update label without fan curve
        }
        private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form3 = new Form3(this))
            {
                form3.ShowDialog();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.TimerRefreshRate = (int)numericUpDown1.Value;
            Properties.Settings.Default.Save();

            // apply the new interval to the running timer
            if (timer != null)
            {
                timer.Interval = (int)numericUpDown1.Value;
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {
            string message = "Fan Control Status:\n";
            if (Properties.Settings.Default.FanCurveEnabled)
            {
                message += "Fan Curve is enabled and actively controlling the fan speed.\n";
                message += "Fan Curve Settings (CPU):\n";
                var fanCurve = Properties.Settings.Default.FanCurvePoints1;
                if (fanCurve != null)
                {
                    foreach (var entry in fanCurve)
                    {
                        message += $"{entry}\n";
                    }
                }
                else
                {
                    message += "No CPU fan curve settings found.\n";
                }
            }
            else
            {
                message += "Fan Curve is disabled. Fan speed is controlled manually.\n";
            }

            var fanSpeeds = asusControl.GetFanSpeeds();
            if (fanSpeeds != null && fanSpeeds.Count > 0)
            {
                int firstFanSpeed = fanSpeeds[0];
                int secondFanSpeed = fanSpeeds.Count > 1 ? fanSpeeds[1] : 0;

                message += $"Current Fan Speed: CPU: {firstFanSpeed} RPM, GPU: {secondFanSpeed} RPM\n";
            }
            else
            {
                message += "Current Fan Speed: N/A\n";
            }

            message += $"Fan Speed from TrackBar: {trackBarFanSpeed.Value}%\n";
            message += $"Desired Fan Speed: {fanSpeed}%\n";

            message += $"Timer Refresh Rate: {Properties.Settings.Default.TimerRefreshRate} milliseconds\n";

            if (currentActiveFanCurve == ActiveFanCurve.FanCurve1)
            {
                message += "Using Fan Curve Points 1.\n";
            }
            else if (currentActiveFanCurve == ActiveFanCurve.FanCurve2)
            {
                message += "Using Fan Curve Points 2.\n";
            }

            ShowInfoPopup(message, "Debug Information");
        }
        private void LoadSettings()
        {
            Properties.Settings.Default.Reload();

            ApplyDarkMode();

            if (Properties.Settings.Default.TimerRefreshRate >= numericUpDown1.Minimum &&
                Properties.Settings.Default.TimerRefreshRate <= numericUpDown1.Maximum)
            {
                numericUpDown1.ValueChanged -= numericUpDown1_ValueChanged;
                numericUpDown1.Value = Properties.Settings.Default.TimerRefreshRate;
                numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            }
            else
            {
                numericUpDown1.ValueChanged -= numericUpDown1_ValueChanged;
                numericUpDown1.Value = numericUpDown1.Minimum;
                Properties.Settings.Default.TimerRefreshRate = (int)numericUpDown1.Value;
                Properties.Settings.Default.Save();
                numericUpDown1.ValueChanged += numericUpDown1_ValueChanged;
            }

            fanSpeed = Properties.Settings.Default.fanSpeed;
            trackBarFanSpeed.Value = fanSpeed;
            checkBoxTurnOn.Checked = Properties.Settings.Default.CustomControlEnabled;
            startWithWindowsToolStripMenuItem.Checked = Properties.Settings.Default.boot;

            // this is the new block for loading the radio button state
            string lastMode = Properties.Settings.Default["LastSelectedMode"] as string;
            switch (lastMode)
            {
                case "Curve1":
                    fanCurve1RadioButton.Checked = true;
                    break;
                case "Curve2":
                    fanCurve2RadioButton.Checked = true;
                    break;
                default: // default to manual if nothing is saved
                    manualModeRadioButton.Checked = true;
                    break;
            }

            nudHysteresis.Value = Properties.Settings.Default.HysteresisValue;
            // load the active fan curve (this is still needed)
            if (Enum.TryParse(Properties.Settings.Default.ActiveFanCurve, out ActiveFanCurve activeFanCurve))
            {
                currentActiveFanCurve = activeFanCurve;
            }
        }
        private void label2_Click(object sender, EventArgs e)
        {
            string message = "Fan Control Status:\n";
            if (Properties.Settings.Default.FanCurveEnabled)
            {
                message += "Fan Curve is enabled and actively controlling the fan speed.\n";
                var cpuTemp = asusControl.Thermal_Read_Cpu_Temperature();
                var gpuTemp = asusControl.Thermal_Read_Highest_Gpu_Temperature();
                var usedTemp = Math.Max(cpuTemp, gpuTemp);
                var tempSource = usedTemp == cpuTemp ? "CPU" : "GPU";
                message += $"Fan curve is reading from {tempSource} temperature: {usedTemp}°C\n";
            }
            else
            {
                message += "Fan Curve is disabled. Fan speed is controlled manually.\n";
            }

            var fanSpeeds = asusControl.GetFanSpeeds();
            if (fanSpeeds != null && fanSpeeds.Count > 0)
            {
                int firstFanSpeed = fanSpeeds[0];
                int secondFanSpeed = fanSpeeds.Count > 1 ? fanSpeeds[1] : 0;

                message += $"Current Fan Speed: CPU: {firstFanSpeed} RPM, GPU: {secondFanSpeed} RPM\n";
            }
            else
            {
                message += "Current Fan Speed: N/A\n";
            }

            message += $"Fan Speed from TrackBar: {trackBarFanSpeed.Value}%\n";
            message += $"Desired Fan Speed: {fanSpeed}%\n";

            message += $"Timer Refresh Rate: {Properties.Settings.Default.TimerRefreshRate} milliseconds\n";
            ShowInfoPopup(message, "Debug Information");
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            try
            {
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings on close: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void UpdateStats_Tick(object sender, EventArgs e)
        {
            if (isUpdatingStats)
            {
                return;
            }

            try
            {
                isUpdatingStats = true;

                var cpuTempTask = Task.Run(() => asusControl.Thermal_Read_Cpu_Temperature());
                var gpuTempTask = Task.Run(() => asusControl.Thermal_Read_GpuTS1L_Temperature());
                var fanSpeedsTask = Task.Run(() => asusControl.GetFanSpeeds());

                await Task.WhenAll(cpuTempTask, gpuTempTask, fanSpeedsTask);

                ulong cpuTemp = cpuTempTask.Result;
                ulong gpuTemp = gpuTempTask.Result;
                var fanSpeeds = fanSpeedsTask.Result;
                string currentRpmString = (fanSpeeds != null && fanSpeeds.Count > 0)
                    ? $"CPU: {fanSpeeds[0]} RPM\nGPU: {(fanSpeeds.Count > 1 ? fanSpeeds[1] : 0)} RPM"
                    : "N/A";

                // create a smarter string for the gpu temp
                string gpuTempText = (gpuTemp > 0) ? $"{gpuTemp}°C" : "N/A";

                if (cpuTemp != lastCpuTempValue || gpuTemp != lastGpuTempValue)
                {
                    // update the label with our new smart string
                    labelCPUTemp.Text = $"CPU: {cpuTemp}°C\nGPU: {gpuTempText}";
                    lastCpuTempValue = cpuTemp;
                    lastGpuTempValue = gpuTemp;
                }

                if (currentRpmString != lastRpmString)
                {
                    labelRPM.Text = currentRpmString;
                    lastRpmString = currentRpmString;
                }

                if (trayIcon != null)
                {
                    // also use the smart string in the tooltip
                    string tooltipText = $"CPU: {cpuTemp}°C | GPU: {gpuTempText}";
                    if (fanSpeeds != null && fanSpeeds.Count > 0) tooltipText += $"\nFans: {fanSpeeds[0]} RPM";
                    trayIcon.Text = tooltipText;
                }

                if (Properties.Settings.Default.FanCurveEnabled)
                {
                    ApplyFanCurve((int)cpuTemp);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Timer tick error: " + ex.Message);
            }
            finally
            {
                isUpdatingStats = false;
            }
        }

        private void ApplyFanCurve(int currentTemp)
        {
            // this method no longer needs to fetch the temperature itself
            System.Collections.Specialized.StringCollection fanCurve = null;
            ActiveFanCurve activeCurveType = currentActiveFanCurve;

            if (activeCurveType == ActiveFanCurve.FanCurve1)
            {
                fanCurve = Properties.Settings.Default.FanCurvePoints1;
            }
            else
            {
                fanCurve = Properties.Settings.Default.FanCurvePoints2;
            }

            if (fanCurve == null || fanCurve.Count == 0) return;

            int hysteresisValue = (int)nudHysteresis.Value;

            if (currentTemp > lastCpuTemp) isTempRising = true;
            else if (currentTemp < lastCpuTemp) isTempRising = false;

            int effectiveTemp = isTempRising ? currentTemp : currentTemp + hysteresisValue;
            int desiredFanSpeed = CalculateFanSpeedFromCurve(effectiveTemp, fanCurve);

            asusControl.SetFanSpeed(desiredFanSpeed, 0);
            asusControl.SetFanSpeed(desiredFanSpeed, 1);
            UpdateFanPercentageLabel(desiredFanSpeed, activeCurveType);

            lastCpuTemp = currentTemp;
        }
        private void FansOff_Click(object sender, EventArgs e)
        {
            DeactivateFanCurve();
            trackBarFanSpeed.Value = 0;
            setFanSpeed();
        }
        private void ShowInfoPopup(string message, string title = "Information")
        {
            if (!isInfoPopupVisible)
            {
                isInfoPopupVisible = true;
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
                isInfoPopupVisible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // apply the fan curve from fancurvepoints2 with the correct activefancurve type
            ApplyFanCurve(Properties.Settings.Default.FanCurvePoints2, ActiveFanCurve.FanCurve2);
            ShowInfoPopup("Switched to Fan Curve Points 2.", "Confirmation");

            // save the active fan curve
            Properties.Settings.Default.ActiveFanCurve = ActiveFanCurve.FanCurve2.ToString();
            Properties.Settings.Default.Save();
        }

        private void Form1_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                SetDarkModeTitleBar(this.Handle);
                PositionFormNextToTray();
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isReallyExiting = true;
            Application.Exit();
        }

        private void nudHysteresis_ValueChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.HysteresisValue = nudHysteresis.Value;
            Properties.Settings.Default.Save();
        }
    }
}