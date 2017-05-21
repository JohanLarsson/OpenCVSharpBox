namespace OpenCVSharpBox.Tests
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Text;
    using NUnit.Framework;

    public class CascadeTests
    {

        //[NUnit.Framework.Test]
        //public void Classifier()
        //{
        //    using (var mat = new Mat(ThreeSquaresWB, ImreadModes.GrayScale))
        //    {
        //        var classifier = new CascadeClassifier();
        //        classifier.
        //    }
        //}

        [Explicit("Script")]
        [Test]
        public void Train()
        {
            Directory.CreateDirectory(FullFileName("Positives"));
            var index = new StringBuilder();
            using (var image = new Bitmap(100, 100))
            {
                using (var graphics = Graphics.FromImage(image))
                {
                    var rnd = new Random();
                    graphics.Clear(Color.FromArgb(1, rnd.Next(0, 20), rnd.Next(0, 20), rnd.Next(0, 20)));
                    for (var i = 0; i < 40; i++)
                    {
                        graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, rnd.Next(25, 255), rnd.Next(25, 255), rnd.Next(25, 255))), 10, 10, 80, 80);
                        var fileName = FullFileName($"Positives\\{i}.bmp");
                        index.AppendLine(fileName);
                        image.Save(fileName, ImageFormat.Bmp);
                    }
                }
            }

            File.WriteAllText(FullFileName("Positives.txt"), index.ToString());
        }

        [Explicit("Script")]
        [Test]
        public void DumpPositives()
        {
            Directory.CreateDirectory(FullFileName("Positives"));
            var index = new StringBuilder();
            using (var image = new Bitmap(100, 100))
            {
                using (var graphics = Graphics.FromImage(image))
                {
                    var rnd = new Random();
                    graphics.Clear(Color.FromArgb(1, rnd.Next(0, 20), rnd.Next(0, 20), rnd.Next(0, 20)));
                    for (var i = 0; i < 40; i++)
                    {
                        graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, rnd.Next(25, 255), rnd.Next(25, 255), rnd.Next(25, 255))), 10, 10, 80, 80);
                        var fileName = FullFileName($"Positives\\{i}.bmp");
                        index.AppendLine(fileName);
                        image.Save(fileName, ImageFormat.Bmp);
                    }
                }
            }

            File.WriteAllText(FullFileName("Positives.txt"), index.ToString());
        }

        [Explicit("Script")]
        [Test]
        public void DumpNegatives()
        {
            Directory.CreateDirectory(FullFileName("Negatives"));
            var index = new StringBuilder();
            using (var client = new WebClient())
            {
                using (var stream = client.OpenRead("http://i.imgur.com/xT3ay.jpg"))
                {
                    using (var source = new Bitmap(stream))
                    {
                        //// source.Save($"C:\\Temp\\Negatives\\Source.bmp", ImageFormat.Bmp);
                        using (var target = new Bitmap(100, 100))
                        {
                            using (var graphics = Graphics.FromImage(target))
                            {
                                var rnd = new Random();
                                for (var i = 0; i < 600; i++)
                                {
                                    var sourceRect = new Rectangle(rnd.Next(0, source.Width - 100), rnd.Next(0, source.Height - 100), target.Width, target.Height);
                                    var targetRect = new Rectangle(0, 0, target.Width, target.Width);
                                    graphics.DrawImage(source, targetRect, sourceRect, GraphicsUnit.Pixel);
                                    var fileName = FullFileName($"Negatives\\{i}.bmp");
                                    index.AppendLine(fileName);
                                    target.Save(fileName, ImageFormat.Bmp);
                                }
                            }
                        }
                    }
                }
            }

            File.WriteAllText(FullFileName("Negatives.txt"), index.ToString());
        }

        private static string FullFileName(string name)
        {
            return System.IO.Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, name);
        }
    }
}