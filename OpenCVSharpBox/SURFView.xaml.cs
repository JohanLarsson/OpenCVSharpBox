namespace OpenCVSharpBox
{
    using System.Windows.Controls;
    using OpenCvSharp;
    using OpenCvSharp.Extensions;
    using OpenCvSharp.XFeatures2D;

    public partial class SURFView : UserControl
    {
        public SURFView()
        {
            InitializeComponent();

            using (var image = new Mat("Images//3SquaresWB.bmp", ImreadModes.GrayScale))
            {
                using (var surf = SURF.Create(200))
                {
                    var keyPoints = surf.Detect(image);
                    using (var overLay = image.OverLay())
                    {
                        Cv2.DrawKeypoints(image, keyPoints, overLay);
                        this.Result.Source = overLay.ToBitmapSource();
                    }
                }
            }
        }
    }
}
