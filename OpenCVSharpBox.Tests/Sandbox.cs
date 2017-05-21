namespace OpenCVSharpBox.Tests
{
    using OpenCvSharp;

    public class Sandbox
    {
        private static string ThreeSquaresBW = ImagePath("Images\\3SquaresBW.bmp");
        private static string ThreeSquaresWB = ImagePath("Images\\3SquaresWB.bmp");

        [NUnit.Framework.Test]
        public void Test()
        {
            using (var mat = new Mat(ThreeSquaresWB, ImreadModes.GrayScale))
            {
                var points = mat.FindContoursAsArray(RetrievalModes.External, ContourApproximationModes.ApproxSimple);
                using (var result = new Mat(300, 300, MatType.CV_8UC3, Scalar.White))
                {
                    result.DrawContours(points, -1, Scalar.Red, 2);
                    Window.ShowImages(new[] { mat, result }, new[] { "Original", "Result" });
                }
            }
        }

        private static string ImagePath(string name)
        {
            return System.IO.Path.Combine(NUnit.Framework.TestContext.CurrentContext.TestDirectory, name);
        }
    }
}
