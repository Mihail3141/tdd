using System.Collections;
using System.Drawing;

namespace TagsCloudVisualization;

public class PointGenerator
{
    private readonly int xSize;
    private readonly int ySize;
    private readonly Point center;
    public PointGenerator(Point centre, int xSize = 1920, int ySize = 1080)
    {
        if (centre.X < 0 || centre.Y < 0)
            throw new ArgumentException("Center coordinates must be non-negative");
        if (xSize < 0 || ySize < 0)
            throw new ArgumentException("Size must be  non-negative");
        center = centre;
        this.xSize = xSize;
        this.ySize = ySize;
    }

    public IEnumerable<Point> GetPointsOnSpiral(int pointsCount, double angle = double.Pi/12, double radius = 10)
    {
        if (pointsCount < 0)
            throw new ArgumentException("pointsCount must be > 0");
        if (radius < 0)
            throw new ArgumentException("Radius must be >= 0");
        if (0 > center.X || center.X > xSize ||
            0 > center.Y || center.Y > ySize)
            throw new ArgumentException("Center is out of bounds");

        double startAngle = 0;
        double startRadius = 0;
        for (var i = 0; i < pointsCount; i++)
        {
            var x = (int)Math.Round(Math.Cos(startAngle) * startRadius) + center.X;
            var y = (int)Math.Round(Math.Sin(startAngle) * startRadius) + center.Y;
            yield return new Point(x, y);
            startAngle += angle;
            startRadius += radius;
        }
    }
}