using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AsusFanControl;

namespace ZenithFanUtility
{
    public partial class Form2 : Form
    {
        private bool isAddingPointMode = false;
        private Timer mainAppTimer;
        private bool darkMode;
        private Dictionary<int, Point> fanCurvePoints = new Dictionary<int, Point>();
        private int selectedPointId = 0;
        private const int maxDistance = 10;
        private ToolTip toolTip1;
        private AsusControl asusControl;
        private Dictionary<int, Point> fanCurvePoints1 = new Dictionary<int, Point>();
        private Dictionary<int, Point> fanCurvePoints2 = new Dictionary<int, Point>();
        private bool isUsingFanCurvePoints1 = true;
        private System.ComponentModel.IContainer components = null;

        [DllImport("dwmapi.dll")]
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
                catch { /* fails on unsupported os versions */ }
            }
            return false;
        }

        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_DLGMODALFRAME = 0x0001;
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        private static void HideIconFromTitleBar(IntPtr handle)
        {
            int extendedStyle = GetWindowLong(handle, GWL_EXSTYLE);
            // set style to hide the title bar icon
            SetWindowLong(handle, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);
        }

        public Form2(Form owner, bool darkMode, AsusControl asusControl, Timer updateTimer)
        {
            InitializeComponent();
            this.Owner = owner;
            this.darkMode = darkMode;
            this.asusControl = asusControl;

            // save a reference to the main timer
            this.mainAppTimer = updateTimer;

            // subscribe our named handler to the tick event
            if (this.mainAppTimer != null)
            {
                this.mainAppTimer.Tick += UpdateGraphFromTimer;
            }

            ApplyTheme();
            InitializePictureBox1();
            LoadFanCurvePoints();
            LoadFanCurvePoints2();
            toolTip1 = new ToolTip();
        }
        public Form2()
        {
            InitializeComponent();
            toolTip1 = new ToolTip();
            LoadFanCurvePoints();
        }

        private void ApplyTheme()
        {
            if (darkMode)
            {
                ApplyDarkMode();
            }
            else
            {
                ApplyLightMode();
            }
        }

        private void UpdateGraphFromTimer(object sender, EventArgs e)
        {
            // forces the picturebox to repaint itself, which will update the live temperature lines
            if (pictureBox1 != null && pictureBox1.IsHandleCreated)
            {
                pictureBox1.Invalidate();
            }
        }

        private void ApplyDarkMode()
        {
            var backgroundColor = Color.FromArgb(28, 28, 28);
            var foregroundColor = Color.White;
            var controlBackgroundColor = Color.FromArgb(30, 30, 30);
            var trackBarBackgroundColor = Color.FromArgb(30, 30, 30);
            var outlineColor = Color.DarkGray;

            this.BackColor = backgroundColor;
            this.ForeColor = foregroundColor;

            foreach (Control control in this.Controls)
            {
                if (control is Label || control is CheckBox || control is TextBox)
                {
                    control.BackColor = controlBackgroundColor;
                    control.ForeColor = foregroundColor;
                }
                else if (control is TrackBar)
                {
                    control.BackColor = trackBarBackgroundColor;
                }
                else if (control is Button button)
                {
                    button.BackColor = controlBackgroundColor;
                    button.ForeColor = foregroundColor;
                    button.FlatAppearance.BorderColor = outlineColor;
                    button.FlatAppearance.BorderSize = 1;
                    button.FlatStyle = FlatStyle.Flat;
                }
            }
        }

        private void ApplyLightMode()
        {
            var backgroundColor = Color.White;
            var foregroundColor = Color.Black;
            var controlBackgroundColor = Color.White;
            var trackBarBackgroundColor = Color.White;

            this.BackColor = backgroundColor;
            this.ForeColor = foregroundColor;

            foreach (Control control in this.Controls)
            {
                if (control is Label || control is Button || control is CheckBox || control is TextBox || control is DataGridView)
                {
                    control.BackColor = controlBackgroundColor;
                    control.ForeColor = foregroundColor;
                }
                else if (control is TrackBar)
                {
                    control.BackColor = trackBarBackgroundColor;
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            var pointsToUse = isUsingFanCurvePoints1 ? fanCurvePoints1 : fanCurvePoints2;

            if (isAddingPointMode)
            {
                (int temperature, int fanSpeed) = mousePosition(e.Location);

                // clamp values to be within the graph's bounds
                temperature = Math.Max(20, Math.Min(100, temperature));
                fanSpeed = Math.Max(1, Math.Min(100, fanSpeed));

                int newPointId = pointsToUse.Keys.Count > 0 ? pointsToUse.Keys.Max() + 1 : 1;
                pointsToUse[newPointId] = new Point(temperature, fanSpeed);

                pictureBox1.Invalidate();

                // exit "add point mode"
                isAddingPointMode = false;
                pictureBox1.Cursor = Cursors.Default;
                return; // stop here to prevent selecting the point we just added
            }

            if (e.Button == MouseButtons.Left)
            {
                KeyValuePair<int, Point> reachablePoint = nearestPointToMouse(e, maxDistance, mousePosition, pointsToUse);
                if (reachablePoint.Value != Point.Empty)
                {
                    selectedPointId = reachablePoint.Key;
                }
                else
                {
                    selectedPointId = 0;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                KeyValuePair<int, Point> reachablePoint = nearestPointToMouse(e, maxDistance, mousePosition, pointsToUse);
                if (reachablePoint.Value != Point.Empty)
                {
                    pointsToUse.Remove(reachablePoint.Key);
                    ((PictureBox)sender).Invalidate();
                }
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // unsubscribe from the event to prevent the memory leak
                if (this.mainAppTimer != null)
                {
                    this.mainAppTimer.Tick -= UpdateGraphFromTimer;
                }

                components?.Dispose();
                toolTip1?.Dispose();
            }
            base.Dispose(disposing);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            byte minimumTemperature = 20;
            byte maximumTemperature = 100;
            byte minFanSpeed = 1;
            byte maxFanSpeed = 100;

            var pointsToUse = isUsingFanCurvePoints1 ? fanCurvePoints1 : fanCurvePoints2;

            if (selectedPointId != 0)
            {
                (int temperature, int fanSpeed) = mousePosition(e.Location);

                if (temperature < minimumTemperature || temperature > maximumTemperature)
                {
                    temperature = Math.Max(minimumTemperature, Math.Min(temperature, maximumTemperature));
                }
                if (fanSpeed < minFanSpeed || fanSpeed > maxFanSpeed)
                {
                    fanSpeed = Math.Max(minFanSpeed, Math.Min(fanSpeed, maxFanSpeed));
                }

                pointsToUse[selectedPointId] = new Point(temperature, fanSpeed);

                toolTip1.SetToolTip((PictureBox)sender, $"Temperature: {temperature}°C, Fan Speed: {fanSpeed}%");

                ((PictureBox)sender).Invalidate();
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            selectedPointId = 0;
            toolTip1?.SetToolTip((PictureBox)sender, "Fan Curve Graph");

            if (isUsingFanCurvePoints1)
            {
                SaveFanCurvePoints1();
            }
            else
            {
                SaveFanCurvePoints2();
            }

            runFanCurve(true, true);
        }

        private void SaveFanCurvePoints1()
        {
            var fanCurvePointsCollection = new System.Collections.Specialized.StringCollection();
            foreach (var point in fanCurvePoints1)
            {
                fanCurvePointsCollection.Add($"{point.Key},{point.Value.X},{point.Value.Y}");
            }
            Properties.Settings.Default.FanCurvePoints1 = fanCurvePointsCollection;
            Properties.Settings.Default.Save();
        }

        private (int temperature, int fanSpeed) mousePosition(Point e)
        {
            int padding = 40;
            int graphWidth = pictureBox1.Width - 2 * padding;
            int graphHeight = pictureBox1.Height - 2 * padding;

            int tempMin = 20;
            int tempMax = 100;
            int speedMax = 100;

            int temperature = tempMin + (e.X - padding) * (tempMax - tempMin) / graphWidth;
            int fanSpeed = speedMax - (e.Y - padding) * speedMax / graphHeight;

            return (temperature, fanSpeed);
        }

        private KeyValuePair<int, Point> nearestPointToMouse(MouseEventArgs e, int maxDistance, Func<Point, (int temperature, int fanSpeed)> mousePositionFunc, Dictionary<int, Point> fanCurvePoints)
        {
            (int temperature, int fanSpeed) = mousePositionFunc(e.Location);

            var nearestPoints = fanCurvePoints
                            .OrderBy(p => Distance(p.Value, new Point(temperature, fanSpeed)));

            KeyValuePair<int, Point> reachablePoint = nearestPoints
                .FirstOrDefault(p => Distance(p.Value, new Point(temperature, fanSpeed)) <= maxDistance);

            return reachablePoint;
        }

        private double Distance(Point p1, Point p2)
        {
            int dx = p1.X - p2.X;
            int dy = p1.Y - p2.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Color backgroundColor = Color.FromArgb(38, 38, 38);
            Color axisColor = Color.FromArgb(200, 200, 200);
            Color gridColor = Color.FromArgb(80, 80, 80);
            Color curveColor = Color.White;

            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush bgBrush = new SolidBrush(backgroundColor))
            using (Pen axisPen = new Pen(axisColor))
            using (Pen gridPen = new Pen(gridColor))
            using (SolidBrush textBrush = new SolidBrush(axisColor))
            {
                g.FillRectangle(bgBrush, this.ClientRectangle);

                int graphWidth = pictureBox1.Width - 80;
                int graphHeight = pictureBox1.Height - 80;

                g.DrawLine(axisPen, 40, pictureBox1.Height - 40, pictureBox1.Width - 40, pictureBox1.Height - 40);
                for (int temp = 20; temp <= 100; temp += 10)
                {
                    int x = 40 + (temp - 20) * graphWidth / 80;
                    g.DrawLine(axisPen, x, pictureBox1.Height - 40 - 5, x, pictureBox1.Height - 40 + 5);
                    g.DrawString(temp.ToString(), Control.DefaultFont, textBrush, x - 10, pictureBox1.Height - 40 + 10);
                    g.DrawLine(gridPen, x, 40, x, pictureBox1.Height - 40);
                }
                g.DrawString("Temperature (°C)", Control.DefaultFont, textBrush, pictureBox1.Width / 2 - 40, pictureBox1.Height - 40 + 20);
                g.DrawLine(axisPen, 40, 40, 40, pictureBox1.Height - 40);
                for (int speed = 0; speed <= 100; speed += 20)
                {
                    int y = pictureBox1.Height - 40 - speed * graphHeight / 100;
                    g.DrawLine(axisPen, 35, y, 45, y);
                    g.DrawString(speed.ToString(), Control.DefaultFont, textBrush, 5f, y - 10);
                    if (speed > 0) g.DrawLine(gridPen, 40, y, pictureBox1.Width - 40, y);
                }
                g.DrawString("Fan Speed (%)", Control.DefaultFont, textBrush, 20f, 0f, new StringFormat(StringFormatFlags.DirectionVertical));

                // only fetch the slow hardware sensor data if we are not currently dragging a point
                if (selectedPointId == 0)
                {
                    ulong cpuTemp = asusControl.Thermal_Read_Cpu_Temperature();
                    ulong gpuTemp = asusControl.Thermal_Read_Highest_Gpu_Temperature();
                    int currentCpuTempX = 40 + ((int)cpuTemp - 20) * graphWidth / 80;
                    g.DrawLine(Pens.Red, currentCpuTempX, 40, currentCpuTempX, pictureBox1.Height - 40);
                    g.DrawString($"CPU Temp: {cpuTemp}°C", Control.DefaultFont, Brushes.Red, currentCpuTempX - 30, 20);
                    int currentGpuTempX = 40 + ((int)gpuTemp - 20) * graphWidth / 80;
                    g.DrawLine(Pens.Blue, currentGpuTempX, 40, currentGpuTempX, pictureBox1.Height - 40);
                    g.DrawString($"GPU Temp: {gpuTemp}°C", Control.DefaultFont, Brushes.Blue, currentGpuTempX - 30, 40);
                }

                var pointsToDraw = isUsingFanCurvePoints1 ? fanCurvePoints1 : fanCurvePoints2;
                foreach (Point point in pointsToDraw.Values)
                {
                    int x = 40 + (point.X - 20) * graphWidth / 80;
                    int y = pictureBox1.Height - 40 - point.Y * graphHeight / 100;
                    g.FillEllipse(Brushes.Gray, x - 11, y - 11, 22, 22);
                }
                if (pointsToDraw.Count >= 2)
                {
                    Point[] graphPoints = pointsToDraw.Values
                        .OrderBy(p => p.X)
                        .Select(p => new Point(40 + (p.X - 20) * graphWidth / 80, pictureBox1.Height - 40 - p.Y * graphHeight / 100))
                        .ToArray();
                    using (Pen thickPen = new Pen(curveColor, 4f))
                    {
                        thickPen.LineJoin = LineJoin.Round;
                        DrawCurve(g, thickPen, graphPoints);
                    }
                }
            }
        }

        private void DrawCurve(Graphics g, Pen pen, Point[] points)
        {
            if (points.Length < 2)
                return;

            using (GraphicsPath path = new GraphicsPath())
            {
                path.AddCurve(points, 0.5f);
                g.DrawPath(pen, path);
            }
        }

        private void runFanCurve(bool v1, bool v2)
        {
            // this method can be removed if not used
        }

        private void InitializePictureBox1()
        {
            pictureBox1.MouseDown += pictureBox1_MouseDown;
            pictureBox1.MouseMove += pictureBox1_MouseMove;
            pictureBox1.MouseUp += pictureBox1_MouseUp;
            pictureBox1.Paint += pictureBox1_Paint;
        }

        private void LoadFanCurvePoints()
        {
            var fanCurvePointsCollection = Properties.Settings.Default.FanCurvePoints1;
            if (fanCurvePointsCollection != null)
            {
                fanCurvePoints1.Clear();
                foreach (var entry in fanCurvePointsCollection)
                {
                    var parts = entry.Split(',');
                    if (parts.Length == 3 &&
                        int.TryParse(parts[0], out int id) &&
                        int.TryParse(parts[1], out int x) &&
                        int.TryParse(parts[2], out int y))
                    {
                        fanCurvePoints1[id] = new Point(x, y);
                    }
                }
            }
            pictureBox1.Invalidate();
        }

        private void buttonSave_Click_1(object sender, EventArgs e)
        {
            SaveFanCurvePoints1();
            SaveFanCurvePoints2();
            this.Close();
        }

        private void buttonCancel_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void reset_Click(object sender, EventArgs e)
        {
            var pointsToUse = isUsingFanCurvePoints1 ? fanCurvePoints1 : fanCurvePoints2;
            if (pointsToUse.Count > 0)
            {
                pointsToUse.Clear();
                int newPointId1 = pointsToUse.Keys.Count > 0 ? pointsToUse.Keys.Max() + 1 : 1;
                pointsToUse[newPointId1] = new Point(20, 0);
                int newPointId2 = pointsToUse.Keys.Count > 0 ? pointsToUse.Keys.Max() + 1 : 2;
                pointsToUse[newPointId2] = new Point(100, 100);
                pictureBox1.Invalidate();
            }
        }

        private void addpoint_Click_1(object sender, EventArgs e)
        {
            isAddingPointMode = true;
            pictureBox1.Cursor = Cursors.Cross;
        }

        public void PositionNextToOwner()
        {
            if (this.Owner == null) return;

            this.StartPosition = FormStartPosition.Manual;

            int newLeft = this.Owner.Left - this.Width;

            if (newLeft < Screen.FromControl(this).WorkingArea.Left)
            {
                newLeft = this.Owner.Right;
            }

            int newTop = this.Owner.Bottom - this.Height;
            this.Location = new Point(newLeft, newTop);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            isUsingFanCurvePoints1 = true;
            pictureBox1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isUsingFanCurvePoints1 = false;
            pictureBox1.Invalidate();
        }

        private void LoadFanCurvePoints2()
        {
            var fanCurvePointsCollection = Properties.Settings.Default.FanCurvePoints2;
            if (fanCurvePointsCollection != null)
            {
                fanCurvePoints2.Clear();
                foreach (var entry in fanCurvePointsCollection)
                {
                    var parts = entry.Split(',');
                    if (parts.Length == 3 &&
                        int.TryParse(parts[0], out int id) &&
                        int.TryParse(parts[1], out int x) &&
                        int.TryParse(parts[2], out int y))
                    {
                        fanCurvePoints2[id] = new Point(x, y);
                    }
                }
            }
            pictureBox1.Invalidate();
        }

        private void SaveFanCurvePoints2()
        {
            var fanCurvePointsCollection = new System.Collections.Specialized.StringCollection();
            foreach (var point in fanCurvePoints2)
            {
                fanCurvePointsCollection.Add($"{point.Key},{point.Value.X},{point.Value.Y}");
            }
            Properties.Settings.Default.FanCurvePoints2 = fanCurvePointsCollection;
            Properties.Settings.Default.Save();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            SetDarkModeTitleBar(this.Handle);
        }
    }
}