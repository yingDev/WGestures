using System.Drawing;
using System.Windows.Forms;

namespace WGestures.App.Gui.Windows.Controls
{
    class LineLabel : Label
    {
        public bool IsVertical { get; set; }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            base.OnPaintBackground(pevent);
            var g = pevent.Graphics;
            g.SetClip(pevent.ClipRectangle);

            using (var pen = new Pen(this.ForeColor))
            {
                if (IsVertical)
                {
                    g.DrawLine(pen, Width/2,0, this.Width/2, this.Height); 

                }
               
                else g.DrawLine(pen,0,this.Height /2 ,this.Width,this.Height/2); 
            }
            
        }
    }
}
