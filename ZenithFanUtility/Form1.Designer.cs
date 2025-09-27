using System.Windows.Forms;
using System;

namespace ZenithFanUtility
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.trackBarFanSpeed = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTurnOffControlOnExit = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemForbidUnsafeSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemMinimizeToTrayOnClose = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemAutoRefreshStats = new System.Windows.Forms.ToolStripMenuItem();
            this.startWithWindowsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemCheckForUpdates = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.creditsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.MaxFans = new System.Windows.Forms.Button();
            this.FansOff = new System.Windows.Forms.Button();
            this.FanCurve = new System.Windows.Forms.Button();
            this.manualControlGroupBox = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.fanCurve2RadioButton = new System.Windows.Forms.RadioButton();
            this.fanCurve1RadioButton = new System.Windows.Forms.RadioButton();
            this.manualModeRadioButton = new System.Windows.Forms.RadioButton();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.nudHysteresis = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.labelCPUTemp = new System.Windows.Forms.Label();
            this.buttonRefreshCPUTemp = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.labelRPM = new System.Windows.Forms.Label();
            this.buttonRefreshRPM = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxTurnOn = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFanSpeed)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.manualControlGroupBox.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHysteresis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // trackBarFanSpeed
            // 
            this.trackBarFanSpeed.AutoSize = false;
            this.trackBarFanSpeed.BackColor = System.Drawing.SystemColors.Window;
            this.trackBarFanSpeed.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.trackBarFanSpeed.Location = new System.Drawing.Point(3, 42);
            this.trackBarFanSpeed.Margin = new System.Windows.Forms.Padding(6);
            this.trackBarFanSpeed.Maximum = 100;
            this.trackBarFanSpeed.Name = "trackBarFanSpeed";
            this.trackBarFanSpeed.Size = new System.Drawing.Size(463, 50);
            this.trackBarFanSpeed.TabIndex = 0;
            this.trackBarFanSpeed.Value = 100;
            this.trackBarFanSpeed.KeyUp += new System.Windows.Forms.KeyEventHandler(this.trackBarFanSpeed_KeyUp);
            this.trackBarFanSpeed.MouseCaptureChanged += new System.EventHandler(this.trackBarFanSpeed_MouseCaptureChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 188);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(248, 25);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current Fan Percentage:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolStripMenuItem1,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(751, 40);
            this.menuStrip1.TabIndex = 10;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(71, 36);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(184, 44);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemTurnOffControlOnExit,
            this.toolStripMenuItemForbidUnsafeSettings,
            this.toolStripMenuItemMinimizeToTrayOnClose,
            this.toolStripMenuItem2,
            this.toolStripMenuItemAutoRefreshStats,
            this.startWithWindowsToolStripMenuItem});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(118, 36);
            this.toolStripMenuItem1.Text = "Options";
            // 
            // toolStripMenuItemTurnOffControlOnExit
            // 
            this.toolStripMenuItemTurnOffControlOnExit.CheckOnClick = true;
            this.toolStripMenuItemTurnOffControlOnExit.Name = "toolStripMenuItemTurnOffControlOnExit";
            this.toolStripMenuItemTurnOffControlOnExit.Size = new System.Drawing.Size(418, 44);
            this.toolStripMenuItemTurnOffControlOnExit.Text = "Turn off control on exit";
            this.toolStripMenuItemTurnOffControlOnExit.CheckedChanged += new System.EventHandler(this.toolStripMenuItemTurnOffControlOnExit_CheckedChanged);
            // 
            // toolStripMenuItemForbidUnsafeSettings
            // 
            this.toolStripMenuItemForbidUnsafeSettings.CheckOnClick = true;
            this.toolStripMenuItemForbidUnsafeSettings.Name = "toolStripMenuItemForbidUnsafeSettings";
            this.toolStripMenuItemForbidUnsafeSettings.Size = new System.Drawing.Size(418, 44);
            this.toolStripMenuItemForbidUnsafeSettings.Text = "Forbid unsafe settings";
            this.toolStripMenuItemForbidUnsafeSettings.CheckedChanged += new System.EventHandler(this.ToolStripMenuItemForbidUnsafeSettings_CheckedChanged);
            // 
            // toolStripMenuItemMinimizeToTrayOnClose
            // 
            this.toolStripMenuItemMinimizeToTrayOnClose.CheckOnClick = true;
            this.toolStripMenuItemMinimizeToTrayOnClose.Name = "toolStripMenuItemMinimizeToTrayOnClose";
            this.toolStripMenuItemMinimizeToTrayOnClose.Size = new System.Drawing.Size(418, 44);
            this.toolStripMenuItemMinimizeToTrayOnClose.Text = "Minimize to tray on close";
            this.toolStripMenuItemMinimizeToTrayOnClose.Click += new System.EventHandler(this.toolStripMenuItemMinimizeToTrayOnClose_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(415, 6);
            // 
            // toolStripMenuItemAutoRefreshStats
            // 
            this.toolStripMenuItemAutoRefreshStats.CheckOnClick = true;
            this.toolStripMenuItemAutoRefreshStats.Name = "toolStripMenuItemAutoRefreshStats";
            this.toolStripMenuItemAutoRefreshStats.Size = new System.Drawing.Size(418, 44);
            this.toolStripMenuItemAutoRefreshStats.Text = "Auto refresh stats";
            this.toolStripMenuItemAutoRefreshStats.Click += new System.EventHandler(this.ToolStripMenuItemAutoRefreshStats_Click);
            // 
            // startWithWindowsToolStripMenuItem
            // 
            this.startWithWindowsToolStripMenuItem.Name = "startWithWindowsToolStripMenuItem";
            this.startWithWindowsToolStripMenuItem.Size = new System.Drawing.Size(418, 44);
            this.startWithWindowsToolStripMenuItem.Text = "Start with Windows";
            this.startWithWindowsToolStripMenuItem.Click += new System.EventHandler(this.startWithWindowsToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemCheckForUpdates,
            this.toolStripMenuItem4,
            this.creditsToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(84, 36);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // toolStripMenuItemCheckForUpdates
            // 
            this.toolStripMenuItemCheckForUpdates.Name = "toolStripMenuItemCheckForUpdates";
            this.toolStripMenuItemCheckForUpdates.Size = new System.Drawing.Size(359, 44);
            this.toolStripMenuItemCheckForUpdates.Text = "Check for updates";
            this.toolStripMenuItemCheckForUpdates.Click += new System.EventHandler(this.toolStripMenuItemCheckForUpdates_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(356, 6);
            // 
            // creditsToolStripMenuItem
            // 
            this.creditsToolStripMenuItem.Name = "creditsToolStripMenuItem";
            this.creditsToolStripMenuItem.Size = new System.Drawing.Size(359, 44);
            this.creditsToolStripMenuItem.Text = "Credits";
            this.creditsToolStripMenuItem.Click += new System.EventHandler(this.creditsToolStripMenuItem_Click);
            // 
            // MaxFans
            // 
            this.MaxFans.Location = new System.Drawing.Point(29, 115);
            this.MaxFans.Margin = new System.Windows.Forms.Padding(4);
            this.MaxFans.Name = "MaxFans";
            this.MaxFans.Size = new System.Drawing.Size(200, 48);
            this.MaxFans.TabIndex = 7;
            this.MaxFans.Text = "Max Speed";
            this.MaxFans.UseVisualStyleBackColor = true;
            this.MaxFans.Click += new System.EventHandler(this.MaxFans_Click);
            // 
            // FansOff
            // 
            this.FansOff.Location = new System.Drawing.Point(258, 115);
            this.FansOff.Margin = new System.Windows.Forms.Padding(4);
            this.FansOff.Name = "FansOff";
            this.FansOff.Size = new System.Drawing.Size(200, 48);
            this.FansOff.TabIndex = 8;
            this.FansOff.Text = "Fans Off";
            this.FansOff.UseVisualStyleBackColor = true;
            this.FansOff.Click += new System.EventHandler(this.FansOff_Click);
            // 
            // FanCurve
            // 
            this.FanCurve.Location = new System.Drawing.Point(19, 45);
            this.FanCurve.Margin = new System.Windows.Forms.Padding(6);
            this.FanCurve.Name = "FanCurve";
            this.FanCurve.Size = new System.Drawing.Size(205, 71);
            this.FanCurve.TabIndex = 34;
            this.FanCurve.Text = "Edit Fan Curve";
            this.FanCurve.UseVisualStyleBackColor = true;
            this.FanCurve.Click += new System.EventHandler(this.FanCurve_Click);
            // 
            // manualControlGroupBox
            // 
            this.manualControlGroupBox.Controls.Add(this.trackBarFanSpeed);
            this.manualControlGroupBox.Controls.Add(this.label1);
            this.manualControlGroupBox.Controls.Add(this.MaxFans);
            this.manualControlGroupBox.Controls.Add(this.FansOff);
            this.manualControlGroupBox.Location = new System.Drawing.Point(12, 146);
            this.manualControlGroupBox.Name = "manualControlGroupBox";
            this.manualControlGroupBox.Size = new System.Drawing.Size(477, 233);
            this.manualControlGroupBox.TabIndex = 46;
            this.manualControlGroupBox.TabStop = false;
            this.manualControlGroupBox.Text = "Manual Control";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.fanCurve2RadioButton);
            this.groupBox2.Controls.Add(this.fanCurve1RadioButton);
            this.groupBox2.Controls.Add(this.manualModeRadioButton);
            this.groupBox2.Controls.Add(this.FanCurve);
            this.groupBox2.Location = new System.Drawing.Point(495, 60);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(244, 319);
            this.groupBox2.TabIndex = 47;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Fan Curve";
            // 
            // fanCurve2RadioButton
            // 
            this.fanCurve2RadioButton.AutoSize = true;
            this.fanCurve2RadioButton.Location = new System.Drawing.Point(19, 250);
            this.fanCurve2RadioButton.Name = "fanCurve2RadioButton";
            this.fanCurve2RadioButton.Size = new System.Drawing.Size(205, 29);
            this.fanCurve2RadioButton.TabIndex = 37;
            this.fanCurve2RadioButton.TabStop = true;
            this.fanCurve2RadioButton.Text = "Use Fan Curve 2\n";
            this.fanCurve2RadioButton.UseVisualStyleBackColor = true;
            this.fanCurve2RadioButton.CheckedChanged += new System.EventHandler(this.RadioButton_Mode_CheckedChanged);
            // 
            // fanCurve1RadioButton
            // 
            this.fanCurve1RadioButton.AutoSize = true;
            this.fanCurve1RadioButton.Location = new System.Drawing.Point(19, 200);
            this.fanCurve1RadioButton.Name = "fanCurve1RadioButton";
            this.fanCurve1RadioButton.Size = new System.Drawing.Size(205, 29);
            this.fanCurve1RadioButton.TabIndex = 36;
            this.fanCurve1RadioButton.TabStop = true;
            this.fanCurve1RadioButton.Text = "Use Fan Curve 1\n";
            this.fanCurve1RadioButton.UseVisualStyleBackColor = true;
            this.fanCurve1RadioButton.CheckedChanged += new System.EventHandler(this.RadioButton_Mode_CheckedChanged);
            // 
            // manualModeRadioButton
            // 
            this.manualModeRadioButton.AutoSize = true;
            this.manualModeRadioButton.Location = new System.Drawing.Point(19, 150);
            this.manualModeRadioButton.Name = "manualModeRadioButton";
            this.manualModeRadioButton.Size = new System.Drawing.Size(173, 29);
            this.manualModeRadioButton.TabIndex = 35;
            this.manualModeRadioButton.TabStop = true;
            this.manualModeRadioButton.Text = "Manual mode";
            this.manualModeRadioButton.UseVisualStyleBackColor = true;
            this.manualModeRadioButton.CheckedChanged += new System.EventHandler(this.RadioButton_Mode_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label3);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.nudHysteresis);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.numericUpDown1);
            this.groupBox5.Controls.Add(this.labelCPUTemp);
            this.groupBox5.Controls.Add(this.buttonRefreshCPUTemp);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.labelRPM);
            this.groupBox5.Controls.Add(this.buttonRefreshRPM);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Location = new System.Drawing.Point(12, 385);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(727, 161);
            this.groupBox5.TabIndex = 50;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Live Stats and Settings";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(663, 48);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 25);
            this.label6.TabIndex = 57;
            this.label6.Text = "°C";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(419, 48);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(113, 25);
            this.label3.TabIndex = 56;
            this.label3.Text = "Hysteresis";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(663, 100);
            this.label5.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(40, 25);
            this.label5.TabIndex = 55;
            this.label5.Text = "ms";
            // 
            // nudHysteresis
            // 
            this.nudHysteresis.Location = new System.Drawing.Point(544, 46);
            this.nudHysteresis.Margin = new System.Windows.Forms.Padding(6);
            this.nudHysteresis.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudHysteresis.Name = "nudHysteresis";
            this.nudHysteresis.Size = new System.Drawing.Size(114, 31);
            this.nudHysteresis.TabIndex = 54;
            this.nudHysteresis.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudHysteresis.ValueChanged += new System.EventHandler(this.nudHysteresis_ValueChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(394, 100);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(138, 25);
            this.label10.TabIndex = 53;
            this.label10.Text = "Refresh Rate";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(544, 98);
            this.numericUpDown1.Margin = new System.Windows.Forms.Padding(6);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(114, 31);
            this.numericUpDown1.TabIndex = 52;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // labelCPUTemp
            // 
            this.labelCPUTemp.AutoSize = true;
            this.labelCPUTemp.Location = new System.Drawing.Point(237, 100);
            this.labelCPUTemp.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelCPUTemp.Name = "labelCPUTemp";
            this.labelCPUTemp.Size = new System.Drawing.Size(19, 25);
            this.labelCPUTemp.TabIndex = 49;
            this.labelCPUTemp.Text = "-";
            // 
            // buttonRefreshCPUTemp
            // 
            this.buttonRefreshCPUTemp.Location = new System.Drawing.Point(9, 90);
            this.buttonRefreshCPUTemp.Margin = new System.Windows.Forms.Padding(6);
            this.buttonRefreshCPUTemp.Name = "buttonRefreshCPUTemp";
            this.buttonRefreshCPUTemp.Size = new System.Drawing.Size(44, 44);
            this.buttonRefreshCPUTemp.TabIndex = 51;
            this.buttonRefreshCPUTemp.Text = "↻";
            this.toolTip1.SetToolTip(this.buttonRefreshCPUTemp, "Refresh Now");
            this.buttonRefreshCPUTemp.UseVisualStyleBackColor = true;
            this.buttonRefreshCPUTemp.Click += new System.EventHandler(this.UpdateStats_Tick);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(65, 100);
            this.label4.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(160, 25);
            this.label4.TabIndex = 48;
            this.label4.Text = "Current Temps:";
            // 
            // labelRPM
            // 
            this.labelRPM.AutoSize = true;
            this.labelRPM.Location = new System.Drawing.Point(211, 38);
            this.labelRPM.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelRPM.Name = "labelRPM";
            this.labelRPM.Size = new System.Drawing.Size(19, 25);
            this.labelRPM.TabIndex = 47;
            this.labelRPM.Text = "-";
            // 
            // buttonRefreshRPM
            // 
            this.buttonRefreshRPM.Location = new System.Drawing.Point(9, 38);
            this.buttonRefreshRPM.Margin = new System.Windows.Forms.Padding(6);
            this.buttonRefreshRPM.Name = "buttonRefreshRPM";
            this.buttonRefreshRPM.Size = new System.Drawing.Size(44, 44);
            this.buttonRefreshRPM.TabIndex = 50;
            this.buttonRefreshRPM.Text = "↻";
            this.toolTip1.SetToolTip(this.buttonRefreshRPM, "Refresh Now");
            this.buttonRefreshRPM.UseVisualStyleBackColor = true;
            this.buttonRefreshRPM.Click += new System.EventHandler(this.UpdateStats_Tick);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(65, 48);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 25);
            this.label2.TabIndex = 46;
            this.label2.Text = "Fan Speeds:";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxTurnOn);
            this.groupBox4.Location = new System.Drawing.Point(12, 60);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(477, 80);
            this.groupBox4.TabIndex = 49;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Master Control";
            // 
            // checkBoxTurnOn
            // 
            this.checkBoxTurnOn.AutoSize = true;
            this.checkBoxTurnOn.Location = new System.Drawing.Point(9, 33);
            this.checkBoxTurnOn.Margin = new System.Windows.Forms.Padding(6);
            this.checkBoxTurnOn.Name = "checkBoxTurnOn";
            this.checkBoxTurnOn.Size = new System.Drawing.Size(308, 29);
            this.checkBoxTurnOn.TabIndex = 1;
            this.checkBoxTurnOn.Text = "Enable Custom Fan Control";
            this.checkBoxTurnOn.UseVisualStyleBackColor = true;
            this.checkBoxTurnOn.CheckedChanged += new System.EventHandler(this.checkBoxTurnOn_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(751, 558);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.manualControlGroupBox);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Zenith Fan Utility";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.VisibleChanged += new System.EventHandler(this.Form1_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFanSpeed)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.manualControlGroupBox.ResumeLayout(false);
            this.manualControlGroupBox.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHysteresis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTurnOffControlOnExit;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemForbidUnsafeSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemMinimizeToTrayOnClose;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemAutoRefreshStats;
        private System.Windows.Forms.Button MaxFans;
        private System.Windows.Forms.Button FansOff;
        private Button FanCurve;
        private ToolStripMenuItem startWithWindowsToolStripMenuItem;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, int vk);
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem helpToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItemCheckForUpdates;
        private ToolStripSeparator toolStripMenuItem4;
        private ToolStripMenuItem creditsToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private GroupBox manualControlGroupBox;
        private GroupBox groupBox2;
        private RadioButton fanCurve2RadioButton;
        private RadioButton fanCurve1RadioButton;
        private RadioButton manualModeRadioButton;
        private GroupBox groupBox5;
        private Label label6;
        private Label label3;
        private Label label5;
        private NumericUpDown nudHysteresis;
        private Label label10;
        private NumericUpDown numericUpDown1;
        private Label labelCPUTemp;
        private Button buttonRefreshCPUTemp;
        private Label label4;
        private Label labelRPM;
        private Button buttonRefreshRPM;
        private Label label2;
        private ToolTip toolTip1;
        private GroupBox groupBox4;
        private CheckBox checkBoxTurnOn;
        private TrackBar trackBarFanSpeed;
    }
    }


