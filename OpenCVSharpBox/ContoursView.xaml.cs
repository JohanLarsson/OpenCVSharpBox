namespace OpenCVSharpBox
{
    using System.Windows.Controls;
    using OpenCvSharp;
    using OpenCvSharp.Extensions;

    public partial class ContoursView : UserControl
    {
        public ContoursView()
        {
            this.InitializeComponent();
            using (var image = new Mat("Images\\3SquaresWB.bmp", ImreadModes.GrayScale))
            {
                var points = image.FindContoursAsArray(RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                using (var overLay = image.OverLay())
                {
                    overLay.DrawContours(points, -1, Scalar4.Red, 2);
                    this.Result.Source = overLay.ToBitmapSource();
                }
            }
        }
    }
}
