using System;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;
using System.Collections.Generic;

namespace ZenithFanUtility
{
    partial class Form2
    {
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.addpoint = new System.Windows.Forms.Button();
            this.reset = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new ZenithFanUtility.DoubleBufferedPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(450, 712);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(332, 64);
            this.buttonSave.TabIndex = 0;
            this.buttonSave.Text = "Save Everything and Close";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click_1);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Location = new System.Drawing.Point(56, 712);
            this.buttonCancel.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(332, 64);
            this.buttonCancel.TabIndex = 1;
            this.buttonCancel.Text = "Close Without Saving";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click_1);
            // 
            // addpoint
            // 
            this.addpoint.Location = new System.Drawing.Point(160, 15);
            this.addpoint.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.addpoint.Name = "addpoint";
            this.addpoint.Size = new System.Drawing.Size(228, 52);
            this.addpoint.TabIndex = 7;
            this.addpoint.Text = "Add New Point";
            this.addpoint.UseVisualStyleBackColor = true;
            this.addpoint.Click += new System.EventHandler(this.addpoint_Click_1);
            // 
            // reset
            // 
            this.reset.Location = new System.Drawing.Point(450, 15);
            this.reset.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.reset.Name = "reset";
            this.reset.Size = new System.Drawing.Size(228, 52);
            this.reset.TabIndex = 8;
            this.reset.Text = "Reset Points";
            this.reset.UseVisualStyleBackColor = true;
            this.reset.Click += new System.EventHandler(this.reset_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(269, 643);
            this.label3.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(297, 25);
            this.label3.TabIndex = 9;
            this.label3.Text = "(Right click a point to remove)";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(56, 630);
            this.button1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(168, 51);
            this.button1.TabIndex = 10;
            this.button1.Text = "Load Profile 1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(613, 630);
            this.button2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(168, 51);
            this.button2.TabIndex = 11;
            this.button2.Text = "Load Profile 2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(12, 76);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(819, 546);
            this.pictureBox1.TabIndex = 13;
            this.pictureBox1.TabStop = false;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(843, 794);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.reset);
            this.Controls.Add(this.addpoint);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "Form2";
            this.Text = "Edit Fan Curve";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.FanCurveEnabled = false;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void InitializeAddPointButton()
        {
            addpoint.Click += Addpoint_Click;
        }

        private void Addpoint_Click(object sender, EventArgs e)
        {
            int newPointId = fanCurvePoints.Keys.Count > 0 ? fanCurvePoints.Keys.Max() + 1 : 1;
            fanCurvePoints[newPointId] = new Point(50, 50); // Point is now recognized
            pictureBox1.Invalidate();
        }
        private System.Windows.Forms.Button addpoint;
        private void PictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var pointToRemove = fanCurvePoints.FirstOrDefault(p => p.Value == new Point(e.X, e.Y));
                if (!pointToRemove.Equals(default(KeyValuePair<int, Point>)))
                {
                    fanCurvePoints.Remove(pointToRemove.Key);
                    pictureBox1.Invalidate();
                }
            }
        }

        private Button reset;
        private Label label3;
        private Button button1;
        private Button button2;
        private DoubleBufferedPictureBox pictureBox1;
    }
}