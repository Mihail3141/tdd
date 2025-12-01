using System.Drawing;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization.PointProvider;

public class CircularCloudLayouter
{
    private List<Rectangle> Rectangles { get; }

    private readonly SpiralPointGenerator spiralPointGenerator;

    private int maxPointCount = 10000;

    public void SetMaxPointCount(int maxPointCount)
    {
        if (maxPointCount < 0)
            throw new ArgumentException("maxPointCount must be greater than 0");
        this.maxPointCount = maxPointCount;
    }

    public CircularCloudLayouter(Point center)
    {
        if (center.X <= 0 || center.Y <= 0)
            throw new ArgumentException("Center should be greater than 0");
        Rectangles = new List<Rectangle>();
        spiralPointGenerator = new SpiralPointGenerator(center);
    }


    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle size must be positive");

        var points = spiralPointGenerator
            .GetPoints()
            .Take(maxPointCount);

        foreach (var point in points)
        {
            var rectangle = new Rectangle(point, rectangleSize);
            if (Rectangles.Any(rectangle.IntersectsWith))
                continue;
            Rectangles.Add(rectangle);
            return rectangle;
        }

        throw new InvalidOperationException($"Failed to find place for the {Rectangles.Count} rectangle");
    }
}