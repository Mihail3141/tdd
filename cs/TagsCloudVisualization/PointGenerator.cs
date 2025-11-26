using System.Collections;
using System.Drawing;

namespace TagsCloudVisualization;

public class PointGenerator
{
    private readonly Point center;
    private readonly Size imageSize;

    public PointGenerator(Point center, Size imageSize)
    {
        if (center.X < 0 || center.Y < 0)
            throw new ArgumentException("Center coordinates must be non-negative");
        if (imageSize.Width <= 0 || imageSize.Height <= 0)
            throw new ArgumentException("Image size must be positive");
        this.center = center;
        this.imageSize = imageSize;
    }

    public IEnumerable<Point> GetPointsOnSpiral(double angle = double.Pi / 24, double radius = 10)
    {
        if (radius <= 0)
            throw new ArgumentException("Radius must be positive");
        
        var currentAngle = 0.0;
        var currentRadius = 0.0;
        var x = 0;
        var y = 0;
        while (x < imageSize.Width || y < imageSize.Height)
        {
            var vector = radius * currentAngle / (2 * Math.PI);
            x = center.X + (int)Math.Round(Math.Cos(currentAngle) * vector);
            y = center.Y + (int)Math.Round(Math.Sin(currentAngle) * vector);
            yield return new Point(x, y);
            currentAngle += angle;
        }
    }
}