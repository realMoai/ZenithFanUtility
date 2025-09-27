using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
public class DarkColorTable : ProfessionalColorTable
{
    private Color backColor = Color.FromArgb(38, 38, 38);
    private Color selectedColor = Color.FromArgb(65, 65, 68);
    private Color borderColor = Color.FromArgb(80, 80, 80);

    public override Color MenuStripGradientBegin => backColor;
    public override Color MenuStripGradientEnd => backColor;
    public override Color ToolStripDropDownBackground => backColor;
    public override Color ImageMarginGradientBegin => backColor;
    public override Color ImageMarginGradientMiddle => backColor;
    public override Color ImageMarginGradientEnd => backColor;
    public override Color MenuItemSelected => selectedColor;
    public override Color MenuItemBorder => borderColor;
    public override Color MenuBorder => borderColor;
    public override Color SeparatorDark => borderColor;
    public override Color MenuItemPressedGradientBegin => selectedColor;
    public override Color MenuItemPressedGradientEnd => selectedColor;
    public override Color MenuItemSelectedGradientBegin => selectedColor;
    public override Color MenuItemSelectedGradientEnd => selectedColor;
}

public class DarkMenuRenderer : ToolStripProfessionalRenderer
{
    public DarkMenuRenderer() : base(new DarkColorTable()) { }

    protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
    {
        e.TextColor = Color.White;
        base.OnRenderItemText(e);
    }

    protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
    {
        if (e.Item is ToolStripMenuItem tsmi && tsmi.Checked)
        {
            var g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            var rect = e.ImageRectangle;
            rect.Inflate(-4, -4); // 4px padding looks clean

            var p1 = new Point(rect.Left, rect.Top + rect.Height / 2);
            var p2 = new Point(rect.Left + rect.Width / 3, rect.Bottom);
            var p3 = new Point(rect.Right, rect.Top);

            // draw the checkmark with a 2px thick, rounded pen
            using (var pen = new Pen(Color.WhiteSmoke, 2f))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                g.DrawLines(pen, new[] { p1, p2, p3 });
            }
        }
    }
}