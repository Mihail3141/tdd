using System.Drawing;
using TagsCloudVisualization.PointGenerator;

namespace TagsCloudVisualization.PointProvider;

public class CircularCloudLayouter : ICloudLayouter
{
    private readonly List<Rectangle> _rectangles = [];

    private readonly IPointGenerator _pointGenerator;

    private readonly int _maxPointsPerRectangle;

    public CircularCloudLayouter(Point center, int maxPointsPerRectangle, IPointGenerator pointGenerator)
    {
        ArgumentNullException.ThrowIfNull(pointGenerator);

        if (center.X <= 0 || center.Y <= 0)
            throw new ArgumentException("Center should be greater than 0");

        if (maxPointsPerRectangle <= 0)
            throw new ArgumentException("maxPointsPerRectangle must be greater than 0");

        _maxPointsPerRectangle = maxPointsPerRectangle;
        _pointGenerator = pointGenerator;
    }


    public Rectangle PutNextRectangle(Size rectangleSize)
    {
        if (rectangleSize.Width <= 0 || rectangleSize.Height <= 0)
            throw new ArgumentException("Rectangle size must be positive");

        var points = _pointGenerator
            .GetPoints()
            .Take(_maxPointsPerRectangle);


        foreach (var point in points)
        {
            var rectangle = new Rectangle(point, rectangleSize);
            for (var i = _rectangles.Count - 1; i >= 0; i--)
            {
                if (_rectangles[i].IntersectsWith(rectangle))
                    goto NextPoint;
            }

            _rectangles.Add(rectangle);
            return rectangle;
            NextPoint: ;
        }

        throw new ArgumentException($"Failed to find place for the {_rectangles.Count} rectangle");
    }
}