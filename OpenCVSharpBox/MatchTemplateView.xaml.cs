namespace OpenCVSharpBox
{
    using System.Windows.Controls;
    using OpenCvSharp;
    using OpenCvSharp.Extensions;
    using OpenCvSharp.XFeatures2D;

    public partial class MatchTemplateView : UserControl
    {
        public MatchTemplateView()
        {
            this.InitializeComponent();
            using (var template = new Mat("Images\\Circle_Template.bmp", ImreadModes.GrayScale))
            {
                using (var surf = SURF.Create(200))
                {
                    using (var templateDescriptors = new Mat())
                    {
                        surf.DetectAndCompute(template, null, out KeyPoint[] templateKeyPoints, templateDescriptors);
                        using (var image = new Mat("Images\\Circle.bmp", ImreadModes.GrayScale))
                        {
                            using (var imageDescriptors = new Mat())
                            {
                                surf.DetectAndCompute(image, null, out KeyPoint[] imageKeyPoints, imageDescriptors);
                                using (var matcher = new BFMatcher())
                                {
                                    var matches = matcher.Match(imageDescriptors, templateDescriptors);
                                    using (var overLay = image.OverLay())
                                    {
                                        Cv2.DrawMatches(image, imageKeyPoints, template, templateKeyPoints, matches, overLay);
                                        this.Result.Source = overLay.ToBitmapSource();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
