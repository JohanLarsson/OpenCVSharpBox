namespace OpenCVSharpBox
{
    using System.Diagnostics;
    using System.Windows.Controls;
    using OpenCvSharp;
    using OpenCvSharp.Extensions;
    using OpenCvSharp.XFeatures2D;

    public partial class SURFView : UserControl
    {
        public SURFView()
        {
            this.InitializeComponent();

            using (var image = new Mat("Images//3SquaresWB.bmp", ImreadModes.GrayScale))
            {
                var sw = Stopwatch.StartNew();
                using (var surf = SURF.Create(200))
                {
                    var keyPoints = surf.Detect(image);
                    this.Status.Text = $"{sw.ElapsedMilliseconds} ms";
                    using (var overLay = image.Overlay())
                    {
                        Cv2.DrawKeypoints(image, keyPoints, overLay);
                        this.Result.Source = overLay.ToBitmapSource();
                    }
                }
            }
        }
    }
}
