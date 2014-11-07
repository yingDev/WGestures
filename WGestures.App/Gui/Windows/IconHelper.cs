using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WGestures.App.Properties;
using WGestures.Core;

namespace WGestures.App.Gui.Windows
{
    public static class IconHelper
    {
        public static Bitmap ExtractIconForPath(string path, Size size, bool enabled = true, int bias=2)
        {
            var bmp = new Bitmap(size.Width, size.Height);

            using (var g = Graphics.FromImage(bmp))
            {
                if (!File.Exists(path))
                {
                    g.DrawImage(Resources.unknown, new Rectangle(bias, bias, bmp.Width - 2 * bias, bmp.Height - 2 * bias));
                }
                else
                {
                    var icon = Icon.ExtractAssociatedIcon(path);
                    g.DrawIcon(icon, new Rectangle(bias, bias, bmp.Width - 2 * bias, bmp.Height - 2 * bias));
                }

                if (enabled) return bmp;
            }

            //如果禁用了，则灰度处理
            using (bmp)
            {
                var grayed = MakeGrayscale3(bmp);
                using (var g = Graphics.FromImage(grayed))
                using (var forbidSymbol = Properties.Resources.forbidden)
                {
                    var fordibSize = bmp.Width / 3;
                    g.DrawImage(forbidSymbol, bmp.Width - fordibSize - bias, bmp.Height - fordibSize - bias, fordibSize, fordibSize);
                }

                return grayed;
            }
        }

        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);

            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][] {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}});

            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();

            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);

            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);

            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }

        public static bool AppExists(this ExeApp app)
        {
            return File.Exists(app.ExecutablePath);
        }

    }
}
