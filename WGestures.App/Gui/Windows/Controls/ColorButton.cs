using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using WGestures.Common.OsSpecific.Windows;

namespace WGestures.App.Gui.Windows.Controls
{
    internal class ColorButton : LazyPaintButton
    {
        private float _dpiFactor = Native.GetScreenDpi() / 96.0f;

        private Color _color = Color.Transparent;
        public Color Color
        {
            get { return _color; }
            set
            {
                _color = value;
                mainPen.Color = _color;
                Invalidate();
                if (ColorChanged != null) ColorChanged(this, new EventArgs());
            }
        }

        private Pen mainPen;
        private Pen borderPen;
        private Pen shadowPen;

        public event EventHandler ColorChanged;

        public ColorButton()
        {
            mainPen = new Pen(Color, 2.0f * _dpiFactor);
            borderPen = new Pen(Color.FromArgb(255, 255, 255, 255), 4.5f * _dpiFactor);
            shadowPen = new Pen(Color.FromArgb(80, 0, 0, 0), 5.0f * _dpiFactor);
        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            
            base.OnPaint(pevent);
            var g = pevent.Graphics;

            g.SetClip(pevent.ClipRectangle);

            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            //g.Clear(BackColor);

            var bias = 5 * _dpiFactor;

            var rect = Rectangle.Inflate(Bounds, (int)-bias, (int)-bias);
            rect.X = (int)bias;
            rect.Y = (int)bias;
            
            /*g.DrawEllipse(shadowPen, rect);
            g.DrawEllipse(borderPen, rect);
            g.DrawEllipse(mainPen, rect);*/

            var p0 = new Point((int) rect.Left, (int) rect.Bottom);
            var p1 = new Point((int) rect.Right, (int) rect.Bottom);
            g.DrawLine(shadowPen, p0, p1);
            g.DrawLine(borderPen, p0, p1);
            g.DrawLine(mainPen, p0, p1);
            /*g.DrawRectangle(shadowPen, rect);
            g.DrawRectangle(borderPen, rect);
            g.DrawRectangle(mainPen, rect);*/
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            using (var colorDlg = new ColorDialog())
            {
                colorDlg.AnyColor = true;
                colorDlg.FullOpen = true;
                
                colorDlg.Color = Color;

                var ok = colorDlg.ShowDialog();
                if (ok == DialogResult.OK)
                {
                    Color = colorDlg.Color;
                }
            }

        }

        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                mainPen.Dispose();
                borderPen.Dispose();
                shadowPen.Dispose();
            }

            base.Dispose(disposing);

        }
    }
}
