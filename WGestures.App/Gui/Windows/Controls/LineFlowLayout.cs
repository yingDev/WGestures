using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WGestures.App.Gui.Windows.Controls
{
    class LineFlowLayout : FlowLayoutPanel
    {
        [Browsable(true)]
        public bool Vertical { get; set; }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);

            using (var pen = new Pen(this.ForeColor))
            {
                if(!Vertical) e.Graphics.DrawLine(pen, 0, this.Height / 2, this.Width, this.Height / 2);
                else
                {
                    e.Graphics.DrawLine(pen,this.Width/2,0,this.Width/2,this.Height);
                }
            }
        }
    }
}
