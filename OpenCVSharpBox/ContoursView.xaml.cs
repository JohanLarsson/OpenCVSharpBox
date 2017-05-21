namespace OpenCVSharpBox
{
    using System.Windows.Controls;
    using OpenCvSharp;
    using OpenCvSharp.Extensions;

    public partial class ContoursView : UserControl
    {
        public ContoursView()
        {
            InitializeComponent();
            using (var mat = new Mat("Images//3SquaresWB.bmp", ImreadModes.GrayScale))
            {
                var points = mat.FindContoursAsArray(RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                using (var result = new Mat(mat.Size(), MatType.CV_8UC4, new Scalar(0, 0, 0, 0)))
                {
                    result.DrawContours(points, -1, new Scalar(0, 0, 255, 255), 2);
                    this.Result.Source = result.ToBitmapSource();
                }
            }
        }
    }
}
