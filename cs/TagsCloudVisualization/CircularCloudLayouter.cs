using System.Drawing;

namespace TagsCloudVisualization;

public class CircularCloudLayouter
{
    private List<Rectangle> Rects { get; }

    private Size imageSize;

    private readonly PointGenerator pointGenerator;

    private readonly Random random = new Random();

    public CircularCloudLayouter(Point center, int imageWidth = 1920, int imageHeight = 1080)
    {
        if (imageWidth <= 0 || imageHeight <= 0)
            throw new ArgumentException("Image width or height must be positive");
        if (center.X <= 0 || center.Y <= 0)
            throw new ArgumentException("Center should be greater than 0");
        if (center.X >= imageWidth || center.Y >= imageHeight)
            throw new ArgumentException("Center must be inside image bounds");
        Rects = new List<Rectangle>();
        imageSize = new Size(imageWidth, imageHeight);
        pointGenerator = new PointGenerator(center, imageSize);
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

    public IEnumerable<Rectangle> GetCircularCloudRectangles(int count, Size rectangleSize, double coefficient = 0)
    {
        if (count <= 0)
            throw new ArgumentException("Count must be positive");
        if (coefficient == 0)
        {
            for (var i = 0; i < count; i++)
                yield return PutNextRectangle(rectangleSize);
            yield break;
        }
        
        if (coefficient is < 0 or > 1)
            throw new ArgumentException("Coefficient must be between 0 and 1");
        for (var i = 0; i < count; i++)
        {
            var rnd = random.NextDouble() + coefficient;
            var rndWidth = (int)Math.Round(rectangleSize.Width * rnd);
            var rndHeight = (int)Math.Round(rectangleSize.Height * rnd);
            yield return PutNextRectangle(new Size(rndWidth, rndHeight));
        }
    }
}