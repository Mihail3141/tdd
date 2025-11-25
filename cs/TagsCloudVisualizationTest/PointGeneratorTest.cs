using System.Drawing;
using TagsCloudVisualization;
using FluentAssertions;

namespace TagsCloudVisualizationTest;

public class PointGeneratorTest
{
    private Point validCenter;

    [SetUp]
    public void Setup()
    {
        validCenter = new Point(100, 100);
    }


    [TestCase(-1, 100)]
    [TestCase(100, -1)]
    public void PointGenerator_ShouldThrowException_WhenInvalidCenter(int centerX, int centerY)
    {
        var act = () => new PointGenerator(new Point(centerX, centerY));

        act.Should().Throw<ArgumentException>();
    }

    [TestCase(-100, 100)]
    [TestCase(100, -100)]
    public void PointGenerator_ShouldThrowException_WhenInvalidSize(int xSize, int ySize)
    {
        var act = () => new PointGenerator(validCenter, xSize, ySize);

        act.Should().Throw<ArgumentException>();
    }

    [TestCase(10, -1)]
    [TestCase(-1, 100)]
    public void GetPointsOnSpiral_ShouldThrowException_WhenInvalidParameters(int countPoint, int radius)
    {
        var pointGenerator = new PointGenerator(validCenter);
        var act = () => pointGenerator.GetPointsOnSpiral(countPoint, 0, radius);

        act.Enumerating().Should().Throw<ArgumentException>();
    }

    [TestCase(50)]
    public void GetPointsOnSpiral_ShouldEachNextPointFartherFromCenter_WhenValidParameters(int countPoint)
    {
        var pointGenerator = new PointGenerator(validCenter);
        var actualPoints = pointGenerator.GetPointsOnSpiral(countPoint);
        var prevDistance = -1.0;
        
        foreach (var point in actualPoints)
        {
            var currentDistance = GetDistance(point, validCenter);
            currentDistance.Should().BeGreaterThan(prevDistance);
            prevDistance = currentDistance;
        }
    }

    private static double GetDistance(Point point1, Point point2)
    {
        var dx = point1.X - point2.X;
        var dy = point1.Y - point2.Y;
        return Math.Sqrt(dx * dx + dy * dy);
    }
}