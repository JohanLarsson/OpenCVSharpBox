namespace OpenCVSharpBox
{
    using OpenCvSharp;

    public static class MatExt
    {
        public static Mat OverLay(this Mat mat)
        {
            return new Mat(mat.Size(), MatType.CV_8UC4, new Scalar(0, 0, 0, 0));
        }

        public static Point2f[] Corners(this Mat mat)
        {
            var rect = mat.BoundingRect();
            return new[]
            {
                rect.TopLeft.AsPointF(),
                new Point2f(rect.Width, 0),
                rect.BottomRight.AsPointF(),
                new Point2f(0, rect.Height),
            };
        }

        private static Point2f AsPointF(this Point p) => new Point2f(p.X, p.Y);
    }
}