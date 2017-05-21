namespace OpenCVSharpBox
{
    using System.Diagnostics;
    using System.Windows.Controls;
    using OpenCvSharp;
    using OpenCvSharp.Extensions;

    public partial class CascadeClassifierView : UserControl
    {
        public CascadeClassifierView()
        {
            this.InitializeComponent();

            using (var image = new Mat("Images//3SquaresBW.bmp", ImreadModes.GrayScale))
            {
                var sw = Stopwatch.StartNew();
                using (var classifier = new CascadeClassifier("data\\cascade.xml"))
                {
                    var matches = classifier.DetectMultiScale(image);
                    this.Status.Text = $"{sw.ElapsedMilliseconds} ms";
                    using (var overLay = image.OverLay())
                    {
                        foreach (var match in matches)
                        {
                            Cv2.Rectangle(overLay, match, Scalar4.Red);
                        }

                        this.Result.Source = overLay.ToBitmapSource();
                    }
                }
            }
        }
    }
}
