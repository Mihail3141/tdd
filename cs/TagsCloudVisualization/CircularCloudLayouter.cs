using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private List<Rectangle> Rects { get; }
    
    private Size windowSize;
    
    private readonly PointGenerator pointGenerator;

    public CircularCloudLayouter(Point center, int windowWidth = 1920, int windowHeight = 1080)
    {
        if (windowWidth <= 0 || windowHeight <= 0)
            throw new ArgumentException("Window width or height must be positive");
        if (center.X <= 0 || center.Y <= 0)
            throw new ArgumentException("Center should be greater than 0");
        if (center.X >= windowWidth || center.Y >= windowHeight)
            throw new ArgumentException("Center must be in window");
        Rects = new List<Rectangle>();
        windowSize = new Size(windowWidth, windowHeight);
        pointGenerator = new PointGenerator(center, windowSize);
    }


    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle size must be positive");

        var points = pointGenerator.GetPointsOnSpiral();
        foreach (var point in points)
        {
            var rectangle = new Rectangle(point, rectangleSize);
            if (Rects.Any(rectangle.IntersectsWith))
                continue;
            Rects.Add(rectangle);
            return rectangle;
        }

        throw new InvalidOperationException($"Failed to find place for the {Rects.Count} rectangle");
    }

    public IEnumerable<Rectangle> GetCircularCloud(int count, Size rectangleSize)
    {
        if (count <= 0)
            throw new ArgumentException("Count must be positive");
        for (var i = 0; i < count; i++)
            yield return PutNextRectangle(rectangleSize);
    }
}