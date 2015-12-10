using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebImageMaker
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = args[0];
            DirectoryInfo targetDirectory = Directory.CreateDirectory(Path.Combine(sourcePath, "output"));

            Directory.GetFiles(args[0], "*.png").ToList().ForEach(file =>
                {
                    string destFolderName = Path.GetFileNameWithoutExtension(file);
                    Console.WriteLine("Re-sizing {0}", destFolderName);
                    DirectoryInfo imageDestination = Directory
                        .CreateDirectory(Path.Combine(targetDirectory.FullName, destFolderName));
                    using (Image originalImage = Image.FromFile(file))
                    {
                        using (Bitmap xl = ResizeImage(originalImage, 600))
                        {
                            xl.Save(Path.Combine(imageDestination.FullName, string.Format("{0}_XL.png", destFolderName)));
                        }

                        using (Bitmap l = ResizeImage(originalImage, 500))
                        {
                            l.Save(Path.Combine(imageDestination.FullName, string.Format("{0}_L.png", destFolderName)));
                        }

                        using (Bitmap m = ResizeImage(originalImage, 400))
                        {
                            m.Save(Path.Combine(imageDestination.FullName, string.Format("{0}_M.png", destFolderName)));
                        }

                        using (Bitmap s = ResizeImage(originalImage, 300))
                        {
                            s.Save(Path.Combine(imageDestination.FullName, string.Format("{0}_S.png", destFolderName)));
                        }

                        using (Bitmap t = ResizeImage(originalImage, 200))
                        {
                            t.Save(Path.Combine(targetDirectory.FullName, string.Format("{0}.png", destFolderName)));
                        }
                    }
                });
        }

        public static Bitmap ResizeImage(Image image, int width)
        {
            int height = (int)((1.0 * width / image.Width) * image.Height);
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
    }
}
