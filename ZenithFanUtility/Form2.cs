using System.Runtime.InteropServices;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Drawing.Drawing2D;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AsusFanControl;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

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
        private System.Windows.Forms.ToolTip toolTip1;
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
            // This works on Windows 11 and later builds of Windows 10
            if (Environment.OSVersion.Version.Major >= 10)
            {
                try
                {
                    int enabled = 1;
                    // For Win11
                    DwmSetWindowAttribute(handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref enabled, sizeof(int));
                    // For older Win10
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

        // API calls to manipulate window styles
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_DLGMODALFRAME = 0x0001;
        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hwnd, int index);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hwnd, int index, int newStyle);

        private static void HideIconFromTitleBar(IntPtr handle)
        {
            // Get the current window style
            int extendedStyle = GetWindowLong(handle, GWL_EXSTYLE);
            // Set the style to include WS_EX_DLGMODALFRAME, which hides the icon
            SetWindowLong(handle, GWL_EXSTYLE, extendedStyle | WS_EX_DLGMODALFRAME);
        }

        public Form2(Form owner, bool darkMode, AsusControl asusControl, Timer updateTimer)
        {
            InitializeComponent();
            this.Owner = owner;
            this.darkMode = darkMode;
            this.asusControl = asusControl;

            // --- START OF CHANGES ---

            // 1. Save a reference to the main timer
            this.mainAppTimer = updateTimer;

            // 2. Subscribe our named handler to the Tick event
            if (this.mainAppTimer != null)
            {
                this.mainAppTimer.Tick += UpdateGraphFromTimer;
            }

            // --- END OF CHANGES ---

            ApplyTheme();
            InitializePictureBox1();
            LoadFanCurvePoints();
            LoadFanCurvePoints2();
            toolTip1 = new System.Windows.Forms.ToolTip();
        }
        public Form2()
        {
            InitializeComponent();
            toolTip1 = new System.Windows.Forms.ToolTip();
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
            // This just forces the PictureBox to repaint itself, 
            // which will update the live temperature lines.
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
            var outlineColor = Color.DarkGray; // Set the outline color to dark grey

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
                if (control is Label || control is System.Windows.Forms.Button || control is CheckBox || control is System.Windows.Forms.TextBox || control is DataGridView)
                {
                    control.BackColor = controlBackgroundColor;
                    control.ForeColor = foregroundColor;
                }
                else if (control is System.Windows.Forms.TrackBar)
                {
                    control.BackColor = trackBarBackgroundColor;
                }
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            var pointsToUse = isUsingFanCurvePoints1 ? fanCurvePoints1 : fanCurvePoints2;

            // --- NEW LOGIC FOR ADDING A POINT ---
            if (isAddingPointMode)
            {
                (int temperature, int fanSpeed) = mousePosition(e.Location);

                // Clamp values to be within the graph's bounds
                temperature = Math.Max(20, Math.Min(100, temperature));
                fanSpeed = Math.Max(1, Math.Min(100, fanSpeed));

                int newPointId = pointsToUse.Keys.Count > 0 ? pointsToUse.Keys.Max() + 1 : 1;
                pointsToUse[newPointId] = new Point(temperature, fanSpeed);

                pictureBox1.Invalidate(); // Redraw the graph with the new point

                // Exit "add point mode"
                isAddingPointMode = false;
                pictureBox1.Cursor = Cursors.Default;
                return; // Stop here to prevent selecting the point we just added
            }
            // --- END OF NEW LOGIC ---

            // Original logic for selecting and deleting points
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
                // --- THIS IS THE CRITICAL FIX ---
                // Unsubscribe from the event to prevent the memory leak
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
            byte maximumTemperature = 100; // Adjusted to 100 to prevent moving past 100
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
            // --- DARK THEME COLORS ---
            Color backgroundColor = Color.FromArgb(38, 38, 38);
            Color axisColor = Color.FromArgb(200, 200, 200);
            Color gridColor = Color.FromArgb(80, 80, 80);
            Color curveColor = Color.White;

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            using (SolidBrush bgBrush = new SolidBrush(backgroundColor))
            using (Pen axisPen = new Pen(axisColor))
            using (Pen gridPen = new Pen(gridColor))
            using (SolidBrush textBrush = new SolidBrush(axisColor))
            {
                // 1. Draw the dark background
                g.FillRectangle(bgBrush, this.ClientRectangle);

                // Calculate graph area
                int graphWidth = pictureBox1.Width - 80;
                int graphHeight = pictureBox1.Height - 80;

                // Draw Axes, Gridlines, and Labels (this is all fast)
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

                // --- OPTIMIZATION ---
                // Only fetch the slow hardware sensor data IF we are NOT currently dragging a point.
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

                // Draw the fan curve points and line (this is fast)
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
                    using (Pen thickPen = new Pen(curveColor, 4f)) // Made line slightly thinner
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
                path.AddCurve(points, 0.5f); // Adjust the tension parameter as needed
                g.DrawPath(pen, path);
            }
        }

        private void runFanCurve(bool v1, bool v2)
        {
            // Implement the logic to run the fan curve
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
            pictureBox1.Invalidate(); // Repaint to display loaded points
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
                pictureBox1.Invalidate(); // Redraw the picture box to show the new points
            }
        }

        private void ApplyFanCurve()
        {
            // Implement the logic to apply the fan curve
        }
        private void addpoint_Click_1(object sender, EventArgs e)
        {
            isAddingPointMode = true;
            pictureBox1.Cursor = Cursors.Cross;
        }
        public Form2(bool darkMode, AsusControl asusControl, Timer updateTimer)
        {
            InitializeComponent();
            this.darkMode = darkMode;
            this.asusControl = asusControl; // Assign the asusControl parameter to the class field
            ApplyTheme();
            InitializePictureBox1();
            InitializeAddPointButton();
            LoadFanCurvePoints();
            toolTip1 = new System.Windows.Forms.ToolTip();

            // Verify if the fan control is enabled at start
            if (Properties.Settings.Default.FanCurveEnabled)
            {
                ApplyFanCurve();
            }

            // Attach the timer's Tick event to update the fan curve
            updateTimer.Tick += (s, e) => ApplyFanCurve();
        }

        public void PositionNextToOwner()
        {
            if (this.Owner == null) return;

            // Set start position to Manual to override any defaults
            this.StartPosition = FormStartPosition.Manual;

            // Calculate the desired location to the left of the owner (Form1)
            int newLeft = this.Owner.Left - this.Width;

            // Check if this would place the form off-screen
            if (newLeft < Screen.FromControl(this).WorkingArea.Left)
            {
                // If it would be off-screen, place it on the right side instead
                newLeft = this.Owner.Right;
            }

            // Set our own location
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
            pictureBox1.Invalidate(); // Repaint to display loaded points
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
