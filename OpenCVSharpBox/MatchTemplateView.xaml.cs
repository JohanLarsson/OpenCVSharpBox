namespace OpenCVSharpBox
{
    using System.Linq;
    using System.Windows.Controls;
    using OpenCvSharp;
    using OpenCvSharp.Extensions;
    using OpenCvSharp.XFeatures2D;

    public partial class MatchTemplateView : UserControl
    {
        public MatchTemplateView()
        {
            this.InitializeComponent();
            //this.FindAndDrawMatches();
            this.FindAndDrawHomo();
        }

        private void FindAndDrawHomo()
        {
            using (var template = new Mat("Images\\Circle_Template.bmp", ImreadModes.Color))
            {
                using (var surf = SURF.Create(1000))
                {
                    using (var templateDescriptors = new Mat())
                    {
                        surf.DetectAndCompute(template, null, out KeyPoint[] templateKeyPoints, templateDescriptors);
                        using (var image = new Mat("Images\\Circle.bmp", ImreadModes.Color))
                        {
                            using (var imageDescriptors = new Mat())
                            {
                                surf.DetectAndCompute(image, null, out KeyPoint[] imageKeyPoints, imageDescriptors);
                                using (var matcher = new BFMatcher())
                                {
                                    var matches = matcher.Match(imageDescriptors, templateDescriptors);
                                    var goodMatches = matches;//.Where(m => m.Distance < 0.2).ToArray();
                                    using (var srcPoints = InputArray.Create(goodMatches.Select(m => templateKeyPoints[m.TrainIdx].Pt)))
                                    {
                                        using (var dstPoints = InputArray.Create(goodMatches.Select(m => imageKeyPoints[m.QueryIdx].Pt)))
                                        {
                                            using (var homo = Cv2.FindHomography(srcPoints, dstPoints, HomographyMethods.Ransac))
                                            {
                                                using (var tmp = image.Overlay())
                                                {
                                                    //DrawBox(template, homo, overLay);
                                                    Cv2.BitwiseNot(template, template);
                                                    Cv2.WarpPerspective(template, tmp, homo, tmp.Size());
                                                    using (var overlay = tmp.Overlay())
                                                    {
                                                        for (var r = 0; r < tmp.Rows; r++)
                                                        {
                                                            for (var c = 0; c < tmp.Cols; c++)
                                                            {
                                                                overlay.Set(r, c,
                                                                    tmp.At<int>(r, c) == 0
                                                                        ? new Vec4b(0, 0, 0, 0)
                                                                        : new Vec4b(0, 0, 255, 150));
                                                            }
                                                        }

                                                        this.Result.Source = overlay.ToBitmapSource();
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
            }
        }

        private static void DrawBox(Mat template, Mat homo, Mat overLay)
        {
            var corners = Cv2.PerspectiveTransform(template.Corners(), homo);
            Cv2.Line(overLay, corners[0], corners[1], Scalar4.Red);
            Cv2.Line(overLay, corners[1], corners[2], Scalar4.Red);
            Cv2.Line(overLay, corners[2], corners[3], Scalar4.Red);
            Cv2.Line(overLay, corners[3], corners[0], Scalar4.Red);
        }

        private void FindAndDrawMatches()
        {
            using (var template = new Mat("Images\\Circle_Template.bmp", ImreadModes.GrayScale))
            {
                using (var surf = SURF.Create(1000))
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
                                    using (var overLay = image.Overlay())
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
