namespace OpenCVSharpBox.Tests
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Text;
    using NUnit.Framework;
    using OpenCvSharp;

    public class CascadeTests
    {
        // 1 Download http://opencv.org/releases.html win pack
        // Extract
        // c:\opencv\opencv_createsamples.exe
        // c:\opencv\opencv_traincascade.exe
        // c:\opencv\opencv_world320.dll
        // c:\opencv\opencv_world320d.dll

        private static string ThreeSquaresWB = FullFileName("Images\\3SquaresWB.bmp");

        [NUnit.Framework.Test]
        public void Classifier()
        {
            using (var mat = new Mat(ThreeSquaresWB, ImreadModes.GrayScale))
            {
                using (var classifier = new CascadeClassifier(FullFileName("Squares.xml")))
                {
                    var matches = classifier.DetectMultiScale(new Mat(ThreeSquaresWB, ImreadModes.Unchanged));
                    Assert.AreEqual(3, matches.Length);
                }
            }
        }

        [Explicit("Script")]
        [Test]
        public void Train()
        {
            const string OpenCvDir = "C:\\opencv";
            using (var process = Process.Start(
                new ProcessStartInfo
                {
                    FileName = Path.Combine(OpenCvDir, "opencv_createsamples.exe"),
                    WorkingDirectory = FullFileName("Data"),
                    Arguments = $"-info squares.info -vec squares.vec -w 24 -h 24 -num 40"
                }))
            {
                process.WaitForExit();
            }

            if (false)
            {
                // for previewing the vec file
                using (var process = Process.Start(new ProcessStartInfo
                {
                    FileName = Path.Combine(OpenCvDir, "opencv_createsamples.exe"),
                    WorkingDirectory = FullFileName("Data"),
                    Arguments = $"-vec squares.vec -w 24 -h 24"
                }))
                {
                    process.WaitForExit();
                }
            }

            using (var process = Process.Start(new ProcessStartInfo
            {
                FileName = Path.Combine(OpenCvDir, "opencv_traincascade.exe"),
                WorkingDirectory = FullFileName("Data"),
                Arguments = $"-data data -vec squares.vec -bg bg.txt -numPos 40 -numNeg 600 -w 24 -h 24"
            }))
            {
                process.WaitForExit();
            }
        }

        [Explicit("Script")]
        [Test]
        public void DumpPositives()
        {
            Directory.CreateDirectory(FullFileName("Data\\Positives"));
            var index = new StringBuilder();
            using (var image = new Bitmap(24, 24))
            {
                using (var graphics = Graphics.FromImage(image))
                {
                    var rnd = new Random();
                    graphics.Clear(Color.FromArgb(255, rnd.Next(0, 20), rnd.Next(0, 20), rnd.Next(0, 20)));
                    for (var i = 0; i < 40; i++)
                    {
                        graphics.FillRectangle(
                            new SolidBrush(Color.FromArgb(255, rnd.Next(25, 255), rnd.Next(25, 255), rnd.Next(25, 255))),
                            new Rectangle(0, 0, 24, 24));
                        var fileName = FullFileName($"Data\\Positives\\{i}.bmp");
                        index.AppendLine($"{fileName} 1 0 0 24 24");
                        image.Save(fileName, ImageFormat.Bmp);
                    }
                }
            }

            File.WriteAllText(FullFileName("Data\\squares.info"), index.ToString());
        }

        [Explicit("Script")]
        [Test]
        public void DumpNegatives()
        {
            Directory.CreateDirectory(FullFileName("Data\\Negatives"));
            var index = new StringBuilder();
            using (var client = new WebClient())
            {
                using (var stream = client.OpenRead("http://i.imgur.com/xT3ay.jpg"))
                {
                    using (var source = new Bitmap(stream))
                    {
                        //// source.Save($"C:\\Temp\\Negatives\\Source.bmp", ImageFormat.Bmp);
                        using (var target = new Bitmap(24, 24))
                        {
                            using (var graphics = Graphics.FromImage(target))
                            {
                                var rnd = new Random();
                                for (var i = 0; i < 600; i++)
                                {
                                    var sourceRect = new Rectangle(rnd.Next(0, source.Width - 24), rnd.Next(0, source.Height - 24), target.Width, target.Height);
                                    var targetRect = new Rectangle(0, 0, target.Width, target.Width);
                                    graphics.DrawImage(source, targetRect, sourceRect, GraphicsUnit.Pixel);
                                    var fileName = FullFileName($"Data\\Negatives\\{i}.bmp");
                                    index.AppendLine(fileName);
                                    target.Save(fileName, ImageFormat.Bmp);
                                }
                            }
                        }
                    }
                }
            }

            File.WriteAllText(FullFileName("Data\\bg.txt"), index.ToString());
        }

        private static string FullFileName(string name)
        {
            return System.IO.Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, name);
        }
    }
}