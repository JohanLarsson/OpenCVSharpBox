namespace OpenCVSharpBox.Tests
{
    using NUnit.Framework.Internal;
    using OpenCvSharp;
    using OpenCvSharp.XFeatures2D;

    public class Sandbox
    {
        private static string ThreeSquaresBW = FullFileName("Images\\3SquaresBW.bmp");
        private static string ThreeSquaresWB = FullFileName("Images\\3SquaresWB.bmp");

        [NUnit.Framework.Test]
        public void FindContoursAsArray()
        {
            using (var mat = new Mat(ThreeSquaresWB, ImreadModes.GrayScale))
            {
                var points = mat.FindContoursAsArray(RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                using (var result = new Mat(300, 300, MatType.CV_8UC3, Scalar.White))
                {
                    result.DrawContours(points, -1, Scalar.Red, 2);
                    Window.ShowImages(new[] {mat, result}, new[] {"Original", "Result"});
                }
            }
        }

        [NUnit.Framework.Test]
        public void SURFTest()
        {
            using (var mat = new Mat(ThreeSquaresWB, ImreadModes.GrayScale))
            {
                using (var surf = SURF.Create(200))
                {
                    var keyPoints = surf.Detect(mat);
                    using (var result = new Mat(300, 300, MatType.CV_8UC3, Scalar.White))
                    {
                        Cv2.DrawKeypoints(mat, keyPoints, result);
                        Window.ShowImages(new[] {mat, result}, new[] {"Original", "Result"});
                    }
                }
            }
        }

        private static string FullFileName(string name)
        {
            return System.IO.Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, name);
        }
    }
}
