using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

class IconGenerator {
    static void Main() {
        string srcPath = @"d:\sora-hr-frontend\sora_mobile_flutter\assets\sora-hr-icon-512.png";
        if (!File.Exists(srcPath)) {
            srcPath = @"d:\sora-hr-frontend\sora-hr-icon-512.png";
        }

        Console.WriteLine("Source icon: " + srcPath);
        using (Image srcImg = Image.FromFile(srcPath)) {
            var densities = new Tuple<string, int>[] {
                Tuple.Create("mipmap-mdpi", 48),
                Tuple.Create("mipmap-hdpi", 72),
                Tuple.Create("mipmap-xhdpi", 96),
                Tuple.Create("mipmap-xxhdpi", 144),
                Tuple.Create("mipmap-xxxhdpi", 192)
            };

            string outBase = @"d:\sora-hr-frontend\sora_mobile_flutter\android_res";

            foreach (var d in densities) {
                string folder = Path.Combine(outBase, d.Item1);
                Directory.CreateDirectory(folder);

                int size = d.Item2;
                using (Bitmap bmp = new Bitmap(size, size)) {
                    using (Graphics g = Graphics.FromImage(bmp)) {
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        g.Clear(Color.Transparent);
                        g.DrawImage(srcImg, 0, 0, size, size);
                    }

                    string iconPath = Path.Combine(folder, "ic_launcher.png");
                    bmp.Save(iconPath, ImageFormat.Png);

                    string roundPath = Path.Combine(folder, "ic_launcher_round.png");
                    bmp.Save(roundPath, ImageFormat.Png);

                    Console.WriteLine(string.Format("Generated {0} ({1}x{1})", d.Item1, size));
                }
            }
        }
        Console.WriteLine("ALL ICONS GENERATED SUCCESSFULLY!");
    }
}
